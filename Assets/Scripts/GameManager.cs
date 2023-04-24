using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] float _SpawnRadiusMin;
    [SerializeField] float _SpawnRadiusMax;
    [SerializeField] int _NumOfObjects;
    [SerializeField] GameObject _ObstaclePrefab;
    [SerializeField] PlayerController _PlayerPrefab;
    [SerializeField] List<Transform> _Points;
    [SerializeField] LineRenderer _Line;

    private GameObject _container;

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        SpawnObjects();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        _Line.positionCount = _Points.Count+1;
        _Line.SetPosition(0, Vector3.zero);
        for(int i = 0; i < _Points.Count; i++)
        {
            _Line.SetPosition(i+1, _Points[i].transform.position);
        }
    }

    private void SpawnObjects()
    {
        PlayerController player= Instantiate(_PlayerPrefab, Vector3.zero, Quaternion.identity);
        player.AssignPoints(_Points);
        if (_container == null)
        {
            CreateContainer();
        }
        else
        {
            Destroy(_container);
            CreateContainer();
        }
        for(int i = 0; i < _NumOfObjects; i++)
        {
            Vector3 pos = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward * Random.Range(_SpawnRadiusMin, _SpawnRadiusMax);
            GameObject obj = Instantiate(_ObstaclePrefab, pos, Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
            obj.transform.parent = _container.transform;
        }
    }

    private void CreateContainer()
    {
        _container = new GameObject();
        _container.name = "ObstacleContainer";
    }
}
