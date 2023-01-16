using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImageRotater : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));
    }
}
