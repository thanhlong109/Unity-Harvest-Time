using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControlUI : MonoBehaviour
{
    public void OpenShopUI()
    {
        ShopAction.Instance.OpenShop();
    }
    public void CloseShopUI()
    {
        ShopAction.Instance.CloseShop();
    }
}
