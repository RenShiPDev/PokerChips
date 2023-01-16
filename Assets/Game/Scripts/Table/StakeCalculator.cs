using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StakeCalculator : MonoBehaviour
{
    private static StakeCalculator _instance;
    public static StakeCalculator Instance { get { return _instance; } }

    public UnityEvent OnClearTable;

    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private WinTextHider _winText;
    [SerializeField] private float _currentMoney;

    private Stack<Chip> _tableChips =  new Stack<Chip>();

    private static int NUMBERS_COUNT = 37;
    private int[] _currentBids = new int[NUMBERS_COUNT];

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        _moneyText.text = _currentMoney.ToString("0");
        PlayerPrefs.SetFloat("Money", _currentMoney);
    }

    public void CalculateStake(int number)
    {
        int bidsCount = 0;
        int prize = 0;

        for(int i = 0; i < _currentBids.Length; i++)
        {
            if(_currentBids[i] != 0)
                bidsCount++;

            if(i == number)
                prize = _currentBids[i];
        }

        prize = prize * (NUMBERS_COUNT/bidsCount);
        
        _currentMoney = PlayerPrefs.GetFloat("Money") + prize;

        _moneyText.text = _currentMoney.ToString("0");
        PlayerPrefs.SetFloat("Money", _currentMoney);
        
        if(prize > 0)
            _winText.Show("Win!\n" + prize);
        else
            _winText.Show("Lose.");

        ClearTable();
    }
    
    public void SetGUI(TMP_Text moneyText, WinTextHider winText)
    {
        _moneyText = moneyText;
        _winText = winText;
    }

    public void ReturnChip()
    {
        if(_tableChips.Count > 0)
        {
            Chip chip = _tableChips.Pop();
            BetButton chipButton = chip.GetButton();
            int price = chip.GetPrice();

            _currentBids[chipButton.GetNumber()] -= price;

            _currentMoney = PlayerPrefs.GetFloat("Money") + price;
            _moneyText.text = _currentMoney.ToString("0");
            PlayerPrefs.SetFloat("Money", _currentMoney);

            chip.GetButton().DeleteLastChip();
        }
    }

    public void AddChip(Chip chip)
    {
        _tableChips.Push(chip);
    }

    private void ClearTable()
    {
        _currentBids = new int[NUMBERS_COUNT];
        _tableChips.Clear();
        OnClearTable.Invoke();
    }

    public void AddStake(int[] choosedBet, int price)
    {
        for(int i = 0; i < choosedBet.Length; i++)
            _currentBids[choosedBet[i]] += price;
    }

    public void RemoveStake(int[] choosedBet, int price)
    {
        for(int i = 0; i < choosedBet.Length; i++)
            _currentBids[choosedBet[i]] -= price;
    }


    public void AddStake(int choosedBet, int price)
    {
        _currentBids[choosedBet] += price;
    }

    public void RemoveStake(int choosedBet, int price)
    {
        _currentBids[choosedBet] -= price;
    }

    public int GetNumbersCount()
    {
        return NUMBERS_COUNT;
    }

    public int GetChipsCount()
    {
        return _tableChips.Count;
    }
}
