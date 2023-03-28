
const registrationServer= 'https://localhost:7259/account/registration';

function registerUser()
{
   const registerEmail = document.getElementById('r-email');
   const registerPassword = document.getElementById('r-email');
   const registerForm = document.getElementById('registration-form');

   user.username = registerEmail.value;
   user.password = registerPassword.value;
   axios.post(registrationServer,user).then(function (response)
   {
      let responseAfter = response.data;
      let cleanResponse = responseAfter.replace(/"/g,"");
      timeOut(cleanResponse + '. Please return to home screen to login.', 'green', errorsDiv);
   }).catch(function (error)
   {
      let errorAfter = error.response.data
      let cleanError = errorAfter.replace(/"/g,"");
      timeOut(cleanError, 'red', errorsDiv);

   });
   registerForm.reset();
   
}
function buildRegistration()
{
    let registrationContainer = document.querySelector(".registration-container");
    let registerForm = document.createElement('form');
    registerForm.id = "registration-form";
    let backBtnDiv = document.createElement('div');
    backBtnDiv.setAttribute('class','back-button');
    let backBtn= document.createElement('button');
    backBtn.setAttribute('type','button');
    backBtn.id = 'reg-home'
    backBtn.textContent = 'Back'
    backBtn.addEventListener('click',homeClicked);
    backBtnDiv.appendChild(backBtn);
    registerForm.appendChild(backBtnDiv);
    let boxDiv = document.createElement('div');
    boxDiv.setAttribute('class', 'reg-box')
    let regTitle = document.createElement('h1');
    regTitle.id = 'reg-title';
    regTitle.textContent = "Register";
    boxDiv.appendChild(regTitle);
    let inputDiv = document.createElement('div');
    inputDiv.setAttribute('class','input-field');
    let email = document.createElement('input');
    let password = document.createElement('input');
    let confirmPassword = document.createElement('input');
    let submitBtn = document.createElement('button');
    email.setAttribute('type','email');
    email.id = "r-email";
    email.setAttribute('placeholder','Email Address');
    email.required = true;
    password.setAttribute('type','password');
    password.id = "r-pw";
    password.setAttribute('placeholder','Password');
    password.required = true;
    password.minLength = 8;
    confirmPassword.setAttribute('type','password');
    confirmPassword.required = 'true';
    confirmPassword.minLength = 8;
    confirmPassword.setAttribute('placeholder','Confirm Password');
    submitBtn.id = "regBtn-submit";
    submitBtn.textContent = "Submit";
    submitBtn.addEventListener('click', function (event)
    {
        event.preventDefault();
        if (password.value == '' || password.value == '' || password.value == '')
        {
           timeOut('Please fill in all fields','red',errorsDiv);

        } else if(password.value !== confirmPassword.value)
        {
            timeOut('Passwords do not match','red',errorsDiv);

     
        } else if(IsValidPassword(password.value) === true && IsValidEmail(email.value) === true)
        {
           registerUser();
     
        }
        else if (IsValidPassword(password.value) === false)
        {
            timeOut('Password must be at least 8 characters long','red',errorsDiv)
        
        } else{
           
            timeOut('Error with email or password. Please try again','red',errorsDiv);
           
        }
    });
    inputDiv.appendChild(email);
    inputDiv.appendChild(password);
    inputDiv.appendChild(confirmPassword);
    inputDiv.appendChild(submitBtn);
    let contactDiv = document.createElement('div');
    contactDiv.setAttribute('class','reg-contact');
    let contactSupportBtn = document.createElement('button');
    contactSupportBtn.textContent = "Contact Support";
    contactDiv.appendChild(contactSupportBtn);
    boxDiv.appendChild(inputDiv);
    boxDiv.appendChild(contactDiv);
    registerForm.appendChild(boxDiv);
    registrationContainer.appendChild(registerForm);
    
}

// * Debugging Purposes
/*
function displayRegisterData()
{
   let testElement = document.getElementById('test-data');

let newUser = 
{
  "username": "test@gmail.com",
   "password": "password"

}
axios.post("https://localhost:7259/account/registration", newUser).then(function (response)
{
   console.log(response.data);
   testElement.innerHTML = JSON.stringify(response.data);

});


}
*/

// displayRegisterData();