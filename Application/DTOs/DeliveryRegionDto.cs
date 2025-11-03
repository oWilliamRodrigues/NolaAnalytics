using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class DeliveryRegionDto
    {
        public string Neighborhood { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int Deliveries { get; set; }
        public double AvgDeliveryMinutes { get; set; }
        public double P90Minutes { get; set; }
    }
}
