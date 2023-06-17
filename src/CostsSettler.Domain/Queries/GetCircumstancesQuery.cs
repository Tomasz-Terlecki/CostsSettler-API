using CostsSettler.Domain.Dtos;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetCircumstancesQuery : IRequest<ICollection<CircumstanceForListDto>>
{
    public string Description { get; set; }

    public class GetCircumstancesQueryHandler : IRequestHandler<GetCircumstancesQuery, ICollection<CircumstanceForListDto>>
    {
        public async Task<ICollection<CircumstanceForListDto>> Handle(GetCircumstancesQuery request, CancellationToken cancellationToken)
        {
            return new List<CircumstanceForListDto>
            {
                new CircumstanceForListDto
                {
                    Description = request.Description
                }
            };
        }
    }
}
