using BubbleSpaceApi.Core.Entities;

namespace BubbleSpaceApi.Application.Models.ViewModels;

public class ProfileViewModel
{
    public string Username { get; set; } = null!;
    public string Bio { get; set; } = "";

    public ICollection<ProfileQuestionViewModel> Questions { get; set; } = new List<ProfileQuestionViewModel>();
    public ICollection<ProfileAnswerViewModel> Answers { get; set; } = new List<ProfileAnswerViewModel>();
}