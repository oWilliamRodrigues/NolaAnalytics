using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class ChurnCustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public DateTime LastOrder { get; set; }
    }
}
