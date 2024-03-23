using Assets.Scripts.DataService;
using Assets.Scripts.Plot;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    
    public static PlotManager Instance;
    private List<Plot> _plots;
    private List<Plot> plotActive;
    private List<Plot> plotUnActive;
    [SerializeField] private PlotItem plotData;

    private JsonService jsonService;
    private static string SavePath = "/PlotData";

    [Header("Price")]
    [SerializeField] private int startPlotPrice;
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
    }

    private void Start()
    {
        jsonService = new JsonService();
        try
        {
            _plots = new List<Plot>();
            _plots = GetComponentsInChildren<Plot>(true).ToList();
            if (ScreenPara.Instance.isContinue)
            {
                var data = jsonService.LoadData<SavedData>(SavePath, false);
                foreach (var item in _plots)
                {
                    PlotState plotStateData;
                    if (data.PlotStates.TryGetValue(item.instanceID, out plotStateData))
                    {
                        item.LoadPlotState(plotStateData);
                    }
                }
                _plots = GetComponentsInChildren<Plot>(true).ToList();
            }
            FetchPlot();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }

    public void FetchPlot()
    {
       
        plotActive = _plots.Where(p => p.isActiveAndEnabled).ToList();
        plotUnActive = _plots.Where(p => !p.isActiveAndEnabled).ToList();
        plotData.buyPrice = startPlotPrice * plotActive.Count;
    }

    private void SavePlotState()
    {
        
        var data = new Dictionary<int, PlotState>();
        foreach (var item in plotActive)
        {
            data.TryAdd(item.instanceID, item.GetPlotSate());
        }
        SavedData savedData = new SavedData()
        {
            PlotStates = data,
        };

        jsonService.SaveData(SavePath, savedData, false);
    }

    private void OnApplicationQuit()
    {
        SavePlotState();
    }



    public void OpenNewPlot()
    {
        if(plotUnActive.Count > 0)
        {
            var plot = plotUnActive[0];
            plot.SetIsBought();
            plotUnActive.Remove(plot);
            plotActive.Add(plot);
            plotData.buyPrice = startPlotPrice * plotActive.Count;
        }
    }

}

public class SavedData
{
    public Dictionary<int, PlotState> PlotStates;
}
