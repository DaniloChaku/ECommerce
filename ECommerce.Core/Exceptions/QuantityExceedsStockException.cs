namespace ECommerce.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a quantity exceeds the available stock.
    /// </summary>
    public class QuantityExceedsStockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityExceedsStockException"/> class.
        /// </summary>
        public QuantityExceedsStockException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityExceedsStockException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public QuantityExceedsStockException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityExceedsStockException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public QuantityExceedsStockException(string message, Exception innerException) : base(message, innerException) { }
    }
}
