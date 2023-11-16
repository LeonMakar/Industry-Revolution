using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoad 
{

    public List<Mark> Getpath(Vector3Int from,Vector3Int to);
    public void SetLastMarksPosition();
    public void SetConnectionToRoadInfo();
}
