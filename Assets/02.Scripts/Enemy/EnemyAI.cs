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
    private Animator animator;
    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    public bool isDie = false;
    private WaitForSeconds ws;
    private MoveAgent moveAgent;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        
        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();

        ws = new WaitForSeconds(0.3f);


    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
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

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;
                case State.DIE:
                    moveAgent.Stop();
                    break;

 
            }

        }

    }

    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

}
