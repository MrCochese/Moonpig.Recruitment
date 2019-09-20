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
            var maximumLeadTime = CalculateMaximumLeadTime(productIds, orderDate.Date);
            return new DespatchDate { Date = maximumLeadTime };
        }

        private DateTime CalculateMaximumLeadTime(List<int> productIds, DateTime orderDate)
        {
            var maximumLeadTime = orderDate;
            var supplierIds = _dbContext.Products.Where(x => productIds.Contains(x.ProductId))
                                                 .Select(x => x.SupplierId)
                                                 .Distinct();

            foreach (var supplierId in supplierIds)
            {
                var receiveDate = _dbContext.Suppliers.Single(x => x.SupplierId == supplierId)
                                                      .CalculateReceiveDate(orderDate);
                if (receiveDate > maximumLeadTime)
                    maximumLeadTime = receiveDate;
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
