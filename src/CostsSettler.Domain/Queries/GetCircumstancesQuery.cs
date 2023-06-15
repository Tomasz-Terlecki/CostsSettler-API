using CostsSettler.Domain.Dtos;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetCircumstancesQuery : IRequest<ICollection<CircumstanceForListDto>>
{
}
