using UnityEngine;

public class FlyingStructure : MonoBehaviour
{
    [SerializeField] protected Vector2Int _size;
    [SerializeField] protected GameObject _errorBaner;
    [SerializeField] protected GameObject _bilding;

    public Vector2Int Size => _size;
    public void SetError(bool availible)
    {
        if (!availible)
            _errorBaner.SetActive(true);
        else
            _errorBaner.SetActive(false);
    }

    public void Rotate(int angleRotate)
    {
        _bilding.transform.Rotate(transform.rotation.x, transform.rotation.y + angleRotate, transform.rotation.z);
    }
}
