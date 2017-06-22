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
    private decimal _weight;
    private decimal _height;
    private decimal _bmi = 0.00M;
    private DateTime _saveStaticTime;

    public Patron(string Name, string Gender, decimal Weight, decimal Height, decimal BMI = 0.00M, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _gender = Gender;
      _weight = Weight;
      _height = Height;
      _bmi = BMI;
    }

    public string GetName()
    {
      return _name;
    }
    public string GetGender()
    {
      return _gender;
    }
    public decimal GetHeight()
    {
      return _height;
    }
    public decimal GetWeight()
    {
      return _weight;
    }
    public decimal GetBMI()
    {

      _bmi = Math.Round(((_weight / (_height * _height)) * 703), 4);
      return _bmi;
    }
    public int GetId()
    {
      return _id;
    }
    public DateTime GetDateTimeNow()
    {
      return _saveStaticTime;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
//WILL BE SET ON BUTTON PRESS IN NANCY
    public void SetDateTimeNow()
    {
      _saveStaticTime = DateTime.Now;
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
        bool bmiEquality = this.GetBMI() == newPatron.GetBMI();
        return (idEquality && nameEquality && genderEquality && weightEquality && heightEquality && bmiEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public double GetTimeDifference(DateTime userDateTime)
    {

      TimeSpan timeDiff = userDateTime - this.GetDateTimeNow();
      double fixedTimeDiff = Math.Round(timeDiff.TotalMinutes);
      Console.WriteLine(fixedTimeDiff);
      return fixedTimeDiff;
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
        decimal patronWeight = rdr.GetInt32(3);
        decimal patronHeight = rdr.GetInt32(4);
        decimal patronBMI = rdr.GetDecimal(5);
        Patron newPatron = new Patron(patronName, patronGender, patronWeight, patronHeight, patronBMI, patronId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name, gender, weight, height, bmi) OUTPUT INSERTED.id VALUES (@PatronName, @PatronGender, @PatronWeight, @PatronHeight, @PatronBMI)", conn);

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
      // Console.WriteLine("this.GetBMI in Save: " + bmiParam.Value);

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
      decimal foundPatronWeight = 0.00M;
      decimal foundPatronHeight = 0.00M;
      decimal foundPatronBMI = 0.00M;

      while(rdr.Read())
      {
        foundPatronId = rdr.GetInt32(0);
        foundPatronName = rdr.GetString(1);
        foundPatronGender = rdr.GetString(2);
        foundPatronWeight = rdr.GetInt32(3);
        foundPatronHeight = rdr.GetInt32(4);
        foundPatronBMI = rdr.GetDecimal(5);
      }
      Patron foundPatron = new Patron(foundPatronName, foundPatronGender, foundPatronWeight, foundPatronHeight, foundPatronBMI, foundPatronId);

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
        int drinkInstances = rdr.GetInt32(5);
        Drink newDrink = new Drink(drinkName, drinkType, Convert.ToDouble(drinkABV), Convert.ToDouble(drinkCost), drinkInstances, drinkId);
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


    public List<Food> GetFood()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT foods.* FROM patrons JOIN orders ON (patrons.id = orders.patrons_id) JOIN foods ON (orders.foods_id = foods.id) WHERE patrons.id = @PatronId;", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(patronIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Food> foods = new List<Food>{};

      while(rdr.Read())
      {
        int foodId = rdr.GetInt32(0);
        string foodType = rdr.GetString(1);
        string foodDescription = rdr.GetString(2);
        decimal foodCost = rdr.GetDecimal(3);
        Food newFood = new Food(foodType, foodDescription, Convert.ToDouble(foodCost), foodId);
        foods.Add(newFood);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foods;
    }
    public void AddDrinkAndFoodToOrdersTable(Drink newDrink = null, Food newFood = null)
    {
      if (newFood == null)
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO orders (patrons_id, drinks_id, foods_id) VALUES (@PatronId, @DrinkId, @FoodId);", conn);
        SqlParameter patronIdParameter = new SqlParameter();
        patronIdParameter.ParameterName = "@PatronId";
        patronIdParameter.Value = this.GetId();
        cmd.Parameters.Add(patronIdParameter);

        SqlParameter drinkIdParameter = new SqlParameter();
        drinkIdParameter.ParameterName = "@DrinkId";
        drinkIdParameter.Value = newDrink.GetId();
        cmd.Parameters.Add(drinkIdParameter);

        SqlParameter foodIdParameter = new SqlParameter();
        foodIdParameter.ParameterName = "@FoodId";
        foodIdParameter.Value = 0;
        cmd.Parameters.Add(foodIdParameter);

        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }
      if (newDrink == null)
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO orders (patrons_id, drinks_id, foods_id) VALUES (@PatronId, @DrinkId, @FoodId);", conn);
        SqlParameter patronIdParameter = new SqlParameter();
        patronIdParameter.ParameterName = "@PatronId";
        patronIdParameter.Value = this.GetId();
        cmd.Parameters.Add(patronIdParameter);

        SqlParameter foodIdParameter = new SqlParameter();
        foodIdParameter.ParameterName = "@FoodId";
        foodIdParameter.Value = newFood.GetId();
        cmd.Parameters.Add(foodIdParameter);

        SqlParameter drinkIdParameter = new SqlParameter();
        drinkIdParameter.ParameterName = "@DrinkId";
        drinkIdParameter.Value = 0;
        cmd.Parameters.Add(drinkIdParameter);

        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }
    }
    // public void AddDrinkAndFoodToOrdersTable(Drink newDrink)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO orders (patrons_id, drinks_id) VALUES (@PatronId, @DrinkId);", conn);
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName = "@PatronId";
    //   patronIdParameter.Value = this.GetId();
    //   cmd.Parameters.Add(patronIdParameter);
    //
    //   SqlParameter drinkIdParameter = new SqlParameter();
    //   drinkIdParameter.ParameterName = "@DrinkId";
    //   drinkIdParameter.Value = newDrink.GetId();
    //   cmd.Parameters.Add(drinkIdParameter);
    //
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public void AddFoodToOrdersTable(Food newFood)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO orders (patrons_id, foods_id) VALUES (@PatronId, @FoodId);", conn);
    //   SqlParameter patronIdParameter = new SqlParameter();
    //   patronIdParameter.ParameterName = "@PatronId";
    //   patronIdParameter.Value = this.GetId();
    //   cmd.Parameters.Add(patronIdParameter);
    //
    //   SqlParameter foodIdParameter = new SqlParameter();
    //   foodIdParameter.ParameterName = "@FoodId";
    //   foodIdParameter.Value = newFood.GetId();
    //   cmd.Parameters.Add(foodIdParameter);
    //
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    public decimal GetPatronBAC()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT SUM(drinks.abv), SUM(drinks.instances) FROM patrons JOIN orders ON (patrons.id = orders.patrons_id) JOIN drinks ON (orders.drinks_id = drinks.id) WHERE patrons.id = @PatronId;", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId().ToString();
      Console.WriteLine("THIS PATRON id : " + this.GetId());

      cmd.Parameters.Add(patronIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      decimal userWeight = this.GetWeight();
      string userGender = this.GetGender();
      decimal drinkABV = 0.00M;
      decimal patronBAC = 0.0000M;
      int drinkInstances = 0;
      int foodBACRemovalValue = 0;
      while(rdr.Read())
      {
        drinkABV = rdr.GetDecimal(0);
        Console.WriteLine("drinkABV: " + drinkABV);
        drinkInstances = rdr.GetInt32(1);
        // foodBACRemovalValue = rdr.GetInt32(2);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      if (userGender == "M")
      {
        patronBAC = (((drinkABV/100 * drinkInstances) * 5.14M)/(userWeight * .73M) - (.015M * 1M)) - (foodBACRemovalValue / 100);
      }
      if (userGender == "F")
      {
        patronBAC = (((drinkABV/100 * drinkInstances) * 5.14M)/(userWeight * .66M) - (.015M * 1M)) - (foodBACRemovalValue / 100);
      }
      if (userGender == "X")
      {
        patronBAC = (((drinkABV/100 * drinkInstances) * 5.14M)/(userWeight * .69M) - (.015M * 1M)) - (foodBACRemovalValue / 100);
      }
      decimal fixedPatronBAC = Math.Round(patronBAC, 6);
      return fixedPatronBAC;
    }
  }
}
