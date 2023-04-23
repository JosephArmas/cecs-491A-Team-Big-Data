
let buildEventDropdown = false


function showEventMenu(latlng)
{
    let eventDiv = document.querySelector(".event-dropdown-container");
    console.log(latlng.lat())
    console.log(latlng.lng())
    buildDropDown(latlng.lat(), latlng.lng());
    eventDiv.style.display = "block";


}

function buildDropDown(lat, lng)
{
    if(!buildEventDropdown)
    {
        let eventDropdown = document.querySelector(".event-dropdown");
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
        markLabel.setAttribute('for','checkmark-label');
        titleEvent.setAttribute('type','text');
        titleEvent.minLength = 8;
        titleEvent.style.placeholder = "Title";
        descriptionEvent.setAttribute('type','text');
        descriptionEvent.minLength = 8;
        titleEvent.id = "titleEvent";
        descriptionEvent.id = "descriptionEvent";
        titleLabel.textContent = "Title";
        descriptionLabel.textContent = "Description";
        markLabel.textContent= "Check to display attendance"
        inputDiv.appendChild(titleLabel);
        inputDiv.appendChild(titleEvent);
        inputDiv.appendChild(descriptionLabel);
        inputDiv.appendChild(descriptionEvent);
        inputDiv.appendChild(markLabel);
        inputDiv.appendChild(checkMark);
        boxDiv.appendChild(inputDiv);
        boxDiv.appendChild(btnDiv);
        submitBtn.addEventListener('click', function (event)
        {
            createEvent(titleEvent.value, descriptionEvent.value, 3175, lat, lng); 

        });
        
        
        buildEventDropdown = true;
    }
}

// showEventMenu()
async function getEvents()
{
    const endPoint = getEndPoint();
    try 
    {
        let result = await axios.get(endPoint.getEventPins);
        let eventPins = result.data;

        return eventPins

   } catch {

        return timeOut('Error retrieving event pins', 'red', responseDiv)
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
        console.log(response.data);
        let marker = new google.maps.Marker({
            position: {lat: lat, lng: lng}, 
            title: title,
            description: description,
            icon: pinColor
    });

    }).catch (function (error)
    {
        timeOut('Error creating event pin', 'red', responseDiv);
    })


}

function markerHelper(lat,lng,title,description)
{
    let pinColor = "http://maps.google.com/mapfiles/ms/icons/yellow-dot.png"
    let marker = new google.maps.Marker({
        position: {lat: lat, lng: lng}, 
        title: title,
        description: description,
        icon: pinColor
    });

    return marker;
}

async function placeMarker(map)
{
    let markers = [];

    // Store the eventPins obj to loop through and use custom helper
    let eventPins = await getEvents();
    eventPins.forEach(pin => {
        markers.push(markerHelper(pin._lat,pin._lng,pin._title,pin._description));
    });

    // Loop through appended pins and add to map
    markers.forEach(marker =>{
        marker.setMap(map)
        
        // attaching an info window to these markers
        const infowindow = new google.maps.InfoWindow({
            content: "<div class='event-info-btn'>" +
                    "<h1>" + marker.title + "</h1>" 
                    + "<p>" + marker.description + "'\n'attendance: " 
                    + "</p>"
                    + "<button id='modify-event'>Modify</button>"
                    + "<button id='cancel-event'>Cancel</button>"
                    + "<button id='info-join-event'>Join</button>"
                    + "<button id='unjoin-event'>Unjoin</button>"
                    + "</div>"
        });
        
        marker.addListener("click", () => {
            infowindow.open(map, marker)
        })
        let cancelBtn = document.querySelector("#cancel-event");
        let modifyBtn = document.querySelector("#cancel-event");
        let joinBtn = document.querySelector("#info-join-event");
        let unjoinBtn = document.querySelector("#unjoin-event");

    });

    return markers;
}

function cancelEvent(eventID,userID)
{
    const endPoint = getEndPoint();
    axios.post(endPoint.cancelEvent, data).then(function (response)
    {
        timeOut(response.data, 'green', responseDiv);
    }).catch(function (error)
    {
        timeOut(error.response.data, 'red', responseDiv);
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
            console.log(pin._title);
            console.log(pin._description);
            console.log(pin._count);
            console.log(pin._lat);
            console.log(pin._lng);
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

// test();