using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    COUNTABLE,
    UN_COUNTABLE
}

[CreateAssetMenu(fileName = "New Plant",menuName ="Plant")]
public class Plant : ScriptableObject, ICountableItem
{
    public Sprite[] planetStateSprites;
    public float timeBtwStages = 2f;
    public ItemType itemType = ItemType.COUNTABLE;

    [SerializeField] private int quantity;
    [SerializeField] private string plantName;
    [SerializeField] private Sprite icon;

    public int Quantity { get => quantity ; set => quantity = value; }
    public string Name { get => plantName; set => plantName = value ; }
    public Sprite Icon { get => icon; set => icon = value; }
}
