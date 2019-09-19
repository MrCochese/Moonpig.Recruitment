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
            int daysUntilSaturday = ((int) DayOfWeek.Saturday - (int) orderDate.DayOfWeek + 7) % 7;

            if (LeadTime >= daysUntilSaturday + 5) {
                return orderDate.AddDays(LeadTime + 4);
            }

            if (LeadTime >= daysUntilSaturday) {
                return orderDate.AddDays(LeadTime + 2);
            }

            return orderDate.AddDays(LeadTime);
        }
    }
}
