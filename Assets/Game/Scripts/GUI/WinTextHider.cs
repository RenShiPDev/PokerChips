using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinTextHider : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _showTime;

    private ObjectHider _hider;

    private float _timer;
    private bool _isHide;

    private void Start()
    {
        _hider = GetComponent<ObjectHider>();
        _isHide = _hider.IsHide();
    }

    private void Update()
    {
        if(!_isHide)
        {
            _timer += Time.deltaTime;
            if(_timer > _showTime)
            {
                _timer = 0;
                _isHide = true;
                _hider.Hide();
            }
        }
        else 
        {

        }
    }

    public void Show(string text)
    {
        _text.text = text;
        _isHide = false;
        _hider.Show();
    }
}
