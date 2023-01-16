using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OddButton : MonoBehaviour
{
    [SerializeField] private bool _isEven;
    
    private BetButton _button;
    private StakeCalculator _calculator;

    private int[] _betNumbers;
    private int _price;
    
    private void Start()
    {
        _calculator = StakeCalculator.Instance;
        _button = GetComponent<BetButton>();
    }

    public void AddNumbersBet()
    {
        int numbersCount = _calculator.GetNumbersCount();
        _betNumbers = new int[numbersCount/2];

        int counter = 1;
        if(_isEven) counter = 2;

        for(int i = 0, j = counter; i < _betNumbers.Length; i++, j+=2)
        {
            _betNumbers[i] = j;
        }
        
        _button.SetNumber(_betNumbers);
        _price = _button.GetPrice();
        _calculator.AddStake(_betNumbers, _price);
    }
}
