using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetCircumstanceByIdQuery : IRequest<CircumstanceForReturnDto>
{
    public Guid Id { get; set; }

    public class GetCircumstanceByIdQueryHandler : IRequestHandler<GetCircumstanceByIdQuery, CircumstanceForReturnDto>
    {
        private readonly ICircumstanceRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetCircumstanceByIdQueryHandler(ICircumstanceRepository repository, IMapper mapper, IUserRepository userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<CircumstanceForReturnDto> Handle(GetCircumstanceByIdQuery request, CancellationToken cancellationToken)
        {
            var circumstance = await _repository.GetByIdAsync(request.Id, new string[] { nameof(Circumstance.Members) });

            if (circumstance == null)
                throw new ObjectNotFoundException(typeof(Circumstance), request.Id);

            foreach (var member in circumstance.Members ?? new List<MemberCharge>())
                member.User = await _userRepository.GetByIdAsync(member.UserId) 
                    ?? throw new ObjectNotFoundException(typeof(User), member.UserId);

            return _mapper.Map<CircumstanceForReturnDto>(circumstance);
        }
    }
}
