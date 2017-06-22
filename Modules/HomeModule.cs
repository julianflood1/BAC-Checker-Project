using Nancy;
using Nancy.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using BloodAlcoholContent.Objects;

namespace BloodAlcoholContent
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/patrons"] = _ => {
        List<Patron> allPatrons = Patron.GetAll();
        return View["patrons.cshtml", allPatrons];
      };
      Get["/patrons/add"] = _ => {
        return View["patron_add.cshtml"];
      };
      Post["/patrons/add"] = _ => {
        Patron newPatron = new Patron(Request.Form["patron-name"], Request.Form["patron-gender"], Request.Form["patron-weight"], Request.Form["patron-height"]);
        newPatron.Save();
        List<Patron> allPatrons = Patron.GetAll();
        return View["patrons.cshtml", allPatrons];
      };
      Get["/patrons/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Patron selectedPatron = Patron.Find(parameters.id);
        List<Drink> allDrinks = Drink.GetAll();
        List<Drink> patronDrinks = selectedPatron.GetDrinks();
        List<Food> allFood = Food.GetAll();
        List<Food> patronFood = selectedPatron.GetFood();
        List<Bartender> patronBartender = selectedPatron.GetBartender();
        model.Add("patron", selectedPatron);
        model.Add("allFood", allFood);
        model.Add("allDrinks", allDrinks);
        model.Add("patronDrinks", patronDrinks);
        model.Add("patronBartender", patronBartender);
        model.Add("patronFood", patronFood);
        return View["patron.cshtml", model];
      };
      Post["/patrons/{id}/add_order"] = parameters => {
        Patron patron = Patron.Find(Request.Form["patron-id"]);
        Drink drink = Drink.Find(Request.Form["drink-id"]);
        Food food = Food.Find(Request.Form["food-id"]);
        patron.AddDrinkAndFoodToOrdersTable(drink, food);
        Dictionary<string, object> model = new Dictionary<string, object>();
        Patron selectedPatron = Patron.Find(parameters.id);
        List<Drink> allDrinks = Drink.GetAll();
        List<Drink> patronDrinks = selectedPatron.GetDrinks();
        List<Bartender> patronBartender = selectedPatron.GetBartender();
        List<Food> allFood = Food.GetAll();
        List<Food> patronFood = selectedPatron.GetFood();
        model.Add("patron", selectedPatron);
        model.Add("allFood", allFood);
        model.Add("allDrinks", allDrinks);
        model.Add("patronDrinks", patronDrinks);
        model.Add("patronBartender", patronBartender);
        model.Add("patronFood", patronFood);
        return View["patron.cshtml", model];
      };
      Delete["/patrons/{id}/delete"] = _ => {
        Patron selectedPatron = Patron.Find(Request.Form["patron-id"]);
        selectedPatron.Delete();
        List<Patron> allPatrons = Patron.GetAll();
        return View["index.cshtml", allPatrons];
      };
      Get["/bartenders"] = _ => {
        List<Bartender> allBartenders = Bartender.GetAll();
        return View["bartenders.cshtml", allBartenders];
      };
      Get["/bartenders/add"] = _ => {
        return View["bartenders_add.cshtml"];
      };
      Post["/bartenders/add"] = _ => {
        Bartender newBartender = new Bartender(Request.Form["bartender-name"]);
        newBartender.Save();
        List<Bartender> allBartenders = Bartender.GetAll();
        return View["bartenders.cshtml", allBartenders];
      };
      Get["/bartenders/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Bartender selectedBartender = Bartender.Find(parameters.id);
        List<Patron> bartenderPatrons = selectedBartender.GetPatrons();
        List<Patron> allPatrons = Patron.GetAll();
        model.Add("bartender", selectedBartender);
        model.Add("allPatrons", allPatrons);
        model.Add("bartenderPatrons", bartenderPatrons);
        return View["bartender.cshtml", model];
      };
      Post["/bartenders/{id}/add_patron"] = parameters => {
        Bartender bartender = Bartender.Find(Request.Form["bartender-id"]);
        Patron patron = Patron.Find(Request.Form["patron-id"]);
        bartender.AddPatronToOrdersTable(patron);
        Dictionary<string, object> model = new Dictionary<string, object>();
        Bartender selectedBartender = Bartender.Find(parameters.id);
        List<Patron> bartenderPatrons = selectedBartender.GetPatrons();
        List<Patron> allPatrons = Patron.GetAll();
        model.Add("bartender", selectedBartender);
        model.Add("allPatrons", allPatrons);
        model.Add("bartenderPatrons", bartenderPatrons);
        return View["bartender.cshtml", model];
      };
      Get["/bartenders/menu"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Drink> allDrinks = Drink.GetAll();
        List<Food> allFood = Food.GetAll();
        model.Add("allFood", allFood);
        model.Add("allDrinks", allDrinks);
        return View["bar_menu.cshtml", model];
      };
      Get["/drinks"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Drink> allDrinks = Drink.GetAll();
        List<Food> allFood = Food.GetAll();
        model.Add("allFood", allFood);
        model.Add("allDrinks", allDrinks);
        return View["bar_menu.cshtml", model];
      };
      Get["/drinks/add"] = _ => {
        return View["drinks_add.cshtml"];
      };
      Post["/drinks/add"] = _ => {
        Drink newDrink = new Drink(Request.Form["drink-name"], Request.Form["drink-type"], Request.Form["drink-abv"], Request.Form["drink-cost"], Request.Form["drink-instances"]);
        newDrink.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Drink> allDrinks = Drink.GetAll();
        List<Food> allFood = Food.GetAll();
        model.Add("allFood", allFood);
        model.Add("allDrinks", allDrinks);
        return View["bar_menu.cshtml", model];
      };
      Get["/food"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Drink> allDrinks = Drink.GetAll();
        List<Food> allFood = Food.GetAll();
        model.Add("allFood", allFood);
        model.Add("allDrinks", allDrinks);
        return View["bar_menu.cshtml", model];
      };
      Get["/food/add"] = _ => {
        return View["food_add.cshtml"];
      };
      Post["/food/add"] = _ => {
        Food newFood = new Food(Request.Form["food-type"], Request.Form["food-description"], Request.Form["food-cost"], Request.Form["food-bac-removal"]);
        newFood.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Drink> allDrinks = Drink.GetAll();
        List<Food> allFood = Food.GetAll();
        model.Add("allFood", allFood);
        model.Add("allDrinks", allDrinks);
        return View["bar_menu.cshtml", model];
      };
    }
  }
}
