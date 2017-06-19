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
      _id = Id;
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

    public void SetId(int Id)
    {
      _id = Id;
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
        decimal drinkABV = rdr.GetDecimal(3);
        decimal drinkCost = rdr.GetDecimal(4);
        Drink newDrink = new Drink(drinkName, drinkType, Convert.ToDouble(drinkABV), Convert.ToDouble(drinkCost), drinkId);
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
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO drinks (name, drink_type, abv, cost) OUTPUT INSERTED.id VALUES (@DrinkName, @DrinkType, @DrinkABV, @DrinkCost)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@DrinkName";
      nameParam.Value = this.GetName();

      SqlParameter typeParam = new SqlParameter();
      typeParam.ParameterName = "@DrinkType";
      typeParam.Value = this.GetDrinkType();

      SqlParameter abvParam = new SqlParameter();
      abvParam.ParameterName = "@DrinkABV";
      abvParam.Value = this.GetABV();

      SqlParameter costParam = new SqlParameter();
      costParam.ParameterName = "@DrinkCost";
      costParam.Value = this.GetCost();

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(typeParam);
      cmd.Parameters.Add(costParam);
      cmd.Parameters.Add(abvParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static Drink Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM drinks WHERE id = @DrinkId", conn);

      SqlParameter drinkIdParameter = new SqlParameter();
      drinkIdParameter.ParameterName = "@DrinkId";
      drinkIdParameter.Value = id.ToString();

      cmd.Parameters.Add(drinkIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundDrinkId = 0;
      string foundDrinkName = null;
      string foundDrinkType = null;
      decimal foundDrinkABV = 0;
      decimal foundDrinkCost = 0;

      while(rdr.Read())
      {
        foundDrinkId = rdr.GetInt32(0);
        foundDrinkName = rdr.GetString(1);
        foundDrinkType = rdr.GetString(2);
        foundDrinkABV = rdr.GetDecimal(3);
        foundDrinkCost = rdr.GetDecimal(4);
      }
      Drink foundDrink = new Drink(foundDrinkName, foundDrinkType, Convert.ToDouble(foundDrinkABV), Convert.ToDouble(foundDrinkCost), foundDrinkId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundDrink;
    }
  }
}
