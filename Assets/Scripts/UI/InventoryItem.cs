using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryItem : MonoBehaviour
{
    private IInventoryItem ItemData;
    [SerializeField] private ScriptableObject scriptableObjectData;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI tmpro;
    private Button btn;
    private Image bgItem;
    private Inventory inventory;


    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
        btn = GetComponent<Button>();
        bgItem = GetComponent<Image>();
    }

    private void OnEnable()
    {
        btn.onClick.AddListener(OnSelect);
    }

    private void OnDisable()
    {
        btn.onClick.RemoveListener(OnSelect);
    }

    private void Start()
    {
        if (scriptableObjectData is IInventoryItem)
        {
            ItemData = (IInventoryItem)scriptableObjectData;
        }

        if (ItemData != null)
        {
            icon.sprite = ItemData.Icon;
            if (ItemData is ICountableItem)
            {
                tmpro.gameObject.SetActive(true);
            }
            else
            {
                tmpro.gameObject.SetActive(false);
            }
        }
        UpdateUI();
    }

    private void OnSelect()
    {
        inventory.SelectedItem(this);

    }

    public void UpdateUI()
    {
        if (ItemData != null)
        {
            bgItem.gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
            if (ItemData is ICountableItem countableItem)
            {
                tmpro.gameObject.SetActive(true);
                tmpro.text = countableItem.Quantity.ToString();
                if (countableItem.Quantity == 0)
                {
                    icon.gameObject.SetActive(false);
                    inventory.SelectedItem(this);
                    tmpro.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
                
            }
            else if (ItemData is IUncountableItem uncountableItem)
            {

            }
        }
        else
        {
            icon.gameObject.SetActive(false);
            tmpro.gameObject.SetActive(false);
            bgItem.gameObject.SetActive(false);
        }
    }

    public void SetBgIcon(Sprite icon)
    {
        bgItem.sprite = icon;
    }

    public void Add(int amount)
    {
        if (ItemData != null)
        {
            if (ItemData is ICountableItem countableItem)
            {
                countableItem.Quantity += amount;
            }
            else if (ItemData is IUncountableItem uncountableItem)
            {
                var temp = uncountableItem.Amounts + amount;
                if (temp < 100)
                {
                    uncountableItem.Amounts = temp;
                }
                else
                {
                    uncountableItem.Amounts = 100;
                }
            }
            UpdateUI();
        }
    }

    public void Subtract(int amount)
    {
        if (ItemData != null)
        {
            if (ItemData is ICountableItem countableItem)
            {
                if (countableItem.Quantity >= amount)
                {
                    countableItem.Quantity -= amount;
                    if (countableItem.Quantity <= 0)
                    {
                        icon.gameObject.SetActive(false);
                        UpdateUI();
                    }
                }

            }
            else if (ItemData is IUncountableItem uncountableItem)
            {
                var temp = uncountableItem.Amounts + amount;
                if (temp < 100)
                {
                    uncountableItem.Amounts = temp;
                }
                else
                {
                    uncountableItem.Amounts = 100;
                }
                UpdateUI();
            }
            UpdateUI();
        }
    }


    public void SetItemData(IInventoryItem item)
    {
        scriptableObjectData = item as ScriptableObject;
        Start();
    }
    public IInventoryItem GetItemData() { return ItemData; }
}
