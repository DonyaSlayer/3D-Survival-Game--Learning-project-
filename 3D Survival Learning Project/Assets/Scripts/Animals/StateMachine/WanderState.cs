using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    private float _timer;
    private bool _isIdle;
    public WanderState(NPCController controller, StateMachine stateMachine) : base(controller, stateMachine) { }

    public override void Enter()
    {
        _controller.agent.speed = _controller.maxSpeed;
        _isIdle = false;
        SetNewDestination();
    }
    public override void LogicUpdate()
    {
        CheckTransitions();
        
        //2. Wandering logic
        if(!_controller.agent.pathPending && _controller.agent.remainingDistance < 0.5f)
        {
            if(!_isIdle)
            {
                _isIdle=true;
                _timer = Random.Range(_controller.minIdleTime, _controller.maxIdleTime);
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

    protected virtual void CheckTransitions()
    {

    }

    private void SetNewDestination()
    {
        Vector3 randomPoint = GetRandomPointInRadius(_controller.spawnPoint, _controller.wanderRadius);
        _controller.agent.SetDestination(randomPoint);
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
