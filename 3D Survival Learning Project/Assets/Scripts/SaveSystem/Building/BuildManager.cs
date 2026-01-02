using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject[] buildings;
    private Vector3[] _buildingPositions;
    private Quaternion[] _buildingRotations;
    public GameObject[] buildingsPrefabs;

    private SaveManager _saveManager;
    public static BuildManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        _saveManager.worldInfo.buildingsNames = new string[transform.childCount];
        _saveManager.worldInfo.buildPositions = new Vector3[transform.childCount];
        _saveManager.worldInfo.buildRotations = new Quaternion[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _saveManager.worldInfo.buildingsNames[i] = transform.GetChild(i).gameObject.GetComponent<BuildsData>().buildingName;
            _saveManager.worldInfo.buildPositions[i] = transform.GetChild(i).position;
            _saveManager.worldInfo.buildRotations[i] = transform.GetChild(i).rotation;
        }
    }

    private void Load()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < _saveManager.worldInfo.buildingsNames.Length; i++)
        {
            GameObject buildingPrafab = null;
            for(int j = 0; j < buildingsPrefabs.Length; j++)
            {
                if (buildingsPrefabs[j].GetComponent<BuildsData>().buildingName == _saveManager.worldInfo.buildingsNames[i])
                {
                    buildingPrafab = buildingsPrefabs[j];
                    break;
                }
            }
            if(buildingPrafab != null)
                Instantiate(buildingPrafab, _saveManager.worldInfo.buildPositions[i], _saveManager.worldInfo.buildRotations[i], transform);
        }
    }
}
