// * TODO: fix error validation 
'use strict';
const authenticationServer= 'https://localhost:7259/account/authentication';
const loginForm = document.getElementById('login-form');
const email = document.getElementById('email');
const password = document.getElementById('password');
const loginBtn = document.getElementById('sub-login');
const loginHome = document.getElementById('login-home');
// var errorsDiv = document.getElementById('errors');
var userType = "";
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


function loginUser()
{
    user.username = email.value;
    user.password = password.value;
    axios.post(authenticationServer, user).then(function (responseAfter)
    {
        var responseAfter = responseAfter.data
        if(responseAfter.identity.isAuthenticated === true && responseAfter.identity.authenticationType !== 'Anonymous User' )
        {
            errorsDiv.innerHTML = "";
            userType = responseAfter.identity.authenticationType;
            showOtp();
            sendOtp();
            
        } else if (responseAfter.identity.isAuthenticate === true && responseAfter.identity.authenticationType === 'Admin User')
        {
            userType = responseAfter.identity.authenticationType;
            showOtp();
            sendOtp();

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
