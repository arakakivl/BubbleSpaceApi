namespace BubbleSpaceApi.Domain.Exceptions;

public class AlreadyAnsweredQuestionException : Exception
{
    public AlreadyAnsweredQuestionException(string msg) : base(msg)
    {
        
    }
}