using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetCircumstancesQuery : IRequest<ICollection<CircumstanceForListDto>>
{
    public class GetCircumstancesQueryHandler : IRequestHandler<GetCircumstancesQuery, ICollection<CircumstanceForListDto>>
    {
        private readonly ICircumstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetCircumstancesQueryHandler(ICircumstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<CircumstanceForListDto>> Handle(GetCircumstancesQuery request, CancellationToken cancellationToken)
        {
            var circumstances = await _repository.GetAllAsync();

            return _mapper.Map<ICollection<CircumstanceForListDto>>(circumstances);
        }
    }
}
