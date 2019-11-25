using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    // Use this for initialization
    public enum State {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;
    private Transform playerTr;
    private Transform enemyTr;
    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    public bool isDie = false;
    private WaitForSeconds ws;

    void Awake()
    {
        var player = GameObject.FindGameObjectsWithTag("PLAYER");

        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();

        ws = new WaitForSeconds(0.3f);


    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        { if (state == State.DIE) yield break;
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist)
            { state = State.ATTACK; }

            else if (dist <= traceDist)
            { state = State.TRACE;  }

            else
            { state = State.PATROL; }

            yield return ws;
        }
    }

}
