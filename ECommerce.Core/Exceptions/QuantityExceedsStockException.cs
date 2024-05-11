namespace ECommerce.Core.Exceptions
{
    public class QuantityExceedsStockException : Exception
    {
        public QuantityExceedsStockException() : base() { }
        public QuantityExceedsStockException(string message) : base(message) { }
        public QuantityExceedsStockException(string message, Exception innerException) : base(message, innerException) { }
    }
}
