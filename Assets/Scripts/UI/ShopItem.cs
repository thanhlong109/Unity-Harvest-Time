using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ShopItemType
{
    SELL, BUY
}
public class ShopItem : MonoBehaviour
{

    [SerializeField] private ScriptableObject ScriptableObject;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private Button ButtonBuy;
    [SerializeField] private ShopItemType shopItemType;

    private IInventoryItem inventoryItem;
    void Start()
    {
       UpdateUI();
        
    }

    public void OnBuyClick()
    {
        
        if(inventoryItem is Plant)
        {
            Plant item = (Plant)inventoryItem.Clone();
            item.Quantity = 1;
            Inventory.Instance.AddItem(item);
        }else if(inventoryItem is CountableItem) {
            CountableItem item = (CountableItem)inventoryItem;
            item.Quantity = 1;
            Inventory.Instance.AddItem(item);
        }
        WalletManager.Instance.SubtractMoney(inventoryItem.BuyPrice);
        AudioManager.Instance.PlaySFX("CashSfx");
    } 

    public void OnSellClick()
    {
        Inventory.Instance.RemoveItem(inventoryItem, 1);
        WalletManager.Instance.AddMoney(inventoryItem.SellPrice);
        UpdateUI();
        AudioManager.Instance.PlaySFX("CashSfx");
    }

    private void UpdateUI()
    {
        if (ScriptableObject is IInventoryItem)
        {
            inventoryItem = (IInventoryItem)ScriptableObject;
            Icon.sprite = inventoryItem.Icon;
            Price.text = "$ " + (ShopItemType.BUY == shopItemType ? inventoryItem.BuyPrice : inventoryItem.SellPrice);
            ItemName.text = inventoryItem.Name;

        }
        if(inventoryItem is ICountableItem countableItem && shopItemType == ShopItemType.SELL)
        {
            if(countableItem.Quantity > 0) { 
                gameObject.SetActive(true);
            }
            else
            {
                ShopAction.Instance.RemoveOutShop(countableItem);
                gameObject.SetActive(false);
            }
        }
    }

    public void SetItemData(IInventoryItem item)
    {
        ScriptableObject = item as ScriptableObject;
        Start();
    }
}
