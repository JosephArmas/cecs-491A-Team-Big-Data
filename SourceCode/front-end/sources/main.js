// Todo: 
/*
 * check if a user is first logged in -> update-profile view
 * home view - hamburger menu
 * Refactor for Login 
 * Refactor for Registration
 * Move Analytics to Analytics.js
 * All of main is anon view -> Homeview(admin or reg user)
*/

document.querySelector("#analytics-logout").addEventListener("click", homeClicked);
document.querySelector("#admin-logout").addEventListener("click", homeClicked);
document.querySelector("#register").addEventListener("click", regClicked);
// * Considered cross cutting so can be called anywhere
var errorsDiv = document.getElementById('errors');


function loginClicked()
{
    var anonContainer = document.querySelector(".anon-container");
    var loginContainer = document.querySelector(".login-container");
    let loginForm = document.createElement('form');
    loginForm.setAttribute('action','/');
    loginForm.setAttribute('method','POST');
    loginForm.setAttribute('id','login-form');
    let backBtnDiv = document.createElement('div');
    backBtnDiv.setAttribute('class','back-button');
    let backBtn = document.createElement('button');
    backBtn.setAttribute('type','button');
    backBtn.textContent = "Back";
    backBtn.addEventListener('click',homeClicked);
    // * Add the back button in side the div element
    backBtnDiv.appendChild(backBtn);
    // * Add this div element inside of the form element
    loginForm.appendChild(backBtnDiv);


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
    var analyticsView = document.querySelector(".analytics-container");
    var adminView = document.querySelector(".home-admin-container");
    anonContainer.style.display = "block";
    otpContainer.style.display="none";
    homeContainer.style.display = "none";
    regContainer.style.display = "none";     
    loginContainer.style.display = "none";
    analyticsView.style.display = "none";
    adminView.style.display = "none";
    // errorsDiv.innerHTML = "";



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
    // var globalErrors = document.querySelector("#errors");
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

function adminView()
{
    var homeContainer = document.querySelector(".home-admin-container");
    var anonContainer = document.querySelector(".anon-container");
    var analyticsView = document.querySelector(".analytics-container");
    var otpContainer =document.querySelector(".otp-container");
    homeContainer.style.display = "block";
    analyticsView.style.display = "none";
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
}

function showAnalytics()
{
    var analyticsView = document.querySelector(".analytics-container");
    var homeContainer = document.querySelector(".home-admin-container");
    analyticsView.style.display = "block";
    homeContainer.style.display = "none";
}


function showAnalyticsRegistrationView()
{
    let analyticsHome = document.querySelector(".analytics-home");
    let analyticsRegistration = document.querySelector(".analytics-registration-view");
    // let analyticTitle = document.createElement("h1");
    // analyticTitle.textContent = "Analytics Registration";
    // analyticTitle.style.textAlign = "center";
    analyticsRegistration.style.display = "block";
    // analyticsRegistration.insertBefore(analyticTitle ,analyticsRegistration.firstChild);
    analyticsHome.style.display = "none";
    
}