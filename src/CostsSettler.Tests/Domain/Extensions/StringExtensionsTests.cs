﻿using CostsSettler.Domain.Extensions;

namespace CostsSettler.Tests.Domain.Extensions;

/// <summary>
/// Tests the StringExtensions class.
/// </summary>
public class StringExtensionsTests
{
    /// <summary>
    /// Tests ToDateOnly with valid date string.
    /// </summary>
    /// <param name="dateString">Date string to parse.</param>
    /// <param name="year">Expected year.</param>
    /// <param name="month">Expected month.</param>
    /// <param name="day">Expected day.</param>
    [Theory]
    [InlineData("2023-08-25", 2023, 8, 25)]
    [InlineData("2023-8-25", 2023, 8, 25)]
    [InlineData("3-8-25", 3, 8, 25)]
    [InlineData("3-8-5", 3, 8, 5)]
    [InlineData("1111-1-1", 1111, 1, 1)]
    public void ToDateOnly_ValidDateString_Test(string dateString, int year, int month, int day)
    {
        var result = dateString.ToDateOnly();

        Assert.Equal(new DateOnly(year, month, day), result);
    }

    /// <summary>
    /// Tests ToDateOnly with invalid date string.
    /// </summary>
    /// <param name="dateString">Date string to parse.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("...")]
    [InlineData("afvasdfvsd")]
    [InlineData("2023-02-")]
    [InlineData("-02-")]
    [InlineData("2023--02")]
    [InlineData("2023-02-12-")]
    [InlineData("2023-13-10")]
    [InlineData("2023-02-30")]
    [InlineData("2023:02:02")]
    public void ToDateOnly_InValidDateString_Test(string dateString)
    {
        var result = dateString.ToDateOnly();

        Assert.Null(result);
    }

    /// <summary>
    /// Tests ToTimeOnly with valid time string.
    /// </summary>
    /// <param name="timeString">Time string to parse.</param>
    /// <param name="hour">Expected hour.</param>
    /// <param name="minute">Expected minute.</param>
    [Theory]
    [InlineData("1:1", 1, 1)]
    [InlineData("01:01", 1, 1)]
    [InlineData("10:10", 10, 10)]
    [InlineData("13:20", 13, 20)]
    public void ToTimeOnly_ValidTimeString_Test(string timeString, int hour, int minute)
    {
        var result = timeString.ToTimeOnly();

        Assert.Equal(new TimeOnly(hour, minute), result);
    }

    /// <summary>
    /// Tests ToTimeOnly with invalid time string.
    /// </summary>
    /// <param name="timeString">Time string to parse.</param>
    [Theory]
    [InlineData("1::1")]
    [InlineData("1:")]
    [InlineData(":1")]
    [InlineData("1:1:")]
    [InlineData(":1:1")]
    [InlineData("25:1")]
    [InlineData("12:60")]
    [InlineData("12:61")]
    public void ToTimeOnly_InvalidTimeString_Test(string timeString)
    {
        var result = timeString.ToTimeOnly();

        Assert.Null(result);
    }
}
