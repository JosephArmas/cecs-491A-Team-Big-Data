// * TODO: fix error validation 
'use strict';

(function (root) {
    // Dependency check
    const isValid = root;

    if(!isValid){
        // Handle missing dependencies
        alert("Missing dependencies");
    }
    
    // Current user
    let jwtToken = "";
    let userID = "";
    let username = "";
    let role = "";
    let authenticated = "";
    let otp = "";
    let otpCreated = "";
    let userhash = "";

var otpContainer = document.querySelector(".otp-container");
var errorsOtp = document.getElementById("errors");
var otpInput = document.querySelector("#otp-input");

var otpContainer = document.querySelector(".otp-container");
var loginContainer = document.querySelector(".login-container");

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
    

    var otpContainer = document.querySelector(".otp-container");
    var loginContainer = document.querySelector(".login-container");

    function loginUser()
    {
        user._username = email.value;
        user._password = password.value;
        getProfileUsername(email.value);
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
                localStorage.setItem("username", jsonObj.unique_name)
                localStorage.setItem("role", jsonObj.role)
                localStorage.setItem("id",jsonObj.nameid)
                localStorage.setItem("userhash",jsonObj.userhash)
                

                /*jwtToken = responseAfter.data;
                userID = jsonObj.nameid;
                username = jsonObj.unique_name;
                role = jsonObj.role;
                authenticated = jsonObj.authenticated;
                otp = jsonObj.otp;
                otpCreated = jsonObj.otpCreated;
                userhash = jsonObj.userHash;*/
            
                errorsDiv.innerHTML = "";

                // display otp
                otpDisplay.style.color = "blue";
                otpDisplay.innerHTML = "<h3>" + jsonObj.otp + "</h3>";
                otpDisplay.style.fontSize = "20px";
                loginContainer.style.display = "none";
                showOtp();

                // take in otp value to post in the back end
                let otpInput = document.querySelector("#otp-input");
                const otpBtn = document.querySelector("#otp-submit");
                otpBtn.addEventListener('click', function (event)
                {
                    event.preventDefault();
                    if (otpInput.value == '')
                    {
                        errorsOtp.innerHTML = `Please enter OTP: ${otpInput.value}`;

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

    // Getters
    function getUserID()
    {
        return userID;
    }
    function getUsername()
    {
        return username;
    }
    function getRole()
    {
        return role;
    }
    function getAuthenticated()
    {
        return authenticated;
    }
    function getOtp()
    {
        return otp;
    }
    function getOtpCreated()
    {
        return otpCreated;
    }
    function getUserhash()
    {
        return userhash;
    }
    
    function getProfileUsername(username)
    {
        let name = username.substring(0,username.lastIndexOf("@"));
        return localStorage.setItem('profileUsername',name);
    }


    // Allow access
    root.Utification = root.Utification || {};

    root.Utification.userID = getUserID();
    root.Utification.username = getUsername();
    root.Utification.role = getRole();
    root.Utification.authenticated = getAuthenticated();
    root.Utification.otp = getOtp();
    root.Utification.otpCreated = getOtpCreated();
    root.Utification.userhash = getUserhash();

    //root.Utification.Login = loginUser;
    //window.Utification.role 
})(window);






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
