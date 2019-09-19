namespace Moonpig.PostOffice.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Model;

    [Route("api/[controller]")]
    public class DespatchDateController : Controller
    {
        private IDbContext _dbContext;

        public DespatchDateController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public DespatchDate Get(List<int> productIds, DateTime orderDate)
        {
            var maximumLeadTime = CalculateMaximumLeadTime(productIds, orderDate);
            return new DespatchDate { Date = maximumLeadTime };
        }

        private DateTime CalculateMaximumLeadTime(List<int> productIds, DateTime orderDate)
        {
            var maximumLeadTime = orderDate;
            foreach (var ID in productIds)
            {
                var supplierId = _dbContext.Products.Single(x => x.ProductId == ID).SupplierId;
                var leadTime = _dbContext.Suppliers.Single(x => x.SupplierId == supplierId).LeadTime;
                if (orderDate.AddDays(leadTime) > maximumLeadTime)
                    maximumLeadTime = orderDate.AddDays(leadTime);
            }
            return AdjustForWeekend(maximumLeadTime);
        }

        private DateTime AdjustForWeekend(DateTime maximumLeadTime)
        {
            switch (maximumLeadTime.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return maximumLeadTime.AddDays(2);
                case DayOfWeek.Sunday:
                    return maximumLeadTime.AddDays(1);
                default:
                    return maximumLeadTime;
            }
        }
    }
}
