
let buildEventDropdown = false

var backend = "";
fetch("./config.json").then((response) => response.json()).then((json) => 
{
backend = json.backend;
})


function showEventMenu(latlng)
{
    let eventDiv = document.querySelector(".event-dropdown-container");
    buildDropDown(latlng.lat(), latlng.lng());
    eventDiv.style.display = "block";

}

function buildDropDown(lat, lng)
{
    if(!buildEventDropdown)
    {
        let eventDropdown = document.querySelector(".event-dropdown-container");
        let titleDiv = document.querySelector(".event-dropdown-container .title");
        let inputDiv = document.querySelector(".event-form .event-inputs");
        let boxDiv = document.querySelector(".event-dropdown-box");
        let title = document.createElement("h3");
        title.textContent = "Create Event";
        title.style.textAlign = "center";
        title.style.color = "white";
        titleDiv.appendChild(title);
        boxDiv.appendChild(titleDiv);
        let titleEvent = document.createElement('input')
        let descriptionEvent = document.createElement('input');
        let titleLabel = document.createElement('label');
        let descriptionLabel = document.createElement('label');
        let checkMark = document.createElement('input');
        let submitBtn = document.createElement('button');
        let cancelBtn = document.createElement('button');
        let btnDiv = document.querySelector(".confirm-btn");
        let markLabel = document.createElement('label');
        submitBtn.textContent = "Submit";

        cancelBtn.textContent = "Cancel";
        btnDiv.appendChild(cancelBtn);
        btnDiv.appendChild(submitBtn);

        checkMark.setAttribute('type','checkbox');
        checkMark.id = 'checkmark-label'
        titleLabel.setAttribute('for','titleEvent');
        descriptionLabel.setAttribute('for','descriptionEvent');
        titleEvent.setAttribute('type','text');
        titleEvent.minLength = 8;
        titleEvent.style.placeholder = "Title";
        descriptionEvent.setAttribute('type','text');
        descriptionEvent.minLength = 8;
        titleEvent.id = "titleEvent";
        descriptionEvent.id = "descriptionEvent";
        titleLabel.textContent = "Title";
        titleLabel.style.color = "white";
        descriptionLabel.textContent = "Description";
        descriptionLabel.style.color = "white";
        inputDiv.appendChild(titleLabel);
        inputDiv.appendChild(titleEvent);
        inputDiv.appendChild(descriptionLabel);
        inputDiv.appendChild(descriptionEvent);
        boxDiv.appendChild(inputDiv);
        boxDiv.appendChild(btnDiv);
        let userID = localStorage.getItem('id');
        console.log(userID)

        submitBtn.addEventListener('click', function (event)
        {
            createEvent(titleEvent.value, descriptionEvent.value, userID, lat, lng); 
            titleEvent.value = "";
            descriptionEvent.value = "";
            eventDropdown.style.display = "none";

        });
        cancelBtn.addEventListener('click', function (event)
        {
            let homeDiv = document.querySelector(".home-container");
            titleEvent.value = "";
            descriptionEvent.value = "";
            eventDropdown.style.display = "none";
            homeDiv.style.display = "block";
        })
        
        
        buildEventDropdown = true;
    }
}

async function getEvents()
{
    const endPoint = getEndPoint();
    try 
    {
        // List of event pin obj stored
        let result = await axios.get(endPoint.getEventPins);
        let eventPins = result.data;

        // returning the list of event pins
        return eventPins

   } catch {

        return timeOut('Error retrieving event pins', 'red', errorsDiv)
   }
}

function createEvent(title, description, userID, lat, lng)
{
    const event = {
        title: title,
        description: description,
        userID: userID,
        lat: lat,
        lng: lng
    }

    const endPoint = getEndPoint();

    axios.post(endPoint.createEventPin, event).then(function (response)
    {
        let pinColor = "http://maps.google.com/mapfiles/ms/icons/yellow-dot.png"
        timeOut(response.data, 'green', errorsDiv);
        let choice = confirm(response.data)
        if (choice)
        {
            new google.maps.Marker
            ({
                position: {lat: lat, lng: lng}, 
                title: title,
                description: description,
                icon: pinColor
            });
            return initMap();
        }
    }).catch (function (error)
    {
        timeOut(error.data, 'red', errorsDiv);
    })
    // return initMap();

}

// Custom Helper to genereate markers 
function markerHelper(lat,lng,title,description, eventID,count)
{
    let pinColor = "http://maps.google.com/mapfiles/ms/icons/yellow-dot.png"
    let marker = new google.maps.Marker({
        position: {lat: lat, lng: lng}, 
        title: title,
        description: description,
        eventID: eventID,
        count: count,
        icon: pinColor
    });

    return marker;
}


function unjoinEvent(eventID, userID)
{
    // let unjoinBtn = document.querySelector("#unjoin-event");
    const endPoint = getEndPoint();
    let choice = confirm("Are you sure you want to unjoin this event?");
    if (choice)
    {
        axios.post(endPoint.unJoinEvent, {"eventID": eventID, "userID": userID}).then((response) => {
            timeOut(response.data +". Refresh to take affect.", 'green', errorsDiv);
            return initMap();
        }).catch((error) => {
            timeOut(error.response.data, 'red', errorsDiv);
        });
    }

}


function joinEvent(eventID,userID)
{
    // let joinBtn = document.querySelector("#info-join-event");
    const endPoint = getEndPoint();
    let result = confirm("Are you sure you want to join this event?");
    if (result)
    {
        axios.post(endPoint.joinEvent, {"eventID": eventID, "userID": userID}).then(function (response)
        {
            
            timeOut(response.data, 'green', errorsDiv);
            return initMap();
            // alert(response.data)
        });
    } else
    {
        return;
    }
}

function cancelEvent(eventID,userID)
{
    const endPoint = getEndPoint();
    let result = confirm("Are you sure you want to cancel this event?");
    if (result)
    {
        axios.post(endPoint.cancelEvent, {"eventID": eventID, "userID": userID}).then(function (response)
        {
            timeOut(response.data, 'green', errorsDiv);
            return initMap();
        }).catch(function (error)
        {
            timeOut(error.response.data, 'red', errorsDiv);
        });
    } else{
        return;
    }
}


async function placeMarker(map, userID)
{
    let markers = [];

    // Store the eventPins obj to loop through and use custom helper
    let eventPins = await getEvents();
    if(eventPins === undefined)
    {
        return -1;
    }
    eventPins.forEach(pin => {
        markers.push(markerHelper(pin.lat,pin.lng,pin.title,pin.description, pin.eventID,pin.count));
    });

    // Loop through appended pins and add to map
    markers.forEach(marker =>{
        marker.setMap(map);
        const infowindow = new google.maps.InfoWindow({
            content: "<div class='event-info-btn'>" +
                    "<h1>" + marker.title + "</h1>" 
                    + "<p>" + marker.description + " attendance: " + marker.count 
                    + "</p>"
                    + "<button id='modify-event'>Modify</button>"
                    + "<button id='cancel-event'>Cancel</button>"
                    + "<button id='info-join-event'>Join</button>"
                    + "<button id='unjoin-event'>Unjoin</button>"
                    + "</div>"
        });

        marker.addListener("click", () => {
            infowindow.open(map, marker);
        });
        
        infowindow.addListener('domready', function (event)
        {
            let joinBtn = document.querySelector("#info-join-event");
            let cancelBtn = document.querySelector("#cancel-event");
            let unjoinBtn = document.querySelector("#unjoin-event");
            let modifyBtn = document.querySelector("#modify-event");

            if (joinBtn)
            {
                joinBtn.addEventListener('click', function (event)
                {
                    return joinEvent(marker.eventID, userID);
                });
                // return;
            } 

            if(unjoinBtn)
            {
                unjoinBtn.addEventListener('click', function (event)
                {
                    unjoinEvent(marker.eventID, userID);
                    return;
                });
            }

            if (cancelBtn)
            {
                cancelBtn.addEventListener('click', function (event)
                {
                    cancelEvent(marker.eventID, userID);
                    return;
                });
            }

            if (modifyBtn)
            {
                modifyBtn.addEventListener('click', function (event)
                {
                    const endPoint = getEndPoint();
                    let choice = prompt("1. Change Title\n2. Change Description\n");
                    if (choice == "1")
                    {
                        let newTitle = prompt("Enter new title");
                        return modifyEvent(newTitle, marker.eventID, userID);

                    }
                    if (choice == "2")
                    {
                        let newDescription = prompt("Enter new description");
                        return modifyEventDescription(newDescription, marker.eventID, userID);
                    }

                });
            }

        });

        });
    


    return markers;
}

function modifyEvent(title, eventID, userID)
{
    let data = {
        "title": title,
        "eventID": eventID,
        "userID": userID
    }
    const endPoint = getEndPoint();

    axios.post(endPoint.modifyTitle, data).then((response) => {
        timeOut(response.data, 'green', errorsDiv);
        return initMap();
    })

}

function modifyEventDescription(description, eventID, userID)
{
    let data = {
        "description": description,
        "eventID": eventID,
        "userID": userID
    }
    const endPoint = getEndPoint();

    axios.post(endPoint.modifyDescription, data).then((response) => {
        timeOut(response.data , 'green', errorsDiv);
        return initMap();
    })

}



function test()
{
    eventDto = 
    {
        "title":[],
        "description":[],
        "count":[],
        "lat":[],
        "lng":[],
    }

    getEvents().then(eventPins => (
        // console.log(eventPins),
        // console.log(eventPins[0]._eventID)
        eventPins.forEach(pin => {
            console.log(pin.title);
            console.log(pin.description);
            console.log(pin.count);
            console.log(pin.lat);
            console.log(pin.lng);
            console.log(pin.eventID)
        })
    ));

        /*
    getEvents().then(eventPins =>{
        eventPins.forEach(pin => {
            console.log(pin._eventID);
        });
    }).catch(error => console.log(error));
    */
    console.log(eventDto.title);



}

// test()

async function getUserEvents(userID)
{
    const endPoint = getEndPoint();

    try
    {
        let result = await axios.post(endPoint.userJoinedEvents,{"userID": userID});
        let events = result.data;

        return events

    } catch
    {
        return timeOut('Error retrieving event pins', 'red', responseDiv)
    }
    
}

async function getUserCreatedEvents(userID)
{
    const endPoint = getEndPoint();

    try
    {
        let result = await axios.post(endPoint.createdEvents,{"userID": userID});
        let events = result.data;

        return events

    } catch
    {
        return timeOut('Error retrieving event pins', 'red', responseDiv)
    }
    
}



// test();