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
    public void PlaySFXOpenShop()
    {
        AudioManager.Instance.PlaySFX("OpenShopSfx");
    }
    public void PlaySFXShopTalk()
    {
        AudioManager.Instance.PlaySFX("ShopTalkSfx");
    }
}
