using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Plant",menuName = "Item/Plant")]
public class Plant : ScriptableObject, ICountableItem
{
    
    public Sprite[] planetStateSprites;
    [Header("Seed")]
    public float timeToHarvest = 60f;
    public Sprite plantDieSprite;
    public int sellPrice;
    public int buyPrice;
    public float liveInDryTime = 30f;
    [HideInInspector] public float timeBtwStages;
    [SerializeField] private int quantity;
    [SerializeField] private string plantName;
    [SerializeField] private Sprite icon;

    [Header("Harvest")]
    public CountableItem harvestedItem;
    public int harvestedItemQuality;

    public int Quantity { get => quantity ; set => quantity = value; }
    public string Name { get => plantName; set => plantName = value ; }
    public Sprite Icon { get => icon; set => icon = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public int BuyPrice { get =>buyPrice; set => buyPrice = value; }

    [Header("Initial Data")]
    [SerializeField]private int initialQuality = 0;

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
        coppy.harvestedItem = harvestedItem;
        return coppy;
    }

    public void SetToInitialData()
    {
        quantity = initialQuality;
    }

    public ICountableItem Harvest()
    {
        IInventoryItem harvestItem = harvestedItem.Clone();
        harvestedItem.Quantity = harvestedItemQuality;
       
        return harvestItem as ICountableItem;
    }

}
