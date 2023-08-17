using UnityEngine;

public class RoadFactory
{
    public GameObject CreatRoad(StructureType roadType, Vector3Int position)
    {
        GameObject road = Resources.Load<GameObject>("Roads/RoadStright");
        GameObject Road = GameObject.Instantiate(road, position, Quaternion.identity);
        return Road;
    }
}
