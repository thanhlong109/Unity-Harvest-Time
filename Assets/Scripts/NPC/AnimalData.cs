using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum AnimalType
{
    CHILD,
    ADULT
}


[CreateAssetMenu(fileName ="New Animal",menuName ="NPC/Animal")]
public class AnimalData : ScriptableObject
{
    public AnimalType type;
    public float timeToGrow = 20f;
    public float timeToHungry = 20f;
    public int health = 100;
    public int hungryAmount = 0;
    public int amountHealthDecreaseWhenHungry;


    [Header("Sound")]
    public string hungrySoundSFXName;
    public string idleSoundSFXName;
    public string feedSoundSFXName;

    [Header("Wander")]
    public float walkSpeed = 5f;
    public float maxWalkTime = 6f;

    [Header("Idle")]
    public float idleTime = 5f;
}
