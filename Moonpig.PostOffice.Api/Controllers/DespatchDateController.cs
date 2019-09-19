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
        [HttpGet]
        public DespatchDate Get(List<int> productIds, DateTime orderDate)
        {
            var maximumLeadTime = orderDate; // max lead time
            foreach (var ID in productIds)
            {
                DbContext dbContext = new DbContext();
                var supplierId = dbContext.Products.Single(x => x.ProductId == ID).SupplierId;
                var leadTime = dbContext.Suppliers.Single(x => x.SupplierId == supplierId).LeadTime;
                if (orderDate.AddDays(leadTime) > maximumLeadTime)
                    maximumLeadTime = orderDate.AddDays(leadTime);
            }
            if (maximumLeadTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return new DespatchDate { Date = maximumLeadTime.AddDays(2) };
            }
            else if (maximumLeadTime.DayOfWeek == DayOfWeek.Sunday) return new DespatchDate { Date = maximumLeadTime.AddDays(1) };
            else return new DespatchDate { Date = maximumLeadTime };
        }
    }
}
