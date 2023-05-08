'use strict';
var recoveryServer= backend + '/recovery/request';
const recoveryForm = document.getElementById('recovery-form');
const recoveryHome = document.getElementById('recovery-home');
const username = document.getElementById('username');
const newPassword = document.getElementById('newPassword');
const recoveryBtn = document.getElementById('recoveryButton');
const homeBtn = document.getElementById('recovery-home');
const errors = document.getElementById('errors');
const request = {};

var backend = "";
fetch("./config.json").then((response) => response.json()).then((json) => 
{
backend = json.backend;
recoveryServer= backend + '/recovery/request';
})

recoveryBtn.addEventListener('click', function(event)
{
    if(username.value == '' || newPassword == '')
    {
        errors.innerHTML = 'Please fill out all feilds'
    }
    else if(!IsValidPassword(newPassword.value))
    {
        errors.innerHTML = 'Invalid Password, Please make it at least 8 characters'
    }
    else if(IsValidPassword(newPassword.value) && IsValidEmail(username.value))
    {
        errors.innerHTML = '';
        showRecOTP();
        sendRecOTP(username.value, newPassword.value);
    }
    else
    {
        errors.innerHTML = 'Error with email or password, please try again'
    }
    recoveryForm.reset();
});

recoveryHome.addEventListener('click', function (event)
{
    errorsDiv.innerHTML= "";
});

function SendRecoveryRequest(u,p)
{
    request.Username = u
    request.NewPassword = p
    request.AdminID = 0
    axios.post(recoveryServer, request).then(function (responseAfter)
    {
        alert("Request Sent, Please Wait for an Admin to Approve It");
        /*
        var responseAfter = responseAfter.data
        if(responseAfter.identity.isAuthenticated === true && responseAfter.identity.authenticationType !== 'Anonymous User' )
        {
            errorsDiv.innerHTML = "";
            showOtp();
            sendOtp();
        }
        */
    }).catch(function (error)
        {
            errors.innerHTML = error;
        });
}

function GetRecoveryRequests()
{
    let dataPlaceholder = document.getElementById("recoveryData")
    request.Username = ""
    request.NewPassword = ""
    request.AdminID = localStorage.getItem("id")
    axios.post(backend + "/recovery/admin/get", request).catch(function (error) {
        if(error.response !== undefined)
        {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g, "");
            timeOut(cleanError, 'red', errorsDiv) 
        }
    }).then(function(response)
    {
        dataPlaceholder.innerHTML = "<ul>"
        response.data.forEach(element =>
        {
            dataPlaceholder.innerHTML += "<li>"
            dataPlaceholder.innerHTML += "Username:  " + element.username + "\tTimeStamp:   " + element.timestamp;
            dataPlaceholder.innerHTML += "</li>"
        })
        dataPlaceholder.innerHTML += "</ul>"
    })
}

function RecoverAccount()
{
    request.NewPassword = ""
    request.Username = document.getElementById("recoveryInput").value;
    request.AdminID = localStorage.getItem("id")
    axios.post(backend + "/recovery/admin/complete", request).catch(function (error) {
    if(error === undefined)
    {
        let errorAfter = error.response.data;
        let cleanError = errorAfter.replace(/"/g, "");
        timeOut(cleanError, 'red', errorsDiv)
    }
    }).then(function()
    {
        timeOut("Account Successfully Recovered", 'green', errorsDiv)
    })
}
