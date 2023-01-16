using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableScreenRotater : MonoBehaviour
{
    [SerializeField] private Camera _verticalCamera;
    [SerializeField] private Canvas _verticalCanvas;

    [SerializeField] private Camera _horisontalCamera;
    [SerializeField] private Canvas _horisontalCanvas;

    [SerializeField] private TMP_Text _vMoneyText;
    [SerializeField] private WinTextHider _vWinText;

    [SerializeField] private TMP_Text _hMoneyText;
    [SerializeField] private WinTextHider _hWinText;

    
    private TMP_Text _currentMoneyText;
    private WinTextHider _currentWinText;

    private StakeCalculator _calculator;

    private bool _isVertical = true;
    private bool _resolutionChanged;

    private void Start()
    {
        _calculator = StakeCalculator.Instance;
        _calculator.SetGUI(_vMoneyText, _vWinText);
    }

    private void Update()
    {
        if(Screen.height > Screen.width)
        {
            if(_isVertical != true)
            {
                _resolutionChanged = true;
                _isVertical = true;
            }
        }
        else 
        {
            if(_isVertical != false)
            {
                _resolutionChanged = true;
                _isVertical = false;
            }
        }

        if(_resolutionChanged)
        {
            if(_isVertical)
            {
                _currentMoneyText = _vMoneyText;
                _currentWinText = _vWinText;
            }
            else
            {
                _currentMoneyText = _hMoneyText;
                _currentWinText = _hWinText;
            }
                
            _calculator.SetGUI(_currentMoneyText, _currentWinText);
            _currentMoneyText.text = PlayerPrefs.GetFloat("Money").ToString("0");
            ChangeResolution();
        }
    }

    private void ChangeResolution()
    {
        _horisontalCamera.gameObject.SetActive(!_isVertical);
        _verticalCamera.gameObject.SetActive(_isVertical);

        _horisontalCanvas.gameObject.SetActive(!_isVertical);
        _verticalCanvas.gameObject.SetActive(_isVertical);

        _resolutionChanged = false;
    }
}
