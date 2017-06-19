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
     Patron firstPatron = new Patron("Sharon Needles", "F", 72, 180);
     Patron secondPatron = new Patron("Sharon Needles", "F", 72, 180);
     Assert.Equal(firstPatron, secondPatron);
    }

    [Fact]
    public void Test_SavesToDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Doneld Drumpf", "T", 66, 250);
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
      Patron testPatron = new Patron("Gus Gus", "M", 2, 1);
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
      Patron testPatron = new Patron("Big Jim", "F", 61, 125);
      testPatron.Save();

      //Act
      Patron result = Patron.Find(testPatron.GetId());

      //Assert
      Assert.Equal(testPatron, result);
    }

    [Fact]
    public void Test_ReturnsAllDrinksAddedToPatronsList()
    {

      Patron testPatron = new Patron("Tiger Woods", "M", 70, 185);
      testPatron.Save();
      Drink testDrink1 = new Drink("Patron", "Neat", 2, 10);
      testDrink1.Save();
      Drink testDrink2 = new Drink("Car Bomb", "Mixed", 1.5, 12);
      testDrink2.Save();

      testPatron.AddDrinkToOrdersTable(testDrink1);
      testPatron.AddDrinkToOrdersTable(testDrink2);

      List<Drink> savedDrinks = testPatron.GetDrinks();
      List<Drink> testList = new List<Drink> {testDrink1, testDrink2};
      // Console.WriteLine(Convert.ToDouble(testPatron.GetBMI()));
      Assert.Equal(testList, savedDrinks);
    }
    [Fact]
    public void Test_ReturnsPatronsBMI()
    {

      Patron testPatron = new Patron("Tiger Woods", "M", 70, 185);
      testPatron.Save();
      decimal testPatronBMI = testPatron.GetBMI();

      decimal expectedBMI = 26.5M;
      // Console.WriteLine(Convert.ToDouble(testPatron.GetBMI()));
      Assert.Equal(expectedBMI, testPatronBMI);
    }
  }
}
