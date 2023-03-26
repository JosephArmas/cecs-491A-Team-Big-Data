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
var errorsDiv = document.getElementById('errors');
const user = {}


function loginClicked()
{
    let anonContainer = document.querySelector(".anon-container");
    let loginContainer = document.querySelector(".login-container");
    let loginForm = document.querySelector("#login-form");
    // * Check if there is any created div element 
    if (!loginForm)
    {
        buildLogin();
    }

    loginContainer.style.display = "block";
    anonContainer.style.display = "none";
}

function buildLogin()
{
    let loginContainer = document.querySelector(".login-container");
    let loginForm = document.createElement('form');
    loginForm.id = "login-form";
    let backBtnDiv = document.createElement('div');
    backBtnDiv.setAttribute('class','back-button');
    let backBtn = document.createElement('button');
    backBtn.setAttribute('type','button');
    backBtn.textContent = "Back";
    backBtn.addEventListener('click',homeClicked);
    backBtnDiv.appendChild(backBtn);
    loginForm.appendChild(backBtnDiv);
    let boxDiv = document.createElement('div');
    boxDiv.setAttribute('class', 'login-box') 
    let logintTitle = document.createElement('h1');
    logintTitle.textContent = "Login";
    logintTitle.style.textAlign = "center";
    boxDiv.appendChild(logintTitle);
    let inputDiv = document.createElement('div');
    inputDiv.setAttribute('class','input-field-login');
    let email = document.createElement('input');
    let password = document.createElement('input');
    let submitBtn = document.createElement('button');
    email.setAttribute('type','email');
    email.id = "email";
    email.setAttribute('placeholder','Email Address');
    email.required = true;
    password.setAttribute('type','password');
    password.id = "password";
    password.setAttribute('placeholder','Password');
    password.required = true;
    password.minLength = 8;
    submitBtn.id = "sub-login";
    submitBtn.textContent = "Submit";
    submitBtn.addEventListener('click', function (event)
    {
            
        event.preventDefault();
        if (email.value == '' || password.value == '')
        {
            timeOut('Please fill in all fields','red',errorsDiv);
        } else if(IsValidPassword(password.value) === false)
        {
            timeOut('Password must be at least 8 characters long', 'red', errorsDiv);

        } else if(IsValidPassword(password.value) === true && IsValidEmail(email.value) === true) 
        {
            
            loginUser();

        } else
        {
            timeOut('Error with email or password. Plrease try agian', 'red',errorsDiv);
        }
        // reset login form when button clicked
        loginForm.reset()

    });
    
    inputDiv.appendChild(email);
    inputDiv.appendChild(password);
    inputDiv.appendChild(submitBtn);
    boxDiv.appendChild(inputDiv);
    let optionsDiv = document.createElement('div');
    let forgotPassword = document.createElement('button');
    let contactSupport = document.createElement('button');
    optionsDiv.setAttribute('class','options-other');
    forgotPassword.textContent = "Forgot Password";
    contactSupport.textContent = "Contact Support";
    optionsDiv.appendChild(forgotPassword);
    optionsDiv.appendChild(contactSupport);
    boxDiv.appendChild(optionsDiv)
    loginForm.appendChild(boxDiv);
    loginContainer.appendChild(loginForm);
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

function buildRegistration()
{
    let registrationContainer = document.querySelector(".registration-container");
    let registerForm = document.createElement('form');
    registerForm.id = "registration-form";
    let backBtnDiv = document.createElement('div');
    backBtnDiv.setAttribute('class','back-button');
    let backBtn= document.createElement('button');
    backBtn.setAttribute('type','button');
    backBtn.id = 'reg-home'
    backBtn.textContent = 'Back'
    backBtn.addEventListener('click',homeClicked);
    backBtnDiv.appendChild(backBtn);
    registerForm.appendChild(backBtnDiv);
    let boxDiv = document.createElement('div');
    boxDiv.setAttribute('class', 'reg-box')
    let regTitle = document.createElement('h1');
    regTitle.id = 'reg-title';
    regTitle.textContent = "Register";
    boxDiv.appendChild(regTitle);
    let inputDiv = document.createElement('div');
    inputDiv.setAttribute('class','input-field');
    let email = document.createElement('input');
    let password = document.createElement('input');
    let confirmPassword = document.createElement('input');
    let submitBtn = document.createElement('button');
    email.setAttribute('type','email');
    email.id = "r-email";
    email.setAttribute('placeholder','Email Address');
    email.required = true;
    password.setAttribute('type','password');
    password.id = "r-pw";
    password.setAttribute('placeholder','Password');
    password.required = true;
    password.minLength = 8;
    confirmPassword.setAttribute('type','password');
    confirmPassword.required = 'true';
    confirmPassword.minLength = 8;
    confirmPassword.setAttribute('placeholder','Confirm Password');
    submitBtn.id = "regBtn-submit";
    submitBtn.textContent = "Submit";
    submitBtn.addEventListener('click', function (event)
    {
        event.preventDefault();
        if (password.value == '' || password.value == '' || password.value == '')
        {
           timeOut('Please fill in all fields','red',errorsDiv);

        } else if(password.value !== confirmPassword.value)
        {
            timeOut('Passwords do not match','red',errorsDiv);

     
        } else if(IsValidPassword(password.value) === true && IsValidEmail(email.value) === true)
        {
           registerUser();
     
        }
        else if (IsValidPassword(password.value) === false)
        {
            timeOut('Password must be at least 8 characters long','red',errorsDiv)
        
        } else{
           
            timeOut('Error with email or password. Please try again','red',errorsDiv);
           
        }
    });
    inputDiv.appendChild(email);
    inputDiv.appendChild(password);
    inputDiv.appendChild(confirmPassword);
    inputDiv.appendChild(submitBtn);
    let contactDiv = document.createElement('div');
    contactDiv.setAttribute('class','reg-contact');
    let contactSupportBtn = document.createElement('button');
    contactSupportBtn.textContent = "Contact Support";
    contactDiv.appendChild(contactSupportBtn);
    boxDiv.appendChild(inputDiv);
    boxDiv.appendChild(contactDiv);
    registerForm.appendChild(boxDiv);
    registrationContainer.appendChild(registerForm);
    
}


function regClicked()
{
    var regContainer = document.querySelector(".registration-container");
    var anonContainer = document.querySelector(".anon-container");
    let regForm = document.querySelector("#registration-form");
    if (!regForm)
    {
        buildRegistration();
    }
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

function timeOut(text,color,divElement)
{
    divElement.style.display = "block";
    divElement.innerHTML = text;
    divElement.style.color = color;
    setTimeout(function(){
        divElement.style.display = "none";
    }, 3000);
}