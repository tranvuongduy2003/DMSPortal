namespace DMSPortal.Models.Exceptions;

public class InvalidTokenException : UnauthorizedException
{
    public InvalidTokenException() : base("invalid.token")
    {
    }
}