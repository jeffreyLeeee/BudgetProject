using NUnit.Framework;


public class Tests
{
    private BudgetService _budgetService;

    [SetUp]
    public void Setup()
    {
        _budgetService = new BudgetService();
    }

    [Test]
    public void PartialMonth()
    {
        Assert.Pass();
    }
}