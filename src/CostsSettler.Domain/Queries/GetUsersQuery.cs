using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetUsersQuery : IRequest<ICollection<User>>
{
    
}
