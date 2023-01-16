using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BetButton : MonoBehaviour
{
    public UnityEvent OnClick;

    [SerializeField] private bool _isNumber = true;

    
    private Stack<GameObject> _currentChips = new Stack<GameObject>();
    private StakeCalculator _calculator;

    private int _price;
    private int _chipCount = 0;
    private int _currentNumber;
    private int[] _betNumbers;

    private void Start()
    {
        _calculator = StakeCalculator.Instance;
        _calculator.OnClearTable.AddListener(ResetChips);
    }

    public bool IsNumber()
    {
        return _isNumber;
    }

    public int GetNumber()
    {
        return _currentNumber;
    }
    public int[] GetNumbers()
    {
        return _betNumbers;
    }
    
    public void SetNumber(int number)
    {
        _currentNumber = number;
    }
    public void SetNumber(int[] numbers)
    {
        _betNumbers = numbers;
    }

    public int GetPrice()
    {
        return _price;
    }


    public void Click(GameObject chipPrefab)
    {
        PlaceChip(chipPrefab);
        _chipCount++;
        OnClick.Invoke();
    }

    private void PlaceChip(GameObject chipPrefab)
    {

        _price = chipPrefab.GetComponent<Chip>().GetPrice();

        var chipObject = Instantiate(chipPrefab);

        var chip = chipObject.GetComponent<Chip>();
        chip.Initialize(this);
        
        var paddingY = chipObject.transform.localScale.y * 6 * _chipCount;

        chipObject.transform.SetParent(transform);
        chipObject.transform.position = transform.position;
        chipObject.transform.position += new Vector3(0, paddingY, 0);
        _currentChips.Push(chipObject);

        _calculator.AddChip(chip);
    }

    public int DeleteLastChip()
    {
        var chip = _currentChips.Pop();
        int price = chip.GetComponent<Chip>().GetPrice();
        Destroy(chip);

        return price;
    }

    private void ResetChips()
    {
        _chipCount = 0;
        foreach(var chip in _currentChips)
        {
            Destroy(chip);
        }

        _currentChips = new Stack<GameObject>();
    }
}
