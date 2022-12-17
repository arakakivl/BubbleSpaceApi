using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Domain.Interfaces;
using BubbleSpaceApi.Application.Common;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetProfileQuery;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileViewModel?>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProfileQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProfileViewModel?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByUsernameAsync(request.Username);
        if (profile is null)
            throw new EntityNotFoundException("Usuário não encontrado.");

        return profile.AsViewModel();
    }
}