using UnityEngine;

public class BildingFoundationFactory : BildingCreator
{
    public override BildingFoundation CreateBilding(GameObject prefab)
    {
        var Prefab = prefab;
        var gameObject = GameObject.Instantiate(prefab);
        var Bilding = gameObject.GetComponent<BildingFoundation>();
        return Bilding;
    }
}
