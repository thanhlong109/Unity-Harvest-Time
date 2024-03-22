using Assets.Scripts.DataService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FarmerAction : MonoBehaviour
{
    public static FarmerAction Instance;
    //Movement fields
    private Vector3? _posTarget = null;
    private Vector3 lastTarget = Vector3.zero;
    private NavMeshAgent _agent;
    private float _currentDirection = 1;
    
    //Animation
    private Animator _animator;
    private static int MOVE_AMIN = Animator.StringToHash("Move");

    //Actions
    private Action _action = null;
    public Animator handAnimator;
    public bool isActionAble = true;
    public static string SAVE_FILE_NAME = "/FarmerAction";
    private JsonService _jsonService;

    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        AudioManager.Instance.PlayMusic("bgmScreenMain");
        _jsonService = new JsonService();
        if (ScreenPara.Instance.isContinue)
        {
            var savedData = _jsonService.LoadData<FamerActionSavedData>(SAVE_FILE_NAME, false);
            gameObject.transform.position = savedData.FamerPosition;
        }


    }


    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isActionAble)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            _posTarget = mouseWorldPosition;
        }
        if (_posTarget != null)
        {
            _agent.SetDestination(_posTarget.Value);
            _posTarget = null;
        }
        
        
        if(_action != null && isActionAble)
        {

            _posTarget = _action.targetPos;
            lastTarget = _action.targetPos;
            if(Vector2.Distance(transform.position,_action.targetPos) < _action.radiusActionPerform)
            {
                isActionAble=false;
                
                _posTarget = transform.position;
                _action.PerformAction();
                _action = null;
            }
        }
        
    }

    private void FixedUpdate()
    {
        Flip();
        UpdateAnimation();
    }


    void UpdateAnimation()
    {
        _animator.SetBool(MOVE_AMIN, Mathf.Abs(_agent.velocity.x)>0.1f);
    }

    void Flip()
    {
        if (_agent.remainingDistance>0 && Mathf.Abs(_agent.velocity.x) > 0.1f && _agent.velocity.x * _currentDirection < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _currentDirection *= -1;
            
        }else if(_agent.remainingDistance == 0 && lastTarget.x * _currentDirection > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _currentDirection *= -1;
        }

    }


    public void SetAction(Action action)
    {
            _action = action;
    }

    private void OnApplicationQuit()
    {
        FamerActionSavedData data = new FamerActionSavedData()
        {
            FamerPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z),
        };
        _jsonService.SaveData(SAVE_FILE_NAME, data,false);
    }

}

public class FamerActionSavedData
{
    public Vector3 FamerPosition { get; set; } 

}
