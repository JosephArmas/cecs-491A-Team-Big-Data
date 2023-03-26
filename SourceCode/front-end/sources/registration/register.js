
const registrationServer= 'https://localhost:7259/account/registration';

function registerUser()
{
   const registerEmail = document.getElementById('r-email');
   const registerPassword = document.getElementById('r-email');
   const registerForm = document.getElementById('registration-form');

   user.username = registerEmail.value;
   user.password = registerPassword.value;
   // console.log(newUser)
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