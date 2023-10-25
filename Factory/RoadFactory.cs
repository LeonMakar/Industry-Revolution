using UnityEngine;

public class RoadFactory : Factory, IService
{

    public override GameObject Bild(BildingType roadType)
    {
        switch (roadType)
        {
            case BildingType.StrightRoad:
                var roadStright = Resources.Load<GameObject>("Roads/RoadStright");
                GameObject roadS = GameObject.Instantiate(roadStright);
                return roadS;
            case BildingType.CurveRoad:
                var roadCurve = Resources.Load<GameObject>("Roads/RoadCurve");
                GameObject roadC = GameObject.Instantiate(roadCurve);
                return roadC;
            case BildingType.TreeWayRoad:
                var roadTreeWay = Resources.Load<GameObject>("Roads/Road3Way");
                GameObject roadT = GameObject.Instantiate(roadTreeWay);
                return roadT;
            case BildingType.FourWayRoad:
                var roadFourWay = Resources.Load<GameObject>("Roads/Road4Way");
                GameObject roadF = GameObject.Instantiate(roadFourWay);
                return roadF;
            case BildingType.NothingRoad:
                return null;
            case BildingType.HorizontalRoad:
                var roadHorizontal = Resources.Load<GameObject>("Roads/RoadHorizontal");
                GameObject roadH = GameObject.Instantiate(roadHorizontal);
                return roadH;
            default: return null;
        }
    }

}

public enum BildingType
{
    StrightRoad = 0,
    CurveRoad = 1,
    TreeWayRoad = 2,
    FourWayRoad = 3,
    NothingRoad = 4,
    HorizontalRoad = 5,
    Bilding = 6,
    District = 7,
    City = 8,
    CanvasHouse = 0,
}
