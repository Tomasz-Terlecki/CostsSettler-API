using AutoMapper;
using CostsSettler.Domain.Dtos.Circumstance;
using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Queries.Circumstance;
public class GetCircumstanceByIdQuery : IRequest<CircumstanceForReturnDto>
{
    public Guid Id { get; set; }

    public class GetCircumstanceByIdQueryHandler : IRequestHandler<GetCircumstanceByIdQuery, CircumstanceForReturnDto>
    {
        private readonly ICircumstanceRepository _repository;
        private readonly IMapper _mapper;

        public GetCircumstanceByIdQueryHandler(ICircumstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CircumstanceForReturnDto> Handle(GetCircumstanceByIdQuery request, CancellationToken cancellationToken)
        {
            var circumstance = await _repository.GetByIdAsync(request.Id);

            return _mapper.Map<CircumstanceForReturnDto>(circumstance);
        }
    }
}
