namespace CostsSettler.Domain.Extensions;
public static class StringExtensions
{
    /// <summary>
    /// Converts string in format 'yyyy-MM-dd' to DateOnly 
    /// where 'yyyy' is year, 'MM' is month and 'dd' is day.
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Convertered DateOnly object if valid value argument. Else null.</returns>
    public static DateOnly? ToDateOnly(this string value)
    {
        try
        {
            var dateOnlySplit = value.Split('-');
            if (dateOnlySplit.Length != 3)
                return null;

            return new DateOnly(
                int.Parse(dateOnlySplit[0]),
                int.Parse(dateOnlySplit[1]),
                int.Parse(dateOnlySplit[2]));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Converts string in format 'HH:mm' to TimeOnly 
    /// where 'HH' is hour and 'mm' is minute.
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Convertered TimeOnly object if valid value argument. Else null.</returns>
    public static TimeOnly? ToTimeOnly(this string value)
    {
        try
        {
            var timeOnlySplit = value.Split(':');
            if (timeOnlySplit.Length != 2)
                return null;

            return new TimeOnly(
                int.Parse(timeOnlySplit[0]),
                int.Parse(timeOnlySplit[1]));
        }
        catch
        {
            return null;
        }
    }
}
