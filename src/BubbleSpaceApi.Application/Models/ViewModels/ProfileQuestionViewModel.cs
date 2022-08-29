namespace BubbleSpaceApi.Application.Models.ViewModels;

public class ProfileQuestionViewModel
{
    public long Id { get; set; }
    
    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }
}