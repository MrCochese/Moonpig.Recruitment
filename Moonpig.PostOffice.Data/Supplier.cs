namespace Moonpig.PostOffice.Data
{
    using System;

    public class Supplier
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }

        public int LeadTime { get; set; }

        public DateTime CalculateReceiveDate(DateTime orderDate)
        {
            return orderDate.AddWorkingDays(LeadTime);
        }
    }
}
