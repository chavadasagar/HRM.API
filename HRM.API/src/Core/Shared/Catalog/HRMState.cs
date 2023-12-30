using System.Collections.ObjectModel;

namespace HRM.API.Shared.Catalog;

public static class HRMState
{
    private static readonly StateModel[] _all = new StateModel[]
    {
        new("Andaman and Nicobar (UT)",  HRMCountry.India),
        new("Andhra Pradesh",  HRMCountry.India),
        new("Arunachal Pradesh",  HRMCountry.India),
        new("Assam",  HRMCountry.India),
        new("Bihar",  HRMCountry.India),
        new("Chandigarh (UT)",  HRMCountry.India),
        new("Chhattisgarh",  HRMCountry.India),
        new("Dadra and Nagar Haveli (UT)",  HRMCountry.India),
        new("Daman and Diu (UT)",  HRMCountry.India),
        new("Delhi",  HRMCountry.India),
        new("Goa",  HRMCountry.India),
        new("Gujarat",  HRMCountry.India),
        new("Haryana",  HRMCountry.India),
        new("Himachal Pradesh",  HRMCountry.India),
        new("Jammu and Kashmir",  HRMCountry.India),
        new("Jharkhand",  HRMCountry.India),
        new("Karnataka",  HRMCountry.India),
        new("Kerala",  HRMCountry.India),
        new("Lakshadweep (UT)",  HRMCountry.India),
        new("Madhya Pradesh",  HRMCountry.India),
        new("Maharashtra",  HRMCountry.India),
        new("Manipur",  HRMCountry.India),
        new("Meghalaya",  HRMCountry.India),
        new("Mizoram",  HRMCountry.India),
        new("Nagaland",  HRMCountry.India),
        new("Orissa",  HRMCountry.India),
        new("Puducherry (UT)",  HRMCountry.India),
        new("Punjab",  HRMCountry.India),
        new("Rajasthan",  HRMCountry.India),
        new("Sikkim",  HRMCountry.India),
        new("Tamil Nadu",  HRMCountry.India),
        new("Telangana",  HRMCountry.India),
        new("Tripura",  HRMCountry.India),
        new("Uttar Pradesh",  HRMCountry.India),
        new("Uttarakhand",  HRMCountry.India),
        new("West Bengal",  HRMCountry.India),
    };

    public static IReadOnlyList<StateModel> All { get; } = new ReadOnlyCollection<StateModel>(_all);
}

public record StateModel(string stateName, string countryName)
{
    public string StateName = stateName;
    public string CountryName = countryName;
}