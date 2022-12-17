using BubbleSpaceApi.Domain.Entities;
using Xunit;

namespace BubbleSpaceApi.DomainTests.Entities;

public class QuestionTests
{
    [Fact]
    public void UserAnswered_ShouldReturnTrue_WhenUserAnsweredQuestion()
    {
        // Arrange
        Profile prof = new Profile() { };
        Question question = new Question()
        {
            Answers = new List<Answer>()
            {
                new Answer()
                {
                    ProfileId = prof.Id,
                    Profile = prof
                }
            }
        };

        // Act
        var result = question.UserAnswered(prof.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UserAnswered_ShouldReturnFalse_WhenUserAnsweredQuestion()
    {
        // Arrange
        Profile prof = new Profile() { };
        Question question = new Question() { };

        // Act
        var result = question.UserAnswered(prof.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UserOwnsQuestion_ShouldReturnTrue_WhenUserOwnsQuestion()
    {
        // Arrange
        Profile prof = new Profile() { };
        Question question = new Question() { ProfileId = prof.Id };

        // Act
        var result = question.UserOwnsQuestion(prof.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UserOwnsQuestion_ShouldReturnFalse_WhenUserDoesNotOwnsQuestion()
    {
        // Arrange
        Profile prof = new Profile() { };
        Question question = new Question() { ProfileId = Guid.NewGuid() };

        // Act
        var result = question.UserOwnsQuestion(prof.Id);

        // Assert
        Assert.False(result);
    }
}