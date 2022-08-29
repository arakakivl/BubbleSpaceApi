namespace BubbleSpaceApi.Application.Exceptions;

public class AlreadyAnsweredQuestionException : Exception
{
    public AlreadyAnsweredQuestionException(string msg) : base(msg)
    {
        
    }
}