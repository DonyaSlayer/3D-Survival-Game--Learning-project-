using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AnimalController : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float _wanderRadius = 25f;
    [SerializeField] private float _minIdleTime = 2f;
    [SerializeField] private float _maxIdleTime = 5f;
    [SerializeField] private float _maxSpeed;


    private Animator _animator;

    private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private bool _isWaiting;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>(); 
        _spawnPoint = transform.position;
        StartCoroutine(WanderRoutine());
    }

    private void Update()
    {
        float currentSpeed = _agent.velocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(currentSpeed / _maxSpeed);
        _animator.SetFloat("State", normalizedSpeed);
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (!_agent.pathPending && !_agent.hasPath && !_isWaiting)
            {
                _isWaiting = true;
                float waitTime = Random.Range(_minIdleTime, _maxIdleTime);
                yield return new WaitForSeconds(waitTime);
                Vector3 destination = GetRandomPointInRadius(_spawnPoint, _wanderRadius);
                _agent.SetDestination(destination);
                _isWaiting = false;
            }
            yield return null;
        }
    }

    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        for(int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;
            randomDirection.y = center.y;
            if(NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return center;
    }
}
