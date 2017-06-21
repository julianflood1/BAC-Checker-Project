$(document).ready(function() {

$('#start-drinking').click(function(event) {
  event.preventDefault();

  $('#hidden-form').fadeIn(1000);
  $('#start-drinking').hide();
  });
});
