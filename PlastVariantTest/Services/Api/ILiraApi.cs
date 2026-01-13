using Lira.Shared;
using Lira.Shared.Requests;
using Lira.Shared.Responses;
using Refit;

namespace PlastVariantTest.Services.Api;

public interface ILiraApi
{
    [Post("/api/auth/login")]
    Task<LoginResponse> Login([Body] LoginRequest request);

    [Get(Routs.PersonsControllerRoute)]
    Task<IEnumerable<PersonResponse>> GetPersons(CancellationToken cancellationToken = default);

    //[Get($"{Routs.PersonsControllerRoute}/{{id}}")]
    //Task<PersonResponse> GetPerson(Guid id, CancellationToken cancellationToken = default);

    //[Post(Routs.PersonsControllerRoute)]
    //Task<Guid> SavePerson(SavePersonRequest request, CancellationToken cancellationToken = default);
}