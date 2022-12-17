namespace BubbleSpaceApi.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string msg) : base(msg)
    {
        
    }
}