using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove;
using MonsterLove.StateMachine;
using UnityEngine.AI;

public class NPC2 : MonoBehaviour {


    StateMachine<NPCStates> fsm;

    [Header("Setup")]
    [SerializeField] public SpriteRenderer npcIndicator;
    [SerializeField] private float maxRigidbodySpeed = 2f;

    [Header("Idle")]
    [SerializeField] private float minIdleTime = 0.25f;
    [SerializeField] private float maxIdleTime = 2f;

    [Header("Walk")]
    [SerializeField] private float maxDistance = 5f;


    private float timeToWalk;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 direction;
    private Vector3 lastFramePosition;
    private NavMeshAgent agent;
    private new Rigidbody rigidbody;
    private new Animator animator;
    public enum NPCStates
    {
        Init,
        Idle,
        Walk,
        Escape
    }

    
    public void Awake()
    {
        fsm = StateMachine<NPCStates>.Initialize(this);
        fsm.ChangeState(NPCStates.Idle);

        if (!npcIndicator)
            Debug.Log("None npc indicator, please manually assing this");
        else
        {
            npcIndicator.color = GetComponent<Infectable>().infectedBy ? GameManager.Instance.GetPlayerColor(GetComponent<Infectable>().infectedBy.playerIndex) : Color.grey;
        }

        initialPosition = transform.position;

        agent = this.GetComponent<NavMeshAgent>();
        rigidbody = this.GetComponent<Rigidbody>();
        animator = this.GetComponentInChildren<Animator>();
    }

    public void FixedUpdate()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * maxRigidbodySpeed;        
    }

    public void Update()
    {
        direction = transform.position - lastFramePosition;

        animator.SetFloat("Horizontal", direction.normalized.x, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", direction.normalized.z, 0.1f, Time.deltaTime);

        lastFramePosition = transform.position;
    }

    #region IdleState
    public void Idle_Enter()
    {
        timeToWalk = Time.time + Random.Range(minIdleTime, maxIdleTime);
    }

    public void Idle_Update()
    {
        if (Time.time >= timeToWalk)
            fsm.ChangeState(NPCStates.Walk);

    }
    #endregion

    #region WalkState
    public void Walk_Enter()
    {
        targetPosition = GetRandomWalkPoint();
        agent.destination = targetPosition;

    }

    public void Walk_Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
            fsm.ChangeState(NPCStates.Idle);


    }

    #endregion

    Vector3 GetRandomWalkPoint()
    {
        NavMeshHit hit;

        Vector3 randomPosition;
        do
        {
            randomPosition = initialPosition + UnityEngine.Random.insideUnitSphere * maxDistance;
        } while (!NavMesh.SamplePosition(randomPosition, out hit, maxDistance, 1));

        return hit.position;
    }
}
