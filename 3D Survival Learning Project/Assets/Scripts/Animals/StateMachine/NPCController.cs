using UnityEngine;
using UnityEngine.AI;

public abstract class NPCController : MonoBehaviour 
{
    [Header("General Stats")]
    public float maxSpeed;
    public float runSpeed;

    [Header("Wander Stats")]
    public float wanderRadius = 25f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;


    [Header("Direction")]
    public float detectionRadius;
    public Transform playerTransform;


    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Vector3 spawnPoint;

    public StateMachine StateMachine { get; protected set; }
    public State WanderState { get; protected set; }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        StateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        if (StateMachine.CurrentState != null)
            StateMachine.CurrentState.LogicUpdate();
        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat("State", Mathf.Clamp01(currentSpeed / runSpeed));
        Debug.Log(currentSpeed);
    }
}
