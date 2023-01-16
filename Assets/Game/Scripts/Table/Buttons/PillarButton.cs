using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarButton : MonoBehaviour
{
    [SerializeField] private int _pillar;
    
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

        int counter = 3;

        _betNumbers = new int[numbersCount/counter];

        for(int i = 0, j = _pillar; i < _betNumbers.Length; i++, j+=counter)
        {
            _betNumbers[i] = j;
        }
        
        _button.SetNumber(_betNumbers);
        _price = _button.GetPrice();
        _calculator.AddStake(_betNumbers, _price);
    }
}
