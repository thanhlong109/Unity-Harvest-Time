using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAction : MonoBehaviour
{
    protected Action action;
    [SerializeField]protected Vector2 _farmerOffsetTarget = Vector2.one;

    private void Awake()
    {
        
    }

    void Start()
    {
        action = new Action()
        {
            radiusActionPerform = 0.1f,
            targetPos = new Vector3(transform.position.x - _farmerOffsetTarget.x, transform.position.y - _farmerOffsetTarget.y, 0),
        };
        action.OnActionPerform += PerformAction;
    }

    protected virtual void PerformAction()
    {
        
    }


    

    private void OnMouseDown()
    {
        FarmerAction.Instance.SetAction(action);
    }
}
