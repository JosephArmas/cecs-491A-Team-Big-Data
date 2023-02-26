// Todo: 
/*
 * notify what a user their username when logged in
 * check if a user is first logged in -> update-profile view
 * input validations for login/registration?
 * home view
 * do error screen
*/

// reuse function to list on back button click
document.querySelector(".back-button").addEventListener("click", homeClicked());
// document.querySelector("#regBtn-submit").addEventListener("click", homeClicked());

function loginClicked()
{
    var anonContainer = document.querySelector(".anon-container");
    var loginContainer = document.querySelector(".login-container");
    anonContainer.style.display = "none";
    loginContainer.style.display = "block";
}

function homeClicked()
{

    var regContainer = document.querySelector(".registration-container");
    var anonContainer = document.querySelector(".anon-container");
    var loginContainer = document.querySelector(".login-container");
    anonContainer.style.display = "block";
    regContainer.style.display = "none";     
    loginContainer.style.display = "none";

}

function regClicked()
{
    var regContainer = document.querySelector(".registration-container");
    var anonContainer = document.querySelector(".anon-container");
    anonContainer.style.display = "none";
    regContainer.style.display = "block";
}