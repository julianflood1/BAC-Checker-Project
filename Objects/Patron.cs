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

  }
}
