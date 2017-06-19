using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BloodAlcoholContent;

namespace BloodAlcoholContent.Objects
{
  public class Drink
  {
    private string _name;
    private string _drinkType;
    private double _abv;
    private double _cost;
    private int _id;

    public Drink(string Name, string DrinkType, double ABV, double Cost, int Id = 0)
    {
      _name = Name;
      _drinkType = DrinkType;
      _abv = ABV;
      _cost = Cost;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetDrinkType()
    {
      return _drinkType;
    }
    public double GetABV()
    {
      return _abv;
    }
    public double GetCost()
    {
      return _cost;
    }
    public override bool Equals(System.Object otherDrink)
    {
      if (!(otherDrink is Drink))
      {
        return false;
      }
      else
      {
        Drink newDrink = (Drink) otherDrink;
        bool idEquality = this.GetId() == newDrink.GetId();
        bool nameEquality = this.GetName() == newDrink.GetName();
        bool drinkTypeEquality = this.GetDrinkType() == newDrink.GetDrinkType();
        bool ABVEquality = this.GetABV() == newDrink.GetABV();
        bool costEquality = this.GetCost() == newDrink.GetCost();
        return (idEquality && nameEquality && drinkTypeEquality && ABVEquality && costEquality);
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM drinks;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Drink> GetAll()
    {
      List<Drink> allDrinks = new List<Drink>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM drinks ORDER BY drink_type;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int drinkId = rdr.GetInt32(0);
        string drinkName = rdr.GetString(1);
        string drinkType = rdr.GetString(2);
        double drinkABV = rdr.GetDouble(3);
        double drinkCost = rdr.GetDouble(4);
        Drink newDrink = new Drink(drinkName, drinkType, drinkABV, drinkCost, drinkId);
        allDrinks.Add(newDrink);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allDrinks;
    }

  }
}
