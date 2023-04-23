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
    var recoveryContainer = document.querySelector(".recovery-container");
    var recoveryOTPContainer = document.querySelector(".recOTP-container");
    var profileContainer = document.querySelector(".profileContainer");
    anonContainer.style.display = "block";
    otpContainer.style.display="none";
    homeContainer.style.display = "none";
    regContainer.style.display = "none";     
    loginContainer.style.display = "none";
    recoveryContainer.style.display = "none";
    recoveryOTPContainer.style.display = "none";
    profileContainer.style.display = "none";
    globalErrors.innerHTML = "";
}

function regClicked()
{
    var regContainer = document.querySelector(".registration-container");
    var anonContainer = document.querySelector(".anon-container");
    anonContainer.style.display = "none";
    regContainer.style.display = "block";
}

function profileClicked()
{
    var profileContainer = document.querySelector(".profileContainer");
    var homeContainer = document.querySelector(".home-container");
    homeContainer.style.display = "none";
    profileContainer.style.display = "block";
    downloadProfilePic();
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
    var globalErrors = document.querySelector("#errors");
    otpContainer.style.display = "none";
    anonContainer.style.display = "none";
    homeContainer.style.display = "block";

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