using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Exceptions
{
    public class QuantityExceedsStockException : Exception
    {
        public QuantityExceedsStockException() : base() { } 
        public QuantityExceedsStockException(string message) : base(message) { }
        public QuantityExceedsStockException(string message, Exception innerException) : base(message, innerException) { }
    }
}
