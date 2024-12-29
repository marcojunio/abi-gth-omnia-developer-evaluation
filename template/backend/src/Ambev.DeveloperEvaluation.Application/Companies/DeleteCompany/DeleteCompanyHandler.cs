using Ambev.DeveloperEvaluation.Application.Companies.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Companies.DeleteCompany;

public class DeleteCompanyHandler : IRequestHandler<DeleteCompanyCommand, DeleteCompanyResult>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMediator _mediator;

    public DeleteCompanyHandler(
        ICompanyRepository companyRepository, IMediator mediator)
    {
        _companyRepository = companyRepository;
        _mediator = mediator;
    }

    public async Task<DeleteCompanyResult> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteCompanyValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var success = await _companyRepository.DeleteAsync(request.Id, cancellationToken);

        if (!success)
            throw new InvalidDomainOperation($"Company with ID {request.Id} not found");

        await _mediator.Publish(new CompanyDeletedEvent(request.Id), cancellationToken);
        
        return new DeleteCompanyResult { Success = true };
    }
}