using UnityEngine;
using UnityEngine.Events;

public class RoadInfo : MonoBehaviour
{
    public UnityEvent RoadSetDirectionEvent;

    public UnityEvent<RoadInfo> GetIRoadEvent;

    public IRoad Road;
    public void SetConnection(IRoad road)
    {
        Road = road;
    }
}
