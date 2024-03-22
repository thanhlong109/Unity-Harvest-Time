using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    IDLE,
    MOVING
}

public enum AnimalStatus
{
    GOOD,
    BAD,
    HUNGRY
}


[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{

    public AnimalData animalData;
    private SpriteRenderer SpriteRenderer;
    private bool isAdult;
    private bool isDeath;
    private int health;
    private int hungryAmount;

    private NavMeshAgent agent;
    private float countTime;
    private AnimalState currentState;
    private static int MOVE_AMIN = Animator.StringToHash("Move");
    private Animator animator;
    private int _currentDirection = 1;
    

    [Header("Status")]
    [SerializeField] private Sprite goodStatus;
    [SerializeField] private Sprite hungryStatus;
    [SerializeField] private Sprite badStatus;
    [SerializeField] private SpriteRenderer statusRender;
    [SerializeField] private float maxTimeDisplayStatus;
    [SerializeField] private ScriptableObject feedName;
    [SerializeField] private float wanderRadius = 17.11f;
    [SerializeField] private Transform milestone;

    private bool isDisplayingStatus;
    private Action action = new Action();
    [SerializeField] protected Vector2 _farmerOffsetTarget = Vector2.one;


    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
       animator = GetComponent<Animator>();
    }

    private void InitializeAnimal()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = animalData.walkSpeed;
        action.radiusActionPerform = 2f;
        action.OnActionPerform += PerformAction;
        isAdult = animalData.type == AnimalType.ADULT;
        health = animalData.health;
        countTime = animalData.timeToHungry;
        currentState = AnimalState.IDLE;
        UpdateState();
    }

    private void PerformAction()
    {
        
        Feed();
        FarmerAction.Instance.isActionAble = true;
    }

    private void Start()
    {
        InitializeAnimal();
        UpdateUI();
    }

    private void Update()
    {
        if (!isDeath)
        {
            CheckStats();
            countTime -= Time.deltaTime;
            if (countTime <= 0)
            {

                Hungry();
                countTime = animalData.timeToHungry;
            }
            Flip();
            animator.SetBool(MOVE_AMIN, Mathf.Abs(agent.velocity.x) > 0);
            action.targetPos = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    private void CheckStats()
    {
        if (!isAdult)
        {
            StartCoroutine(CountTimeToGrow());
        }
    }

    private IEnumerator CountTimeToGrow()
    {
        yield return new WaitForSeconds(animalData.timeToGrow);
        isAdult = true;
        UpdateUI();
    }

    private void Hungry()
    {

        hungryAmount = hungryAmount + animalData.amountHealthDecreaseWhenHungry;
        health -= animalData.amountHealthDecreaseWhenHungry;
        if(health < 30)
        {
            DisplayStatus(AnimalStatus.BAD);
        }
        if(hungryAmount > 10)
        {
            DisplayStatus(AnimalStatus.HUNGRY);
        }
        if (health <= 0)
        {
            isDeath = true;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        
    }

    public void Feed()
    {
        DisplayStatus(AnimalStatus.GOOD);
        AudioManager.Instance.PlaySFXRandomPitch(animalData.feedSoundSFXName, 0.8f,1.2f);
        health = animalData.health;
        if(hungryAmount > 20 && feedName is IInventoryItem item)
        {

            Inventory.Instance.RemoveItem(item, 1);
        }
        UpdateUI();
    }

    private void DisplayStatus(AnimalStatus status)
    {
        if (!isDisplayingStatus)
        {
            
            switch (status)
            {
                case AnimalStatus.GOOD:
                    {
                        statusRender.sprite = goodStatus;

                        break;
                    }
                case AnimalStatus.BAD:
                    {
                        statusRender.sprite = badStatus;
                        break;
                    }
                case AnimalStatus.HUNGRY:
                    {
                        statusRender.sprite = hungryStatus;
                        break;
                    }

            }
            statusRender.gameObject.SetActive(true);
            StartCoroutine(WaitToHindStatus());
        }
    }


    private IEnumerator WaitToHindStatus()
    {
        var time = Random.Range(maxTimeDisplayStatus / 2f, maxTimeDisplayStatus);
        yield return new WaitForSeconds(time);
        isDisplayingStatus = false;
        statusRender.gameObject.SetActive(false);
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case AnimalState.IDLE:
                {
                    HandleIdleState();
                    break;
                }
            case AnimalState.MOVING:
                {
                    HandleMovingState();
                    break;
                }
        }
    }

    private Vector3 GetRandomPosition(Vector3 origin, float distance)
    {
        Vector2 random = Random.insideUnitCircle * distance;
        Vector3 pos = new Vector3(random.x, random.y, 0) + origin;
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(pos, out navMeshHit, distance, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            return GetRandomPosition(origin, distance);
        }
    }

    private void HandleIdleState()
    {
        StartCoroutine(WaitToMove());
    }

    private void HandleMovingState()
    {
        StartCoroutine(WaitToReachDestination());
    }


    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;
        while (agent.remainingDistance > agent.stoppingDistance)
        {
            if (Time.time - startTime > animalData.maxWalkTime)
            {
                agent.ResetPath();
                SetState(AnimalState.IDLE);
                yield break;
            }
            yield return null;
        }
        SetState(AnimalState.IDLE);
    }

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(animalData.idleTime / 2, animalData.idleTime);
        yield return new WaitForSeconds(waitTime);
        Vector3 randomDestination = GetRandomPosition(milestone.position, wanderRadius);
        agent.SetDestination(randomDestination);
        SetState(AnimalState.MOVING);
    }

    private void SetState(AnimalState newState)
    {
        if (currentState == newState)
        {
            return;
        }
        currentState = newState;
        OnStateChanged(newState);
    }

    void Flip()
    {
        if (agent.velocity.x * _currentDirection < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _currentDirection *= -1;

        }

    }


    private void OnStateChanged(AnimalState newState)
    {
        UpdateState();
    }

    private void OnMouseDown()
    {
        FarmerAction.Instance.SetAction(action);
    }
}
