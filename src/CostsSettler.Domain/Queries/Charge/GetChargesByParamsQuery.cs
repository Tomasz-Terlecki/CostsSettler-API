using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetChargesByParamsQuery : IRequest<ICollection<ChargeForListDto>>
{
    public Guid UserId { get; set; }
    public ChargeStatus ChargeStatus { get; set; }

    public class GetChargesByParamsQueryHandler : IRequestHandler<GetChargesByParamsQuery, ICollection<ChargeForListDto>>
    {
        private readonly IChargeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetChargesByParamsQueryHandler(IChargeRepository repository, IMapper mapper, IIdentityService identityService)
        {
            _repository = repository;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<ICollection<ChargeForListDto>> Handle(GetChargesByParamsQuery request, CancellationToken cancellationToken)
        {
            _identityService.CheckEqualityWithLoggedUserId(request.UserId);

            var charges = await _repository.GetByParamsAsync(request);

            return _mapper.Map<ICollection<ChargeForListDto>>(charges);
        }
    }
}
