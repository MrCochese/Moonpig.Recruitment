namespace Moonpig.PostOffice.Tests
{
    using System;
    using System.Collections.Generic;
    using Api.Controllers;
    using Data;
    using Shouldly;
    using Xunit;

    public class PostOfficeTests
    {
        [Theory]
        [InlineData(1, 1)] // 1 lead day
        [InlineData(2, 2)] // 2 lead days
        [InlineData(3, 3)] // 3 lead days
        [InlineData(9, 8)] // 6 lead days, one weekend
        [InlineData(10, 17)] // 13 lead days, two weekends
        public void OneProductWithLeadTimesFromMonday(int productId, int expectedLeadTime)
        {
            DespatchDateController controller = new DespatchDateController(new DbContext());
            var date = controller.Get(new List<int> { productId }, new DateTime(2018,1,22));
            date.Date.Date.ShouldBe(new DateTime(2018,1,22).AddDays(expectedLeadTime));
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(2, 3, 3)]
        [InlineData(3, 1, 3)]
        public void TwoProductsWithLeadTimesFromMonday(int firstProductId, int secondProductId, int expectedLeadTime)
        {
            DespatchDateController controller = new DespatchDateController(new DbContext());
            var date = controller.Get(new List<int> { firstProductId, secondProductId }, new DateTime(2018,1,22));
            date.Date.Date.ShouldBe(new DateTime(2018,1,22).AddDays(expectedLeadTime));
        }
    }
}
