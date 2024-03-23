using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [CreateAssetMenu(fileName = "New PlotItem", menuName = "Item/PlotItem")]
    public class PlotItem : ScriptableObject, ICountableItem
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

        [Header("Initial Data")]
        [SerializeField] private int initialQuality = 0;

        public IInventoryItem Clone()
        {
            PlotItem coppy = ScriptableObject.CreateInstance<PlotItem>();

            coppy.Quantity = quantity;
            coppy.Name = _name;
            coppy.Icon = icon;
            coppy.SellPrice = sellPrice;
            coppy.BuyPrice = buyPrice;
            return coppy;
        }

        public void SetToInitialData()
        {
            quantity = initialQuality;
        }
    }
}
