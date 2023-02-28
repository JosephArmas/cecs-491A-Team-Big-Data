// Todo: 
/*
 * notify what a user their username when logged in
 * check if a user is first logged in -> update-profile view
 * input validations for login/registration?
 * home view - hamburger menu
 * do error screen - show message validations & status codes
*/

// reuse function to list on back button click
document.querySelector(".back-button").addEventListener("click", homeClicked());
// document.querySelector("#regBtn-submit").addEventListener("click", homeClicked());
document.querySelector(".home-logoutBtn").addEventListener("click", homeClicked());

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
    var homeContainer = document.querySelector(".home-container")
    anonContainer.style.display = "block";
    homeContainer.style.display = "none";
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

function regView()
{
    var homeContainer = document.querySelector(".home-container");
    var anonContainer = document.querySelector(".anon-container");
    var otpContainer =document.querySelector(".otp-container");
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
    homeContainer.style.display = "block";
}

function otpView()
{
    var loginContainer = document.querySelector(".login-container");
    var anonContainer = document.querySelector(".anon-container");
    var otpContainer =document.querySelector(".otp-container");
    loginContainer.style.display = "none";
    anonContainer.style.display = "none";
    otpContainer.style.display = "block";

}