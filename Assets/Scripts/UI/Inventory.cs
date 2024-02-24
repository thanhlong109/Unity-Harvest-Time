using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Inventory : MonoBehaviour
{
    private InventoryItem selectedItem;
    
    [SerializeField] private Sprite bgMemuItem;
    [SerializeField] private Sprite bgMemuItemSelected;
    [SerializeField] private List<InventoryItem> inventoryUIItems = new List<InventoryItem>();
    [SerializeField] private GameObject ItemPrefabs;
    [SerializeField] private ScriptableObject[] ItemData;

    private void Awake()
    {

    }

    void Start()
    {
        if(ItemData != null )
        {
            IInventoryItem[] inventoryItem = Array.ConvertAll(ItemData, item => item as IInventoryItem);
           AddRangeItem(ItemData as IInventoryItem[]);
        }
    }

    public void SelectedItem(InventoryItem item)
    {
        if(selectedItem == item)
        {
            selectedItem.SetBgIcon(bgMemuItem);
            selectedItem = null;
        }
        else
        {
            selectedItem?.SetBgIcon(bgMemuItem);
            selectedItem = item;
            selectedItem?.SetBgIcon(bgMemuItemSelected);
        }
        
    }

    public void AddItem(IInventoryItem item)
    {
        int index = inventoryUIItems.FindIndex(i => i.GetItemData().Name == item.Name);
        if(index < 0)
        {
            int indexNull = inventoryUIItems.FindIndex(i => i.GetItemData() == null);
            if(indexNull < 0)
            {
                CreateMenuItem(item);
            }
            else
            {
                inventoryUIItems[indexNull].SetItemData(item)  ;
            }
        }
        else
        {
            int amount = 0;
            if(item is ICountableItem countable)
            {
                amount = countable.Quantity;
            }else if(item is IUncountableItem uncountable)
            {
                amount = uncountable.Amounts;
            }
            inventoryUIItems[index].Add(amount);
        }
    }

    public void CreateMenuItem(IInventoryItem item)
    {
        GameObject newObject = Instantiate(ItemPrefabs);
        newObject.transform.parent = transform;
    }

    

    public void AddRangeItem(IInventoryItem[] items)
    {
        foreach(IInventoryItem item in items)
        {
            AddItem(item);
        }
    }

    

    public InventoryItem GetSelectedItem()
    {
        return selectedItem;
    }


}
