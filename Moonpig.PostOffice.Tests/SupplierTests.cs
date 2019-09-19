namespace Moonpig.PostOffice.Tests
{
    using System;
    using System.Collections.Generic;
    using Api.Controllers;
    using Data;
    using Shouldly;
    using Xunit;

    public class SupplierTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        [InlineData(5, 7)]
        [InlineData(6, 8)]
        [InlineData(7, 9)]
        public void LeadTimeFromMonday(int leadTime, int expectedDays)
        {
            var supplier = new Supplier { LeadTime = leadTime };
            supplier.CalculateReceiveDate(new DateTime(2019, 9, 16)).ShouldBe(new DateTime(2019, 9, 16).AddDays(expectedDays));
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 4)]
        [InlineData(3, 5)]
        [InlineData(4, 6)]
        [InlineData(5, 7)]
        [InlineData(6, 10)]
        [InlineData(7, 11)]
        public void LeadTimeFromFriday(int leadTime, int expectedDays)
        {
            var supplier = new Supplier { LeadTime = leadTime };
            supplier.CalculateReceiveDate(new DateTime(2019, 9, 20)).ShouldBe(new DateTime(2019, 9, 20).AddDays(expectedDays));
        }
    }
}