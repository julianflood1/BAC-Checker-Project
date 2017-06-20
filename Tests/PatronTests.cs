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
     Patron.DeleteAll();
     Drink.DeleteAll();
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
      //Arrange
      Patron testPatron = new Patron("Doneld Drumpf", "T", 200, 60);
      testPatron.Save();

      //Act
      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_IdAssignationWorksAsPlanned()
    {
      //Arrange
      Patron testPatron = new Patron("Gus Gus", "M", 160, 50);
      testPatron.Save();

      //Act
      Patron savedPatron = Patron.GetAll()[0];

      int result = savedPatron.GetId();
      int testId = testPatron.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_FindsPatronInDatabaseWorks()
    {
      //Arrange
      Patron testPatron = new Patron("Big Jim", "F", 125, 65);
      testPatron.Save();

      //Act
      Patron result = Patron.Find(testPatron.GetId());

      //Assert
      Assert.Equal(testPatron, result);
    }

    [Fact]
    public void Test_ReturnsAllDrinksAddedToPatronsList()
    {

      Patron testPatron = new Patron("Tiger Woods", "M", 180, 76);
      testPatron.Save();
      Drink testDrink1 = new Drink("Patron", "Neat", .40, 10, 2);
      testDrink1.Save();
      Drink testDrink2 = new Drink("Car Bomb", "Mixed", .40, 12, 3);
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
      Drink testDrink = new Drink("Long Island Iced Tea", "Mixed", .40, 10, 4);
      testPatron.Save();
      testDrink.Save();

      testPatron.AddDrinkToOrdersTable(testDrink);

      decimal testPatronBAC = testPatron.GetPatronBAC();

      decimal expectedBAC = 4;

      Assert.Equal(expectedBAC, testPatronBAC);
    }
    [Fact]
    public void Test_ReturnsTimeDifference()
    {
      Patron testPatron = new Patron("Kronk", "M", 250, 78);
      testPatron.Save();
      DateTime testDateTime2 = new DateTime(2017, 6, 20, 15, 00, 00);
      double number = 37;
      Assert.Equal(number, testPatron.GetTimeDifference(testDateTime2));
    }
  }
}
