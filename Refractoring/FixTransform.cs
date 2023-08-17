using UnityEngine;

public class FixTransform : MonoBehaviour
{
    private Transform _gameObjectTransform;

    private void OnEnable()
    {
        if (_gameObjectTransform == null)
            _gameObjectTransform = GetComponent<Transform>();
        else
        {
            throw new NullReferendeExeption();
        }
    }
    public void FixRotation(int x, int y, int z)
    {
        _gameObjectTransform.rotation = Quaternion.Euler(x, y, z);
    }
    public void FixRotation(int z)
    {
        _gameObjectTransform.rotation = Quaternion.Euler(-90, 0, z);

    }

}
