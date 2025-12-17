namespace McpTestServer.Core.Tools;

public class PharmacyDrugInfo
{
    public required string PharmacyName { get; set; }
    public required string DrugName { get; set; }
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
}