using AutoFixture;
using BubbleSpaceApi.Domain.Entities;
using Xunit;

namespace BubbleSpaceApi.DomainTests.Entities;

public class QuestionTests
{
    private readonly Fixture _fixture;
    public QuestionTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void UserAnswered_ShouldReturnTrue_WhenUserAnsweredQuestion()
    {
        // Arrange
        var profile = _fixture.Create<Profile>();
        var answers = new List<Answer>() { { _fixture.Build<Answer>().With(x => x.Profile, profile).With(x => x.ProfileId, profile.Id).Create() } };

        var question = _fixture.Build<Question>().With(x => x.Answers, answers).Create();

        // Act
        var result = question.UserAnswered(profile.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UserAnswered_ShouldReturnFalse_WhenUserAnsweredQuestion()
    {
        // Arrange
        var profile = _fixture.Create<Profile>();
        var question = _fixture.Create<Question>();

        // Act
        var result = question.UserAnswered(profile.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UserOwnsQuestion_ShouldReturnTrue_WhenUserOwnsQuestion()
    {
        // Arrange
        var prof = _fixture.Create<Profile>();
        var question = _fixture.Build<Question>().With(x => x.ProfileId, prof.Id).Create();

        // Act
        var result = question.UserOwnsQuestion(prof.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UserOwnsQuestion_ShouldReturnFalse_WhenUserDoesNotOwnsQuestion()
    {
        // Arrange
        var prof = _fixture.Create<Profile>();
        var question = _fixture.Create<Question>();

        // Act
        var result = question.UserOwnsQuestion(prof.Id);

        // Assert
        Assert.False(result);
    }
}