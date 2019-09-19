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
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public void OneProductWithLeadTimesFromMonday(int productId, int expectedLeadTime)
        {
            DespatchDateController controller = new DespatchDateController(new DbContext());
            var date = controller.Get(new List<int> { productId }, new DateTime(2018,1,22));
            date.Date.Date.ShouldBe(new DateTime(2018,1,22).AddDays(expectedLeadTime));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            DespatchDateController controller = new DespatchDateController(new DbContext());
            var date = controller.Get(new List<int>() { 1 }, new DateTime(2018,1,26));
            date.Date.ShouldBe(new DateTime(2018, 1, 26).Date.AddDays(3));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            DespatchDateController controller = new DespatchDateController(new DbContext());
            var date = controller.Get(new List<int>() { 3 }, new DateTime(2018,1,25));
            date.Date.ShouldBe(new DateTime(2018, 1, 25).Date.AddDays(4));
        }
    }
}
