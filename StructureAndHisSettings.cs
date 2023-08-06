using UnityEngine;

public class StructureAndHisSettings : MonoBehaviour
{
    public Vector2Int Size = Vector2Int.one;
    [SerializeField] private GameObject _errorBaner;
    [SerializeField] private GameObject _bilding;
    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x + y) % 2 == 0)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.green;

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }

        }
    }

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
