using Assets.Scripts.DataService;
using Assets.Scripts.Plot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManageData : MonoBehaviour
{
    public SaveData SaveData;

    public static ManageData instance;

    private JsonService jsonService;
    private static string SavePath = "/GeneralData";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        jsonService = new  JsonService();
        try
        {
            if (ScreenPara.Instance.isContinue)
            {
                Plot[] plotList = FindObjectsOfType<Plot>();
                var data = jsonService.LoadData<SavedData>(SavePath, false);
                foreach (var item in plotList)
                {
                    PlotState plotStateData;
                    if (data.PlotStates.TryGetValue(item.instanceID,out plotStateData))
                    {
                        item.LoadPlotState(plotStateData);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
    }

    private void SavePlotState()
    {
        Plot[] plotList = FindObjectsOfType<Plot>();
        var data = new Dictionary<int, PlotState>();
        foreach (var item in plotList)
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


}

public class SavedData {
    public Dictionary<int, PlotState> PlotStates;
}

