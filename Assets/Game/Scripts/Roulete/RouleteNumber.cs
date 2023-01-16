using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouleteNumber : MonoBehaviour
{
    [SerializeField] private int _number;
    
    private void Start() 
    {
        gameObject.name = "" + _number;   
    }

    public void Initialize(int number = 0)
    {
        _number = number;
    }

    public int GetNumber()
    { 
        return _number;
    }
}
