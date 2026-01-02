using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilding : MonoBehaviour
{
    [Header("Building Settings")]
    
    [SerializeField] private float _maxBuildDistance;
    [SerializeField] private float _surfaceAngleLimit = 30f;
    [SerializeField] private float _collisionCheckRadius = 1f;
    [SerializeField] private LayerMask _buildableLayer;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    private bool _canBuild = false;
    private GameObject _currentBuild;
  

    [Header("Input")]
    [SerializeField] private InputActionReference _buildKey;

    [Header("References")]
    [SerializeField] private InventoryController _inventoryController;
    private BuildManager _buildManager;

    private Camera _camera;
    private GameObject _previewInstance;

    

    private void Start()
    {
        _camera = Camera.main;
        _buildManager = BuildManager.instance;
    }

    private void Update()
    {
        if(_currentBuild)
        {
            HandleBuildPreview();
            HandleInput();
        }
    }

    public void NewBuild(GameObject buildPrefab)
    {
        _currentBuild = buildPrefab;
        _previewInstance = Instantiate(buildPrefab);
        _previewInstance.SetActive(false);

        foreach (var collider in _previewInstance.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        for (int i = 0; i < _previewInstance.transform.childCount; i++)
        {
            if (_previewInstance.transform.GetChild(i).GetComponent<MeshRenderer>() == false)
            {
                _previewInstance.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void DeleteBuild()
    {
        Destroy(_previewInstance);
        _currentBuild = null;
        _canBuild = false;
    }

    private void HandleBuildPreview()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if(Physics.Raycast(ray, out RaycastHit hit, _maxBuildDistance, _buildableLayer))
        {   
            Vector3 position = hit.point;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            if(Vector3.Angle(hit.normal, Vector3.up) < _surfaceAngleLimit && !Physics.CheckSphere(position, _collisionCheckRadius, _obstacleLayer))
            {
                _previewInstance.SetActive(true);
                _previewInstance.transform.SetLocalPositionAndRotation(position, rotation);
                SetPreviewColor(_greenMaterial);
                _canBuild = true;
            }
            else
            {
                _previewInstance.SetActive(true);
                _previewInstance.transform.SetLocalPositionAndRotation(position, rotation);
                SetPreviewColor(_redMaterial);
                _canBuild = false;
            }
        }
        else
        {
            _previewInstance.SetActive(false);
        }
    }

    private void HandleInput()
    {
        if (!_previewInstance.activeSelf) return;
        if (_buildKey.action.WasPerformedThisFrame() && _canBuild)
        {
            Instantiate(_currentBuild, _previewInstance.transform.position, _previewInstance.transform.rotation, _buildManager.transform);
            _inventoryController.MinusCurrentSelection();
        }
    }

    private void SetPreviewColor(Material newMaterial)
    {
        foreach (MeshRenderer meshRenderer in _previewInstance.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.material = newMaterial;
        }
    }
}
