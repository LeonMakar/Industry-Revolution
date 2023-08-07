using System;
using UnityEngine;

public class BildingFoundationFactory : BildingCreator
{
    public override BildingFoundation CreateBilding(GameObject prefab)
    {
        var Prefab = prefab;
        var gameObject = GameObject.Instantiate(prefab);
        if (gameObject.GetComponent<BildingFoundation>() != null)
        {
            var Bilding = gameObject.GetComponent<BildingFoundation>();
            return Bilding;
        }
        else
            throw new ArgumentNullException(nameof(BildingFoundation));
    }
}
