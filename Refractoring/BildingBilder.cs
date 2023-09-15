using UnityEngine;

public class BildingBilder : Bilder
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


