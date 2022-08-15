using BubbleSpaceApi.Core.Entities;

namespace BubbleSpaceApi.Application.Models.ViewModels;

public class ProfileViewModel
{
    public string Username { get; set; } = null!;
    public string Bio { get; set; } = "";

    public ICollection<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    public ICollection<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
}