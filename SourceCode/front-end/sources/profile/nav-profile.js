// TODO:
// Test
// document.querySelector("#container").addEventListener("click",secClicked());
let profileBuild = false;
let eventsBuild = false;
let eventsCreatedBuild = false;
let eventsJoinedBuild = false;

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
        profileBuild = true;
        
        
    }
}

function buildEvents()
{

    if(!eventsBuild)
    {
        let titleContainer = document.querySelector('.events-container .title')
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
        ceBtn.textContent = "Create Events";
        ceBtn.id = "create-event";

        // * Add Event Listener
        let jeBtn = document.createElement('button');
        jeBtn.textContent = "Joined Events";
        jeBtn.id = "join-event";
        
        optionsDiv.appendChild(ceBtn);
        optionsDiv.appendChild(jeBtn);
        
        eventsBuild = true;
        

        
    }
}

async function buildEventsCreated(userID)
{
    if(!eventsCreatedBuild)
    {
        let titleContainer = document.querySelector('.events-created-container .title')
        let headerDiv = document.querySelector('.events-created-container .top-button-container')
        let backBtn = document.createElement('button');
        let homeDiv = document.querySelector('.home-container')
        let eventDiv = document.querySelector('.events-created-container')
        backBtn.textContent = "Back"
        backBtn.id = "b-h"

        // * Add Event Listener
        headerDiv.appendChild(backBtn);
        backBtn.addEventListener('click', () => {
            // window.location.reload();
            eventDiv.style.display = "none";
            homeDiv.style.display = "block";

        })
        let title = document.createElement('h1')
        title.textContent = "Events Created"
        title.style.textAlign = "center";
        titleContainer.appendChild(title);
        let eventContainer = document.querySelector('.events-created-container .events-created')
        let userID = localStorage.getItem('id');

        // * Do injection 
        let events = await getUserCreatedEvents(userID);
        events.forEach(event => {
            let eventList = document.createElement('ul')
            let eventID = document.createElement('li')
            let cancelBtn = document.createElement('button')
            cancelBtn.textContent = "Cancel"
            eventList.textContent = event.title;
            eventList.appendChild(cancelBtn);
            eventContainer.appendChild(eventList);
            cancelBtn.addEventListener('click', () => {
                return cancelEvent(event.eventID, userID);
                // return initMap();

            })

        });

        eventsCreatedBuild = true;

    }
}

async function buildEventsJoined(userID)
{

    if(!eventsJoinedBuild)
    {
        // timeOut(userID,'red',errorsDiv);
        let titleContainer = document.querySelector('.events-joined-container .title');
        let headerDiv = document.querySelector('.events-joined-container .top-button-container');
        let backBtn = document.createElement('button');
        let homeDiv = document.querySelector('.home-container');
        let eventJoinDiv = document.querySelector('.events-joined-container');
        backBtn.textContent = "Back";
        backBtn.id = "b-h";

        // * Add Event Listener
        headerDiv.appendChild(backBtn);
        backBtn.addEventListener('click', () => {
            eventJoinDiv.style.display = "none";
            homeDiv.style.display = "block";
        });
        let title = document.createElement('h1')
        title.textContent = "Events Joined"
        title.style.textAlign = "center";
        titleContainer.appendChild(title);
        let eventContainer = document.querySelector('.events-joined')

        // * Do injection 
        let events = await getUserEvents(userID);
        events.forEach(event => {
            let eventList = document.createElement('ul')
            let unjoinBtn = document.createElement('button')
            unjoinBtn.textContent = "Unjoin"
            unjoinBtn.id = "unjoin"
            eventList.textContent = event.title;
            eventList.appendChild(unjoinBtn);
            eventContainer.appendChild(eventList);
            unjoinBtn.addEventListener('click', () => {
                return unjoinEvent(event.eventID, userID);
            })

        });
        

        eventsJoinedBuild = true;

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
// buildEvents();
// buildEventsCreated(3489);
// buildEventsJoined(2108);

