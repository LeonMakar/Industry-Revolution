using UnityEngine;

public class DistrictFoundationFactory : DistrictCreator
{
    public override DistrictFoundation CreateDistrict(GameObject Prefab)
    {
        var prefab = Prefab;
        var gameObject = GameObject.Instantiate(prefab);
        var District = gameObject.GetComponent<DistrictFoundation>();
        return District;
    }
}
