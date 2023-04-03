
let registerBuild = false;

function registerUser()
{
   const user = {};
   const server = getServer();
   const registerEmail = document.getElementById('r-email');
   const registerPassword = document.getElementById('r-pw');
   const registerForm = document.getElementById('registration-form');
   user.username = registerEmail.value;
   user.password = registerPassword.value;
   axios.post(server.registrationServer,user).then(function (response)
   {
      console.log('sucess, logging to database ' + response.data)
      let responseAfter = response.data;
      let cleanResponse = responseAfter.replace(/"/g,"");
      timeOut(cleanResponse + '. Please return to home screen to login.', 'green', responseDiv);
   }).catch(function (error)
   {
      let errorAfter = error.response.data
      let cleanError = errorAfter.replace(/"/g,"");
      timeOut(cleanError, 'red', responseDiv);

   });
   registerForm.reset();
   
}
   
function buildRegistration()
{
   if(!registerBuild)
   {
      let backBtnDiv = document.querySelector('#registration-form .back-button');
      let backBtn = document.createElement('button');
      backBtn.setAttribute('type','button');
      backBtn.textContent = 'Back';
      backBtn.addEventListener('click',homeClicked);
      backBtnDiv.appendChild(backBtn);
      let inputDiv = document.querySelector('.input-field');
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
      confirmPassword.id = "r-cpw"
      confirmPassword.minLength = 8;
      confirmPassword.setAttribute('placeholder','Confirm Password');
      submitBtn.id = "regBtn-submit";
      submitBtn.textContent = "Submit";
      submitBtn.addEventListener('click', function (event)
      {
         if(password.value !== confirmPassword.value)
         {
            timeOut('Passwords do not match','red',responseDiv);

         } else if(IsValidPassword(password.value) === true && IsValidEmail(email.value) === true)
         {
            registerUser();

         } else{
            timeOut('Error with email or password. Please try again','red',responseDiv);
         }
      });
      inputDiv.appendChild(email);
      inputDiv.appendChild(password);
      inputDiv.appendChild(confirmPassword);
      inputDiv.appendChild(submitBtn);
      let contactDiv = document.querySelector('.reg-contact');
      let contactSupportBtn = document.createElement('button');
      contactSupportBtn.textContent = "Contact Support";
      contactSupportBtn.addEventListener('click', function (event)
      {
         alert('Contact Support')

      });
      contactDiv.appendChild(contactSupportBtn);
      registerBuild = true;
   }

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