let otpBuild = false;

function showOtp(otpVal, role)
{
    let otpContainer = document.querySelector(".otp-container");
    let loginContainer = document.querySelector(".login-container");
    let anonContainer = document.querySelector(".anon-container");
    let otpDisplay = document.querySelector('.otp-display');
    buildOTP(otpVal,role);
    otpDisplay.innerHTML = otpVal;
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
    anonContainer.style.display = "none";
}

function buildOTP(otpVal,userType)
{
    if(!otpBuild)
    {
        console.log('inside build OTP')
        console.log(userType)
        let backBtnDiv = document.querySelector('#otp-form .back-button');   
        let homeBtn = document.createElement('button');
        let otpDisplay = document.querySelector('.otp-display');
        let otpForm = document.querySelector('#otp-form');
        let otp = document.querySelector('.otp');
        let submitDiv = document.querySelector('.submit');
        homeBtn.setAttribute('type','button');
        homeBtn.textContent = 'Home';
        homeBtn.addEventListener('click',homeClicked);
        backBtnDiv.appendChild(homeBtn);
        let otpInput = document.createElement('input');
        otpInput.setAttribute('type','text');
        otpInput.id = 'otp-input';
        otpInput.name = 'otp-name';
        otpInput.required = true;
        otp.appendChild(otpInput);
        otpForm.appendChild(otp);
        let submitBtn = document.createElement('button');
        submitBtn.setAttribute('type','submit');
        submitBtn.textContent = 'Submit';
        submitBtn.id = 'otp-submit'
        submitDiv.appendChild(submitBtn);
        otpForm.appendChild(submitDiv);

        let role = getRole()
        submitBtn.addEventListener('click', function(event)
        {
            if (otpInput.value == otpVal && role.reg.includes(userType))
            {
                regView();
            } else if (otpInput.value == otpVal && role.admin.includes(userType))
            {
                adminView();
            } else if (otpInput.value !== otpVal)
            {
                timeOut('Invalid OTP','red',responseDiv);
            }
            else
            {
                timeOut('You are not authorized to register','red',responseDiv);
            }
            
            otpForm.reset();
        });
        otpBuild = true;
    }

}
