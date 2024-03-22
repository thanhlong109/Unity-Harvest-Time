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
}
