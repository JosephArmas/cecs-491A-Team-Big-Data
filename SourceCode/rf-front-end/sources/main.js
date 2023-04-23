// Todo: 
/*
 * check if a user is first logged in -> update-profile view
 * home view - hamburger menu
 * All of main is anon view -> Homeview(admin or reg user)
*/

document.querySelector("#register").addEventListener("click", regClicked);
document.querySelector("#login").addEventListener("click", loginClicked);
// * Considered cross cutting so can be called anywhere
var responseDiv = document.getElementById('response');
let adminViewBuild = false;
let regViewBuild = false


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

    let regContainer = document.querySelector(".registration-container");
    let otpContainer = document.querySelector(".otp-container");
    let anonContainer = document.querySelector(".anon-container");
    let loginContainer= document.querySelector(".login-container");
    let homeContainer = document.querySelector(".home-container")
    let analyticsView = document.querySelector(".analytics-container");
    let adminView = document.querySelector(".home-admin-container");
    adminView.style.display = "none";
    anonContainer.style.display = "block";
    otpContainer.style.display="none";
    homeContainer.style.display = "none";
    regContainer.style.display = "none";     
    loginContainer.style.display = "none";
    analyticsView.style.display = "none";

}


function profileClicked()
{
    let homeContainer = document.querySelector(".home-container");
    let profileContainer = document.querySelector("#profile-container");
    buildProfileView();
    homeContainer.style.display = "none";
    profileContainer.style.display = "block";

}

function regClicked()
{
    let regContainer = document.querySelector(".registration-container");
    let anonContainer = document.querySelector(".anon-container");
    buildRegistration();
    regContainer.style.display = "block";
    anonContainer.style.display = "none";
}


function regView()
{
    let homeContainer = document.querySelector(".home-container");
    let anonContainer = document.querySelector(".anon-container");
    let otpContainer =document.querySelector(".otp-container");
    buildHomeUserView();
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
    homeContainer.style.display = "block";
}


function buildHomeUserView()
{
    let logoutBtn = document.createElement('button');
    let profileBtn = document.createElement('button');
    let nav = document.querySelector(".home-container .ham-menu-container");
    let menu = document.querySelector('.home-container .menu-container')
    let featureBtn = document.createElement('button');
    featureBtn.setAttribute('type','button');
    featureBtn.textContent = 'Features';
    menu.insertBefore(featureBtn, nav);
    let features = document.querySelector(".home-container .features");
    featureBtn.setAttribute('type','button');
    featureBtn.textContent = 'Features';

    for (let i = 1; i <= 6; i++)
    {
        let listFeatures = document.createElement('button');
        listFeatures.textContent = "Feature " + i;
        features.appendChild(listFeatures);
    }

    features.style.display = 'none';
    nav.append(features);

    featureBtn.addEventListener('click',function()
    {
        if (features.style.display === 'none')
        {
            features.style.display = 'flex';

        }
        else{
            features.style.display = 'none';
        }

    });

    let profileDiv = document.querySelector(".home-container #profile");
    let logoutDiv = document.querySelector(".home-container #logout");
    let contactDiv = document.querySelector(".home-container .reg-contact-home");
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
    logoutBtn.addEventListener('click', homeClicked);
    profileBtn.setAttribute('type','button');
    profileBtn.textContent = 'Profile'
    profileBtn.id ="profileBtn"
    profileBtn.addEventListener('click', profileClicked);
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
    buildAdminView();
    homeContainer.style.display = "block";
    analyticsCharts.style.display = "none";
    analyticsView.style.display = "none";
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
}

/*
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
*/

function buildAdminView()
{
    if(!adminViewBuild)
    {
        let logoutBtn = document.createElement('button');
        let titleDiv = document.querySelector('.home-admin-container .title');
        let nav = document.querySelector(".home-admin-container .ham-menu-container");
        let menu = document.querySelector('.home-admin-container .menu-container')
        let featureBtn = document.createElement('button');
        featureBtn.setAttribute('type','button');
        featureBtn.textContent = 'Features';
        menu.insertBefore(featureBtn, nav);
        let features = document.querySelector(".home-admin-container .features");
        featureBtn.setAttribute('type','button');
        featureBtn.textContent = 'Features';
        let title = document.createElement('h1');
        title.textContent = "Admin Home";
        titleDiv.appendChild(title);

        let userManagementBtn = document.createElement('button');
        userManagementBtn.setAttribute('type','button');
        userManagementBtn.textContent = 'User Management';
        features.appendChild(userManagementBtn);

        let analyticsBtn = document.createElement('button');
        analyticsBtn.setAttribute('type','button');
        analyticsBtn.textContent = 'Analytics';
        analyticsBtn.addEventListener('click', showAnalytics)
        features.appendChild(analyticsBtn);

        features.style.display = 'none';
        nav.append(features);
    
        featureBtn.addEventListener('click',function()
        {
            if (features.style.display === 'none')
            {
                features.style.display = 'flex';
    
            }
            else{
                features.style.display = 'none';
            }
    
        });
    
        let logoutDiv = document.querySelector(".home-admin-container .profile-container #logout");
        logoutBtn.setAttribute('type','button');
        logoutBtn.id ="home-logoutBtn"
        logoutBtn.textContent = 'Logout';
        logoutBtn.addEventListener('click', homeClicked);
        logoutDiv.appendChild(logoutBtn);
        adminViewBuild = true;
    }
    
}

// * UnComment to work on either view to work on it
// regView();
// adminView();

