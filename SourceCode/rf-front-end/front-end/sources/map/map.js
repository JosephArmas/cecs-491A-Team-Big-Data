'use strict';

// The wrapper is from Vatanak Vong's AJAX Demo in his GitHub. https://github.com/v-vong3/csulb/tree/master/cecs_491/demo/ajax-demo
// Google maps api code follows Google's map api doc tutorials. https://developers.google.com/maps/documentation/javascript/overview#maps_map_simple-javascript
(function (root) {
    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAAfbLnE9etZVZ0_ZqaAPUMl03BfKLN8kI&region=US&language=en&callback=initMap';
    script.async = true;
    console.log(localStorage.getItem("role"));
    // Dependency check
    const isValid = root;
    if(!isValid){
        // Handle missing dependencies
        alert("Missing dependencies");
    }

    const CALIFORNIA_BOUNDS = {
        north: 42.009517,
        south: 32.528832,
        west: -124.482003,
        east: -114.131211,
    };

    const PIN_ICONS = [
        "http://maps.google.com/mapfiles/ms/icons/blue-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/yellow-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/green-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/purple-dot.png"
    ]

    const CSULB = { lat: 33.7838, lng: -118.1141 };

    let map;

    var pinsInfo = []
    var pinsMarker = []
    var infoWindows = []

    var errorsDiv = document.getElementById('errors');

    // Checks if within California State Borders
    function pinBounds(latLng) {
        if ((latLng.lat()<42.009517&&latLng.lat()>39)&&(latLng.lng()<-124||latLng.lng()>-120))
        {
            return false;
        }
        else if ((latLng.lat()<39&&latLng.lat()>38)&&(latLng.lng()<-124||latLng.lng()>-119))
        {
            return false;
        }
        else if ((latLng.lat()<38&&latLng.lat()>37)&&(latLng.lng()<-123||latLng.lng()>-118))
        {
            return false;
        }
        else if ((latLng.lat()<37&&latLng.lat()>36)&&(latLng.lng()<-122||latLng.lng()>-117))
        {
            return false;
        }
        else if ((latLng.lat()<36&&latLng.lat()>35)&&(latLng.lng()<-121||latLng.lng()>-116))
        {
            return false;
        }  
        else if ((latLng.lat()<35&&latLng.lat()>34)&&(latLng.lng()<-120||latLng.lng()>-115))
        {
            return false;
        }  
        else if ((latLng.lat()<34&&latLng.lat()>33)&&(latLng.lng()<-119||latLng.lng()>-114))
        {
            return false;
        }
        else if ((latLng.lat()<33&&latLng.lat()>32.528832)&&(latLng.lng()<-118||latLng.lng()>-114))
        {
            return false;
        }
        else 
        {
            return true;
        }             
    }

    function titleLimit(title){
        let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]")
        if (title.length < 8 || title.length > 30 || !charactersAllowed.test(title))
        {
            return false;
        }
        return true;
    }

    function descriptionLimit(description){
        let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]")
        if (description.split(' ').length > 150 || !charactersAllowed.test(description))
        {
            return false;
        }
        return true;
    }

    function getMarkerHandler() {
        const webServiceUrl = 'https://localhost:7259/Pin/GetAllPins';
        pinsInfo = []
        pinsMarker = []
        infoWindows = []
        // Connect to backend to get markers as an authorized user
        var request = axios.get(webServiceUrl, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        });
        request.then(function (response) {
        
            for (var i = 0; i < response.data.length; i++) {
                var currResponse = response.data[i]
                
                const pin = new google.maps.Marker({
                    position: {lat:parseFloat(currResponse._lat),lng:parseFloat(currResponse._lng)},
                    map: map,
                    icon: {
                        url: PIN_ICONS[currResponse._pinType]
                    }
                });

                //Users can mark a pin complete
                var pinContent = currResponse._description + `<br>Created: ${currResponse._dateTime}<br><button id='completePin' onclick='completePinHandler(${i})'>Complete Pin</button>`

                //User can modify/delete their pins and admin can modify/delete anyone's pin
                if (localStorage.getItem("role")=="Admin User" || localStorage.getItem("id") == currResponse._userID)
                {
                    pinContent = pinContent + `<button id='modifyPin' onclick='modifyPinHandler(${i})'>Modify Pin</button>`;
                }
    
                const infowindow = new google.maps.InfoWindow({
                    content: pinContent
                });

                pinsInfo.push(response.data[i])
                pinsMarker.push(pin);
                infoWindows.push(infowindow);
                
                // Do not display disabled or complete pins
                if (currResponse._disabled === 1 || currResponse._completed === 1)
                {
                    pinsMarker[i].setMap(null);
                }
                else
                {
                    pin.addListener("click", () => {
                        infowindow.open({
                            anchor: pin,
                            map,
                            shouldFocus: false,
                        });
                    });
                }
            }
        });
    }

    window.completePinHandler = function(pos)
    {
        infoWindows[pos].close();
        pinsMarker[pos].setMap(null);

        const webServiceUrl = 'https://localhost:7259/Pin/CompleteUserPin';
        
        const pin = {}
        pin.pinID = pinsInfo[pos]._pinID;
        pin.userID = pinsInfo[pos]._userID;
        pin.lat = pinsInfo[pos]._lat;
        pin.lng = pinsInfo[pos]._lng;
        pin.pinType = pinsInfo[pos]._pinType;
        pin.description = pinsInfo[pos]._description;
        pin.disabled = pinsInfo[pos]._disabled;
        pin.completed = 1;
        pin.dateTime = pinsInfo[pos]._dateTime;

        axios.post(webServiceUrl, pin, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
            initMap();
        })
        .catch(function (error){
            console.log(error);
            return
        });
    }

    window.modifyPinHandler = function(pos)
    {
        infoWindows[pos].close();
        // errorsDiv.innerHTML = "";

        let userAction = prompt("1. Modify Pin Type\n2. Modify Pin Content\n3. Delete Pin\nPick Options 1-3: ");
        if (!(userAction == "1" || userAction == "2" || userAction == "3")||userAction == null)
        {
            // errorsDiv.innerHTML = "Invalid Pin Input...";
            timeOut('Invalid Pin Input...',red,'response')
            return;
        };
        switch (userAction) { 
            case "1":
                modifyPinTypeHandler(pos);
                break;
            case "2":
                modifyPinContentHandler(pos);
                break;
            case "3":
                deletePinHandler(pos)
                break;
        }
        initMap();
    }

    function modifyPinTypeHandler(pos)
    {
        const webServiceUrl = 'https://localhost:7259/Pin/ModifyPinType';
        // errorsDiv.innerHTML = "";

        let pinType = prompt("Modifying Pin Type\n1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");
        if (!(pinType == "1" || pinType == "2" ||pinType == "3" ||pinType == "4" ||pinType == "5")||pinType == null)
        {
            errorsDiv.innerHTML = "Invalid Pin Input...";
            return;
        };

        const pin = {}
        pin.pinID = pinsInfo[pos]._pinID;
        pin.userID = 0;
        pin.lat = ``
        pin.lng = ``
        pin.pinType = pinType-1;
        pin.description = '';
        pin.disabled = 0;
        pin.completed = 0;
        pin.dateTime = '';

        axios.post(webServiceUrl, pin, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
        })
        .catch(function (error){
        });
    }

    function modifyPinContentHandler(pos)
    {
        const webServiceUrl = 'https://localhost:7259/Pin/ModifyPinContent';

        let title = prompt("Modifying Pin Content\nEnter pin title.");
        if (title == null || !titleLimit(title))
        {
            errorsDiv.innerHTML = "Invalid Title Input...";
            return;
        };

        let description = prompt("Modifying Pin Content\nEnter pin description");
        if (description == null || !descriptionLimit(description))
        {
            errorsDiv.innerHTML = "Invalid Description Input...";
            return;
        };

        let content = `<h1>${title}</h1><p>${description}</p>`;

        const pin = {}
        pin.pinID = pinsInfo[pos]._pinID;
        pin.userID = 0;
        pin.lat = ``
        pin.lng = ``
        pin.pinType = 0;
        pin.description = content;
        pin.disabled = 0;
        pin.completed = 0;
        pin.dateTime = '';

        axios.post(webServiceUrl, pin, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
            infoWindows[pos].close();

            content = content + `<br>Created: ${pinsInfo[pos]._dateTime}<br><button id='completePin' onclick='completePinHandler(${pos});'>Complete Pin</button>`

            //User can delete their pins and admin can delete anyone's pin
            if (localStorage.getItem("role")=="Admin User" || localStorage.getItem("id") == pinsInfo[pos]._userID)
            {
                content = content + `<button id='modifyPin' onclick='modifyPinHandler(${pos});'>Modify Pin</button>`;
            }

            infoWindows[pos].setContent(content);
        })
        .catch(function (error){
        });

        
    }

    function deletePinHandler(pos)
    {
        const webServiceUrl = 'https://localhost:7259/Pin/DisablePin';

        infoWindows[pos].close();
        pinsMarker[pos].setMap(null);
        
        const pin = {}
        pin.pinID = pinsInfo[pos]._pinID;
        pin.userID = 0;
        pin.lat = ``
        pin.lng = ``
        pin.pinType = 0;
        pin.description = '';
        pin.disabled = 1;
        pin.completed = 0;
        pin.dateTime = '';

        axios.post(webServiceUrl, pin, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
        })
        .catch(function (error){
        });
    }

    function placeNewPin(latLng, map) {
        const webServiceUrl = 'https://localhost:7259/Pin/PostNewPin';
        // errorsDiv.innerHTML = "";

        let pinType = prompt("1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");
        if (!(pinType == "1" || pinType == "2" ||pinType == "3" ||pinType == "4" ||pinType == "5")||pinType == null)
        {
            errorsDiv.innerHTML = "Invalid Pin Input...";
            return;
        };

        let title = prompt("Enter pin title.");
        if (title == null || !titleLimit(title))
        {
            errorsDiv.innerHTML = "Invalid Title Input...";
            return;
        };
        let description = prompt("Enter pin description");
        if (description == null || !descriptionLimit(description))
        {
            errorsDiv.innerHTML = "Invalid Description Input...";
            return;
        };

        let content = `<h1>${title}</h1><p>${description}</p>`;

        const marker = new google.maps.Marker({
            position: latLng,
            map: map,
            icon: {
                url: PIN_ICONS[pinType-1]
            }
        });

        const infowindow = new google.maps.InfoWindow({
            content: content
        });

        marker.addListener("click", () => {
            infowindow.open({
              anchor: marker,
              map,
              shouldFocus: false,
            });
        });

        map.panTo(latLng);

        var today = new Date();
        var date = today.getFullYear()+'-'+(today.getMonth()+1)+'-'+today.getDate();
        var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

        const pin = {}
        pin.pinID = 0;
        pin.userID = localStorage.getItem('id');
        pin.lat = `${latLng.lat()}`
        pin.lng = `${latLng.lng()}`
        pin.pinType = pinType-1;
        pin.description = content;
        pin.disabled = 0;
        pin.completed = 0;
        pin.dateTime = date+' '+time;

        axios.post(webServiceUrl, pin, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
            initMap();
        })
        .catch(function (error){
        });
    }
   
    window.initMap = function() 
    {
        // Selects all the map elements on html (admin & reg user)
        const mapElements = document.querySelectorAll(".map");

        // Loop through each of the maps elements on html (admin & reg user)
        mapElements.forEach(mapElement => {
            map = new google.maps.Map(mapElement, {
                center: CSULB,
                minZoom: 8,
                maxZoom: 18,
                zoom: 15,
                mapId: 'bb1d4678c71528ff',
                restriction: {
                    latLngBounds: CALIFORNIA_BOUNDS,
                    strictBounds: false
                },
                mapTypeControl: false,
                clickableIcons: false
            });
            getMarkerHandler();
        //checks jwt signature for role
        if (localStorage.getItem("role")==="Admin User"||localStorage.getItem("role")==="Reputable User")
        {
            //user can add pins to map
            map.addListener("click", (e) => 
            {
                if (!pinBounds(e.latLng)){
                    errorsDiv.innerHTML = "Pin is placed out of bounds... "; 
                    return;
                }
                placeNewPin(e.latLng, map);
            });
        }
        });

        
                
    }
    document.head.appendChild(script);
})(window, window.ajaxClient);

