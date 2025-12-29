using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    private float _timer;
    private bool _isIdle;

    public WanderState(AnimalController animal, StateMachine stateMachine) : base(animal, stateMachine) { }

    public override void Enter()
    {
        _animalController.agent.speed = _animalController.maxSpeed;
        _isIdle = false;
        SetNewDestination();
    }
    public override void LogicUpdate()
    {
        //1. Checking for changing the state to Flee state
        if (Vector3.Distance(_animalController.transform.position, _animalController.playerTransform.position) < _animalController.detectionRadius)
        {
            _stateMachine.ChangeState(_animalController.FleeState);
            return;
        }
        //2. Wandering logic
        if(!_animalController.agent.pathPending && _animalController.agent.remainingDistance < 0.5f)
        {
            if(!_isIdle)
            {
                _isIdle=true;
                _timer = Random.Range(_animalController.minIdleTime, _animalController.maxIdleTime);
            }
        }

        if (_isIdle)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                SetNewDestination();
                _isIdle = false;
            }
        }
    }

    private void SetNewDestination()
    {
        Vector3 randomPoint = GetRandomPointInRadius(_animalController.spawnPoint, _animalController.wanderRadius);
        _animalController.agent.SetDestination(randomPoint);
    }

    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;
            randomDirection.y = center.y;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return center;
    }
}
