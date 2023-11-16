using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectDataForBilding : MonoBehaviour
{
    [SerializeField] private StructureType _selectedObjectStructureType;

    [SerializeField, Space(10)] private Size _bildingSize;
    [SerializeField, Space(10)] private string _pathToResourcesPrefab;

    [field: SerializeField] public bool IsNotSemmetric { get; private set; }
    [field: SerializeField] public List<GameObject> CellsPosition { get; private set; }

    public IService service;
    public int RotationAngle { get; private set; }
    public Vector3 EulerAngle { get; private set; }

    [SerializeField] private GameObject _bildingPrefabForRotate;
    public GameObject BildingPrefabForRotate => _bildingPrefabForRotate;


    [System.Serializable]
    public struct Size
    {
        public int x, y;
    }

    public StructureType SelectedObjectStructureType => _selectedObjectStructureType;
    public Size BildingSize => _bildingSize;
    public string PathToPrefab => _pathToResourcesPrefab;

    public void ChangeBildingRotation()
    {
        if (_bildingPrefabForRotate != null)
        {
            RotationAngle += 90;
            _bildingPrefabForRotate.transform.Rotate(Vector3.forward, 90);
        }
    }
}
