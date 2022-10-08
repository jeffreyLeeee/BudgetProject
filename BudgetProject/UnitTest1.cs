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

    [Test]
    public void BudgetOverMultiMonths()
    {
        GiveMultiMonthBudget();
        var query = _budgetService.Query(new DateTime(2022, 10, 31), new DateTime(2022, 12, 7));
        var expected = 100 + 300 + 7;
        Assert.AreEqual(expected, query);
    }

    [Test]
    public void BudgetOverYears()
    {
        GiveMultiMonthBudget();
        var query = _budgetService.Query(new DateTime(2022, 10, 31), new DateTime(2023, 10, 20));
        var expected = 100 + 300 + 31 + 20000;
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
            new("202310",31000)
        });
    }
}