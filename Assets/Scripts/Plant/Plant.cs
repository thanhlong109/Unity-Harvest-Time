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
    [HideInInspector] public float timeBtwStages;

    [SerializeField] private int quantity;
    [SerializeField] private string plantName;
    [SerializeField] private Sprite icon;

    public int Quantity { get => quantity ; set => quantity = value; }
    public string Name { get => plantName; set => plantName = value ; }
    public Sprite Icon { get => icon; set => icon = value; }
}
