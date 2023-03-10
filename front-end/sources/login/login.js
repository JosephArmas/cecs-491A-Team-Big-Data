// * TODO: fix error validation 
'use strict';
const backendserver= 'https://localhost:7259/account/authentication';
const loginForm = document.getElementById('login-form');
const email = document.getElementById('email');
const password = document.getElementById('password');
const loginBtn = document.getElementById('sub-login');
const errorsDiv = document.getElementById('errors');
loginBtn.addEventListener('click', function (event)
{
    event.preventDefault();
    if (email.value == '' || password.value == '')
    {
        errorsDiv.innerHTML = "Please fill in all fields";
    }else {

    let user = {
        username: email.value,
        password: password.value
    }
    console.log(user.username);
    console.log(user.password);
    console.log(user);

    axios.post(backendserver, user).then(function (response)
    {
        console.log(response.data);
        var responseAfter = response.data
        if(responseAfter.identity.isAuthenticated === true )
        {
            console.log(responseAfter.identity.isAuthenticated)
            var loginContainer = document.querySelector(".login-container");
            var otpContainer = document.querySelector(".otp-container");
            otpContainer.style.display = "block";
            loginContainer.style.display = "none";
        }
    })
    }
    // reset login form when button clicked
    loginForm.reset()

});

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

    axios.post('https://localhost:7259/account/authentication',dataa) .then( function (response)
    {
        console.log(response.data)
        var responseAfter = response.data
        testElement.innerHTML = JSON.stringify(response.data);
        // console.log(responseAfter._userID);
        // console.log(responseAfter.identity.isAuthenticated);
        // testElement.innerHTML = responseAfter._userID;
    })
}
// displayLoginData();
*/
