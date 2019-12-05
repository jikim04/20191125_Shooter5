using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour {
    public List<Transform> wayPoints;
    public int nexidx;
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;
    private NavMeshAgent agent;
    private Transform enemyTR;
    private bool _patrolling;
    

    public bool patrolling
    {
        get { return _patrolling; }
        set {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
                    
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set { _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);

        }

    }

    // Use this for initialization
    public float speed {
        get { return agent.velocity.magnitude; }
    }

    void Start () {

        enemyTR = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);

            nexidx = Random.Range(0, wayPoints.Count);
        }
        MoveWayPoint();
	}

    void MoveWayPoint()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nexidx].position;
        agent.isStopped = false;
            }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;

    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
	// Update is called once per frame
	void Update () {

        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTR.rotation = Quaternion.Slerp(enemyTR.rotation, rot, Time.deltaTime * damping);
        }
        if (!_patrolling) return;

        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            nexidx = ++nexidx % wayPoints.Count;
            nexidx = Random.Range(0, wayPoints.Count);
            MoveWayPoint();
        }
	}
}
