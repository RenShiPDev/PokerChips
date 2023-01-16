using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouleteNumbersSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _numberPrefab;

    [SerializeField] private Transform _centerTransform;
    [SerializeField] private Transform _numberTransform;
    [SerializeField] private Transform _numbersParent;

    private static int[] ROULETE_NUMBERS = new int[]{0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22,18, 29, 7, 28, 12, 35, 3, 26};
    
    private float _radius;
    private float _numbersAngle;

    private void Start() 
    {
        _radius = (_numberTransform.position - _centerTransform.position).magnitude;
        _numbersAngle = 360f / ROULETE_NUMBERS.Length;

        for(int i = 0; i < ROULETE_NUMBERS.Length; i++)
        {
            var clone = Instantiate(_numberPrefab, _numbersParent);
            clone.GetComponent<RouleteNumber>().Initialize(ROULETE_NUMBERS[i]);
            clone.transform.position = _numberTransform.position;
            clone.transform.RotateAround(_centerTransform.position, Vector3.up, _numbersAngle*i);
            //clone.transform.SetParent(_numbersParent);
        }
    }
}
