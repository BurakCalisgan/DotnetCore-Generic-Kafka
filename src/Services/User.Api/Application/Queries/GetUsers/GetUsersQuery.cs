using MediatR;

namespace User.Api.Application.Queries.GetUsers;

public class GetUsersQuery : IRequest<List<Data.Entity.User>>
{
}