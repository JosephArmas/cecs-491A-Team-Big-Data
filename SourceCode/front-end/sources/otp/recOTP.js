var otpContainer = document.querySelector(".recOTP-container");
var errorsOtp = document.getElementById("errors");
const recOTPBtn = document.querySelector("#recOTP-submit");
const recOTPForm = document.querySelector("#recOTP-form");
const recOTPDisplay = document.querySelector("#recOTP-display");
var recOTPInput = document.querySelector("#recOTP-input");
var rUsername = "";
var rNP = "";
recOTPBtn.addEventListener('click', function (event)
{
    event.preventDefault();
    if (recOTPInput.value == '')
    {
        errorsOtp.innerHTML = "Please enter OTP";

    } else if (recOTPInput.value == otpVal) 
    {
        errorsOtp.innerHTML = "";
        SendRecoveryRequest(rUsername, rNP);
    } else 
    {
        errorsOtp.style.color = "red";
        errorsOtp.innerHTML = "Invalid OTP. Please try again";
    } 
    recOTPForm.reset();
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

function sendRecOTP(u,p)
{
    rUsername = u;
    rNP = p;
    otpVal = generateOTP();
    recOTPDisplay.style.color = "blue";
    recOTPDisplay.innerHTML = otpVal;
    recOTPDisplay.style.fontSize = "20px";
}