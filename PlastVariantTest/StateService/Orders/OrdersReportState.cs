using PlastVariantTest.Data;
using PlastVariantTest.Data.DB;
using Fluxor;
using PlastVariantTest.Services.Api;

namespace PlastVariantTest.StateService.Orders;

[FeatureState]
public record OrdersReportState
{
    public bool IsLoading { get; init; }
    public bool IsLoaded { get; init; }
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }
    public IReadOnlyList<Order> Orders { get; init; } = [];
    public string? Error { get; init; }
}

public record LoadOrdersReportAction(DateOnly Start, DateOnly End);
public record LoadOrdersReportSuccessAction(IReadOnlyList<Order> Orders);
public record LoadOrdersReportFailureAction(string Error);

public static class OrdersReportReducers
{
    [ReducerMethod]
    public static OrdersReportState ReduceLoad(
        OrdersReportState state,
        LoadOrdersReportAction action)
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
    public static OrdersReportState ReduceSuccess(
        OrdersReportState state,
        LoadOrdersReportSuccessAction action)
        => state with
        {
            IsLoading = false,
            IsLoaded = true,
            Orders = action.Orders
        };

    [ReducerMethod]
    public static OrdersReportState ReduceFailure(
        OrdersReportState state,
        LoadOrdersReportFailureAction action)
        => state with
        {
            IsLoading = false,
            IsLoaded = false,
            Error = action.Error
        };
}

public class OrdersReportEffects
{
    private readonly DataBase _db;
    private readonly ILiraApi api;

    public OrdersReportEffects(DataBase db, ILiraApi api)
    {
        _db = db;
        this.api = api;
    }

    [EffectMethod]
    public async Task HandleLoad(LoadOrdersReportAction action, IDispatcher dispatcher)
    {
        try
        {
            //await Task.Delay(10000);
            //var p = (await api.GetPersons());
            var result = _db.Orders(action.Start, action.End).ToList();
            dispatcher.Dispatch(new LoadOrdersReportSuccessAction(result));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadOrdersReportFailureAction(ex.Message));
        }
    }
}