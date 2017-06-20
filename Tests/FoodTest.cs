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
  public class FoodTest : IDisposable
  {
    public FoodTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=bac_checker_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Bartender.DeleteAll();
      Food.DeleteAll();
      Drink.DeleteAll();
      Patron.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Food.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualityOfFoodObjects()
    {
      Food firstFood = new Food("Nachos", "BEST BEET NACHOS IN THE WORLD", 4.50);
      Food secondFood = new Food("Nachos", "BEST BEET NACHOS IN THE WORLD", 4.50);
      Assert.Equal(firstFood, secondFood);
    }
    [Fact]
    public void Test_SavesToDatabase()
    {
      //Arrange
      Food testFood = new Food("Mac N Cheese", "Loads of cheese with a little bit of mac.", 13.00);
      testFood.Save();

      //Act
      List<Food> result = Food.GetAll();
      List<Food> testList = new List<Food>{testFood};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_IdAssignationWorksAsPlanned()
    {
      //Arrange
      Food testFood = new Food("Nachos", "BEST BEET NACHOS IN THE WORLD", 4.50);
      testFood.Save();

      //Act
      Food savedFood = Food.GetAll()[0];

      int result = savedFood.GetId();
      int testId = testFood.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_FindsFoodInDatabaseWorks()
    {
      //Arrange
      Food testFood = new Food("Mac N Cheese", "Loads of cheese with a little bit of mac.", 13.00);
      testFood.Save();

      //Act
      Food result = Food.Find(testFood.GetId());

      //Assert
      Assert.Equal(testFood, result);
    }
  }
}
