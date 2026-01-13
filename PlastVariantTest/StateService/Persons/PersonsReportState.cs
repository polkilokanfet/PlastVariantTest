using Fluxor;
using Lira.Shared.Responses;
using PlastVariantTest.Services.Api;
using Refit;

namespace PlastVariantTest.StateService.Persons;

[FeatureState]
public record PersonsReportState
{
    public bool IsLoading { get; init; }
    public bool IsLoaded { get; init; }
    public IReadOnlyList<PersonResponse> Persons { get; init; } = [];
    public string? Error { get; init; }
}

public record LoadPersonsReportAction();
public record LoadPersonsReportSuccessAction(IReadOnlyList<PersonResponse> Persons);
public record LoadPersonsReportFailureAction(string Error);

public static class PersonsReportReducers
{
    [ReducerMethod]
    public static PersonsReportState ReduceLoad(
        PersonsReportState state,
        LoadPersonsReportAction action)
    {
        return state with
        {
            IsLoading = true,
            IsLoaded = false,
            Error = null
        };
    }

    [ReducerMethod]
    public static PersonsReportState ReduceSuccess(
        PersonsReportState state,
        LoadPersonsReportSuccessAction action)
        => state with
        {
            IsLoading = false,
            IsLoaded = true,
            Persons = action.Persons
        };

    [ReducerMethod]
    public static PersonsReportState ReduceFailure(
        PersonsReportState state,
        LoadPersonsReportFailureAction action)
        => state with
        {
            IsLoading = false,
            IsLoaded = false,
            Error = action.Error
        };
}

public class PersonsReportEffects
{
    private readonly ILiraApi _api;

    public PersonsReportEffects(ILiraApi api)
    {
        _api = api;
    }

    [EffectMethod]
    public async Task HandleLoad(LoadPersonsReportAction action, IDispatcher dispatcher)
    {
        try
        {
            var result = (await _api.GetPersons()).ToList();
            dispatcher.Dispatch(new LoadPersonsReportSuccessAction(result));
        }
        catch (ApiException ex)
        {
            dispatcher.Dispatch(
                new LoadPersonsReportFailureAction(ex.Content ?? ex.Message));
        }
        catch (Exception)
        {
            dispatcher.Dispatch(
                new LoadPersonsReportFailureAction("Неизвестная ошибка"));
        }
    }
}