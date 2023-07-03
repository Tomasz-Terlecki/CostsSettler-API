using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetChargesByParamsQuery : IRequest<ICollection<MemberChargeForListDto>>
{
    public Guid UserId { get; set; }
    public ChargeStatus ChargeStatus { get; set; }

    public class GetChargesByParamsQueryHandler : IRequestHandler<GetChargesByParamsQuery, ICollection<MemberChargeForListDto>>
    {
        private readonly IMemberChargeRepository _repository;
        private readonly IMapper _mapper;

        public GetChargesByParamsQueryHandler(IMemberChargeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<MemberChargeForListDto>> Handle(GetChargesByParamsQuery request, CancellationToken cancellationToken)
        {
            var charges = await _repository.GetByParamsAsync(request);

            return _mapper.Map<ICollection<MemberChargeForListDto>>(charges);
        }
    }
}
