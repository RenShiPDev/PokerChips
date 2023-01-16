using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] private float _price;

    private BetButton _currentButton;

    public void Initialize(BetButton currentButton)
    {
        _currentButton = currentButton;
    }

    public int GetPrice()
    {
        return (int)_price;
    }

    public BetButton GetButton()
    {
        return _currentButton;
    }
}
