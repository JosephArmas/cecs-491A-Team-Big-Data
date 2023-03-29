'use strict';
const authenticationServer= 'https://localhost:7259/account/authentication';
var userType = "";
const roles =  ['Regular User']
let loginBuild = false;

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
                timeOut(cleanError, 'red', responseDiv)
                // errorsDiv.innerHTML = cleanError; 
        });
    loginForm.reset();
}

    
function buildLogin()
{
    if(!loginBuild)
    {
        let backBtnDiv = document.querySelector('.back-button');
        let btn = document.createElement('button');
        let inputDiv = document.querySelector('.input-field-login');
        let optionsDiv = document.querySelector('.options-other');
        btn.setAttribute('type','button');
        btn.textContent = "Back";
        btn.addEventListener('click',homeClicked);
        backBtnDiv.appendChild(btn);
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
            if(IsValidPassword(password.value) === true && IsValidEmail(email.value) === true) 
            {
                loginUser();
            } 
            else
            {
                timeOut('Error with email or password. Plrease try agian', 'red', responseDiv);
            }
            loginForm.reset()

        });
        inputDiv.appendChild(email);
        inputDiv.appendChild(password);
        inputDiv.appendChild(submitBtn);
        let forgotPassword = document.createElement('button');
        let contactSupport = document.createElement('button');
        forgotPassword.textContent = "Forgot Password";
        forgotPassword.addEventListener('click', function (event)
        {
            // * Place holder -> should go to Account Recovery view
            timeOut('Redirecting to ')

        });
        contactSupport.textContent = "Contact Support";
        optionsDiv.appendChild(forgotPassword);
        optionsDiv.appendChild(contactSupport);

        loginBuild = true;
    }
    
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
