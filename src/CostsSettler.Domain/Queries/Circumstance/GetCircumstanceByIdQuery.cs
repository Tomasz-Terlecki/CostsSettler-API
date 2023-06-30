using AutoMapper;
using CostsSettler.Domain.Dtos;
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

        public GetCircumstanceByIdQueryHandler(ICircumstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CircumstanceForReturnDto> Handle(GetCircumstanceByIdQuery request, CancellationToken cancellationToken)
        {
            var circumstance = await _repository.GetByIdAsync(request.Id, new string[] { nameof(Circumstance.Members) });

            return _mapper.Map<CircumstanceForReturnDto>(circumstance);
        }
    }
}
