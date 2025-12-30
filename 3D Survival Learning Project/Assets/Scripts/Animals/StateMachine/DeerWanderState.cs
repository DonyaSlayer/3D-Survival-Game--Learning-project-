using UnityEngine;

public class DeerWanderState : WanderState
{
    private DeerController _deerController => _controller as DeerController;
    public DeerWanderState(DeerController controller, StateMachine stateMachine) : base (controller, stateMachine) { }

    protected override void CheckTransitions()
    {
        if(Vector3.Distance(_controller.transform.position, _controller.playerTransform.position) < _controller.detectionRadius)
        {
            _stateMachine.ChangeState(_deerController.FleeState);
        }
    }
}
