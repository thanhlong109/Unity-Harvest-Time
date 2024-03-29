using Assets.Scripts.DataService;
using Assets.Scripts.Plot;
using System;
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
    [SerializeField] private bool isBought;
    [SerializeField] private float dryTime=0;


    [SerializeField]  private bool isDry = true;
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

    public int instanceID;

    private PlotActionType currentActionType;

    private Inventory inventory;

    private static int HOE_ACTION = Animator.StringToHash("HoeAction");
    private static int WATERING_ACTION = Animator.StringToHash("WateringAction");

    private void Awake()
    {
        plotSpriteRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _farmer = FarmerAction.Instance;
        inventory = Inventory.Instance;
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
                       
                    }
                    _farmer.isActionAble = true;
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
                        default:
                            {
                                
                                _farmer.isActionAble = true;
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
                        
                    }
                    _farmer.isActionAble = true;
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
        _farmer.isActionAble = true;
        AudioManager.Instance.StopPlaySFX();
    }

    private void OnWateringCanActionDone()
    {
        toolSelected.OnActionCompleted -= OnWateringCanActionDone;
        handAnimator.SetBool(WATERING_ACTION, false);
        if (isDry)
        {
            isDry = false;
            dryTime = 0;
            growSpeed += 0.5f;
            StartCoroutine(CountToNextDry());
            UpdatePlotUI();
        }
        _farmer.isActionAble = true;
        AudioManager.Instance.StopPlaySFX();
    }

    public IEnumerator CountToNextDry()
    {
        yield return new WaitForSeconds(timeToDry);
        isDry = true;
        growSpeed = 1f;
        UpdatePlotUI();

    }

    void Update()
    {
        if (isPlanted && !isPlantDie)
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
        if (isDry)
        {
            dryTime += Time.deltaTime;
            if (dryTime > plantData.liveInDryTime)
            {
                isPlantDie = true;
                UpdatePlant() ;
            }
        }
    }

    public void BeingMouseClicked()
    {
        itemSelected = inventory.GetSelectedItem();
        _farmerOffsetTarget = farmerOffsetTarget;
        
        if (itemSelected != null)
        {
            var itemSelectedData = itemSelected.GetItemData();
         
            if (isPlanted && plantState == plantData.planetStateSprites.Length - 1||isPlantDie)
            {
                currentActionType = PlotActionType.HAVEST;
            }else
            if (itemSelectedData is Plant && !isPlanted)
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
            else
            {
                return;
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
        if (plantState == plantData.planetStateSprites.Length - 1||isPlantDie)
        {
            ResetPlot();
            UpdatePlotUI();
            if (!isPlantDie)
            {
                inventory.AddItem(plantData.Harvest());
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
        dryTime = 0f;
    }

    void Plant()
    {
        AudioManager.Instance.PlaySFX("PlantingSfx");
        plantData.timeBtwStages = plantData.timeToHarvest / plantData.planetStateSprites.Length;
        isPlanted = true;
        itemSelected.Subtract(1);
        plantState = 0;
        UpdatePlotUI();
        UpdatePlant();
        timer = timeBtwStageWithGrowRate;
        
    }

    private void UpdatePlotUI()
    {
        gameObject.SetActive(isBought);
        plant.gameObject.SetActive(isPlanted);
        
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
        plant.color = isPlantDie ? Color.black : Color.white;
       /* var randomValue = Random.Range(0, 100);
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
        */
    }

    private void CalTimeGrowWithGrowRate()
    {
        timeBtwStageWithGrowRate = (plantData.timeToHarvest - ((growSpeed - 1) * plantData.timeToHarvest)) / plantData.planetStateSprites.Length;
    }
    

    public PlotState GetPlotSate()
    {
   
        return new PlotState()
        {
            growSpeed = growSpeed,
            isPlanted = isPlanted,
            isTilled = isTilled,
            plantName = plantData.Name,
            plantState = plantState,
            timeBtwStageWithGrowRate = timeBtwStageWithGrowRate,
            plotId = instanceID,
            isPlantDie = isPlantDie,
            isDry = isDry,
            isBought = isBought,
        };
        

    }

    public void SetIsBought()
    {
        isBought = true;
        gameObject.SetActive(true);
    }

    public void LoadPlotState(PlotState plotState)
    {
        growSpeed = plotState.growSpeed;
        isPlanted = plotState.isPlanted;
        isTilled= plotState.isTilled;
        isDry = plotState.isDry;
        isBought= plotState.isBought;
        gameObject.SetActive(isBought);
        if (isPlanted)
        {
            plantData = (Plant)Inventory.Instance.GetItemByName(plotState.plantName);
            plantState = plotState.plantState;
            isPlantDie = plotState.isPlantDie;
            timeBtwStageWithGrowRate = plotState.timeBtwStageWithGrowRate;
        }
        if(!isDry)
        {
            StartCoroutine(CountToNextDry());
        }
        UpdatePlotUI();
        UpdatePlant();
    }
}
