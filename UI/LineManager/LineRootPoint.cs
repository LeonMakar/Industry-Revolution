using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRootPoint : MonoBehaviour
{
    private Vector3Int _position;

    public Vector3Int RootPisition => _position;

    public void ChangeRootPosition(Vector3Int position) => _position = position;
}
