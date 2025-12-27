using UnityEngine;

public class ParicleOfResource : MonoBehaviour
{
    private Resource resource;
    [SerializeField] private ParticleSystem _particle;

    private void Start()
    {
        resource = GetComponent<Resource>();
        resource.onGetResources += ParticlePlayed;
    }

    public void ParticlePlayed()
    {
        _particle.Play();
    }

    private void OnDestroy()
    {
        resource.onGetResources -= ParticlePlayed;
    }
}
