using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAction : NPCAction
{
    private static int SHOP_ACTION = Animator.StringToHash("ShopAction");
    public static ShopAction Instance;
    [SerializeField]private CanvasSlide shopUI;

    [SerializeField] private RectTransform ShopListSell;
    [SerializeField] private GameObject SellItemPrefabs;
    private Animator animator;

    private List<ICountableItem> farmerItems = new List<ICountableItem>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        animator = GetComponent<Animator>();

    }


    protected override void PerformAction()
    {
        animator.SetBool(SHOP_ACTION, true);
        //OpenShop();
    }

    public void OpenShop()
    {
        shopUI.SlideIn();
        LoadSellableItemFromFarmer();
    }

    private void LoadSellableItemFromFarmer()
    {
        var loadedItem = Inventory.Instance.GetAllCountableItem();
        foreach(var item in loadedItem)
        {
            var index = farmerItems.Find(x => x.Name == item.Name);
            if(index == null)
            {
                CreateShopSellItem(item);
                farmerItems.Add(item);
            }
            
        }
    }

    public void RemoveOutShop(ICountableItem item)
    {
       farmerItems.Remove(item);
    }


    private void CreateShopSellItem(ICountableItem item)
    {
        GameObject newObject = Instantiate(SellItemPrefabs);
        newObject.GetComponent<ShopItem>().SetItemData(item);
        newObject.transform.SetParent(ShopListSell.gameObject.transform, false);
    }
   

    public void CloseShop()
    {
        shopUI.SlideOut();
        animator.SetBool(SHOP_ACTION, false);
        FarmerAction.Instance.isActionAble = true;
    }



}
