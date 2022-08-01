namespace BubbleSpaceApi.Application.Models.ViewModels;

public class QuestionViewModel
{
    public long Id { get; set; }

    public string UserWhoAsked { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";

    public IQueryable<AnswerViewModel> Answers { get; set; } = null!;
}