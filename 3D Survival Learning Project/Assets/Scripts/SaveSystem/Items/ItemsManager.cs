using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    private Item[] _items;
    private Vector3[] _itemPositions;
    private Quaternion[] _itemRotations;

    public static ItemsManager instance;
    private SaveManager _saveManager;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
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
        _items = new Item[transform.childCount];
        _itemPositions = new Vector3[transform.childCount];
        _itemRotations = new Quaternion[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _items[i] = transform.GetChild(i).GetComponent<ItemInteraction>().item;
            _itemPositions[i] = transform.GetChild(i).position;
            _itemRotations[i] = transform.GetChild(i).rotation;
        }
        _saveManager.worldInfo.items = _items;
        _saveManager.worldInfo.position = _itemPositions;
        _saveManager.worldInfo.rotation = _itemRotations;
    }
    private void Load()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < _saveManager.worldInfo.items.Length; i++)
        {
            Instantiate(_saveManager.worldInfo.items[i].itemPrefab, _saveManager.worldInfo.position[i], _saveManager.worldInfo.rotation[i], transform);
        }
    }
}
