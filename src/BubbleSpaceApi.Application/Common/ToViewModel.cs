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
            Questions = profile.Questions.Select(x => x.AsProfileQuestionViewModel()).ToList(),
            Answers = profile.Answers.Select(x => x.AsProfileAnswerViewModel()).ToList()
        };
    }

    public static QuestionViewModel AsViewModel(this Question question)
    {
        return new QuestionViewModel()
        {
           UserWhoAsked = question.Profile.Username,
           Id = question.Id,
           Title = question.Title,
           Description = question.Description,
           Answers =  question.Answers.Select(x => x.AsViewModel()).ToList()
        };
    }

    public static ProfileQuestionViewModel AsProfileQuestionViewModel(this Question question)
    {
        return new ProfileQuestionViewModel()
        {
            Id = question.Id,
            Title = question.Title,
            Description = question.Description
        };
    }

    public static ProfileAnswerViewModel AsProfileAnswerViewModel(this Answer answer)
    {
        return new ProfileAnswerViewModel()
        {
            QuestionId = answer.QuestionId,
            QuestionTitle = answer.Question.Title,
            Answer = answer.Text
        };
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