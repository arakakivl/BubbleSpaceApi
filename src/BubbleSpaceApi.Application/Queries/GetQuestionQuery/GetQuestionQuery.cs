using BubbleSpaceApi.Application.Models.ViewModels;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionQuery;

public class GetQuestionQuery : IRequest<QuestionViewModel>
{
    public long QuestionId { get; set; }
    public GetQuestionQuery(long id)
    {
        QuestionId = id;
    }
}