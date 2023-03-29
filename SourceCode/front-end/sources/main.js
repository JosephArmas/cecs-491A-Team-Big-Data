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
    // let regForm = document.querySelector("#registration-form");
    // if (!regForm)
    // {
        // buildRegistration();
    // }
    buildRegistration();
    regContainer.style.display = "block";
    anonContainer.style.display = "none";
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

function showOtp()
{
    let otpContainer = document.querySelector(".otp-container");
    let loginContainer = document.querySelector(".login-container");
    let anonContainer = document.querySelector(".anon-container");
    let otpForm = document.querySelector("#otp-form");
    if(!otpForm)
    {
        buildOTP();

    }
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
    anonContainer.style.display = "none";
}

function buildOTP()
{

    let otpContainer = document.querySelector(".otp-container");
    let otpForm = document.createElement('form'); 
    otpForm.id = "otp-form";
    let backBtnDiv = document.createElement('div');
    backBtnDiv.setAttribute('class','back-button');
    let backBtn = document.createElement('button');
    backBtn.setAttribute('type','button');
    backBtn.textContent = "Back";
    backBtn.addEventListener('click',homeClicked);
    backBtnDiv.appendChild(backBtn);
    otpForm.appendChild(backBtnDiv);
    let otpTitle = document.createElement('h2');
    otpTitle.id = "otp-title";
    otpTitle.textContent = "Enter OTP";
    otpForm.appendChild(otpTitle);
    let otpDisplay = document.createElement('div');
    let otpVal = generateOTP();
    otpDisplay.setAttribute('class','otp-display');
    otpDisplay.innerHTML = otpVal;
    otpForm.appendChild(otpDisplay);
    let otp = document.createElement('div');
    otp.setAttribute('class','otp');
    let otpInput = document.createElement('input');
    otpInput.setAttribute('type','text');
    otpInput.required = true;
    otp.appendChild(otpInput);
    otpForm.appendChild(otp);
    let submitDiv = document.createElement('div');
    submitDiv.setAttribute('class','submit')
    let submit = document.createElement('button');
    submit.setAttribute('type','submit');
    submit.textContent = "Submit";
    submit.addEventListener('click',function(event)
    {
        event.preventDefault();
        if (otpInput.value == '')
        {
            timeOut('Please enter OTP','red',errorsDiv)
    
        } else if (otpInput.value == otpVal && roles.includes(userType))  
        {
            regView();
            
        } else if(otpInput.value == otpVal && userType === "Admin User")
        {
    
            adminView();
    
        } else 
        {
            timeOut('Invalid OTP. Please try again','red', errorsDiv)
        } 
        otpForm.reset();
    })
    submitDiv.appendChild(submit);
    otpForm.appendChild(submitDiv);
    otpContainer.appendChild(otpForm);

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