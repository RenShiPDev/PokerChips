using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumButtonsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;

    [SerializeField] private Transform _spawnPos;
    [SerializeField] private Transform _LeftUp_pos;
    [SerializeField] private Transform _RightUp_pos;
    [SerializeField] private Transform _LeftDown_pos;

    private int _numbersCount;
    private float _paddingDown;
    private float _paddigLeft;

    private void Start()
    {
        _numbersCount = StakeCalculator.Instance.GetNumbersCount();

        _paddingDown = (_LeftUp_pos.position - _LeftDown_pos.position).magnitude / (_numbersCount / 3);
        _paddigLeft = (_LeftUp_pos.position - _RightUp_pos.position).magnitude / 3;

        Vector3 lastPosition = _spawnPos.position;
        for(int i = 1; i <= _numbersCount; i++)
        {
            var clone = Instantiate(_buttonPrefab, transform);
            clone.transform.position = lastPosition;

            BetButton button = clone.GetComponent<BetButton>();
            if(button.IsNumber())
            {
                clone.GetComponent<NumberButton>().SetNumber(i);
            }

            lastPosition.x += _paddigLeft;
            if(i % 3 == 0)
            {
                lastPosition.z -= _paddingDown;
                lastPosition.x -= _paddigLeft*3;
            }
        }
    }
}
