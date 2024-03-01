using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New CountableItem",menuName = "Item/CountableItem")]
public class CountableItem : ScriptableObject, ICountableItem
{
    [SerializeField] private int quantity;
    [SerializeField] private string _name;
    [SerializeField] private Sprite icon;
    public int sellPrice;
    public int buyPrice;

    public int Quantity { get => quantity; set => quantity = value; }
    public string Name { get => _name; set => _name = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public int BuyPrice { get => buyPrice; set => buyPrice = value; }

    public IInventoryItem Clone()
    {
        CountableItem coppy = ScriptableObject.CreateInstance<CountableItem>();

        coppy.Quantity = quantity;
        coppy.Name = _name;
        coppy.Icon = icon;
        coppy.SellPrice = sellPrice;
        coppy.BuyPrice = buyPrice;
        return coppy;
    }
}

