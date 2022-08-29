namespace BubbleSpaceApi.Core.Exceptions;

public class AlreadyAnsweredQuestionException : Exception
{
    public AlreadyAnsweredQuestionException(string msg) : base(msg)
    {
        
    }
}