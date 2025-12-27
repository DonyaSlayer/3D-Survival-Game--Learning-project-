using UnityEngine;

public class TreeFalling : MonoBehaviour
{
    private Resource resource;
    public float randomImpulse;

    [SerializeField] private Rigidbody _rb;

    private void Start()
    {
        resource = GetComponent<Resource>();
        resource.onGetResources += Fall;
    }

    public void Fall()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
        Vector3 direction = new Vector3(Random.Range(-randomImpulse, randomImpulse), 0, Random.Range(-randomImpulse, randomImpulse));
        _rb.AddForce(direction, ForceMode.Impulse);
        Destroy(gameObject, 5f);
    }

    private void OnDestroy()
    {
        resource.onGetResources -= Fall;
    }
}
