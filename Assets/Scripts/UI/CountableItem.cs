using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New CountableItem",menuName = "Item/CountableItem")]
public class CountableItem : ScriptableObject, ICountableItem
{
    [SerializeField] private int quantity;
    [SerializeField] private string _name;
    [SerializeField] private Sprite icon;

    public int Quantity { get => quantity; set => quantity = value; }
    public string Name { get => _name; set => _name = value; }
    public Sprite Icon { get => icon; set => icon = value; }
}

