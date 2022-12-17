namespace BubbleSpaceApi.Application.Models.ViewModels;

public class ResultViewModel
{
    public string Message { get; set; }
    public bool Success { get; set; }

    public object? Data { get; set; }

    public ResultViewModel(string msg, bool success, object? data)
    {
        Message = msg;
        Success = success;

        Data = data;
    }
}