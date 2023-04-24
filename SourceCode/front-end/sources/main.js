
var errorsDiv = document.querySelector("#errors");
let adminViewBuild = false;
let regViewBuild = false;

function loginClicked()
{
    var anonContainer = document.querySelector(".anon-container");
    var loginContainer = document.querySelector(".login-container");
    anonContainer.style.display = "none";
    loginContainer.style.display = "block";
}

function homeClicked()
{
    window.location.reload();
    var regContainer = document.querySelector(".registration-container");
    var otpContainer = document.querySelector(".otp-container");
    var anonContainer = document.querySelector(".anon-container");
    var loginContainer = document.querySelector(".login-container");
    var homeContainer = document.querySelector(".home-container")
    var recoveryContainer = document.querySelector(".recovery-container");
    var recoveryOTPContainer = document.querySelector(".recOTP-container");
    anonContainer.style.display = "block";
    otpContainer.style.display="none";
    homeContainer.style.display = "none";
    regContainer.style.display = "none";     
    loginContainer.style.display = "none";
    recoveryContainer.style.display = "none";
    recoveryOTPContainer.style.display = "none";
}

function regClicked()
{
    var regContainer = document.querySelector(".registration-container");
    var anonContainer = document.querySelector(".anon-container");
    anonContainer.style.display = "none";
    regContainer.style.display = "block";
}

function recoveryClicked()
{
    var recoveryContainer = document.querySelector(".recovery-container");
    var anonContainer = document.querySelector(".anon-container");
    anonContainer.style.display = "none";
    recoveryContainer.style.display = "block";
}

function recoveryClickedLogin()
{
    var loginContainer = document.querySelector(".login-container");
    var recoveryContainer = document.querySelector(".recovery-container");
    loginContainer.style.display = "none";
    recoveryContainer.style.display = "block";
}

function regView()
{
    var homeContainer = document.querySelector(".home-container");
    var anonContainer = document.querySelector(".anon-container");
    var otpContainer =document.querySelector(".otp-container");
    buildHomeUserView();
    otpContainer.style.display = "none";

    // anonContainer.style.display = "none";
    homeContainer.style.display = "block";

    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAAfbLnE9etZVZ0_ZqaAPUMl03BfKLN8kI&region=US&language=en&callback=initMap';
    script.async = true;

    document.head.appendChild(script);
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
    
    // anonContainer.style.display = "none";
    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAAfbLnE9etZVZ0_ZqaAPUMl03BfKLN8kI&region=US&language=en&callback=initMap';
    script.async = true;

    document.head.appendChild(script);
}



function showOtp()
{
    var otpContainer = document.querySelector(".otp-container");
    var loginContainer = document.querySelector(".login-container");
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
}

function showRecOTP()
{
    var otpContainer = document.querySelector(".recOTP-container");
    var recoveryContainer = document.querySelector(".recovery-container");
    otpContainer.style.display = "block";
    recoveryContainer.style.display = "none";
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

function profileClicked()
{
    let homeContainer = document.querySelector(".home-container");
    let profileContainer = document.querySelector("#profile-container");
    buildProfileView();
    homeContainer.style.display = "none";
    profileContainer.style.display = "block";
}


// Build the home view dynamically 
// pass in userid
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
    let createdEvent = document.createElement('button');
    createdEvent.setAttribute('type','button');
    createdEvent.textContent = 'Created Events';
    let joinedEvent = document.createElement('button');
    joinedEvent.setAttribute('type','button');
    joinedEvent.textContent = 'Joined Events';


    features.appendChild(createdEvent)
    features.appendChild(joinedEvent)
    let userID = localStorage.getItem('id');

    // create events listener
    createdEvent.addEventListener('click', function(event)
    {
        let homeDiv = document.querySelector(".home-container");
        let createEventDiv = document.querySelector(".events-created-container");
        buildEventsCreated(userID);
        homeDiv.style.display = "none";
        createEventDiv.style.display = "block";
        
    })

    // joined events listener
    joinedEvent.addEventListener('click', function(event)
    {
        let homeDiv = document.querySelector(".home-container");
        let createEventDiv = document.querySelector(".events-joined-container");
        buildEventsJoined(userID);
        homeDiv.style.display = "none";
        createEventDiv.style.display = "block";
    })


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


// Build Admin view
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

// Show views, Uncommnet to work on it
// regView()
// adminView()