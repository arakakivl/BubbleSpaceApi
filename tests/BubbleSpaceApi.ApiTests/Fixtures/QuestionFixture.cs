using BubbleSpaceApi.Application.Common;
using BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Domain.Entities;

namespace BubbleSpaceApi.ApiTests.Fixtures;

public static class QuestionFixture
{
    public static Question GenerateQuestion() =>
        new Question()
        {
            Id = 1,
            ProfileId = Guid.Empty,
            Title = "",
            Description = "",
            CreatedAt = DateTimeOffset.UtcNow,
        };

    public static QuestionViewModel GenerateQuestionViewModel() =>
        GenerateQuestion().AsViewModel();

    public static ProfileQuestionViewModel GenerateProfileQuestionViewModel() =>
        GenerateQuestion().AsProfileQuestionViewModel();

    public static AskQuestionInputModel GenerateValidAskQuestionInputModel() =>
        new AskQuestionInputModel()
        {
            Title = GenerateQuestion().Title,
            Description = GenerateQuestion().Description,
            Errors = new List<string>()
        };

    public static AskQuestionInputModel GenerateInvalidAskQuestionInputModel() =>
        new AskQuestionInputModel()
        {
            Title = "THIS IS A TITLE WHERE ITS TOTAL LENGTH IS GREATHER THAN THE ALLOWED. THIS IS A TITLE WHERE ITS TOTAL LENGTH IS GREATHER THAN THE ALLOWED. THIS IS A TITLE WHERE ITS TOTAL LENGTH IS GREATHER THAN THE ALLOWED. THIS IS A TITLE WHERE ITS TOTAL LENGTH IS GREATHER THAN THE ALLOWED.",
            Description = ""
        };
}