using System;

public class Budget
{
    public Budget(string yearMonth, int amount)
    {
        YearMonth = yearMonth;
        Amount = amount;
    }

    public string YearMonth { get; set; }

    public int Amount { get; set; }

    public int DailyAmount()
    {
        var dateTime = DateTime.ParseExact(YearMonth, "yyyyMM", null);

        return Amount / DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
    }

    public int Days()
    {
        var dateTime = DateTime.ParseExact(YearMonth, "yyyyMM", null);

        return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
    }
}