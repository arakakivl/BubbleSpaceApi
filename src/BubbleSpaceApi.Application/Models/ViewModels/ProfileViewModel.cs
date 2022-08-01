namespace BubbleSpaceApi.Application.Models.ViewModels;

public class ProfileViewModel
{
    public string Username { get; set; } = null!;
    public string Bio { get; set; } = "";

    public IQueryable<QuestionViewModel> Questions { get; set; } = null!;
    public IQueryable<AnswerViewModel> Answers { get; set; } = null!;
}