using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetUsersQuery : IRequest<ICollection<UserForListDto>>
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ICollection<UserForListDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<UserForListDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<ICollection<UserForListDto>>(users);
        }
    }
}
