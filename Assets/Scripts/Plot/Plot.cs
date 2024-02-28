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
    private Vector2 _farmerOffsetTarget;
    // plant 
    [SerializeField] private bool isPlanted = false;
    [SerializeField] private SpriteRenderer plant;
    [SerializeField] private Plant plantData;
    [SerializeField] private int plantState = 0;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private float timeToDry = 300f;
    [SerializeField] private Sprite drySprite;
    [SerializeField] private Sprite wateredSprite;
    [SerializeField] private Sprite dryTilledSprite;
    [SerializeField] private Sprite wateredTilledSprite;
    

    private bool isDry = true;
    private int rateToDie = 0;
    private bool isPlantDie = false;
    private float timer;
    private bool isTilled = false;
    private float growSpeed = 1f;
    [SerializeField] private float timeBtwStageWithGrowRate;
    private float tempGrowSpeed = 0f;

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
            targetPos = new Vector3(transform.position.x - _farmerOffsetTarget.x, transform.position.y - _farmerOffsetTarget.y, 0),
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
                                if (!isTilled)
                                {
                                    handAnimator.SetBool(HOE_ACTION, true);
                                    toolSelected.OnActionCompleted += OnHoeActionDone;
                                    StartCoroutine(toolSelected.WaitToActionDone());
                                }
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
                    AudioManager.Instance.StartPlaySFX(toolSelected.sfxName);
                    break;
                }
            case PlotActionType.PLANT:
                {
                    if (plantData.Quantity > 0 && !isPlanted)
                    {
                        if (isTilled)
                        {
                            Plant();
                           
                        }
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
        toolSelected.OnActionCompleted -= OnHoeActionDone;
        handAnimator.SetBool(HOE_ACTION, false);
        isTilled = true;
        UpdatePlotUI();
        _farmer.isDoingAction = false;
        AudioManager.Instance.StopPlaySFX();
    }

    private void OnWateringCanActionDone()
    {
        toolSelected.OnActionCompleted -= OnWateringCanActionDone;
        handAnimator.SetBool(WATERING_ACTION, false);
        if (isDry)
        {
            isDry = false;
            growSpeed += 0.5f;
            StartCoroutine(CountToNextDry());
            UpdatePlotUI();
        }
        _farmer.isDoingAction = false;
        AudioManager.Instance.StopPlaySFX();
    }

    public IEnumerator CountToNextDry()
    {
        yield return new WaitForSeconds(timeToDry);
        isDry = true;
        UpdatePlotUI();

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
            timer = timeBtwStageWithGrowRate;
            plantState++;
            UpdatePlant();
        }
    }

    private void OnMouseDown()
    {
        itemSelected = inventory.GetSelectedItem();
        _farmerOffsetTarget = farmerOffsetTarget;
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
                _farmerOffsetTarget = toolSelected.offset;
            }
        }
        else
        {
            if (isPlanted)
            {
                currentActionType = PlotActionType.HAVEST;
            }
        }
        action.targetPos = new Vector3(transform.position.x - _farmerOffsetTarget.x, transform.position.y - _farmerOffsetTarget.y, 0);
        _farmer.SetAction(action);
    }



    void Harvest()
    {
        if (plantState == plantData.planetStateSprites.Length - 1)
        {
            ResetPlot();
            UpdatePlotUI();
            if (!isPlantDie)
            {
                CountableItem harvestItem =ScriptableObject.CreateInstance<CountableItem>();
                harvestItem.Icon = plantData.planetStateSprites[plantData.planetStateSprites.Length - 1];
                harvestItem.Name = plantData.harvestedName;
                harvestItem.Quantity = 4;

                inventory.AddItem(harvestItem);
            }
           
        }

    }

    void ResetPlot()
    {
        plant.gameObject.SetActive(false);
        isPlanted = false;
        isTilled = false;
        growSpeed = 1f;
        tempGrowSpeed = 0f;
        rateToDie = 0;
        isPlantDie = false;
    }

    void Plant()
    {
        plantData.timeBtwStages = plantData.timeToHarvest / plantData.planetStateSprites.Length;
        isPlanted = true;
        itemSelected.Subtract(1);
        plantState = 0;
        UpdatePlotUI();
        timer = timeBtwStageWithGrowRate;
        plant.gameObject.SetActive(true);
    }

    private void UpdatePlotUI()
    {
        
        if (isTilled)
        {
            plotSpriteRender.sprite = isDry ? dryTilledSprite : wateredTilledSprite;
        }
        else
        {
            plotSpriteRender.sprite = isDry ? drySprite : wateredSprite;
        }
        if(tempGrowSpeed != growSpeed)
        {
            CalTimeGrowWithGrowRate();
        }
    }

    private void UpdatePlant()
    {
        plant.sprite = plantData.planetStateSprites[plantState];
        var randomValue = Random.Range(0, 100);
        if(randomValue < rateToDie)
        {
            isPlantDie = true;
        }

        if(isDry)
        {
            rateToDie += 15;
        }
        if (isPlantDie)
        {
            plant.sprite = plantData.plantDieSprite;
        }
        
    }

    private void CalTimeGrowWithGrowRate()
    {
        timeBtwStageWithGrowRate = (plantData.timeToHarvest - ((growSpeed - 1) * plantData.timeToHarvest)) / plantData.planetStateSprites.Length;
    }


}
