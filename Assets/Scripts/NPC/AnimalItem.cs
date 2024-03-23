using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.NPC
{
    [CreateAssetMenu(fileName = "New AnimalItem", menuName = "Item/AnimalItem")]
    public class AnimalItem : ScriptableObject, ICountableItem
    {
        [SerializeField] private int quantity;
        [SerializeField] private string _name;
        [SerializeField] private Sprite icon;
        public AnimalKind kind;
        public int sellPrice;
        public int buyPrice;

        public int Quantity { get => quantity; set => quantity = value; }
        public string Name { get => _name; set => _name = value; }
        public Sprite Icon { get => icon; set => icon = value; }
        public int SellPrice { get => sellPrice; set => sellPrice = value; }
        public int BuyPrice { get => buyPrice; set => buyPrice = value; }

        [Header("Initial Data")]
        [SerializeField] private int initialQuality = 0;

        public IInventoryItem Clone()
        {
            AnimalItem coppy = ScriptableObject.CreateInstance<AnimalItem>();

            coppy.Quantity = quantity;
            coppy.Name = _name;
            coppy.Icon = icon;
            coppy.SellPrice = sellPrice;
            coppy.BuyPrice = buyPrice;
            coppy.kind = kind;
            return coppy;
        }

        public void SetToInitialData()
        {
            quantity = initialQuality;
        }
    }
}
