﻿using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetCircumstanceByIdQuery : IRequest<Circumstance>
{
    public Guid Id { get; set; }

    public GetCircumstanceByIdQuery(Guid id)
    {
        Id = id;
    }

    public class GetCircumstanceByIdQueryHandler : IRequestHandler<GetCircumstanceByIdQuery, Circumstance>
    {
        private readonly ICircumstanceRepository _repository;
        private readonly IUserRepository _userRepository;

        public GetCircumstanceByIdQueryHandler(ICircumstanceRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<Circumstance> Handle(GetCircumstanceByIdQuery request, CancellationToken cancellationToken)
        {
            var circumstance = await _repository.GetByIdAsync(request.Id, new string[] { nameof(Circumstance.Charges) });

            if (circumstance == null)
                throw new ObjectNotFoundException(typeof(Circumstance), request.Id);

            foreach (var charge in circumstance.Charges ?? new List<Charge>())
            {
                charge.Creditor = await _userRepository.GetByIdAsync(charge.CreditorId) 
                    ?? throw new ObjectNotFoundException(typeof(User), charge.CreditorId);
                
                charge.Debtor = await _userRepository.GetByIdAsync(charge.DebtorId)
                    ?? throw new ObjectNotFoundException(typeof(User), charge.DebtorId);
            }

            return circumstance;
        }
    }
}
