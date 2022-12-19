using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Domain.Interfaces;
using BubbleSpaceApi.Application.Common;
using MediatR;
using BubbleSpaceApi.Core.Communication.Mediator;

namespace BubbleSpaceApi.Application.Queries.GetProfileQuery;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileViewModel?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;

    public GetProfileQueryHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _unitOfWork = unitOfWork;
        _mediatorHandler = mediatorHandler;
    }

    public async Task<ProfileViewModel?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByUsernameAsync(request.Username);
        if (profile is null)
            throw new EntityNotFoundException("Usuário não encontrado.");

        return profile.AsViewModel();
    }
}