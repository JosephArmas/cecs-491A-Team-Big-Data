'use strict';
let loginBuild = false;

function loginUser(email, password)
{
    const user = {}
    const server = getServer();
    user.username = email;
    user.password = password;
    let loginForm = document.getElementById('login-form');
    const role = getRole();
    console.log("inside of loginUser function");
    console.log(user.username);
    console.log(user.password);
    console.log(server.authenticationServer);
    console.log(user);
    axios.post(server.authenticationServer, user).then(function (responseAfter)
    {
        var base64Url = responseAfter.data.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        const jsonObj = JSON.parse(jsonPayload)
        console.log(jsonObj);
        // console.log(roles.reg.includes(jsonObj.role));

        if (jsonObj.authenticated === "true" && role.reg.includes(jsonObj.role) || role.service.includes(jsonObj.role) || role.admin.includes(jsonObj.role))
        {

            localStorage.setItem("jwtToken", responseAfter.data)
            localStorage.setItem("role", jsonObj.role)
            localStorage.setItem("id",jsonObj.nameid)
            console.log('user is authenticated and has a role lets go to otp view');
            showOtp(jsonObj.otp,jsonObj.role);

        }

    }).catch (function (error)
    {
        let errorAfter = error.response.data;
        let cleanError = errorAfter.replace(/"/g,"");
        timeOut(cleanError, 'red', responseDiv);
    });
    loginForm.reset();
}

function buildLogin()
{
    if(!loginBuild)
    {
        let backBtnDiv = document.querySelector('#login-form .back-button');
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
                // console.log(IsValidPassword(password.value));
                loginUser(email.value,password.value);
            } 
            else
            {
                timeOut('Error with email or password. Plrease try agian', 'red', responseDiv);
            }

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
