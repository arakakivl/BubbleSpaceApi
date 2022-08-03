using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Entities;

namespace BubbleSpaceAPi.Application.Common;

public static class ToViewModel
{
    public static ProfileViewModel AsViewModel(this Profile profile)
    {
        return new ProfileViewModel()
        {
            Username = profile.Username,
            Bio = profile.Bio,
            Questions = profile.Questions.Select(x => x.AsViewModel()),
            Answers = profile.Answers.Select(x => x.AsViewModel())
        };
    }

    public static QuestionViewModel AsViewModel(this Question question)
    {
        return new QuestionViewModel()
        {
           Id = question.Id,
           Title = question.Title,
           Description = question.Description,
           Answers =  question.Answers.Select(x => x.AsViewModel())
        } ;
    }

    public static AnswerViewModel AsViewModel(this Answer answer)
    {
        return new AnswerViewModel()
        {
            UserWhoAnswered = answer.Profile.Username,
            Text = answer.Text
        };
    }
}