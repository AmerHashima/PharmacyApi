using MediatR;
using Pharmacy.Application.Commands.Link;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Link;

public class DeleteLinkHandler : IRequestHandler<DeleteLinkCommand, bool>
{
    private readonly ILinkRepository _linkRepository;
    private readonly IReportParameterRepository _parameterRepository;

    public DeleteLinkHandler(ILinkRepository linkRepository, IReportParameterRepository parameterRepository)
    {
        _linkRepository = linkRepository;
        _parameterRepository = parameterRepository;
    }

    public async Task<bool> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _linkRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        await _parameterRepository.DeleteByLinkIdAsync(request.Id, cancellationToken);
        await _linkRepository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
