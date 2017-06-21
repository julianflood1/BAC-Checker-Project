using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BloodAlcoholContent;

namespace BloodAlcoholContent.Objects
{
  public class Food
  {
    private string _foodType;
    private string _description;
    private decimal _cost;
    private int _bac_removal;
    private int _id;

    public Food(string FoodType, string Description, double Cost, int bacRemoval = 0, int Id = 0)
    {
      _id = Id;
      _foodType = FoodType;
      _description = Description;
      _cost = Convert.ToDecimal(Cost);
      _bac_removal = bacRemoval;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetFoodType()
    {
      return _foodType;
    }
    public string GetDescription()
    {
      return _description;
    }
    public decimal GetCost()
    {
      return _cost;
    }
    public int GetBACRemoval()
    {
      return _bac_removal;
    }
    public void SetId(int Id)
    {
      _id = Id;
    }
    public void SetBACRemoval(int bacRemovalValue)
    {
      _bac_removal = bacRemovalValue;
    }

    public override bool Equals(System.Object otherFood)
    {
      if (!(otherFood is Food))
      {
        return false;
      }
      else
      {
        Food newFood = (Food) otherFood;
        bool idEquality = this.GetId() == newFood.GetId();
        bool foodTypeEquality = this.GetFoodType() == newFood.GetFoodType();
        bool descriptionEquality = this.GetDescription() == newFood.GetDescription();
        bool costEquality = this.GetCost() == newFood.GetCost();
        bool bacRemovalEquality = this.GetBACRemoval() == newFood.GetBACRemoval();
        return (idEquality && foodTypeEquality && descriptionEquality && costEquality && bacRemovalEquality);
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM foods;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Food> GetAll()
    {
      List<Food> allFoods = new List<Food>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM foods;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int foodId = rdr.GetInt32(0);
        string foodType = rdr.GetString(1);
        string foodDescription = rdr.GetString(2);
        decimal foodCost = rdr.GetDecimal(3);
        int foodBACRemovalValue = rdr.GetInt32(4);
        Food newFood = new Food(foodType, foodDescription, Convert.ToDouble(foodCost), foodBACRemovalValue, foodId);
        allFoods.Add(newFood);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allFoods;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO foods (food_type, description, cost, bac_removal) OUTPUT INSERTED.id VALUES (@FoodType, @FoodDescription, @FoodCost, @FoodBACRemovalValue)", conn);

      SqlParameter typeParam = new SqlParameter();
      typeParam.ParameterName = "@FoodType";
      typeParam.Value = this.GetFoodType();

      SqlParameter descriptionParam = new SqlParameter();
      descriptionParam.ParameterName = "@FoodDescription";
      descriptionParam.Value = this.GetDescription();

      SqlParameter costParam = new SqlParameter();
      costParam.ParameterName = "@FoodCost";
      costParam.Value = this.GetCost();

      SqlParameter bacRemovalParam = new SqlParameter();
      bacRemovalParam.ParameterName = "@FoodBACRemovalValue";
      bacRemovalParam.Value = this.GetBACRemoval();

      cmd.Parameters.Add(typeParam);
      cmd.Parameters.Add(costParam);
      cmd.Parameters.Add(descriptionParam);
      cmd.Parameters.Add(bacRemovalParam);

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
    public static Food Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM foods WHERE id = @FoodId", conn);

      SqlParameter foodIdParameter = new SqlParameter();
      foodIdParameter.ParameterName = "@FoodId";
      foodIdParameter.Value = id.ToString();

      cmd.Parameters.Add(foodIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundFoodId = 0;
      string foundFoodType = null;
      string foundFoodDescription = null;
      decimal foundFoodCost = 0;
      int foundFoodBACRemovalValue = 0;

      while(rdr.Read())
      {
        foundFoodId = rdr.GetInt32(0);
        foundFoodType = rdr.GetString(1);
        foundFoodDescription = rdr.GetString(2);
        foundFoodCost = rdr.GetDecimal(3);
        foundFoodBACRemovalValue = rdr.GetInt32(4);
      }
      Food foundFood = new Food(foundFoodType, foundFoodDescription, Convert.ToDouble(foundFoodCost), foundFoodBACRemovalValue, foundFoodId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundFood;
    }
  }
}
