using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;


public class AnimalController : MonoBehaviour
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
    public LayerMask predatorLayer;

    
    [HideInInspector]public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Vector3 spawnPoint;
    
    public StateMachine StateMachine { get; private set; }
    public WanderState WanderState { get; private set; }
    public FleeState FleeState { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;

        //initialization of Mchine and States

        StateMachine = new StateMachine();
        WanderState = new WanderState(this, StateMachine);
        FleeState = new FleeState(this, StateMachine);
    }
    private void Start()
    {
        StateMachine.Initialize(WanderState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat("State", Mathf.Clamp01(currentSpeed / maxSpeed));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
