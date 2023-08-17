using UnityEngine;

public class City : MonoBehaviour, IFlyingStructure
{
    [SerializeField] private Vector2Int _size = Vector2Int.one;
    [SerializeField] private GameObject _errorBaner;
    [SerializeField] private GameObject _bilding;

    public Vector2Int Size { get => _size; }
    public void SetError(bool availible)
    {
        if (!availible)
            _errorBaner.SetActive(true);
        else
            _errorBaner.SetActive(false);
    }
    public void RotateBilding(int angleRotate)
    {
        _bilding.transform.Rotate(transform.rotation.x, transform.rotation.y + angleRotate, transform.rotation.z);
    }
}
