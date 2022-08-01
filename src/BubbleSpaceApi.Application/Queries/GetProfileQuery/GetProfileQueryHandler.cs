using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetProfileQuery;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileViewModel?>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProfileQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<ProfileViewModel?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}