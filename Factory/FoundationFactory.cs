using System;
using UnityEngine;

public class FoundationFactory : FoundationCreator
{
    public override BildingFoundation CreateBilding(string pathToResourcesBildingPrefab)
    {
        var prefab = Resources.Load<GameObject>(pathToResourcesBildingPrefab);
        var gameObject = GameObject.Instantiate(prefab);
        if (gameObject.TryGetComponent<BildingFoundation>(out BildingFoundation foundation))
        {
            var Bilding = foundation;
            return Bilding;
        }
        else
            throw new ArgumentNullException(nameof(BildingFoundation));
    }

    public override CityFoundation CreateCity(string pathToResourcesBildingPrefab)
    {
        var prefab = Resources.Load<GameObject>(pathToResourcesBildingPrefab);
        var gameObject = GameObject.Instantiate(prefab);
        if (gameObject.TryGetComponent<CityFoundation>(out CityFoundation foundation))
        {
            var Bilding = foundation;
            return Bilding;
        }
        else
            throw new ArgumentNullException(nameof(CityFoundation));
    }

    public override DistrictFoundation CreateDistrict(string pathToResourcesBildingPrefab)
    {
        var prefab = Resources.Load<GameObject>(pathToResourcesBildingPrefab);
        var gameObject = GameObject.Instantiate(prefab);
        if (gameObject.TryGetComponent<DistrictFoundation>(out DistrictFoundation foundation))
        {
            var Bilding = foundation;
            return Bilding;
        }
        else
            throw new ArgumentNullException(nameof(DistrictFoundation));
    }
}
