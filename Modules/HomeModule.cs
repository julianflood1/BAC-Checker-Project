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
        return View["success.cshtml"];
      };
      Get["/patrons/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Patron selectedPatron = Patron.Find(parameters.id);
        List<Drink> allDrinks = Drink.GetAll();
        List<Drink> patronDrinks = selectedPatron.GetDrinks();
        model.Add("patron", selectedPatron);
        model.Add("allDrinks", allDrinks);
        model.Add("patronDrinks", patronDrinks);
        return View["patron.cshtml", model];
      };
      Post["/patrons/{id}/add_drink"] = _ => {
        Patron patron = Patron.Find(Request.Form["patron-id"]);
        Drink drink = Drink.Find(Request.Form["drink-id"]);
        patron.AddDrinkToOrdersTable(drink);
        return View["success.cshtml"];
      };
      Get["/drinks"] = _ => {
        List<Drink> allDrinks = Drink.GetAll();
        return View["drinks.cshtml", allDrinks];
      };
      Get["/drinks/add"] = _ => {
        return View["drinks_add.cshtml"];
      };
      Post["/drinks/add"] = _ => {
        
        Drink newDrink = new Drink(Request.Form["drink-name"], Request.Form["drink-type"], Request.Form["drink-abv"], Request.Form["drink-cost"], Request.Form["drink-instances"]);
        newDrink.Save();
        return View["success.cshtml"];
      };
      Get["/drinks/{id}"] = parameters => {
        Drink selectedDrink = Drink.Find(parameters.id);
        return View["drink.cshtml", selectedDrink];
      };
    }
  }
}
