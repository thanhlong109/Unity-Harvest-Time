using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Save Data New", menuName ="Save Data")]
public class SaveData : ScriptableObject, ISaveAble
{
    [Header("Current value")]
    public int money;

    [Header("Initial value")]
    private int initialMoney = 50;

    public void SetToInitialData()
    {
        money = initialMoney;
    }

   
}
