using Fluxor;

namespace PlastVariantTest.StateService.Customers;

[FeatureState]
public record CustomerReportState
{
    public bool IsLoading { get; init; }
    public IReadOnlyList<CustomerReportItem> Items { get; init; } = [];
    public string? Error { get; init; }

    public CustomerReportState()
    {
    }
}
