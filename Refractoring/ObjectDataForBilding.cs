using UnityEngine;

public class ObjectDataForBilding : MonoBehaviour
{
    [SerializeField] private SelectedObjectForBilding _selectedObjectForBilding;

    [SerializeField] private Size _bildingSize;

    [System.Serializable]
    struct Size
    {
        public int x, y;
    }

    public SelectedObjectForBilding SelectedObjectForBilding => _selectedObjectForBilding;
}
