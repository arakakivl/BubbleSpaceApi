using BubbleSpaceApi.Application.Models.ViewModels;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionQuery;

public class GetQuestionQuery : IRequest<QuestionViewModel>
{
    public long Id { get; set; }
    public GetQuestionQuery(long id)
    {
        Id = id;
    }
}