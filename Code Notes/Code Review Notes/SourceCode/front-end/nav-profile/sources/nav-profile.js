// TODO:
// * input validations
// * check if passwords match when confirming
// * have the ability to see password?
document.querySelector("#container").addEventListener("click",secClicked());
function secClicked()
{
    // var btnContainer = document.querySelector(".container");
    var btnContainer = document.querySelector("#container");
    if (btnContainer.style.display === "none")
    {
        btnContainer.style.display = "block";
    }
    else
    {
        btnContainer.style.display = "none";
    }

}

function updateClicked()
{
    var updateContainer = document.querySelector(".nav-update");
    var navContainer = document.querySelector(".nav-home");
    navContainer.style.display = "none";
    updateContainer.style.display = "block";

}

function homeClicked()
{
    var navContainer = document.querySelector(".nav-home");
    var updateContainer = document.querySelector(".nav-update");
    var passwordContainer = document.querySelector(".change-password-container");
    var userNameContainer = document.querySelector(".change-username-container");
    var btnContainer = document.querySelector("#container");
    if (updateContainer.style.display === "block")
    {
        updateContainer.style.display = "none";
        navContainer.style.display = "block";
    }
    else if(passwordContainer.style.display === "block")
    {
        passwordContainer.style.display = "none";
        btnContainer.style.display = "none";
        navContainer.style.display = "block";

    } 
    else if(userNameContainer.style.display === "block")
    {
        userNameContainer.style.display = "none";
        navContainer.style.display = "block";

    }
    // else {
    //     passwordContainer.style.dispaly ="none";
    //     updateContainer.style.dispaly ="none";
    //     navContainer.style.dispaly = "none";


    // }

}

function changePasswordClicked()
{
    var navContainer = document.querySelector(".nav-home");
    var passwordContainer = document.querySelector(".change-password-container");
    navContainer.style.display = "none";
    passwordContainer.style.display = "block";
}

function changeUsernameClicked()
{

    var navContainer = document.querySelector(".nav-home");
    var userNameContainer = document.querySelector(".change-username-container");
    navContainer.style.display = "none";
    userNameContainer.style.display = "block";
}