using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum PlotActionType
{
    PLANT, HAVEST, TOOLS_ACTION
}

public class Plot : MonoBehaviour
{

    [SerializeField] private Vector2 farmerOffsetTarget = Vector2.zero;

    // plant 
    [SerializeField] private bool isPlanted = false;
    [SerializeField] private SpriteRenderer plant;
    [SerializeField] private Plant plantData;
    [SerializeField] private int plantState = 0;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private float timeToDry = 300f;
    [SerializeField] private Sprite drySprite;
    [SerializeField] private Sprite wateredSprite;

    private bool isDry = true;
    private int rateToDie = 0;
    private float timer;


    private SpriteRenderer plotSpriteRender;
    private FarmerAction _farmer;
    private Action action;
    private InventoryItem itemSelected;
    private Tools toolSelected;

    private PlotActionType currentActionType;

    private Inventory inventory;

    private static int HOE_ACTION = Animator.StringToHash("HoeAction");
    private static int WATERING_ACTION = Animator.StringToHash("WateringAction");

    private void Awake()
    {
        _farmer = FindObjectOfType<FarmerAction>();
        inventory = FindObjectOfType<Inventory>();
        plotSpriteRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        handAnimator = _farmer.handAnimator;
        action = new Action()
        {
            radiusActionPerform = 0.1f,
            targetPos = new Vector3(transform.position.x - farmerOffsetTarget.x, transform.position.y - farmerOffsetTarget.y, 0),
        };
        action.OnActionPerform += PerformAction;
    }

    private void PerformAction()
    {
        switch (currentActionType)
        {
            case PlotActionType.HAVEST:
                {
                    if (isPlanted)
                    {
                        Harvest();
                        _farmer.isDoingAction = false;
                    }
                    break;
                }
            case PlotActionType.TOOLS_ACTION:
                {
                    ResetToolsAnimation();
                    switch (toolSelected.ToolType)
                    {
                        case ToolsType.HOE:
                            {
                                handAnimator.SetBool(HOE_ACTION, true);
                                toolSelected.OnActionCompleted += OnHoeActionDone;
                                StartCoroutine(toolSelected.WaitToActionDone());
                                break;
                            }
                        case ToolsType.WATERING_CAN:
                            {
                                handAnimator.SetBool(WATERING_ACTION, true);
                                toolSelected.OnActionCompleted += OnWateringCanActionDone;
                                StartCoroutine(toolSelected.WaitToActionDone());
                                break;
                            }
                    }
                    break;
                }
            case PlotActionType.PLANT:
                {
                    if (plantData.Quantity > 0 && !isPlanted)
                    {
                        Plant();
                        _farmer.isDoingAction = false;
                    }
                    break;
                }

        }

    }

    private void ResetToolsAnimation()
    {
        handAnimator.SetBool(HOE_ACTION, false);
        handAnimator.SetBool(WATERING_ACTION, false);
    }

    private void OnHoeActionDone()
    {
        Debug.Log("Hoe done!");
        toolSelected.OnActionCompleted -= OnHoeActionDone;
        handAnimator.SetBool(HOE_ACTION, false);
        _farmer.isDoingAction = false;
    }

    private void OnWateringCanActionDone()
    {
        toolSelected.OnActionCompleted -= OnWateringCanActionDone;
        handAnimator.SetBool(WATERING_ACTION, false);
        if (isDry)
        {
            isDry = false;
            StartCoroutine(CountToNextDry());
            UpdatePlant();
        }
        _farmer.isDoingAction = false;
    }

    public IEnumerator CountToNextDry()
    {
        yield return new WaitForSeconds(timeToDry);
        isDry = true;
        UpdatePlant();

    }

    void Update()
    {
        if (isPlanted)
        {
            Grow();
        }


    }

    void Grow()
    {

        timer -= Time.deltaTime;
        if (timer < 0 && plantState < plantData.planetStateSprites.Length - 1)
        {
            timer = plantData.timeBtwStages;
            plantState++;
            UpdatePlant();
        }
    }

    private void OnMouseDown()
    {
        itemSelected = inventory.GetSelectedItem();
        if (itemSelected != null)
        {
            var itemSelectedData = itemSelected.GetItemData();
            if (itemSelectedData is Plant)
            {
                plantData = itemSelected.GetItemData() as Plant;
                currentActionType = PlotActionType.PLANT;
            }
            else if (itemSelectedData is Tools)
            {
                currentActionType = PlotActionType.TOOLS_ACTION;
                toolSelected = itemSelectedData as Tools;
            }
        }
        else
        {
            if (isPlanted)
            {
                currentActionType = PlotActionType.HAVEST;
            }
        }

        _farmer.SetAction(action);
    }



    void Harvest()
    {
        if (plantState == plantData.planetStateSprites.Length - 1)
        {
            plant.gameObject.SetActive(false);
            isPlanted = false;
            CountableItem harvestItem =ScriptableObject.CreateInstance<CountableItem>();
                harvestItem.Icon = plantData.planetStateSprites[plantData.planetStateSprites.Length - 1];
                harvestItem.Name = plantData.harvestedName;
                harvestItem.Quantity = 4;

            inventory.AddItem(harvestItem);
        }

    }

    void Plant()
    {
        plantData.timeBtwStages = plantData.timeToHarvest / plantData.planetStateSprites.Length;
        isPlanted = true;
        itemSelected.Subtract(1);
        plantState = 0;
        UpdatePlant();
        timer = plantData.timeBtwStages;
        plant.gameObject.SetActive(true);
    }

    private void UpdatePlant()
    {
        plant.sprite = plantData.planetStateSprites[plantState];
        plotSpriteRender.sprite = isDry ? drySprite : wateredSprite;
    }


}
