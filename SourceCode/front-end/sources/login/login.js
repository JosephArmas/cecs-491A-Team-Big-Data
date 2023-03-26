'use strict';
const authenticationServer= 'https://localhost:7259/account/authentication';
var userType = "";
const roles =  ['Regular User']

function loginUser()
{
    let email = document.getElementById('email');
    let password = document.getElementById('password');
    user.username = email.value;
    user.password = password.value;
    let loginForm = document.getElementById('login-form');
    axios.post(authenticationServer, user).then(function (responseAfter)
    {
        var responseAfter = responseAfter.data
        if(responseAfter.identity.isAuthenticated === true && responseAfter.identity.authenticationType !== 'Anonymous User' )
        {
            userType = responseAfter.identity.authenticationType;
            showOtp();
            // sendOtp();
            
        } else if (responseAfter.identity.isAuthenticate === true && responseAfter.identity.authenticationType === 'Admin User')
        {
            userType = responseAfter.identity.authenticationType;
            showOtp();
            // sendOtp();

        }
    }).catch(function (error)
        {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                errorsDiv.innerHTML = cleanError; 
        });
    loginForm.reset();
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
