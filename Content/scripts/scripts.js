function Person(name, age, weight, feet, inches, gender) {
  this.name = name;
  this.age = age;
  this.weight = weight;
  this.feet = feet;
  this.inches = inches;
  this.gender = gender;
}

Person.prototype.profile = function () {
  return this.name + this.age + this.weight + this.feet + this.inches + this.gender;
}


$(document).ready(function() {
  $('form').submit(function(event) {
    event.preventDefault();
    var name = $('#name').val();
    var age = $('#age').val();
    var weight = $('#weight').val();
    var feet = $('#feet').val();
    var inches = $('#inches').val();
    var gender = $('#gender').val();
    console.log(name, age, weight, feet, inches, gender);

    var newPerson = new Person(name, age, weight, feet, inches, gender);
    console.log(newPerson);

  });
});
