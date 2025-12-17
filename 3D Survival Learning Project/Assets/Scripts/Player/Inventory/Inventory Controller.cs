using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference _leftSelectionAction;
    [SerializeField] private InputActionReference _rightSelectionAction;
    [SerializeField] private InputActionReference _scrollSelectionAction;
    [SerializeField] private InputActionReference _dropAction;
    [SerializeField] private InputActionReference _useAction;


    [Header("Selection")]
    [SerializeField] private int _currentSelection;


    [Header("References")]
    [SerializeField] private Inventory _playerInventory;
    private InventoryCell[] _cells;
    private Camera _mainCamera;
    private NeedsManager _needsManager;

    private void Awake()
    {
        _cells = _playerInventory.inventoryCells;
        _mainCamera = Camera.main;
        _needsManager = NeedsManager.instance;
    }

    private void Start()
    {
        RefreshSelection();
    }

    private void Update()
    {
        HandleSelection();
        HandleDrop();
        HandleUse();
    }

    private void HandleSelection()
    {
        if (_leftSelectionAction.action.triggered)
        {
            SetSelection(-1);
        }
        else if (_rightSelectionAction.action.triggered)
        {
            SetSelection(1);
        }

        float scroll = _scrollSelectionAction.action.ReadValue<float>();
        if (scroll < 0)
        {
            SetSelection(-1);
        }
        else if (scroll > 0)
        {
            SetSelection(1);
        }
    }
    private void HandleDrop()
    {
        if( _dropAction.action.triggered && _playerInventory.items[_currentSelection])
        {
            Instantiate(_playerInventory.items[_currentSelection].itemPrefab, _mainCamera.transform.position + transform.forward, Quaternion.identity, null);
            _playerInventory.ClearSlot(_currentSelection);
        }
    }

    private void HandleUse()
    {
        if (_useAction.action.triggered)
        {
            if (_playerInventory.items[_currentSelection] && _playerInventory.items[_currentSelection].usable.isUsable == true)
            {
                _needsManager.UseItem(_playerInventory.items[_currentSelection]);
                _playerInventory.itemCount[_currentSelection]--;
                _playerInventory.Refresh();
            }
        }
    }
    private void SetSelection(int value)
    {
        _currentSelection += value;
        if (_currentSelection == -1)
        {
            _currentSelection = _playerInventory.inventoryCells.Length - 1;
        }
        else if (_currentSelection == _playerInventory.inventoryCells.Length)
        {
            _currentSelection = 0;
        }
        RefreshSelection();
    }

    private void RefreshSelection()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i].selection.SetActive(false);
        }
        _cells[_currentSelection].selection.SetActive(true);
    }
}
