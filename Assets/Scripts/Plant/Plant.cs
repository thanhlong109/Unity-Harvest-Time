using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Plant",menuName = "Item/Plant")]
public class Plant : ScriptableObject, ICountableItem
{
    public Sprite[] planetStateSprites;
    public float timeToHarvest = 60f;
    public string harvestedName = "";
    public Sprite plantDieSprite;
    public int sellPrice;
    public int buyPrice;
    [HideInInspector] public float timeBtwStages;

    [SerializeField] private int quantity;
    [SerializeField] private string plantName;
    [SerializeField] private Sprite icon;

    public int Quantity { get => quantity ; set => quantity = value; }
    public string Name { get => plantName; set => plantName = value ; }
    public Sprite Icon { get => icon; set => icon = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public int BuyPrice { get =>buyPrice; set => buyPrice = value; }

    public IInventoryItem Clone()
    {
        Plant coppy = ScriptableObject.CreateInstance<Plant>();

        coppy.Name = Name;
        coppy.BuyPrice = BuyPrice;
        coppy.plantDieSprite = plantDieSprite;
        coppy.Quantity = Quantity;
        coppy.Icon = Icon;
        coppy.SellPrice = SellPrice;
        coppy.timeBtwStages = timeBtwStages;
        coppy.planetStateSprites = planetStateSprites;
        coppy.timeToHarvest = timeToHarvest;
        coppy.harvestedName = harvestedName;
     
        return coppy;
    }
}
