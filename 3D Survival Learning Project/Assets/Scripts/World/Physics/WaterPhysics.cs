using UnityEngine;
using UnityEngine.InputSystem;

public class WaterPhysics : MonoBehaviour
{
    [Header("Water Settings")]
    public float waterDrag = 3f;
    public float waterAngularDrag = 1f;
    public float buoyancyForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                player.isInWater = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")) return;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearDamping = waterDrag;
            if (other.transform.position.y < transform.position.y)
            {
                float depthFactor = Mathf.Clamp(transform.position.y - other.transform.position.y, 0f, 1f);
                Vector3 upForce = Vector3.up * buoyancyForce * depthFactor;
                rb.AddForce(upForce, ForceMode.Acceleration);
            } 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.isInWater = false;
        }
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && !other.CompareTag("Player"))
        {
            rb.linearDamping = 0.05f;
            rb.angularDamping = 0.05f;
        }
    }
}
