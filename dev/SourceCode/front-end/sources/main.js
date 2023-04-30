let regViewBuild = false;
function loginClicked()
{
    let anonContainer = document.querySelector(".anon-container");
    let loginContainer = document.querySelector(".login-container");
    anonContainer.style.display = "none";
    loginContainer.style.display = "block";
}

function homeClicked()
{
    window.location.reload();
    let regContainer = document.querySelector(".registration-container");
    let otpContainer = document.querySelector(".otp-container");
    let anonContainer = document.querySelector(".anon-container");
    let loginContainer = document.querySelector(".login-container");
    let homeContainer = document.querySelector(".home-container")
    let globalErrors = document.querySelector("#errors");
    let recoveryContainer = document.querySelector(".recovery-container");
    let recoveryOTPContainer = document.querySelector(".recOTP-container");
    let profileContainer = document.querySelector(".profileContainer");
    anonContainer.style.display = "block";
    otpContainer.style.display="none";
    homeContainer.style.display = "none";
    regContainer.style.display = "none";     
    loginContainer.style.display = "none";
    recoveryContainer.style.display = "none";
    recoveryOTPContainer.style.display = "none";
    // profileContainer.style.display = "none";
    globalErrors.innerHTML = "";
}

function regClicked()
{
    let regContainer = document.querySelector(".registration-container");
    let anonContainer = document.querySelector(".anon-container");
    anonContainer.style.display = "none";
    regContainer.style.display = "block";
}

function profileClicked()
{
    let profileContainer = document.querySelector(".profileContainer");
    let homeContainer = document.querySelector(".home-container");
    homeContainer.style.display = "none";
    profileContainer.style.display = "block";
    downloadProfilePic();
}

function recoveryClicked()
{
    let recoveryContainer = document.querySelector(".recovery-container");
    let anonContainer = document.querySelector(".anon-container");
    anonContainer.style.display = "none";
    recoveryContainer.style.display = "block";
}

function recoveryClickedLogin()
{
    let loginContainer = document.querySelector(".login-container");
    let recoveryContainer = document.querySelector(".recovery-container");
    loginContainer.style.display = "none";
    recoveryContainer.style.display = "block";
}

function regView()
{
    buildHomeUserView();
    var homeContainer = document.querySelector(".home-container");
    var anonContainer = document.querySelector(".anon-container");
    var otpContainer =document.querySelector(".otp-container");
    var reputationContainer = document.querySelector(".reputation-reports-container");
    var globalErrors = document.querySelector("#errors");
    otpContainer.style.display = "none";
    // anonContainer.style.display = "none";
    // reputationContainer.style.display = "none";
    homeContainer.style.display = "block";

    let script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAAfbLnE9etZVZ0_ZqaAPUMl03BfKLN8kI&region=US&language=en&callback=initMap';
    script.async = true;

    document.head.appendChild(script);
}

function showOtp()
{
    let otpContainer = document.querySelector(".otp-container");
    let loginContainer = document.querySelector(".login-container");
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
}

function showRecOTP()
{
    let otpContainer = document.querySelector(".recOTP-container");
    let recoveryContainer = document.querySelector(".recovery-container");
    otpContainer.style.display = "block";
    recoveryContainer.style.display = "none";
}

function backToMap()
{
    let homeContainer = document.querySelector(".home-container");
    let profileContainer = document.querySelector(".profileContainer");
    profileContainer.style.display = "none";
    homeContainer.style.display = "block";
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
    let uploadBtn = document.createElement('input');
    uploadBtn.setAttribute('type','file');
    uploadBtn.setAttribute('accept','image/jpg');
    uploadBtn.setAttribute('name','Select File');
    uploadBtn.id = 'fileSelector';


    features.appendChild(createdEvent)
    features.appendChild(joinedEvent)
    features.appendChild(uploadBtn);
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
    
    // upload profile pic listener


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
    profileBtn.textContent = localStorage.getItem('profileUsername');
    // profileBtn.textContent = localStorage.getItem('username');
    profileBtn.id ="profileBtn"
    profileBtn.addEventListener('click', profileClicked);
    profileDiv.appendChild(profileBtn);
    logoutDiv.appendChild(logoutBtn);

    regViewBuild = true;
    
}

// regView();