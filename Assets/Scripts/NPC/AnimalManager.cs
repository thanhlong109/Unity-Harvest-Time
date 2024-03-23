using Assets.Scripts.DataService;
using Assets.Scripts.NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;
    [Header("Animal Prefab")]
    [SerializeField] private GameObject CowPrefab;
    [SerializeField] private GameObject ChickenPrefab;
    [SerializeField] private GameObject SheepPrefab;

    private JsonService jsonService;
    private string SAVE_PATH = "/AnimalsData";

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
        if (ScreenPara.Instance.isContinue)
        {
            var animals = GetComponentsInChildren<Animal>();
            foreach (var item in animals)
            {
                Destroy(item.gameObject);
            }
            var data = jsonService.LoadData<AnimalSavedData>(SAVE_PATH, false);
            
            foreach (var item in data.animalStats)
            {
                GameObject newAnimal = null;
                switch (item.kind)
                {
                    case AnimalKind.COW:
                        {
                            newAnimal = Instantiate(CowPrefab);
                            break;
                        }
                    case AnimalKind.CKICKEN:
                        {
                            newAnimal = Instantiate(ChickenPrefab);
                            break;
                        }
                    case AnimalKind.SHEEP:
                        {
                            newAnimal = Instantiate(SheepPrefab);
                            break;
                        }
                }
                if (newAnimal != null)
                {
                   newAnimal.transform.SetParent(gameObject.transform, false);
                    newAnimal.GetComponent<Animal>()?.LoadStats(item);
                }

                
            }
           
        }
    }

    public void CreateAnimal(AnimalKind animalKind)
    {
        GameObject newAnimal = null;
        switch(animalKind)
        {
            case AnimalKind.COW:
                {
                    newAnimal = Instantiate(CowPrefab);
                    break;
                }
            case AnimalKind.CKICKEN:
                {
                    newAnimal = Instantiate(ChickenPrefab);
                    break;
                }
            case AnimalKind.SHEEP:
                {
                    newAnimal = Instantiate(SheepPrefab);
                    break;
                }
        }
        if(newAnimal == null)
        {
            return;
        }
        newAnimal.transform.SetParent(gameObject.transform, false);
        
    }

    private void OnApplicationQuit()
    {
        var animals = GetComponentsInChildren<Animal>();
        List<AnimaStats> animalStats = new List<AnimaStats>();
        foreach (var animal in animals)
        {
            animalStats.Add(animal.GetAnimalStats());
        }
        var saveData = new AnimalSavedData()
        {
            animalStats = animalStats
        };

        jsonService.SaveData<AnimalSavedData>(SAVE_PATH, saveData, false);
    }
}

public class AnimalSavedData
{
    public List<AnimaStats> animalStats {  get; set; }
}
