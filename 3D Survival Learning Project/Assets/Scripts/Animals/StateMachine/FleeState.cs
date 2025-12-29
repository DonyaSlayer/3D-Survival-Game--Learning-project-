using UnityEngine;

public class FleeState : State
{
    public FleeState (AnimalController animal, StateMachine stateMachine) : base (animal, stateMachine) { }

    public override void Enter()
    {
        _animalController.agent.speed = _animalController.runSpeed;
        _animalController.agent.ResetPath();
    }

    public override void LogicUpdate()
    {
        //Vector from Player
        Vector3 directionToPlayer = _animalController.transform.position - _animalController.playerTransform.position;
        //Target point
        Vector3 runPosition = _animalController.transform.position + directionToPlayer.normalized * 5f;
        //Start of runiing
        _animalController.agent.SetDestination(runPosition);
        // Condition for returning to idle state
        if (Vector3.Distance(_animalController.transform.position, _animalController.playerTransform.position) > _animalController.detectionRadius + 5f)
        {
            _stateMachine.ChangeState(_animalController.WanderState);
        }
    }

    public override void Exit()
    {
        _animalController.agent.ResetPath();
        _animalController.agent.speed = _animalController.maxSpeed;
    }
}
