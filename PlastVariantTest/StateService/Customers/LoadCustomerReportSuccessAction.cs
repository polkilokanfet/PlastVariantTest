using PlastVariantTest.Data.DB;
using Fluxor;
using PlastVariantTest.StateService.Customers;

namespace PlastVariantTest.StateService.Customers;

public record LoadCustomerReportAction(DateOnly Start, DateOnly End);
public record LoadCustomerReportSuccessAction(IReadOnlyList<CustomerReportItem> Items);
public record LoadCustomerReportFailureAction(string Error);

public static class CustomerReportReducers
{
    [ReducerMethod(typeof(LoadCustomerReportAction))]
    public static CustomerReportState ReduceLoad(CustomerReportState state)
    {
        return state with
        {
            IsLoading = true,
            Error = null
        };
    }

    [ReducerMethod]
    public static CustomerReportState ReduceSuccess(
        CustomerReportState state,
        LoadCustomerReportSuccessAction action)
        => state with
        {
            IsLoading = false,
            Items = action.Items
        };

    [ReducerMethod]
    public static CustomerReportState ReduceFailure(
        CustomerReportState state,
        LoadCustomerReportFailureAction action)
        => state with
        {
            IsLoading = false,
            Error = action.Error
        };
}

public class CustomerReportEffects
{
    private readonly DataBase _db;

    public CustomerReportEffects(DataBase db)
    {
        _db = db;
    }

    [EffectMethod]
    public async Task HandleLoad(LoadCustomerReportAction action, IDispatcher dispatcher)
    {
        try
        {
            await Task.Delay(1500);
            var result = _db.Orders(action.Start, action.End)
                .GroupBy(x => x.Company)
                .Select(x => new CustomerReportItem(x))
                .OrderByDescending(x => x.Sum)
                .ToList();

            dispatcher.Dispatch(new LoadCustomerReportSuccessAction(result));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadCustomerReportFailureAction(ex.Message));
        }
    }
}

