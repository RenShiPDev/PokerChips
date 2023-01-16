using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : MonoBehaviour
{
    public delegate void OnClickHandler();

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Vector3 _hidePos = new Vector3(0, -1500, 0);
    [SerializeField] private float _moveSpeed = 2;
    [SerializeField] private bool _isHide;

    private Vector3 _showPos;

    private void OnEnable()
    {
        _rectTransform = GetComponent<RectTransform>();
        _showPos = _rectTransform.anchoredPosition3D;

        if(_isHide)
            _rectTransform.anchoredPosition3D = _hidePos;
    }

    private void Update()
    {
        if(_isHide)
        {
            MoveToPos(_hidePos);
        }
        else
        {
            MoveToPos(_showPos);
        }
    }

    public void ChangeHiding()
    {
        _isHide = !_isHide;
    }

    public void Hide()
    {
        _isHide = true;
    }
    public void Show()
    {
        _isHide = false;
    }

    public void ShowNow()
    {
        _isHide = false;
        _rectTransform.anchoredPosition3D = _showPos;
    }

    public bool IsHide()
    {
        return _isHide;
    }

    private void MoveToPos(Vector3 target)
    {
        if( Mathf.Abs((Mathf.Abs(_rectTransform.anchoredPosition3D.y) - Mathf.Abs(target.y))) >= 0.1f )
        {
            var delta = Vector3.LerpUnclamped(_rectTransform.anchoredPosition3D, target, _moveSpeed * Time.deltaTime);
            _rectTransform.anchoredPosition3D = delta;
        }
    }
}
