using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButton : MonoBehaviour
{
    private BetButton _button;
    [SerializeField] private int _currentNumber;

    private StakeCalculator _calculator;
    private int _price;
    
    private void OnEnable()
    {
        _button = GetComponent<BetButton>();
    }

    private void Start()
    {
        _calculator = StakeCalculator.Instance;
    }

    public void SetNumber(int number)
    {
        _currentNumber = number;
        _button.SetNumber(number);
    }

    public void AddNumberBet()
    {
        _price = _button.GetPrice();
        _calculator.AddStake(_currentNumber, _price);
    }
}
