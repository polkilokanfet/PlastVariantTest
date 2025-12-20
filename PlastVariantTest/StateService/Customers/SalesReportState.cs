using PlastVariantTest.Data.DB;
using Fluxor;
using PlastVariantTest.StateService.Customers;

namespace PlastVariantTest.StateService.Customers;

[FeatureState]
public record SalesReportState
{
    public bool IsLoading { get; init; }
    public bool IsLoaded { get; init; }
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }
    public IReadOnlyList<CustomerReportItem> Items { get; init; } = [];
    public string? Error { get; init; }
}

public record LoadSalesReportAction(DateOnly Start, DateOnly End);
public record LoadSalesReportSuccessAction(IReadOnlyList<CustomerReportItem> Items);
public record LoadSalesReportFailureAction(string Error);

public static class SalesReportReducers
{
    [ReducerMethod]
    public static SalesReportState ReduceLoad(
        SalesReportState state,
        LoadSalesReportAction action)
    {
        return state with
        {
            IsLoading = true,
            IsLoaded = false,
            Error = null,
            Start = action.Start, 
            End = action.End,
        };
    }

    [ReducerMethod]
    public static SalesReportState ReduceSuccess(
        SalesReportState state,
        LoadSalesReportSuccessAction action)
        => state with
        {
            IsLoading = false,
            IsLoaded = true,
            Items = action.Items
        };

    [ReducerMethod]
    public static SalesReportState ReduceFailure(
        SalesReportState state,
        LoadSalesReportFailureAction action)
        => state with
        {
            IsLoading = false,
            IsLoaded = false,
            Error = action.Error
        };
}

public class SalesReportEffects
{
    private readonly DataBase _db;

    public SalesReportEffects(DataBase db)
    {
        _db = db;
    }

    [EffectMethod]
    public async Task HandleLoad(LoadSalesReportAction action, IDispatcher dispatcher)
    {
        try
        {
            await Task.Delay(1500);
            var result = _db.Orders(action.Start, action.End)
                .GroupBy(x => x.Company)
                .Select(x => new CustomerReportItem(x))
                .OrderByDescending(x => x.Sum)
                .ToList();

            dispatcher.Dispatch(new LoadSalesReportSuccessAction(result));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadSalesReportFailureAction(ex.Message));
        }
    }
}