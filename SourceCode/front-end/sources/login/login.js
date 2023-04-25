// * TODO: fix error validation 
'use strict';
const authenticationServer= 'https://localhost:7259/account/authentication';
const loginForm = document.getElementById('login-form');
const email = document.getElementById('email');
const password = document.getElementById('password');
const loginBtn = document.getElementById('sub-login');
const loginHome = document.getElementById('login-home');
var errorsDiv = document.getElementById('errors');
const roles =  ['Regular User']
const user = {}
loginBtn.addEventListener('click', function (event)
{
    event.preventDefault();
    if (email.value == '' || password.value == '')
    {
        errorsDiv.innerHTML = "Please fill in all fields";
    } else if(IsValidPassword(password.value) === false)
    {
        errorsDiv.innerHTML = "Password must be at least 8 characters long";

    } else if(IsValidPassword(password.value) === true && IsValidEmail(email.value) === true) 
    {
        loginUser();
        // sendOtp();

    } else
    {
        errorsDiv.innerHTML = "Error with email or password. Please try again"; 
    }
    // reset login form when button clicked
    loginForm.reset()

});

loginHome.addEventListener('click', function (event)
{
    errorsDiv.innerHTML= "";
});

var otpContainer = document.querySelector(".otp-container");
var errorsOtp = document.getElementById("errors");
const otpForm = document.querySelector("#otp-form");
const otpDisplay = document.querySelector("#otp-display");
var otpInput = document.querySelector("#otp-input");
/*otpBtn.addEventListener('click', function (event)
{
    event.preventDefault();
    if (otpInput.value == '')
    {
        errorsOtp.innerHTML = "Please enter OTP";

    } else if (otpInput.value == otpVal) 
    {
        errorsOtp.innerHTML = "";
        regView();
        
    } else 
    {
        errorsOtp.style.color = "red";
        errorsOtp.innerHTML = "Invalid OTP. Please try again";
    } 
    otpForm.reset();
});

function sendOtp()
{
    otpVal = generateOTP();
    otpDisplay.style.color = "blue";
    otpDisplay.innerHTML = otpVal;
    otpDisplay.style.fontSize = "20px";
}

function showOtp()
{
    
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
}*/

var otpContainer = document.querySelector(".otp-container");
var loginContainer = document.querySelector(".login-container");

function loginUser()
{
    user.username = email.value;
    user.password = password.value;
    axios.post(authenticationServer, user).then(function (responseAfter)
    {
        // turning jwt signature from the response into a json object
        var base64Url = responseAfter.data.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        const jsonObj = JSON.parse(jsonPayload);
        if(jsonObj.authenticated === "true" && jsonObj.role !== 'Anonymous User' )
        {
            // save JWT token to local storage
            localStorage.setItem("jwtToken", responseAfter.data)
            localStorage.setItem("role", jsonObj.role)
            localStorage.setItem("id",jsonObj.nameid)
        
            errorsDiv.innerHTML = "";

            // display otp
            otpDisplay.style.color = "blue";
            otpDisplay.innerHTML = "<h3>" + jsonObj.otp + "</h3>";
            otpDisplay.style.fontSize = "20px";
            loginContainer.style.display = "none";
            showOtp();

            // take in otp value to post in the back end
            const otpBtn = document.querySelector("#otp-submit");
            otpBtn.addEventListener('click', function (event)
            {
                event.preventDefault();
                if (otpInput.value == '')
                {
                    errorsOtp.innerHTML = "Please enter OTP";

                } else if (otpInput.value == jsonObj.otp) 
                {
                    errorsOtp.innerHTML = "";
                    regView();
                    
                } else 
                {
                    errorsOtp.style.color = "red";
                    errorsOtp.innerHTML = "Invalid OTP. Please try again";
                } 
                otpForm.reset();
            });
        }
        else 
        {
            // unauthorized user
        }
    }).catch(function (error)
        {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g,"");
            errorsDiv.innerHTML = cleanError; 
        });
}




// * Debugging Purposes
/* 
function displayLoginData()
{
    let testElement = document.getElementById('test-data');
    let dataa = {
        username:"testUser@yahoo.com",
        password: "password"
    }
    // let headers = 
    // {
    //     "Access-Control-Allow-Origin": "*",
    //     "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, OPTIONS",
    //     "Access-Control-Allow-Credentials":"true",
    //     "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept, Authorization"
    // }

    axios.post('https://localhost:7259/account/authentication',dataa) .then( function (responseAfter)
    {
        console.log(responseAfter.data)
        var responseAfter = responseAfter.data
        testElement.innerHTML = JSON.stringify(responseAfter.data);
        // console.log(responseAfter._userID);
        // console.log(responseAfter.identity.isAuthenticated);
        // testElement.innerHTML = responseAfter._userID;
    })
}
// displayLoginData();

function displayRegisterData()
*/
