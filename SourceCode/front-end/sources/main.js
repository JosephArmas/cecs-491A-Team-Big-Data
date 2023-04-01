// Todo: 
/*
 * check if a user is first logged in -> update-profile view
 * home view - hamburger menu
 * All of main is anon view -> Homeview(admin or reg user)
*/

// document.querySelector("#analytics-logout").addEventListener("click", homeClicked);
// document.querySelector("#admin-logout").addEventListener("click", homeClicked);
document.querySelector("#register").addEventListener("click", regClicked);
document.querySelector("#login").addEventListener("click", loginClicked);
// * Considered cross cutting so can be called anywhere
var responseDiv = document.getElementById('response');
let adminViewBuild = false;
let regViewBuild = false
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
    regViewBuild = true;
    
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
    let homeContainer = document.querySelector(".home-admin-container");
    let anonContainer = document.querySelector(".anon-container");
    let analyticsView = document.querySelector(".analytics-container");
    let otpContainer = document.querySelector(".otp-container");
    let analyticsCharts = document.querySelector(".charts");
    // let analyticsMaps = document.querySelector(".analytics-maps-view");
    // let analyticsRegisters = document.querySelector(".analytics-registration-view");
    // let analyticsLogins = document.querySelector(".analytics-logins-view");
    // let analyticsPins = document.querySelector(".analytics-pins-view");
    buildAdminView();
    homeContainer.style.display = "block";
    analyticsCharts.style.display = "none";
    // analyticsMaps.style.display = "none";
    // analyticsRegisters.style.display = "none";
    // analyticsLogins.style.display = "none";
    // analyticsPins.style.display = "none";
    analyticsView.style.display = "none";
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
}

function buildAdminView()
{
    if(!adminViewBuild)
    {
        let logoutContainer = document.querySelector(".logout-container");
        let analyticsHome = document.querySelector(".home-admin-container");
        let logoutBtn = document.createElement('button');
        logoutBtn.setAttribute('type','button');
        logoutBtn.textContent = 'Logout';
        logoutBtn.id ="admin-logout"
        logoutBtn.addEventListener('click', homeClicked)
        logoutContainer.appendChild(logoutBtn);
        let adminTitle = document.createElement('h1');
        adminTitle.textContent = "Admin Home";
        adminTitle.style.textAlign = "center";
        analyticsHome.insertBefore(adminTitle, logoutContainer.nextSibling);
        let userManagementDiv = document.querySelector(".user-management");
        let usageAnalysisDiv = document.querySelector(".usage-analysis");
        let userManagementBtn = document.createElement('button');
        userManagementBtn.setAttribute('type','button');
        userManagementBtn.textContent = 'User Management';
        userManagementDiv.appendChild(userManagementBtn);
        let usageAnalysisBtn = document.createElement('button');
        usageAnalysisBtn.setAttribute('type','button');
        usageAnalysisBtn.textContent = 'Usage Analysis';
        usageAnalysisBtn.id = "usage-dashboard";
        usageAnalysisDiv.appendChild(usageAnalysisBtn);
        usageAnalysisBtn.addEventListener('click', showAnalytics);
        adminViewBuild = true;
    }
    
}
showAnalytics();
function timeOut(errorMsg,color,divElement)
{
    divElement.style.display = "block";
    divElement.innerHTML = errorMsg;
    divElement.style.color = color;
    setTimeout(function(){
        divElement.style.display = "none";
    }, 3000);
}