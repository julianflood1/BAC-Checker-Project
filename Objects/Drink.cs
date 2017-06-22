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
    private int _instances;
    private int _id;

    private decimal _ozAlcohol = .6M;

    public Drink(string Name, string DrinkType, double ABV, double Cost, int Instances, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _drinkType = DrinkType;
      _abv = ABV;
      _cost = Cost;
      _instances = Instances;
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
    public int GetInstances()
    {
      return _instances;
    }
    public decimal GetOzAlcohol()
    {
      return _ozAlcohol;
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
        bool instancesEquality = this.GetInstances() == newDrink.GetInstances();
        return (idEquality && nameEquality && drinkTypeEquality && ABVEquality && costEquality && instancesEquality);
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

      SqlCommand cmd = new SqlCommand("SELECT * FROM drinks ORDER BY abv;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int drinkId = rdr.GetInt32(0);
        string drinkName = rdr.GetString(1);
        string drinkType = rdr.GetString(2);
        decimal drinkABV = rdr.GetDecimal(3);
        decimal drinkCost = rdr.GetDecimal(4);
        int drinkInstances = rdr.GetInt32(5);
        Drink newDrink = new Drink(drinkName, drinkType, Convert.ToDouble(drinkABV), Convert.ToDouble(drinkCost), drinkInstances, drinkId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO drinks (name, drink_type, abv, cost, instances) OUTPUT INSERTED.id VALUES (@DrinkName, @DrinkType, @DrinkABV, @DrinkCost, @DrinkInstances)", conn);

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

      SqlParameter instancesParam = new SqlParameter();
      instancesParam.ParameterName = "@DrinkInstances";
      instancesParam.Value = this.GetInstances();

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(typeParam);
      cmd.Parameters.Add(costParam);
      cmd.Parameters.Add(abvParam);
      cmd.Parameters.Add(instancesParam);

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
      int foundDrinkInstances = 0;

      while(rdr.Read())
      {
        foundDrinkId = rdr.GetInt32(0);
        foundDrinkName = rdr.GetString(1);
        foundDrinkType = rdr.GetString(2);
        foundDrinkABV = rdr.GetDecimal(3);
        foundDrinkCost = rdr.GetDecimal(4);
        foundDrinkInstances = rdr.GetInt32(5);
      }
      Drink foundDrink = new Drink(foundDrinkName, foundDrinkType, Convert.ToDouble(foundDrinkABV), Convert.ToDouble(foundDrinkCost), foundDrinkInstances, foundDrinkId);

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
