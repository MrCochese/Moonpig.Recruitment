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
            var maximumLeadTime = orderDate;
            foreach (var ID in productIds)
            {
                var supplierId = _dbContext.Products.Single(x => x.ProductId == ID).SupplierId;
                var leadTime = _dbContext.Suppliers.Single(x => x.SupplierId == supplierId).LeadTime;
                if (orderDate.AddDays(leadTime) > maximumLeadTime)
                    maximumLeadTime = orderDate.AddDays(leadTime);
            }

            switch (maximumLeadTime.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return new DespatchDate { Date = maximumLeadTime.AddDays(2) };
                case DayOfWeek.Sunday:
                    return new DespatchDate { Date = maximumLeadTime.AddDays(1) };
                default:
                    return new DespatchDate { Date = maximumLeadTime };
            }
        }
    }
}
