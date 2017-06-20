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
  public class BartenderTest : IDisposable
  {
    public BartenderTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=bac_checker_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Bartender.DeleteAll();
      Drink.DeleteAll();
      Patron.DeleteAll();
      Food.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     int result = Bartender.GetAll().Count;
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualityOfBartenderObjects()
    {
     Bartender firstBartender = new Bartender("Bob");
     Bartender secondBartender = new Bartender("Bob");
     Assert.Equal(firstBartender, secondBartender);
    }
    [Fact]
    public void Test_SavesToDatabase()
    {
      //Arrange
      Bartender testBartender = new Bartender("Chet Stedman");
      testBartender.Save();

      //Act
      List<Bartender> result = Bartender.GetAll();
      List<Bartender> testList = new List<Bartender>{testBartender};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_IdAssignationWorksAsPlanned()
    {
      //Arrange
      Bartender testBartender = new Bartender("Ysma");
      testBartender.Save();

      //Act
      Bartender savedBartender = Bartender.GetAll()[0];

      int result = savedBartender.GetId();
      int testId = testBartender.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_FindsBartenderInDatabaseWorks()
    {
      //Arrange
      Bartender testBartender = new Bartender("Kronk");
      testBartender.Save();

      //Act
      Bartender result = Bartender.Find(testBartender.GetId());

      //Assert
      Assert.Equal(testBartender, result);
    }
  }
}
