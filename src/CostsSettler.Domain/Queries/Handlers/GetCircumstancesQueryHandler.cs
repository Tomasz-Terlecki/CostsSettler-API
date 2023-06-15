using CostsSettler.Domain.Dtos;
using MediatR;

namespace CostsSettler.Domain.Queries.Handlers;
public class GetCircumstancesQueryHandler : IRequestHandler<GetCircumstancesQuery, ICollection<CircumstanceForListDto>>
{
    public async Task<ICollection<CircumstanceForListDto>> Handle(GetCircumstancesQuery request, CancellationToken cancellationToken)
    {
        return new List<CircumstanceForListDto>
        {
            new CircumstanceForListDto
            {
                Description = "test desc"
            }
        };
    }
}
