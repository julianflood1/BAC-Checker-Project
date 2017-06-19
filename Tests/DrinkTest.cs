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
     Drink firstDrink = new Drink("Negroni", "Mixed", 1.5, 11.50);
     Drink secondDrink = new Drink("Negroni", "Mixed", 1.5, 11.50);
     Assert.Equal(firstDrink, secondDrink);
    }
  }
}
