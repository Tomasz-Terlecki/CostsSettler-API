using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Queries;

/// <summary>
/// Gets users by filter parameters.
/// </summary>
public class GetUsersByParamsQuery : IRequest<ICollection<UserForListDto>>
{
    /// <summary>
    /// GetUsersByParamsQuery handler.
    /// </summary>
    public class GetUsersByParamsQueryHandler : IRequestHandler<GetUsersByParamsQuery, ICollection<UserForListDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates new GetUsersByParamsQueryHandler instance.
        /// </summary>
        /// <param name="userRepository">Repository that manages users data.</param>
        /// <param name="mapper">Registered AutoMapper.</param>
        public GetUsersByParamsQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetUsersByParamsQuery query.
        /// </summary>
        /// <param name="request">GetUsersByParamsQuery to handle.</param>
        /// <param name="cancellationToken">Cncellation token.</param>
        /// <returns>Users that matches filters.</returns>
        public async Task<ICollection<UserForListDto>> Handle(GetUsersByParamsQuery request, CancellationToken cancellationToken)
        {
            var users = (await _userRepository.GetAllAsync()).ToList();

            return _mapper.Map<ICollection<UserForListDto>>(users);
        }
    }
}
