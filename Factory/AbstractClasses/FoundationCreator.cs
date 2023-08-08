using UnityEngine;

public abstract class FoundationCreator
{
    public abstract BildingFoundation CreateBilding(string pathToResourcesBildingPrefab);
    public abstract DistrictFoundation CreateDistrict(string pathToResourcesBildingPrefab);
    public abstract CityFoundation CreateCity(string pathToResourcesBildingPrefab);

}