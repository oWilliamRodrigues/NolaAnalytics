using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public double UnitsSold { get; set; }
        public double Revenue { get; set; }
    }
}
