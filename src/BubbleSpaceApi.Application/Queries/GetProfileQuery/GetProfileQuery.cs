using BubbleSpaceApi.Application.Models.ViewModels;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetProfileQuery;

public class GetProfileQuery : IRequest<ProfileViewModel?>
{
    public string Username { get; set; }
    public GetProfileQuery(string username)
    {
        Username = username;
    }
}