using UnityEngine;

public class TigerWanderState : WanderState
{
    private TigerController _tigerController => _controller as TigerController;
    public TigerWanderState (NPCController controller, StateMachine stateMachine) : base (controller, stateMachine) { }

    protected override void CheckTransitions()
    {
        Collider[] hits = Physics.OverlapSphere(_controller.transform.position, _controller.detectionRadius, _tigerController.preyLayer);
        if (hits.Length > 0)
        {
            _tigerController.currentTarget = hits[0].transform;
            _stateMachine.ChangeState(_tigerController.SeekState);
        }
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("TIGER: Entering Wander State");
    }
}
