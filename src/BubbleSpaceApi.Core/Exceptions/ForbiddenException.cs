namespace BubbleSpaceApi.Core.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string msg) : base(msg)
    {
        
    }
}