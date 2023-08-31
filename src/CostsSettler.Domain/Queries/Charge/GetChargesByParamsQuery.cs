using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Queries;

/// <summary>
/// Gets charges by filter parameters.
/// </summary>
public class GetChargesByParamsQuery : IRequest<ICollection<ChargeForListDto>>
{
    /// <summary>
    /// Filters charges by user id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// If not null, filter charges which date is greater or equal the given day.
    /// Accepted format: 'yyyy-mm-dd'.
    /// </summary>
    public string? DateFrom { get; set; }

    /// <summary>
    /// If not null, filter charges which date is less or equal the given day.
    /// Accepted format: 'yyyy-mm-dd'.
    /// </summary>
    public string? DateTo { get; set; }

    /// <summary>
    /// If not null, filters charges by description of circumstance the charge refers to.
    /// </summary>
    public string? CircumstanceDescription { get; set; }

    /// <summary>
    /// If not null, filter charges by which amount is grater or equal given number.
    /// </summary>
    public decimal? AmountFrom { get; set; }

    /// <summary>
    /// If not null, filter charges by which amount is less or equal given number.
    /// </summary>
    public decimal? AmountTo { get; set; }

    /// <summary>
    /// GetChargesByParamsQuery handler.
    /// </summary>
    public class GetChargesByParamsQueryHandler : IRequestHandler<GetChargesByParamsQuery, ICollection<ChargeForListDto>>
    {
        private readonly IChargeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Creates new GetChargesByParamsQueryHandler instance.
        /// </summary>
        /// <param name="repository">Repository that manages charges data.</param>
        /// <param name="mapper">Registered AutoMapper.</param>
        /// <param name="identityService">Service that authorizes user that invokes the Handler.</param>
        public GetChargesByParamsQueryHandler(IChargeRepository repository, IMapper mapper, IIdentityService identityService)
        {
            _repository = repository;
            _mapper = mapper;
            _identityService = identityService;
        }

        /// <summary>
        /// Handles the GetChargesByParamsQuery query. Checks if user is authorized.
        /// </summary>
        /// <param name="request">GetChargesByParamsQuery to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Charges that matches filters.</returns>
        public async Task<ICollection<ChargeForListDto>> Handle(GetChargesByParamsQuery request, CancellationToken cancellationToken)
        {
            _identityService.CheckEqualityWithLoggedUserId(request.UserId);

            var charges = await _repository.GetByParamsAsync(request);

            return _mapper.Map<ICollection<ChargeForListDto>>(charges);
        }
    }
}
