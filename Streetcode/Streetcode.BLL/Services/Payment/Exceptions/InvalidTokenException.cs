namespace Streetcode.BLL.Services.Payment.Exceptions
{
    /// <summary>
    /// Represents error caused by invalid AccessToken value.
    /// </summary>
    public class InvalidTokenException : MonobankException
    {
        internal InvalidTokenException()
            : base("The provided token is not recognized by Monobank API.")
        {
        }
    }
}
