using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerAction : MonoBehaviour
{
    //Movement fields
    private Vector3? _posTarget = null;
    private NavMeshAgent _agent;
    private float _currentDirection = 1;
    
    //Animation
    private Animator _animator;
    private static int MOVE_AMIN = Animator.StringToHash("Move");

    //Actions
    private Action _action = null;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
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
        if(_action != null)
        {
            _posTarget = _action.targetPos;
            if(Vector2.Distance(transform.position,_action.targetPos) < _action.radiusActionPerform)
            {
                _posTarget = transform.position;
                _action.PerformAction();
                _action = null;
            }
        }
        
    }

    void UpdateAnimation()
    {
        _animator.SetBool(MOVE_AMIN, Mathf.Abs(_agent.velocity.x)>0.01f);
    }

    void Flip()
    {
        if (_agent.velocity.x * _currentDirection < 0)
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
