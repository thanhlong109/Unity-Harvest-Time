using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShopAction : NPCAction
{
    private static int SHOP_ACTION = Animator.StringToHash("ShopAction");
    public static ShopAction Instance;
    [SerializeField] private CanvasSlide shopUI;

    [SerializeField] private RectTransform ShopListSell;
    [SerializeField] private GameObject SellItemPrefabs;
    private List<ShopItem> shopSellItems = new List<ShopItem>();
    private Animator animator;

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
        foreach (var item in loadedItem)
        {
            var foundItem = shopSellItems.Find(x => x.GetItemData().Name == item.Name);
            if (foundItem == null)
            {
                CreateShopSellItem(item);
                
            }
            else
            {
                foundItem.SetItemData(item);
            }
        }
    }

    public void RemoveOutShop(ShopItem item)
    {
        shopSellItems.Remove(item);
    }



    private void CreateShopSellItem(ICountableItem item)
    {
        GameObject newObject = Instantiate(SellItemPrefabs);
        var shopItem = newObject.GetComponent<ShopItem>();
        shopItem.SetItemData(item);
        shopSellItems.Add(shopItem);
        newObject.transform.SetParent(ShopListSell.gameObject.transform, false);
    }


    public void CloseShop()
    {
        shopUI.SlideOut();
        animator.SetBool(SHOP_ACTION, false);
        FarmerAction.Instance.isActionAble = true;
    }

    private void OnMouseDown()
    {
        FarmerAction.Instance.SetAction(action);
    }



}
