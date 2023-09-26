using UnityEngine;

public class BildingFactory : Factory
{
    public string PathForBildingPrefab { get; set; }
    public override GameObject Bild(BildingType bildingType)
    {
        if (bildingType == BildingType.Bilding)
        {
            var bilding = Resources.Load<GameObject>(PathForBildingPrefab);
            GameObject Bilding = GameObject.Instantiate(bilding);
            return Bilding;
        }
        else
        {
            Debug.Log("Cannot Bild not Bilding");
            return null;
        }

    }
}


