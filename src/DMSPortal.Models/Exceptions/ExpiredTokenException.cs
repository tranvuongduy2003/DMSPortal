namespace DMSPortal.Models.Exceptions;

public class ExpiredTokenException : UnauthorizedException
{
    public ExpiredTokenException() : base("expired.token")
    {
    }
}