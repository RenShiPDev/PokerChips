using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RouleteRotater : MonoBehaviour
{
    public UnityEvent OnRotate;
    public UnityEvent OnRotationEnded;

    [SerializeField] private CurrentNumberChecker _checker;

    [SerializeField] private float _speed;
    [SerializeField] private float _slowdownSpeed;

    private StakeCalculator _calculator;

    private float _currentSpeed;
    private bool _isRotating;

    private void Start()
    {
        _calculator = StakeCalculator.Instance;
    }

    private void Update() 
    {
        if(_isRotating)
        {
            transform.Rotate(0, _currentSpeed * Time.deltaTime, 0);

            _currentSpeed -= _slowdownSpeed*Time.deltaTime;

            if(_currentSpeed <= 0)
            {
                OnRotationEnded.Invoke();
                
                _isRotating = false;
                _checker.CheckCurrentNumber();
            }
        }
    }

    public void Rotate()
    {
        if(_calculator.GetChipsCount() > 0)
        {
            OnRotate.Invoke();

            _isRotating = true;
            _currentSpeed = _speed;
        }
    }
}
