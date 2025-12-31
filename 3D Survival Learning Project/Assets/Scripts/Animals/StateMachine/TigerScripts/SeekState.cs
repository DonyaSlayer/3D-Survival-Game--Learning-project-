using UnityEngine;

public class SeekState : State
{
    private TigerController _tigerController => _controller as TigerController;
    public SeekState (TigerController tiger, StateMachine stateMachine) : base(tiger, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("TIGER: Entering SEEK State. Target: " + (_tigerController.currentTarget ? _tigerController.currentTarget.name : "NULL"));
        _controller.agent.speed = _controller.runSpeed;
        _controller.agent.ResetPath();
    }

    public override void LogicUpdate()
    {
        if (_tigerController.currentTarget == null)
        {
            _stateMachine.ChangeState(_controller.WanderState);
            return;
        }

        _controller.agent.SetDestination(_tigerController.currentTarget.position);
        float distance = Vector3.Distance(_controller.transform.position, _tigerController.currentTarget.position);

        if (distance > _controller.detectionRadius + 5f)
        {
            _tigerController.currentTarget = null;
            _stateMachine.ChangeState(_controller.WanderState);
        }
    }

    public override void Exit()
    {
        _controller.agent.ResetPath();
        _controller.agent.speed = _controller.maxSpeed;
    }
}
