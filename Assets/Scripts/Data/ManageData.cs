using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageData : MonoBehaviour
{
    public SaveData SaveData;

    public static ManageData instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
