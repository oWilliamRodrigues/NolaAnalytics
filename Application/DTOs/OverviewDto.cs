using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class OverviewDto
    {
        public decimal TotalRevenue { get; set; }
        public int Orders { get; set; }
        public decimal AvgTicket { get; set; }
        public int AvgDeliverySeconds { get; set; }
        public string? MonthLabel { get; set; }
    }
}
