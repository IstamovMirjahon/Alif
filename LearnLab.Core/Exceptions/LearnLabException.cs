namespace LearnLab.Core.Exceptions;

public class LearnLabException : Exception
{
    public LearnLabException()
    {
    }

    public LearnLabException(string? message) : base(message)
    {
    }

    public LearnLabException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
