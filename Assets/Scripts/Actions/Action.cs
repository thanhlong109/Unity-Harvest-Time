using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public Vector3 targetPos;
    public float radiusActionPerform;
    public delegate void ActionPerform();
    public event ActionPerform OnActionPerform;
    public void PerformAction ()
    {
        OnActionPerform?.Invoke();
    }
}
