using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using BloodAlcoholContent;
using BloodAlcoholContent.Objects;

namespace BloodAlcoholContentTests
{
  [Collection("BloodAlcoholContentTests")]
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=bac_checker_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Bartender.DeleteAll();
      Patron.DeleteAll();
      Drink.DeleteAll();
      Food.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Patron.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualityOfPatronObjects()
    {
      Patron firstPatron = new Patron("Sharon Needles", "F", 125, 65);
      Patron secondPatron = new Patron("Sharon Needles", "F", 125, 65);
      Assert.Equal(firstPatron, secondPatron);
    }

    [Fact]
    public void Test_SavesToDatabase()
    {
      Patron testPatron = new Patron("Doneld Drumpf", "T", 200, 60);
      testPatron.Save();

      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_IdAssignationWorksAsPlanned()
    {
      Patron testPatron = new Patron("Gus Gus", "M", 160, 50);
      testPatron.Save();

      Patron savedPatron = Patron.GetAll()[0];

      int result = savedPatron.GetId();
      int testId = testPatron.GetId();

      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_FindsPatronInDatabaseWorks()
    {
      Patron testPatron = new Patron("Big Jim", "F", 125, 65);
      testPatron.Save();

      Patron result = Patron.Find(testPatron.GetId());

      Assert.Equal(testPatron, result);
    }

    [Fact]
    public void Test_ReturnsAllDrinksAddedToPatronsList()
    {
      Patron testPatron = new Patron("Tiger Woods", "M", 180, 76);
      testPatron.Save();
      Drink testDrink1 = new Drink("Patron", "Neat", 40, 10, 2);
      testDrink1.Save();
      Drink testDrink2 = new Drink("Car Bomb", "Mixed", 40, 12, 3);
      testDrink2.Save();

      testPatron.AddDrinkToOrdersTable(testDrink1);
      testPatron.AddDrinkToOrdersTable(testDrink2);

      List<Drink> savedDrinks = testPatron.GetDrinks();
      List<Drink> testList = new List<Drink> {testDrink1, testDrink2};
      Assert.Equal(testList, savedDrinks);
    }
    [Fact]
    public void Test_ReturnsPatronsBMI()
    {
      Patron testPatron = new Patron("Tiger Woods", "M", 180, 76);
      testPatron.Save();
      decimal testPatronBMI = testPatron.GetBMI();

      decimal expectedBMI = Math.Round(((testPatron.GetWeight() / (testPatron.GetHeight() * testPatron.GetHeight())) * 703), 4);
      Assert.Equal(expectedBMI, testPatronBMI);
    }
    [Fact]
    public void Test_ReturnsPatronsBAC()
    {
      Patron testPatron = new Patron("Gary Busey", "M", 220, 72);
      Drink testDrink = new Drink("Long Island Iced Tea", "Mixed", 40, 10, 4);
      testPatron.Save();
      testDrink.Save();

      testPatron.AddDrinkToOrdersTable(testDrink);

      decimal testPatronBAC = testPatron.GetPatronBAC();

      decimal expectedBAC = Math.Round((((Convert.ToDecimal(testDrink.GetABV())/100M * testDrink.GetInstances()) * 5.14M)/(testPatron.GetWeight() * .73M) - (.015M * 1M)), 4);

      Assert.Equal(expectedBAC, testPatronBAC);
    }
    [Fact]
    public void Test_PatronEatsFoodToReduceBAC()
    {
      Patron testPatron = new Patron("Kurt RockJaw", "M", 200, 72);
      testPatron.Save();
      Drink testDrink = new Drink("The Manliest Drink", "Goat Balls", 40, 20, 4);
      testDrink.Save();
      Drink testDrink2 = new Drink("The Manliest Drink", "Goat Balls", 40, 20, 4);
      testDrink2.Save();
      Food testFood = new Food("BEEF", "RAW. MEAT. EAT.", 12, 2);
      testFood.Save();
      Food testFood2 = new Food("BEEF", "RAW. MEAT. EAT.", 12, 2);
      testFood2.Save();

      testPatron.AddDrinkToOrdersTable(testDrink);
      testPatron.AddDrinkToOrdersTable(testDrink2);
      testPatron.AddFoodToOrdersTable(testFood);
      testPatron.AddFoodToOrdersTable(testFood2);

      decimal testPatronBAC = testPatron.GetPatronBAC();
      decimal expectedBAC = Math.Round((((Convert.ToDecimal(testDrink.GetABV())/100M * testDrink.GetInstances()) * 5.14M)/(testPatron.GetWeight() * .73M) - (.015M * 1M) - ((testFood.GetBACRemoval() + testFood2.GetBACRemoval()) / 100)), 6);

      Assert.Equal(expectedBAC, testPatronBAC);
    }
    [Fact]
    public void Test_ReturnsTimeDifference()
    {
      Patron testPatron = new Patron("Kronk", "M", 250, 78);
      testPatron.Save();
      DateTime testDateTime2 = new DateTime(2017, 6, 20, 16, 00, 00);
      testPatron.SetDateTimeNow();
      double number = Math.Round((testDateTime2 - testPatron.GetDateTimeNow()).TotalMinutes);

      Console.WriteLine(testPatron.GetDateTimeNow());
      Assert.Equal(number,  testPatron.GetTimeDifference(testDateTime2));
    }
  }
}
