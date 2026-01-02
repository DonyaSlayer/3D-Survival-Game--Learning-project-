using System.Collections;
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

    [Header("Tools")]
    [SerializeField] private Transform _handTransform;
    public Animator handAnimator;
    public Item currentTool;
    private GameObject _currentToolInHand;

    [Header("References")]
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private PlayerBuilding _playerBuilding;
    private SaveManager _saveManager;

    private InventoryCell[] _cells;
    private Camera _mainCamera;
    private NeedsManager _needsManager;
    private Item _currentBuild;


    private void Awake()
    {
        _cells = _playerInventory.inventoryCells;
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        RefreshSelection();
        _needsManager = NeedsManager.instance;
        _saveManager = SaveManager.instance;
        _saveManager.OnSaveRequested += Save;
        _saveManager.OnLoadCompleted += Load;
    }

    private void OnDisable()
    {
        _saveManager.OnSaveRequested -= Save;
        _saveManager.OnLoadCompleted -= Load;
    }

    private void Save() 
    {
        _saveManager.playerInfo.items = _playerInventory.items;
        _saveManager.playerInfo.counts = _playerInventory.itemCount;
    }

    private void Load()
    {
        _playerInventory.items = _saveManager.playerInfo.items;
        _playerInventory.itemCount = _saveManager.playerInfo.counts;

        RefreshSelection();
        _playerInventory.Refresh();
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
            SetSelection(1);
        }
        else if (scroll > 0)
        {
            SetSelection(-1);
        }
    }
    private void HandleDrop()
    {
        if (_dropAction.action.triggered && _playerInventory.items[_currentSelection])
        {
            Instantiate(_playerInventory.items[_currentSelection].itemPrefab, _mainCamera.transform.position + transform.forward, Quaternion.identity, ItemsManager.instance.transform);
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

    public void MinusCurrentSelection()
    {
        _playerInventory.itemCount[_currentSelection]--;
        _playerInventory.Refresh();
        RefreshSelection();
    }

    public void RefreshSelection()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i].selection.SetActive(false);
        }
        _cells[_currentSelection].selection.SetActive(true);
        RefreshTool();
        RefreshBuild();
    }

    private void RefreshBuild()
    {
        if (_playerInventory.items[_currentSelection] && _playerInventory.items[_currentSelection].build.isBuild)
        {
            if (_currentBuild == null)
            {
                _currentBuild = _playerInventory.items[_currentSelection];
                _playerBuilding.NewBuild(_currentBuild.build._prefab);
            }
            else
            {
                _playerBuilding.DeleteBuild();
                _currentBuild = _playerInventory.items[_currentSelection];
                _playerBuilding.NewBuild(_currentBuild.build._prefab);
            }
        }
        else
        {
            _playerBuilding.DeleteBuild();
            _currentBuild = null;
        }
    }

    public void RefreshTool()
    {
        if (_playerInventory.items[_currentSelection] && _playerInventory.items[_currentSelection].tool.isTool)
        {
            if (currentTool && currentTool != _playerInventory.items[_currentSelection])
            {
                StartCoroutine(Disactivate(_currentToolInHand));
                handAnimator.Play("HideTool");
                currentTool = null;
                _currentToolInHand = null;
            }
            for (int i = 0; i < _handTransform.childCount; i++)
            {
                if (_handTransform.GetChild(i).name == _playerInventory.items[_currentSelection].itemName)
                {
                    _handTransform.GetChild(i).gameObject.SetActive(true);
                    currentTool = _playerInventory.items[_currentSelection];
                    _currentToolInHand = _handTransform.GetChild(i).gameObject;
                    handAnimator.Play("TakeToolAnim");
                }
            }
        } else
        {
            for (int i = 0; i < _handTransform.childCount; i++)
            {
                if (_currentToolInHand)
                {
                    StartCoroutine(Disactivate(_currentToolInHand));
                    handAnimator.Play("HideTool");
                }
                _currentToolInHand = null;
                currentTool = null;
            }
        }
    }

    private IEnumerator Disactivate(GameObject objectToDisactivate)
    {
        yield return new WaitForSeconds(0.2f);
        objectToDisactivate.SetActive(false);
    }
}
