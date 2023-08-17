public class FirstCity : Megapolis
{
    public FirstCity(int citizens) : base(citizens) { }


    private static int _numberOfIndustryDistrict;

    public static int NumberOfIndustryDistrict => _numberOfIndustryDistrict;

    public static void AddOneIndustryDistrictInTheCity()
    {
        _numberOfIndustryDistrict++;
    }
}
