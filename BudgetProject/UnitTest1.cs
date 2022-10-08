using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;

public class Tests
{
    private BudgetService _budgetService;
    private IBudgetRepo _budgetRepo;

    [SetUp]
    public void Setup()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Test]
    public void PartialMonth()
    {
        GiveSingleMonthBudget();
        var query = _budgetService.Query(new DateTime(2022, 10, 1), new DateTime(2022, 10, 2));
        var expected = 200;
        Assert.AreEqual(expected, query);
    }

    [Test]
    public void FullMonth()
    {
        GiveSingleMonthBudget();
        var query = _budgetService.Query(new DateTime(2022, 10, 1), new DateTime(2022, 10, 31));
        var expected = 3100;
        Assert.AreEqual(expected, query);
    }

    [Test]
    public void Invalidate()
    {
        GiveSingleMonthBudget();
        var query = _budgetService.Query(new DateTime(2022, 10, 21), new DateTime(2022, 10, 1));
        var expected = 0;
        Assert.AreEqual(expected, query);
    }

    [Test]
    public void BudgetNotExists()
    {
        GiveSingleMonthBudget();
        var query = _budgetService.Query(new DateTime(2021, 4, 1), new DateTime(2021, 4, 1));
        var expected = 0;
        Assert.AreEqual(expected, query);
    }

    [Test]
    public void BudgetOverTwoMonths()
    {
        GiveMultiMonthBudget();
        var query = _budgetService.Query(new DateTime(2022, 10, 1), new DateTime(2022, 11, 30));
        var expected = 3100 + 300;
        Assert.AreEqual(expected, query);
    }

    private void GiveSingleMonthBudget()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>
        {
            new("202210", 3100),
        });
    }

    private void GiveMultiMonthBudget()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>
        {
            new("202210", 3100),
            new("202211", 300),
            new("202212", 31),
        });
    }
}