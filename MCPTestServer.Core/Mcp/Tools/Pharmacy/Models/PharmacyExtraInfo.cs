using System.ComponentModel;

namespace MCPTestServer.Core.Mcp.Tools.Pharmacy.Models;

public class PharmacyExtraInfo
{
    [Description("Enable checking in Apteka911 pharmacy")]
    [DefaultValue(true)]
    [DisplayName("Check Apteka 911")]
    public bool CheckApteka911 { get; set; } = true;

    [Description("Enable checking in ANC pharmacy")]
    [DefaultValue(true)]
    public bool CheckANC { get; set; } = true;

    [Description("Enable checking in Podorognik pharmacy")]
    [DefaultValue(true)]
    public bool CheckPodorognik { get; set; } = true;

    [Description("Your location (city) to check.")]
    public string? Location { get; set; }

    [Description("Maximum distance in kilometers to search for pharmacies from your location.")]
    [DefaultValue(10)]
    public int MaxDestinationKm { get; set; } = 10;
}
