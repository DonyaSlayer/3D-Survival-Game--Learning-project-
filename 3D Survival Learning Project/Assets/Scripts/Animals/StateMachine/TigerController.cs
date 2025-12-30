using UnityEngine;
using UnityEngine.AI;

public class TigerController : NPCController
{
    [Header("Tiger Specifics")]
    public LayerMask preyLayer;
    [HideInInspector] public Transform currentTarget;

    public State SeekState {  get; private set; }
    protected override void Awake()
    {
        base.Awake();
        WanderState = new TigerWanderState(this, StateMachine);
        SeekState = new SeekState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(WanderState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
