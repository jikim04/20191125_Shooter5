using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour {
    public List<Transform> wayPoints;
    public int nexidx;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
        }
        MoveWayPoint();
	}

    void MoveWayPoint()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nexidx].position;
        agent.isStopped = false;
            }

	// Update is called once per frame
	void Update () {
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            nexidx = ++nexidx % wayPoints.Count;
            MoveWayPoint();
        }
	}
}
