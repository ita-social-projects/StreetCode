namespace Streetcode.BLL.Services.Payment.Exceptions
{
    /// <summary>
    /// Represents abstract class for all errors occured during <see cref="Monobank"/> execution
    /// </summary>
    public abstract class MonobankException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonobankException"/> class with a specified error message.
        /// </summary>
        /// <param name="exception">The error message.</param>
        internal MonobankException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonobankException"/> class with a reference to the wrapped exception.
        /// </summary>
        /// <param name="exception">The exception which is wrapped by this instance of <see cref="MonobankException"/>.</param>
        internal MonobankException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
