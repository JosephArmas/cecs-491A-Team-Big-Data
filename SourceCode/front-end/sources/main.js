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
document.querySelector("#login").addEventListener("click", loginClicked);
// * Considered cross cutting so can be called anywhere
var responseDiv = document.getElementById('response');
const user = {}


function loginClicked()
{
    let anonContainer = document.querySelector(".anon-container");
    let loginContainer = document.querySelector(".login-container");
    buildLogin();
    loginContainer.style.display = "block";
    anonContainer.style.display = "none";
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

}

function regClicked()
{
    var regContainer = document.querySelector(".registration-container");
    var anonContainer = document.querySelector(".anon-container");
    buildRegistration();
    regContainer.style.display = "block";
    anonContainer.style.display = "none";
}


function regView()
{
    var homeContainer = document.querySelector(".home-container");
    var anonContainer = document.querySelector(".anon-container");
    var otpContainer =document.querySelector(".otp-container");
    buildHomeUserView();
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
    homeContainer.style.display = "block";
}

function buildHomeUserView()
{
    let logoutBtn = document.createElement('button');
    let profileBtn = document.createElement('button');
    let nav = document.querySelector(".ham-menu-container");
    let featureBtn = document.createElement('button');
    featureBtn.setAttribute('type','button');
    featureBtn.textContent = 'Features';
    nav.appendChild(featureBtn);
    let features = document.querySelector(".features");
    for (let i = 1; i <= 6; i++)
    {
        let li = document.createElement('li');
        li.textContent = "feature " + i;
        features.appendChild(li);
    }
    let profileDiv = document.querySelector("#profile");
    let logoutDiv = document.querySelector("#logout");
    let contactDiv = document.querySelector(".reg-contact-home");
    let contactBtn = document.createElement('button');
    contactBtn.setAttribute('type','button');
    contactBtn.textContent = 'Contact Support';
    contactBtn.addEventListener('click', function(event)
    {
        alert("Contact Support");
    });
    contactDiv.appendChild(contactBtn);
    nav.appendChild(features);
    logoutBtn.setAttribute('type','button');
    logoutBtn.id ="home-logoutBtn"
    logoutBtn.textContent = 'Logout';
    profileBtn.setAttribute('type','button');
    profileBtn.textContent = 'Profile'
    profileBtn.id ="profileBtn"
    profileDiv.appendChild(profileBtn);
    logoutDiv.appendChild(logoutBtn);
    
}

function IsValidPassword(password)
{
    let passwordAllowed = new RegExp("^[a-zA-Z0-9@.,!\s-]")
    if (passwordAllowed.test(password) && password.length > 7)
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

function timeOut(errorMsg,color,divElement)
{
    divElement.style.display = "block";
    divElement.innerHTML = errorMsg;
    divElement.style.color = color;
    setTimeout(function(){
        divElement.style.display = "none";
    }, 3000);
}