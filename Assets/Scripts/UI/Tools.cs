using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolsType
{
    WATERING_CAN,
    HOE,
    PICKAXE,
    AXE
}

[CreateAssetMenu(fileName = "New Tool", menuName = "Item/Tools")]
public class Tools : ScriptableObject, IUncountableItem
{
    [SerializeField] private int _amounts;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    public int sellPrice;
    public int buyPrice;

    public Vector2 offset = new Vector2(-1.5f,-1);
    public string sfxName;
    public float timeToCompleteAction = 2f;
    public ToolsType ToolType;
    public delegate void ActionCallback();
    public event ActionCallback OnActionCompleted;
    public int Amounts { get => _amounts; set => _amounts = value; }
    public string Name { get => _name; set => _name = value; }
    public Sprite Icon { get => _icon; set => _icon = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public int BuyPrice { get => buyPrice; set => buyPrice = value; }

    public IEnumerator WaitToActionDone()
    {
        yield return new WaitForSeconds(timeToCompleteAction);
        OnActionCompleted?.Invoke();
    }

    public IInventoryItem Clone()
    {
        Tools coppy = ScriptableObject.CreateInstance<Tools>();

        coppy.Name = Name;
        coppy.buyPrice = BuyPrice;
        coppy.Icon = Icon;
        coppy.SellPrice = SellPrice;
        coppy.offset = offset;
        coppy.timeToCompleteAction = timeToCompleteAction;
        coppy.ToolType = ToolType;
        return coppy;
    }
}

     
    
