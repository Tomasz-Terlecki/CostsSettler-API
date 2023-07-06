using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetCircumstancesByParamsQuery : IRequest<ICollection<CircumstanceForListDto>>
{
    public Guid UserId { get; set; }

    public class GetCircumstancesQueryHandler : IRequestHandler<GetCircumstancesByParamsQuery, ICollection<CircumstanceForListDto>>
    {
        private readonly ICircumstanceRepository _repository;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetCircumstancesQueryHandler(ICircumstanceRepository repository, 
            IMapper mapper, IIdentityService identityService)
        {
            _repository = repository;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<ICollection<CircumstanceForListDto>> Handle(GetCircumstancesByParamsQuery request, CancellationToken cancellationToken)
        {
            _identityService.CheckEqualityWithLoggedUserId(request.UserId);

            var circumstances = await _repository.GetByParamsAsync(request);

            return _mapper.Map<ICollection<CircumstanceForListDto>>(circumstances);
        }
    }
}
