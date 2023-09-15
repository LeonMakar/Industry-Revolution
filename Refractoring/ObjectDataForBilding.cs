using UnityEngine;

public class ObjectDataForBilding : MonoBehaviour
{
    [SerializeField] private StructureType _selectedObjectStructureType;

    [SerializeField,Space(10)] private Size _bildingSize;
    [SerializeField, Space(10)] private string _pathToResourcesPrefab;

    [System.Serializable]
    public struct Size
    {
        public int x, y;
    }

    public StructureType SelectedObjectStructureType => _selectedObjectStructureType;
    public Size BildingSize => _bildingSize;
    public string PathToPrefab => _pathToResourcesPrefab;
}
