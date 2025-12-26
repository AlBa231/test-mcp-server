namespace McpTestServer.Core.Tools.Pharmacy;

internal static class PharmacyDrugInfoRandomGenerator
{
    private static readonly List<string> Pharmacies = ["Apteka 911", "ANC", "Podorognik"];

    public static IEnumerable<PharmacyDrugInfo> GenerateDrugInfos(IEnumerable<string> drugNames)
    {
        return Pharmacies.SelectMany(pharmacy => drugNames.Select(name => CreateDrugInfo(pharmacy, name)));
    }

    public static PharmacyDrugInfo CreateDrugInfo(string pharmacy, string name)
    {
        return new PharmacyDrugInfo
        {
            PharmacyName = pharmacy,
            DrugName = name,
            AvailableQuantity = Random.Shared.Next(0, 20),
            Price = Random.Shared.Next(200, 30000) / 100m
        };
    }
}