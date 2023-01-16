using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNumberChecker : MonoBehaviour
{
    [SerializeField] private StakeCalculator _calculator;

    private RouleteNumber _currentNumber;

    public void CheckCurrentNumber()
    {
        _calculator.CalculateStake(_currentNumber.GetNumber());
        Debug.Log(_currentNumber.GetNumber());
    }


    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.TryGetComponent(out RouleteNumber number))
        {
            _currentNumber = number;
        }
    }
}
