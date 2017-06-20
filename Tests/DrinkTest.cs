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
  public class DrinkTest : IDisposable
  {
    public DrinkTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=bac_checker_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
     Drink.DeleteAll();
     Patron.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     int result = Drink.GetAll().Count;
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualityOfDrinkObjects()
    {
     Drink firstDrink = new Drink("Negroni", "Mixed", .40, 11.50, 2);
     Drink secondDrink = new Drink("Negroni", "Mixed", .40, 11.50, 2);
     Assert.Equal(firstDrink, secondDrink);
    }
    [Fact]
    public void Test_SavesToDatabase()
    {
      //Arrange
      Drink testDrink = new Drink("Four Horsemen", "Mixed", .40, 13.00, 4);
      testDrink.Save();

      //Act
      List<Drink> result = Drink.GetAll();
      List<Drink> testList = new List<Drink>{testDrink};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_IdAssignationWorksAsPlanned()
    {
      //Arrange
      Drink testDrink = new Drink("Four Horsemen", "Mixed", .40, 13.00, 4);
      testDrink.Save();

      //Act
      Drink savedDrink = Drink.GetAll()[0];

      int result = savedDrink.GetId();
      int testId = testDrink.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_FindsDrinkInDatabaseWorks()
    {
      //Arrange
      Drink testDrink = new Drink("Negroni", "Mixed", .40, 11.50, 2);
      testDrink.Save();

      //Act
      Drink result = Drink.Find(testDrink.GetId());

      //Assert
      Assert.Equal(testDrink, result);
    }
  }
}
