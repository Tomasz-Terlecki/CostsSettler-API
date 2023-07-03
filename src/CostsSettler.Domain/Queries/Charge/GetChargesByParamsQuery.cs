using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetChargesByParamsQuery : IRequest<ICollection<ChargeForListDto>>
{
    public Guid CreditorId { get; set; }
    public Guid DebtorId { get; set; }
    public ChargeStatus ChargeStatus { get; set; }

    public class GetChargesByParamsQueryHandler : IRequestHandler<GetChargesByParamsQuery, ICollection<ChargeForListDto>>
    {
        private readonly IChargeRepository _repository;
        private readonly IMapper _mapper;

        public GetChargesByParamsQueryHandler(IChargeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<ChargeForListDto>> Handle(GetChargesByParamsQuery request, CancellationToken cancellationToken)
        {
            var charges = await _repository.GetByParamsAsync(request);

            return _mapper.Map<ICollection<ChargeForListDto>>(charges);
        }
    }
}
