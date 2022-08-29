namespace BubbleSpaceApi.Application.Models.ViewModels;

public class ProfileAnswerViewModel
{
    public long QuestionId { get; set; }
    public string QuestionTitle { get; set; } = null!;
    
    public string Answer { get; set; } = null!;
}