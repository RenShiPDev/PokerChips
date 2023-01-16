using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenClickHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _chipPrefab;
    [SerializeField] private TMP_Text _moneyText;

    private void Update() 
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit cameraHit;

            if (Physics.Raycast (cameraRay, out cameraHit, Mathf.Infinity))
            {
                PlaceChip(cameraHit.collider.gameObject);
            }
        }    
    }
    
    private void PlaceChip(GameObject hitObject)
    {
        if(hitObject.TryGetComponent(out BetButton button))
        {
            float money = PlayerPrefs.GetFloat("Money");
            int price = _chipPrefab.GetComponent<Chip>().GetPrice();
            if(money >= price)
            {
                button.Click(_chipPrefab);
                
                money -= price;

                PlayerPrefs.SetFloat("Money", money);
                _moneyText.text = money.ToString();
            }
            else
                Debug.Log("NEMoney");
        }
    }
}
