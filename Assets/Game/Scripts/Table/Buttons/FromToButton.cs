using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromToButton : MonoBehaviour
{
    private BetButton _button;
    [SerializeField] private int _fromNumber;
    [SerializeField] private int _toNumber;

    private StakeCalculator _calculator;

    private int[] _betNumbers;
    private int _price;
    
    private void Start()
    {
        _calculator = StakeCalculator.Instance;
        _button = GetComponent<BetButton>();
    }
    
    public void Add_FromTo_bet()
    {
        _betNumbers = new int[_toNumber-_fromNumber];
        for(int i = 0; i < _betNumbers.Length; i++)
            _betNumbers[i] = _fromNumber+i;
        
        _button.SetNumber(_betNumbers);
        _price = _button.GetPrice();
        _calculator.AddStake(_betNumbers, _price);
    }
}
