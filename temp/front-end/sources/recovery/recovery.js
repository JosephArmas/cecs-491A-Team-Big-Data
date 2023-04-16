'use strict';
const recoveryServer= 'https://localhost:7259/recovery/request';
const recoveryForm = document.getElementById('recovery-form');
const recoveryHome = document.getElementById('recovery-home');
const username = document.getElementById('username');
const newPassword = document.getElementById('newPassword');
const recoveryBtn = document.getElementById('recoveryButton');
const homeBtn = document.getElementById('recovery-home');
const errors = document.getElementById('errors');
const request = {};

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
    request.username = u
    request.newPassword = p
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
