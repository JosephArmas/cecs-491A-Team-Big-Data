// Todo: 
/*
 * check if a user is first logged in -> update-profile view
 * home view - hamburger menu
*/

// reuse function to list on back button click
document.querySelector(".back-button").addEventListener("click", homeClicked());
// document.querySelector("#regBtn-submit").addEventListener("click", homeClicked());
//document.querySelector(".home-logoutBtn").addEventListener("click", homeClicked());

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
    var otpContainer = document.querySelector(".otp-container");
    var anonContainer = document.querySelector(".anon-container");
    var loginContainer = document.querySelector(".login-container");
    var homeContainer = document.querySelector(".home-container")
    var globalErrors = document.querySelector("#errors");
    var analyticsView = document.querySelector(".analytics-container");
    anonContainer.style.display = "block";
    otpContainer.style.display="none";
    homeContainer.style.display = "none";
    regContainer.style.display = "none";     
    loginContainer.style.display = "none";
    analyticsView.style.display = "none";
    globalErrors.innerHTML = "";


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
    var globalErrors = document.querySelector("#errors");
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
    homeContainer.style.display = "block";
}

function showOtp()
{
    var otpContainer = document.querySelector(".otp-container");
    var loginContainer = document.querySelector(".login-container");
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
}

function IsValidPassword(password)
{
    let passwordAllowed = new RegExp("^[a-zA-Z0-9@.,!\s-]")
    // var passwordAllowed = /"^[a-zA-Z0-9@.,!\s-]"/;
    if (passwordAllowed.test() && password.length > 7)
    {
        return true;
    } else
    {
        return false;
    }
}

function IsValidEmail(email)
{
    let emailAllowed = new RegExp("^[a-zA-Z0-9@.-]*$");
    if (emailAllowed.test(email) && email.includes("@") && !email.startsWith("@"))     
    {
        return true;
    } else
    {
        return false;
    }

}