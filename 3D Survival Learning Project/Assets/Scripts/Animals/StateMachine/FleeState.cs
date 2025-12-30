using UnityEngine;
using UnityEngine.AI;

public class FleeState : State
{
    public FleeState (NPCController controller, StateMachine stateMachine) : base (controller, stateMachine) { }

    public override void Enter()
    {
        _controller.agent.speed = _controller.runSpeed;
        _controller.agent.ResetPath();
    }

    public override void LogicUpdate()
    {
        //Vector from Player
        Vector3 directionToPlayer = _controller.transform.position - _controller.playerTransform.position;
        //Target point
        Vector3 runPosition = _controller.transform.position + directionToPlayer.normalized * 5f;
        //Start of runiing
        _controller.agent.SetDestination(runPosition);
        // Condition for returning to idle state
        if (Vector3.Distance(_controller.transform.position, _controller.playerTransform.position) > _controller.detectionRadius + 5f)
        {
            _stateMachine.ChangeState(_controller.WanderState);
        }
    }

    public override void Exit()
    {
        _controller.agent.ResetPath();
        _controller.agent.speed = _controller.maxSpeed;
    }
}
