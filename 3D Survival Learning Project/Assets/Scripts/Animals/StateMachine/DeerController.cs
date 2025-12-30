using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;


public class DeerController : NPCController
{
    public FleeState FleeState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        WanderState = new DeerWanderState(this, StateMachine);
        FleeState = new FleeState(this, StateMachine);
    }
    private void Start()
    {
        StateMachine.Initialize(WanderState);
    }
}
