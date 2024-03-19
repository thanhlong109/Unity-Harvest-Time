using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance;
    [SerializeField] private int firstGive = 50;
    [SerializeField] private TextMeshProUGUI WalletText;

    private int wallet = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        wallet = ManageData.instance.SaveData.money;
        UpdateUI();
    }

    public void AddMoney(int money)
    {
        wallet += money;
        UpdateUI();
    }


    public void SubtractMoney(int money)
    {
        if(wallet - money <= 0)
        {
            wallet = money;
        }
        else
        {
            wallet -= money;
        }
        UpdateUI();
    }


    public void UpdateUI()
    {
        WalletText.text = "$ " + wallet.ToString();
    }

    private void OnApplicationQuit()
    {
        ManageData.instance.SaveData.money = wallet;
    }
}
