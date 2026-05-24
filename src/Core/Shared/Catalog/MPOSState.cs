using System.Collections.ObjectModel;

namespace HRM.API.Shared.Catalog;

public static class MPOSState
{
    private static readonly StateModel[] _all = new StateModel[]
    {
        new("Andaman and Nicobar (UT)",  MPOSCountry.India),
        new("Andhra Pradesh",  MPOSCountry.India),
        new("Arunachal Pradesh",  MPOSCountry.India),
        new("Assam",  MPOSCountry.India),
        new("Bihar",  MPOSCountry.India),
        new("Chandigarh (UT)",  MPOSCountry.India),
        new("Chhattisgarh",  MPOSCountry.India),
        new("Dadra and Nagar Haveli (UT)",  MPOSCountry.India),
        new("Daman and Diu (UT)",  MPOSCountry.India),
        new("Delhi",  MPOSCountry.India),
        new("Goa",  MPOSCountry.India),
        new("Gujarat",  MPOSCountry.India),
        new("Haryana",  MPOSCountry.India),
        new("Himachal Pradesh",  MPOSCountry.India),
        new("Jammu and Kashmir",  MPOSCountry.India),
        new("Jharkhand",  MPOSCountry.India),
        new("Karnataka",  MPOSCountry.India),
        new("Kerala",  MPOSCountry.India),
        new("Lakshadweep (UT)",  MPOSCountry.India),
        new("Madhya Pradesh",  MPOSCountry.India),
        new("Maharashtra",  MPOSCountry.India),
        new("Manipur",  MPOSCountry.India),
        new("Meghalaya",  MPOSCountry.India),
        new("Mizoram",  MPOSCountry.India),
        new("Nagaland",  MPOSCountry.India),
        new("Orissa",  MPOSCountry.India),
        new("Puducherry (UT)",  MPOSCountry.India),
        new("Punjab",  MPOSCountry.India),
        new("Rajasthan",  MPOSCountry.India),
        new("Sikkim",  MPOSCountry.India),
        new("Tamil Nadu",  MPOSCountry.India),
        new("Telangana",  MPOSCountry.India),
        new("Tripura",  MPOSCountry.India),
        new("Uttar Pradesh",  MPOSCountry.India),
        new("Uttarakhand",  MPOSCountry.India),
        new("West Bengal",  MPOSCountry.India),
    };

    public static IReadOnlyList<StateModel> All { get; } = new ReadOnlyCollection<StateModel>(_all);
}

public record StateModel(string stateName, string countryName)
{
    public string StateName = stateName;
    public string CountryName = countryName;
}