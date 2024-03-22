using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem:ISaveAble
{
    string Name { get; set; }
    Sprite Icon { get; set; }
    int SellPrice { get; set; }
    int BuyPrice { get; set; }
    IInventoryItem Clone();
   
}
