const password = document.getElementById('r-pw');
const confirmPassword = document.getElementById('r-cpw');
const errorResponse = document.getElementById('errors');
// add special characters to array that is included
const specialChar = []
if (password.value.length < 8) {
   errorResponse.innerText = "passwerd must be at least 8 characters long";
}
if (password !== confirmPassword) {
   errorResponse.innerText = "passwords do not match";
}


