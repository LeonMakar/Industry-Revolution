using System;
using UnityEngine;

public class DistrictFoundationFactory : DistrictCreator
{
    public override DistrictFoundation CreateDistrict(GameObject Prefab)
    {
        var prefab = Prefab;
        var gameObject = GameObject.Instantiate(prefab);
        if (gameObject.GetComponent<DistrictFoundation>() != null)
        {
            var District = gameObject.GetComponent<DistrictFoundation>();
            return District;
        }
        else
            throw new ArgumentNullException(nameof(CityFoundation));
    }
}
