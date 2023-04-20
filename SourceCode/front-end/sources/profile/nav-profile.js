// TODO:
// * input validations
// * check if passwords match when confirming
// * have the ability to see password?
// document.querySelector("#container").addEventListener("click",secClicked());
profileBuild = false;
eventsBuild = false;
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
/*
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
*/


function buildProfileView()
{
    if(!profileBuild)
    {
        let headerDiv = document.querySelector(".top-button-container");
        let homeBtn= document.createElement('button');
        let logOutBtn= document.createElement('button');
        homeBtn.setAttribute('type', 'button');
        logOutBtn.setAttribute('type', 'button');
        homeBtn.textContent = "Home"
        logOutBtn.textContent = "Logout"
        // * Add Event Listener
        homeBtn.id = "b-h";
        // * Add Event Listener
        logOutBtn.id = "b-l";
        headerDiv.appendChild(homeBtn);
        headerDiv.appendChild(logOutBtn);
        let optionDiv = document.querySelector(".options-container");
        let security = document.createElement('h3')
        let updateProfile = document.createElement('h3')
        let events = document.createElement('h3')
        security.id = "sec";
        updateProfile.id = "update-profile"
        security.textContent = "Security"
        updateProfile.textContent = "Update Profile"
        events.textContent = "Events"
        events.id = "events-option"
        optionDiv.appendChild(security)
        optionDiv.appendChild(events)
        optionDiv.appendChild(updateProfile)
        
        
    }
}

function buildEvents()
{
    if(!eventsBuild)
    {
        let titleContainer = document.querySelector('.title')
        let headerDiv = document.querySelector('.events-container .top-button-container')
        let backBtn = document.createElement('button');
        backBtn.textContent = "Back"
        backBtn.id = "b-h"
        // * Add Event Listener
        headerDiv.appendChild(backBtn);
        let title = document.createElement('h1')
        title.textContent = "Events"
        title.style.textAlign = "center";
        titleContainer.appendChild(title);
        let optionsDiv = document.querySelector('.options')
        // * Add Event Listener
        let ceBtn = document.createElement('button');
        ceBtn.textContent = "Create Events"

        // * Add Event Listener
        let jeBtn = document.createElement('button');
        jeBtn.textContent = "Joined Events"
        
        optionsDiv.appendChild(ceBtn);
        optionsDiv.appendChild(jeBtn);
        

        
    }
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

// buildProfileView();
buildEvents();
