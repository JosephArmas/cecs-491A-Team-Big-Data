
const registrationServer= 'https://localhost:7259/account/registration';
const registerEmail = document.getElementById('r-email');
const registerPassword = document.getElementById('r-pw');
const confirmedPassword = document.getElementById('r-cpw');
const regBtn = document.getElementById('regBtn-submit');
const regForm = document.getElementById('registration-form');
var regHome = document.getElementById("reg-home");
const newUser = {}
regBtn.addEventListener('click', function (event)
{
   event.preventDefault();
   if (registerEmail.value == '' || registerPassword.value == '' || confirmedPassword.value == '')
   {
      timeOut('Please fill in all fields', 'red', errorsDiv);
   } else if(!(registerPassword.value === confirmedPassword.value))
   {
      timeOut('Passwords do not match', 'red', errorsDiv)

   } else if(IsValidPassword(registerPassword.value) === true && IsValidEmail(registerEmail.value) === true)
   {
      registerUser();

   }
   else if (IsValidPassword(registerPassword.value) === false)
   {
      timeOut('Password must be at least 8 characters long', 'red', errorsDiv);
   } else{
      
      timeOut('Error with email or password. Please try again', 'red', errorsDiv)
      
   }
   regForm.reset()

});

regHome.addEventListener('click', function (event)
{
   errorsCont.innerHTML= "";
});


function registerUser()
{
   
   newUser._username = registerEmail.value;
   newUser._password = registerPassword.value;
   // console.log(newUser)
   axios.post(registrationServer,newUser).then(function (response)
   {
      let responseAfter = response.data
      timeOut(responseAfter, 'green', errorsDiv)
   }).catch(function (error)
   {
      let errorAfter = error.data
      timeOut(errorAfter, 'red', errorsDiv)

   });
   regForm.reset();
   
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