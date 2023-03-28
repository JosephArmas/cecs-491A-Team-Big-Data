
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
function showOtp()
{
    let otpContainer = document.querySelector(".otp-container");
    let loginContainer = document.querySelector(".login-container");
    let anonContainer = document.querySelector(".anon-container");
    let otpForm = document.querySelector("#otp-form");
    if(!otpForm)
    {
        buildOTP();

    }
    otpContainer.style.display = "block";
    loginContainer.style.display = "none";
    anonContainer.style.display = "none";
}

function buildOTP()
{

    let otpContainer = document.querySelector(".otp-container");
    let otpForm = document.createElement('form'); 
    otpForm.id = "otp-form";
    let backBtnDiv = document.createElement('div');
    backBtnDiv.setAttribute('class','back-button');
    let backBtn = document.createElement('button');
    backBtn.setAttribute('type','button');
    backBtn.textContent = "Back";
    backBtn.addEventListener('click',homeClicked);
    backBtnDiv.appendChild(backBtn);
    otpForm.appendChild(backBtnDiv);
    let otpTitle = document.createElement('h2');
    otpTitle.id = "otp-title";
    otpTitle.textContent = "Enter OTP";
    otpForm.appendChild(otpTitle);
    let otpDisplay = document.createElement('div');
    let otpVal = generateOTP();
    otpDisplay.setAttribute('class','otp-display');
    otpDisplay.innerHTML = otpVal;
    otpForm.appendChild(otpDisplay);
    let otp = document.createElement('div');
    otp.setAttribute('class','otp');
    let otpInput = document.createElement('input');
    otpInput.setAttribute('type','text');
    otpInput.required = true;
    otp.appendChild(otpInput);
    otpForm.appendChild(otp);
    let submitDiv = document.createElement('div');
    submitDiv.setAttribute('class','submit')
    let submit = document.createElement('button');
    submit.setAttribute('type','submit');
    submit.textContent = "Submit";
    submit.addEventListener('click',function(event)
    {
        event.preventDefault();
        if (otpInput.value == '')
        {
            timeOut('Please enter OTP','red',errorsDiv)
    
        } else if (otpInput.value == otpVal && roles.includes(userType))  
        {
            regView();
            
        } else if(otpInput.value == otpVal && userType === "Admin User")
        {
    
            adminView();
    
        } else 
        {
            timeOut('Invalid OTP. Please try again','red', errorsDiv)
        } 
        otpForm.reset();
    })
    submitDiv.appendChild(submit);
    otpForm.appendChild(submitDiv);
    otpContainer.appendChild(otpForm);

}
