using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.UI;

public class WebViewer : MonoBehaviour
{
    private Stack<string> _urlHistory = new Stack<string>();
    private WebViewObject _webViewObject;

    private string _loadedURL = "";
    private string _currentPage = "";

    public IEnumerator InitWebViewer(string pageUrl, WebViewObject webViewObject)
    {
        _loadedURL = pageUrl;
        _currentPage = pageUrl;
        _urlHistory.Push(pageUrl);

        _webViewObject = webViewObject;

        _webViewObject.Init
        (
            cb: (msg) =>
            {
                Debug.Log(string.Format("CallFromJS[{0}]", msg));
            },
            err: (msg) =>
            {
                Debug.Log(string.Format("CallOnError[{0}]", msg));
            },
            httpErr: (msg) =>
            {
                Debug.Log(string.Format("CallOnHttpError[{0}]", msg));
            },
            ld: (msg) =>
            {
                SaveNextPage(msg);
                Debug.Log(msg);
            },
            started: (msg) =>
            {
                
            },
            hooked: (msg) =>
            {
                Debug.Log(string.Format("CallOnHooked[{0}]", msg));
            }
        );

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        _webViewObject.bitmapRefreshCycle = 1;
#endif

        _webViewObject.SetMargins(0,0,0,0);
        _webViewObject.SetTextZoom(100);  // android only
        _webViewObject.SetVisibility(true);

        StartCoroutine(LoadPage(pageUrl));

        yield return null;
    }

    public IEnumerator LoadPage(string pageUrl)
    {
        Debug.Log("Loading - " + pageUrl);

#if !UNITY_WEBPLAYER && !UNITY_WEBGL
        if (pageUrl.StartsWith("http")) 
        {
            _webViewObject.LoadURL(pageUrl.Replace(" ", "%20"));
        } 
        else 
        {
            var url = pageUrl.Replace(".html", ".html");
            var src = System.IO.Path.Combine(Application.streamingAssetsPath + "/WebAssets/", url);
            var dst = System.IO.Path.Combine(Application.persistentDataPath, url);
            byte[] result = null;

            if (src.Contains("://")) 
            {
#if UNITY_2018_4_OR_NEWER
                var unityWebRequest = UnityWebRequest.Get(src);
                yield return unityWebRequest.SendWebRequest();
                result = unityWebRequest.downloadHandler.data;
#else
                var www = new WWW(src);
                yield return www;
                result = www.bytes;
#endif
            }
            else 
            {
                result = System.IO.File.ReadAllBytes(src);
            }

            System.IO.File.WriteAllBytes(dst, result);
            _webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
        }
#else
        if (pageUrl.StartsWith("http") || pageUrl.StartsWith("https")) 
        {
            try
            {
                _webViewObject.LoadURL(pageUrl.Replace(" ", "%20"));
            }
            catch
            {
                Debug.Log("Failed load page = " + pageUrl);
            }
        } 
        else 
        {
            _webViewObject.LoadURL("StreamingAssets/" + pageUrl.Replace(" ", "%20"));
        }
#endif
        Debug.Log("Loaded");
        yield break;
    }

    private void SaveNextPage(string url)
    {
        if(_currentPage != url)
        {
            _urlHistory.Push(url);
            _currentPage = url;
            Debug.Log(_urlHistory.Count);
        }
    }

    private string GetPreviousPage()
    {
        if(_urlHistory.Count >= 2){
            string currentPage = _urlHistory.Pop();
            string previousPage = _urlHistory.Pop();
            return previousPage;
        }
        else
            return _loadedURL;
            
    }
    
    private void Update() 
    {
        if( Input.GetKeyUp(KeyCode.Escape) )
        {
            string previousPage = GetPreviousPage();
            StartCoroutine(LoadPage(previousPage));
            if(previousPage == _loadedURL)
            {
                _urlHistory.Clear();
                _urlHistory.Push(_loadedURL);
            }
        }
    }
}
