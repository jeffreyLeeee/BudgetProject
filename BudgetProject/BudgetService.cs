using System;
using System.Collections.Generic;
using System.Linq;

public class BudgetService
{
    private static IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public double Query(DateTime start, DateTime end)
    {
        var startFormat = start.ToString("yyyyMM");
        var endFormat = end.ToString("yyyyMM");

        if (start.Month == end.Month)
        {
            var totalDays = (end - start).TotalDays + 1;

            if (totalDays < 0)
            {
                return 0;
            }

            var budget = BudgetByYearMonth(startFormat);

            return budget.DailyAmount() * totalDays;
        }

        var middleMonths = new List<Budget>();

        for (int i = 1;; i++)
        {
            if (start.AddMonths(i).Month == end.Month)
            {
                break;
            }

            middleMonths.Add(BudgetByYearMonth(start.AddMonths(i).ToString("yyyyMM")));
        }

        var totalAmount = 0;

        foreach (var middleMonth in middleMonths)
        {
            totalAmount += middleMonth.Amount;
        }

        var startBudget = BudgetByYearMonth(startFormat);
        var endBudget = BudgetByYearMonth(endFormat);

        var startDays = startBudget.Days() - start.Day + 1;
        var endDays = end.Day;

        return startBudget.DailyAmount() * startDays + endBudget.DailyAmount() * endDays + totalAmount;
    }

    private static Budget? BudgetByYearMonth(string startFormat)
    {
        return _budgetRepo.GetAll().FirstOrDefault(x => x.YearMonth == startFormat) ?? new Budget(startFormat, 0);
    }
}