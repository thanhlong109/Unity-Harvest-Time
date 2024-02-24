using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    
    [SerializeField] private Vector2 farmerOffsetTarget = Vector2.zero;

    // plant 
    [SerializeField] private bool isPlanted = false;
    [SerializeField] private SpriteRenderer plant;
    [SerializeField] private Plant plantData;
    [SerializeField] private int plantState = 0;
    private float timer;

    private FarmerAction _farmer;
    private Action action;
    private InventoryItem itemSelected;

    private Inventory inventory;
    private void Awake()
    {
        _farmer = FindObjectOfType<FarmerAction>();
        inventory = FindObjectOfType<Inventory>();
    }

    void Start()
    {

        action = new Action()
        {
            radiusActionPerform = 2f,
            targetPos = transform.position,
        };
        action.OnActionPerform += PerformAction;
    }

    private void PerformAction()
    {
        if (isPlanted)
        {
            Harvest();
        }
        else if(plantData.Quantity> 0)
        {
            Plant();
        }
    }

    void Update()
    {
        if(isPlanted)
        {
            timer -= Time.deltaTime;
            if(timer < 0 && plantState < plantData.planetStateSprites.Length - 1 )
            {
                timer = plantData.timeBtwStages;
                plantState++;
                UpdatePlant();
                
            }
        }
    }

    private void OnMouseDown()
    {
        itemSelected = inventory.GetSelectedItem();
        if(itemSelected.GetItemData() is Plant)
        {
            plantData = itemSelected.GetItemData() as Plant;
            _farmer.SetAction(action);
        }
    }

    void Harvest() {
        if(plantState == plantData.planetStateSprites.Length -1 )
        {
            plant.gameObject.SetActive(false);
            isPlanted = false;
        }
        
    }

    void Plant()
    {
        isPlanted =true;
        itemSelected.Subtract(1);
        plantState = 0;
        UpdatePlant();
        timer = plantData.timeBtwStages;
        plant.gameObject.SetActive(true);
    }

    private void UpdatePlant()
    {
        plant.sprite = plantData.planetStateSprites[plantState];
    }

    
}
