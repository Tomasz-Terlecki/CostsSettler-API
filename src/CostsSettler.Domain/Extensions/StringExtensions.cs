namespace CostsSettler.Domain.Extensions;
public static class StringExtensions
{
    /// <summary>
    /// Converts string in format 'yyyy-MM-dd' to DateOnly 
    /// where 'yyyy' is year, 'MM' is month and 'dd' is day.
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Convertered DateOnly object</returns>
    public static DateOnly ToDateOnly(this string value)
    {
        var dateOnlySplit = value.Split('-');

        return new DateOnly(
            int.Parse(dateOnlySplit[0]),
            int.Parse(dateOnlySplit[1]),
            int.Parse(dateOnlySplit[2]));
    }

    /// <summary>
    /// Converts string in format 'HH:mm' to TimeOnly 
    /// where 'HH' is hour and 'mm' is minute.
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Convertered TimeOnly object</returns>
    public static TimeOnly ToTimeOnly(this string value)
    {
        var timeOnlySplit = value.Split(':');

        return new TimeOnly(
            int.Parse(timeOnlySplit[0]),
            int.Parse(timeOnlySplit[1]));
    }
}
