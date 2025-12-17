using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header ("Interaction parameters")]
    [SerializeField] private float _interactionDistance;
    private Camera _mainCamera;


    [Header("UI")]
    [SerializeField] private TMP_Text _interactionText;

    [Header ("References")]
    [SerializeField] private Inventory _playerInventory;

    [Header("Input")]
    [SerializeField] private InputActionReference _interactAction;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        _interactionText.text = "";
        Ray ray = _mainCamera.ScreenPointToRay (new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit; //info container for RayCastPoint

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            if (hit.collider.gameObject.TryGetComponent<ItemInteraction> (out ItemInteraction itemToInteract))
            {
                _interactionText.text = itemToInteract.item.itemName;

                if(_interactAction.action.triggered)
                {
                    _playerInventory.AddItem(itemToInteract.item);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
