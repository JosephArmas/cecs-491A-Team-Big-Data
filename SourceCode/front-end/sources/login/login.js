// * TODO: fix error validation 
'use strict';

(function (root) {
    // Dependency check
    const isValid = root;

    if (!isValid) {
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

    const authenticationServer = "/account/authentication";
    const loginForm = document.getElementById('login-form');
    const email = document.getElementById('email');
    const password = document.getElementById('password');
    const loginBtn = document.getElementById('sub-login');
    const loginHome = document.getElementById('login-home');
    const roles = ['Regular User']
    const user = {}
    var backend = "";
    var s3;
    loginBtn.addEventListener('click', function (event) {
        event.preventDefault();
        if (email.value == '' || password.value == '') {
            timeOut("Please fill in all fields", 'red', errorsDiv);
        } else if (IsValidPassword(password.value) === false) {
            timeOut("Password must be at least 8 characters long", 'red', errorsDiv);

        } else if (IsValidPassword(password.value) === true && IsValidEmail(email.value) === true) {
            loginUser();

        } else {
            timeOut("Error with email or password. Please try again", 'red', errorsDiv)
        }
        // reset login form when button clicked
        loginForm.reset()

    });

    loginHome.addEventListener('click', function (event) {

    });

    var otpContainer = document.querySelector(".otp-container");
    var errorsOtp = document.getElementById("errors");
    const otpForm = document.querySelector("#otp-form");
    const otpDisplay = document.querySelector("#otp-display");
    var otpContainer = document.querySelector(".otp-container");
    var loginContainer = document.querySelector(".login-container");

    function loginUser() {
        user.Username = email.value;
        user.Password = password.value;
        getProfileUsername(email.value)
        fetch("./config.json").then((response) => response.json()).then((json) => {
            backend = json.backend;
            s3 = json.s3;
            axios.post(json.backend + authenticationServer, user).then(function (responseAfter) {
                // turning jwt signature from the response into a json object
                var base64Url = responseAfter.data.split('.')[1];
                var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
                var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
                    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
                }).join(''));
                const jsonObj = JSON.parse(jsonPayload);
                if (jsonObj.authenticated === "true" && jsonObj.role !== 'Anonymous User') {

                    // save JWT token to local storage
                    localStorage.setItem("jwtToken", responseAfter.data)
                    localStorage.setItem("username", jsonObj.unique_name)
                    localStorage.setItem("role", jsonObj.role)
                    localStorage.setItem("id", jsonObj.nameid)
                    localStorage.setItem("userhash", jsonObj.userhash)



                    // display otp
                    otpDisplay.style.color = "blue";
                    otpDisplay.innerHTML = "<h3>" + jsonObj.otp + "</h3>";
                    otpDisplay.style.fontSize = "20px";
                    loginContainer.style.display = "none";
                    showOtp();

                    // take in otp value to post in the back end
                    let otpInput = document.querySelector("#otp-input");
                    const otpBtn = document.querySelector("#otp-submit");
                    otpBtn.addEventListener('click', function (event) {
                        const role = getRole()
                        event.preventDefault();
                        if (otpInput.value == '') {
                            timeOut("Please enter OTP", 'red', errorsOtp);

                        } else if (otpInput.value == jsonObj.otp) {
                            axios.get(json.backend + "/Pin/LoadMap", {
                                headers: {
                                    'Authorization': `Bearer ${responseAfter.data}`
                                }
                            }).then(function (response) {
                                var script = document.createElement('script');
                                script.src = response.data;
                                script.async = true;

                                document.head.appendChild(script);
                            });

                            if (role.reg.includes(jsonObj.role) || role.service.includes(jsonObj.role)) {
                                timeOut("OTP verified", 'green', errorsOtp);
                                return regView();

                            }

                            if (role.admin.includes(jsonObj.role)) {
                                timeOut("OTP verified", 'green', errorsOtp);
                                return adminView();
                            }

                        } else {
                            timeOut("Invalid OTP. Please try again", 'red', errorsOtp);
                        }
                        otpForm.reset();
                    });
                }
                else {
                    // unauthorized user
                }
            }).catch(function (error) {
                let errorAfter = error.responseAfter.data;
                let cleanError = errorAfter.replace(/"/g, "");
                timeOut(cleanError, 'red', errorsDiv)
            })
        })
    }

    //
    function getProfileUsername(username) {
        let name = username.substring(0, username.lastIndexOf("@"));
        return localStorage.setItem('profileUsername', name);
    }
    // Getters
    function getUserID() {
        return userID;
    }
    function getUsername() {
        return username;
    }
    /*
    function getRole()
    {
        return role;
    }
    */
    function getAuthenticated() {
        return authenticated;
    }
    function getOtp() {
        return otp;
    }
    function getOtpCreated() {
        return otpCreated;
    }
    function getUserhash() {
        return userhash;
    }


    // Allow access
    root.Utification = root.Utification || {};

    root.Utification.userID = getUserID();
    root.Utification.username = getUsername();
    // root.Utification.role = getRole();
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
