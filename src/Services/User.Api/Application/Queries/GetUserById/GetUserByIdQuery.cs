using MediatR;

namespace User.Api.Application.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Data.Entity.User>
{
    public Guid Id { get; set; }
}