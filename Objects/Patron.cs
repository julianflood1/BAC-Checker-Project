using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BloodAlcoholContent;


namespace BloodAlcoholContent.Objects
{
  public class Patron
  {
    private int _id;
    private string _name;
    private string _gender;
    private int _weight;
    private int _height;

    public Patron(string Name, string Gender, int Weight, int Height, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _gender = Gender;
      _weight = Weight;
      _height = Height;
    }

    public string GetName()
    {
      return _name;
    }
    public string GetGender()
    {
      return _gender;
    }
    public int GetHeight()
    {
      return _height;
    }
    public int GetWeight()
    {
      return _weight;
    }
    public int GetBMI()
    {
      return ((_weight) / (_height * _height)) * 703;
    }
    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = this.GetId() == newPatron.GetId();
        bool nameEquality = this.GetName() == newPatron.GetName();
        bool genderEquality = this.GetGender() == newPatron.GetGender();
        bool weightEquality = this.GetWeight() == newPatron.GetWeight();
        bool heightEquality = this.GetHeight() == newPatron.GetHeight();
        return (idEquality && nameEquality && genderEquality && weightEquality && heightEquality);
      }
    }
//MAYBE SETTERS

//END MAYBE SETTERS
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons ORDER BY name;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        string patronGender = rdr.GetString(2);
        int patronWeight = rdr.GetInt32(3);
        int patronHeight = rdr.GetInt32(4);
        Patron newPatron = new Patron(patronName, patronGender, patronWeight, patronHeight, patronId);
        allPatrons.Add(newPatron);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allPatrons;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name, gender, height, weight, bmi) OUTPUT INSERTED.id VALUES (@PatronName, @PatronGender, @PatronHeight, @PatronWeight, @PatronBMI)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@PatronName";
      nameParam.Value = this.GetName();

      SqlParameter genderParam = new SqlParameter();
      genderParam.ParameterName = "@PatronGender";
      genderParam.Value = this.GetGender();

      SqlParameter heightParam = new SqlParameter();
      heightParam.ParameterName = "@PatronHeight";
      heightParam.Value = this.GetHeight();

      SqlParameter weightParam = new SqlParameter();
      weightParam.ParameterName = "@PatronWeight";
      weightParam.Value = this.GetWeight();

      SqlParameter bmiParam = new SqlParameter();
      bmiParam.ParameterName = "@PatronBMI";
      bmiParam.Value = this.GetBMI();

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(genderParam);
      cmd.Parameters.Add(weightParam);
      cmd.Parameters.Add(heightParam);
      cmd.Parameters.Add(bmiParam);

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

    public static Patron Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = id.ToString();

      cmd.Parameters.Add(patronIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundPatronId = 0;
      string foundPatronName = null;
      string foundPatronGender = null;
      int foundPatronHeight = 0;
      int foundPatronWeight = 0;

      while(rdr.Read())
      {
        foundPatronId = rdr.GetInt32(0);
        foundPatronName = rdr.GetString(1);
        foundPatronGender = rdr.GetString(2);
        foundPatronHeight = rdr.GetInt32(3);
        foundPatronWeight = rdr.GetInt32(4);
      }
      Patron foundPatron = new Patron(foundPatronName, foundPatronGender, foundPatronHeight, foundPatronWeight, foundPatronId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundPatron;
    }

    public List<Drink> GetDrinks()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT drinks.* FROM patrons JOIN orders ON (patrons.id = orders.patrons_id) JOIN drinks ON (orders.drinks_id = drinks.id) WHERE patrons.id = @PatronId;", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(patronIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Drink> drinks = new List<Drink>{};

      while(rdr.Read())
      {
        int drinkId = rdr.GetInt32(0);
        string drinkName = rdr.GetString(1);
        string drinkType = rdr.GetString(2);
        decimal drinkABV = rdr.GetDecimal(3);
        decimal drinkCost = rdr.GetDecimal(4);
        Drink newDrink = new Drink(drinkName, drinkType, Convert.ToDouble(drinkABV), Convert.ToDouble(drinkCost), drinkId);
        drinks.Add(newDrink);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return drinks;
    }
    public void AddDrinkToOrdersTable(Drink newDrink)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO orders (patrons_id, drinks_id) VALUES (@PatronId, @DrinkId);", conn);
      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId();
      cmd.Parameters.Add(patronIdParameter);

      SqlParameter drinkIdParameter = new SqlParameter();
      drinkIdParameter.ParameterName = "@DrinkId";
      drinkIdParameter.Value = newDrink.GetId();
      cmd.Parameters.Add(drinkIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

  }
}
