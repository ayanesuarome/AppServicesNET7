﻿using FluentAssertions;
using FluentAssertions.Extensions;

namespace MappingObjects.Tests;

public class FluentExamples
{
    [Fact]
    public void TestString()
    {
        string city = "London";
        string expectedCity = "London";

        city.Should().StartWith("Lo")
            .And.EndWith("on")
            .And.Contain("do")
            .And.HaveLength(6);

        city.Should().NotBeNull()
            .And.Be("London")
            .And.BeSameAs(expectedCity)
            .And.BeOfType<string>();

        city.Length.Should().Be(6);
    }

    [Fact]
    public void TestCollections()
    {
        string[] names = new[] { "Alice", "Bob", "Charly" };

        names.Should().HaveCountLessThan(4, "because the maximum items should be 3 or fewer");
        names.Should().OnlyContain(name => name.Length <= 6);
    }

    [Fact]
    public void TestDateTimes()
    {
        DateTime when = new(
            hour: 9, minute: 30, second: 0,
            day: 25, month: 3, year: 2022);

        when.Should().Be(25.March(2022).At(9, 30));
        when.Should().BeOnOrAfter(23.March(2022));
        when.Should().NotBeSameDateAs(12.February(2022));
        when.Should().HaveYear(2022);

        DateTime due = new(
            hour: 12, minute: 0, second: 0,
            day: 25, month: 3, year: 2022);

        when.Should().BeAtLeast(2.Hours()).Before(due);
    }
}