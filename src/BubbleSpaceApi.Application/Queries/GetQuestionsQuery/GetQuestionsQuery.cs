using BubbleSpaceApi.Application.Models.ViewModels;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionsQuery;

public class GetQuestionsQuery : IRequest<ICollection<QuestionViewModel>>
{
    
}