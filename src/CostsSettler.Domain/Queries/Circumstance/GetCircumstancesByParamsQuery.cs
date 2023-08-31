using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Queries;

/// <summary>
/// Gets circumstances by filter parameters.
/// </summary>
public class GetCircumstancesByParamsQuery : IRequest<ICollection<CircumstanceForListDto>>
{
    /// <summary>
    /// Filters circumstances by user id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// GetCircumstancesByParamsQuery handler.
    /// </summary>
    public class GetCircumstancesByParamsQueryHandler : IRequestHandler<GetCircumstancesByParamsQuery, ICollection<CircumstanceForListDto>>
    {
        private readonly ICircumstanceRepository _repository;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Creates new GetCircumstancesByParamsQueryHandler instance.
        /// </summary>
        /// <param name="repository">Repository that manages circumstances data.</param>
        /// <param name="mapper">Registered AutoMapper.</param>
        /// <param name="identityService">Service that authorizes user that invokes the Handler.</param>
        public GetCircumstancesByParamsQueryHandler(ICircumstanceRepository repository, 
            IMapper mapper, IIdentityService identityService)
        {
            _repository = repository;
            _mapper = mapper;
            _identityService = identityService;
        }

        /// <summary>
        /// Handles the GetCircumstancesByParamsQuery query. Checks if user is authorized.
        /// </summary>
        /// <param name="request">GetCircumstancesByParamsQuery to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Circumstances that matches filters.</returns>
        public async Task<ICollection<CircumstanceForListDto>> Handle(GetCircumstancesByParamsQuery request, CancellationToken cancellationToken)
        {
            _identityService.CheckEqualityWithLoggedUserId(request.UserId);

            var circumstances = await _repository.GetByParamsAsync(request);

            return _mapper.Map<ICollection<CircumstanceForListDto>>(circumstances);
        }
    }
}
