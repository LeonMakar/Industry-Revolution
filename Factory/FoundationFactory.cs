using System;
using UnityEngine;

public class FoundationFactory : FoundationCreator
{
    public override FlyingStructure Create(StructureType foundationType)
    {
        switch (foundationType)
        {
            case StructureType.Bilding:
                var bilding = Resources.Load<GameObject>("Path");
                var gameObject = GameObject.Instantiate(bilding);
                if (gameObject.TryGetComponent<BildingFoundation>(out BildingFoundation bildingFoundation))
                {
                    var Bilding = bildingFoundation;
                    return Bilding;
                }
                else
                    throw new ArgumentNullException(nameof(BildingFoundation));
            case StructureType.District:
                var district = Resources.Load<GameObject>("Path");
                var districtGameObject = GameObject.Instantiate(district);
                if (districtGameObject.TryGetComponent<DistrictFoundation>(out DistrictFoundation districtFiundation))
                {
                    var District = districtFiundation;
                    return District;
                }
                else
                    throw new ArgumentNullException(nameof(DistrictFoundation));
            case StructureType.City:
                var city = Resources.Load<GameObject>("Path");
                var cityGameObject = GameObject.Instantiate(city);
                if (cityGameObject.TryGetComponent<CityFoundation>(out CityFoundation cityFoundation))
                {
                    var City = cityFoundation;
                    return City;
                }
                else
                    throw new ArgumentNullException(nameof(CityFoundation));
            default:
                throw new ArgumentNullException(nameof(StructureType));
        }
    }

    public int testNumber = 4;
}
public enum StructureType
{
    City = 0,
    Bilding = 1,
    District = 2,
    Road = 3,
}
