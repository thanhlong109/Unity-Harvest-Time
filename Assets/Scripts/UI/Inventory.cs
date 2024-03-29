using Assets.Scripts.DataService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;


public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private InventoryItem selectedItem;
    
    [SerializeField] private Sprite bgMemuItem;
    [SerializeField] private Sprite bgMemuItemSelected;
    [SerializeField] private List<InventoryItem> inventoryUIItems = new List<InventoryItem>();
    [SerializeField] private GameObject ItemPrefabs;
    [SerializeField] private ScriptableObject[] ItemData;
    [SerializeField] private Animator handAnimator;
    private IInventoryItem[] initialItems;

    private static int SELECT_WATERING_CAN = Animator.StringToHash("WateringCanSelected");
    private static int SELECT_HOE = Animator.StringToHash("HoeSelected");
    private static int SELECT_AXE = Animator.StringToHash("AxeSelected");
    private static int SELECT_PICKAXE = Animator.StringToHash("PickaxeSelected");
    private static string PLANT_SAVE_PATH = "Plants";
    private static string ITEM_SAVE_PATH = "Items";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        var plants = Resources.LoadAll(PLANT_SAVE_PATH, typeof(ScriptableObject));
        var items = Resources.LoadAll(ITEM_SAVE_PATH, typeof(ScriptableObject));
        var availableItems = ItemData;
        var list = new List<ScriptableObject>();
        list.AddRange(availableItems);
        list.AddRange(items);
        list.AddRange(plants);
        ItemData = list.ToArray();

        if (ItemData != null)
        {
            initialItems = Array.ConvertAll(ItemData, item => item as IInventoryItem);
            if (!ScreenPara.Instance.isContinue)
            {
                foreach (var item in initialItems)
                {
                    item.SetToInitialData();
                }
            }
            AddRangeItem(initialItems);
        }
    }

    void Start()
    {
      
        
        
    }

    private void ResetAnimator()
    {
        handAnimator.SetBool(SELECT_WATERING_CAN, false);
        handAnimator.SetBool(SELECT_PICKAXE, false);
        handAnimator.SetBool(SELECT_HOE, false);
        handAnimator.SetBool(SELECT_AXE, false);
    }

    public void SelectedItem(InventoryItem item)
    {
        ResetAnimator();
        if (selectedItem == item)
        {
            selectedItem.SetBgIcon(bgMemuItem);
            selectedItem = null;
        }
        else
        {
            selectedItem?.SetBgIcon(bgMemuItem);
            selectedItem = item;
            selectedItem?.SetBgIcon(bgMemuItemSelected);

            var itemData = selectedItem.GetItemData();
            if (itemData is IUncountableItem)
            {
                if (itemData is Tools)
                {
                    var tools = (Tools)itemData;
                    switch (tools.ToolType)
                    {
                        case ToolsType.WATERING_CAN:
                            {
                                handAnimator.SetBool(SELECT_WATERING_CAN, true);
                                break;
                            }
                        case ToolsType.PICKAXE:
                            {
                                handAnimator.SetBool(SELECT_PICKAXE, true);
                                break;
                            }
                        case ToolsType.AXE:
                            {
                                handAnimator.SetBool(SELECT_AXE, true);
                                break;
                            }
                        case ToolsType.HOE:
                            {
                                handAnimator.SetBool(SELECT_HOE, true);
                                break;
                            }
                    }
                }
            }
            else if (itemData is ICountableItem)
            {

            }
        }

    }

    public void RemoveItem(IInventoryItem item, int quantity)
    {
        int index = inventoryUIItems.FindIndex(i => i.GetItemData() != null && i.GetItemData().Name == item.Name);
        if (index >= 0)
        {
            if (item is ICountableItem)
            {
                 inventoryUIItems[index].Subtract(quantity);
            }
           
        }
    }

    public void AddItem(IInventoryItem item)
    {
        int index = inventoryUIItems.FindIndex(i => i.GetItemData() != null && i.GetItemData().Name == item.Name);
        if (index < 0)
        {
            int indexNull = inventoryUIItems.FindIndex(i => i.GetItemData() == null);
            if (indexNull < 0)
            {
                CreateMenuItem(item);
            }
            else
            {
                inventoryUIItems[indexNull].SetItemData(item);
            }
        }
        else
        {
            int amount = 0;
            if (item is ICountableItem countable)
            {
                amount = countable.Quantity;
            }
            else if (item is IUncountableItem uncountable)
            {
                amount = uncountable.Amounts;
            }
            inventoryUIItems[index].Add(amount);
        }
    }

    public void CreateMenuItem(IInventoryItem item)
    {
        GameObject newObject = Instantiate(ItemPrefabs);
        InventoryItem inventoryItem = newObject.GetComponent<InventoryItem>();
        inventoryItem.SetItemData(item);
        newObject.transform.SetParent(transform, false);
        inventoryUIItems.Add(inventoryItem);
       
    }



    public void AddRangeItem(IInventoryItem[] items)
    {
        foreach (IInventoryItem item in items)
        {
            AddItem(item);
        }
    }



    public InventoryItem GetSelectedItem()
    {
        return selectedItem;
    }

    public List<ICountableItem> GetAllCountableItem()
    {
        
        List<ICountableItem> items = new List<ICountableItem>();
        
        foreach (InventoryItem item in inventoryUIItems)
        {
            var dataItem =  item.GetItemData();
            if ( dataItem!=null && dataItem is ICountableItem countable)
            {
                items.Add(countable);
                
            }
        }
        return items; 
    }

    public IInventoryItem GetItemByName(string name)
    {
        return initialItems.Where(c => c.Name == name).FirstOrDefault().Clone();
    }
}
