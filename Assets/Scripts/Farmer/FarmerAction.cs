using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

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
   
    
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        Flip();
        UpdateAnimation();
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


    void UpdateAnimation()
    {
        _animator.SetBool(MOVE_AMIN, Mathf.Abs(_agent.velocity.x)>0.1f);
    }

    void Flip()
    {
        if (_agent.remainingDistance>0 && _agent.velocity.x * _currentDirection < 0)
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

}
