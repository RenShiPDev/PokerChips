using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour
{
    [SerializeField] private Button _button;

    public void Disable()
    {
        _button.enabled = false;
    }
    public void Enable()
    {
        _button.enabled = true;
    }
}
