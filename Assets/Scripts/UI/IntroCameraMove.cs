using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IntroCameraMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float radius = 5f;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector3 target;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        RandomTarget();
    }

    private void Update()
    {
        if (Vector2.Distance(agent.gameObject.transform.position, target) < 0.5f)
        {
            RandomTarget();
        }
        
    }

    private void RandomTarget()
    {

        Vector2 randomPoint = Random.insideUnitCircle * radius;
        target = new Vector3(randomPoint.x + (randomPoint.x < 0 ? -min.x:min.x), randomPoint.y + (randomPoint.x < 0 ? -min.y : min.y), 0);
        agent.SetDestination(target);
    }
}
