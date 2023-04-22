let otpBuild = false;
function generateOTP()
{
    let otp = '';
    var random = Math.floor(Math.random() * 3);
    var count = 0 
    while (count < 10)
    {
        var character = random;
        // 0-9
        if (character == 0)
        {
            otp += Math.floor(Math.random() * 10);
            count++;
        }
        // a-z
        if (character == 1)
        {
            otp = otp + String.fromCharCode(Math.floor(Math.random() * 26) + 97);
            count++;
        }
        // A-Z
        if (character == 2)
        {
            otp = otp + String.fromCharCode(Math.floor(Math.random() * 26) + 65);
            count++;
        }
        random = Math.floor(Math.random() * 3);
    }
    return otp.toString();
}

function showOtp(otpVal, role)
{
    let otpContainer = document.querySelector(".otp-container");
    let loginContainer = document.querySelector(".login-container");
    let anonContainer = document.querySelector(".anon-container");
    let otpDisplay = document.querySelector('.otp-display');
    console.log('inside showOtp')
    console.log('otpVal: ' + otpVal)
    buildOTP(otpVal,role);
    otpDisplay.innerHTML = otpVal;
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
    anonContainer.style.display = "none";
    let submitBtn = document.querySelector('#otp-submit');
    submitBtn.addEventListener('click', function(event)
    {
        if (otpInput.value == otpVal && roles.reg.includes(role))
        {
            console.log(otpInput.value)
            regView();
        } else if (otpInput.value == otpVal && roles.admin.includes(role))
        {
            console.log(role)
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
        // let otpVal = generateOTP();
        // otpDisplay.innerHTML = otpVal;
        let otpInput = document.createElement('input');
        otpInput.setAttribute('type','text');
        otpInput.id = 'otp-input';
        otpInput.required = true;
        otp.appendChild(otpInput);
        let submitBtn = document.createElement('button');
        submitBtn.setAttribute('type','submit');
        submitBtn.textContent = 'Submit';
        submitBtn.id = 'otp-submit'
        submitBtn.addEventListener('click', function(event)
        {
            if (otpInput.value == otpVal && roles.reg.includes(userType))
            {
                console.log(otpInput.value)
                regView();
            } else if (otpInput.value == otpVal && roles.admin.includes(userType))
            {
                console.log(userType)
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
        submitDiv.appendChild(submitBtn);
        otpBuild = true;
    }

}
