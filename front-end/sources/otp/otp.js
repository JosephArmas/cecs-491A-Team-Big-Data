var otpContainer = document.querySelector(".otp-container");
var errorsOtp = document.getElementById("errors");
const otpBtn = document.querySelector("#otp-submit");
const otpForm = document.querySelector("#otp-form");
const otpDisplay = document.querySelector("#otp-display");
var otpInput = document.querySelector("#otp-input");
otpBtn.addEventListener('click', function (event)
{
    event.preventDefault();
    if (otpInput.value == '')
    {
        errorsOtp.stytle.color = "red";
        errorsOtp.innerHTML = "Please enter OTP";

    } else if (otpInput.value == otpVal) 
    {
        errorsOtp.innerHTML = "";
        regView();
        
    } else 
    {
        errorsOtp.style.color = "red";
        errorsOtp.innerHTML = "Invalid OTP. Please try again";
    } 
    otpForm.reset();
});
    

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

function sendOtp()
{
    otpVal = generateOTP();
    otpDisplay.style.color = "blue";
    otpDisplay.innerHTML = otpVal;
    otpDisplay.style.fontSize = "20px";
}