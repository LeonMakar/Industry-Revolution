using UnityEngine;

public class RoadBilder : Bilder
{

    public override GameObject Bild(RoadType roadType)
    {
        switch (roadType)
        {
            case RoadType.Stright:
                var roadStright = Resources.Load<GameObject>("Roads/RoadStright");
                GameObject roadS = GameObject.Instantiate(roadStright);
                return roadS;
            case RoadType.Curve:
                var roadCurve = Resources.Load<GameObject>("Roads/RoadCurve");
                GameObject roadC = GameObject.Instantiate(roadCurve);
                return roadC;
            case RoadType.TreeWay:
                var roadTreeWay = Resources.Load<GameObject>("Roads/Road3Way");
                GameObject roadT = GameObject.Instantiate(roadTreeWay);
                return roadT;
            case RoadType.Nothing:
                return null;
            default: return null;
        }
    }

}

public enum RoadType
{
    Stright = 0,
    Curve = 1,
    TreeWay = 2,
    FourWay = 3,
    Nothing = 4,
}


