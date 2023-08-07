using System;
using UnityEngine;

public class CityFoundationFactory : CityCreator
{
    public override CityFoundation CreatCity(GameObject prefab)
    {
        var Prefab = prefab;
        var gameObject = GameObject.Instantiate(prefab);
        if (gameObject.GetComponent<CityFoundation>() != null)
        {
            var City = gameObject.GetComponent<CityFoundation>();
            return City;
        }
        else
            throw new ArgumentNullException(nameof(CityFoundation));
    }
}
