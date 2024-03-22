using Assets.Scripts.DataService;
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
        if (!ScreenPara.Instance.isContinue)
        {
            ManageData.instance.SaveData.SetToInitialData();
        }
        wallet = ManageData.instance.SaveData.money;
        UpdateUI();
    }

    public void AddMoney(int money)
    {
        wallet += money;
        UpdateUI();
    }


    public bool SubtractMoney(int money)
    {
        
        if(wallet - money >= 0)
        {
            wallet -= money;
            UpdateUI();
            return true;
        }
        return false;
        
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
