namespace Streetcode.BLL.Services.Payment.Exceptions
{
    /// <summary>
    /// Represents error received as a response with status 400 that is caused by invalid parameters of the request.
    /// </summary>
    public class InvalidRequestParameterException : MonobankException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestParameterException"/> class.
        /// </summary>
        /// <param name="error">The error info received from Monobank's API.</param>
        internal InvalidRequestParameterException(Error error)
            : base($"{error.Code}: {error.Text}")
        {
        }
    }
}
