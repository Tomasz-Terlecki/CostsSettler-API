using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetUsersByParamsQuery : IRequest<ICollection<UserForListDto>>
{
    public string? Email { get; set; }

    public class GetUsersByParamsQueryHandler : IRequestHandler<GetUsersByParamsQuery, ICollection<UserForListDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersByParamsQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<UserForListDto>> Handle(GetUsersByParamsQuery request, CancellationToken cancellationToken)
        {
            var users = (await _userRepository.GetAllAsync()).ToList();

            if (!string.IsNullOrEmpty(request.Email))
                users = users.Where(user => user.Email.Contains(request.Email)).ToList();

            return _mapper.Map<ICollection<UserForListDto>>(users);
        }
    }
}
