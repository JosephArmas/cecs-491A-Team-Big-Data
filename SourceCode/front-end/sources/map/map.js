'use strict';

// The wrapper is from Vatanak Vong's AJAX Demo in his GitHub. https://github.com/v-vong3/csulb/tree/master/cecs_491/demo/ajax-demo
// Google maps api code follows Google's map api doc tutorials. https://developers.google.com/maps/documentation/javascript/overview#maps_map_simple-javascript
(function (root) {
    
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

    function getMarkerHandler() {
        const webServiceUrl = 'https://localhost:7259/Pin/GetAllPins';
        // Connect to backend to get markers as an authorized user
        var request = axios.get(webServiceUrl, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        });
        request.then(function (response) {
            for (var i = 0; i < response.data.length; i++) {
                var currResponse = response.data[i]

                // Do not display disabled or complete pins
                if (!(currResponse._disabled === 1 || currResponse._completed === 1))
                {
                    const pin = new google.maps.Marker({
                        position: {lat:parseFloat(currResponse._lat),lng:parseFloat(currResponse._lng)},
                        map: map,
                        icon: {
                            url: PIN_ICONS[currResponse._pinType]
                        }
                    });
        
                    const infowindow = new google.maps.InfoWindow({
                        content: currResponse._description
                    });
            
                    pin.addListener("click", () => {
                        infowindow.open({
                          anchor: pin,
                          map,
                          shouldFocus: false,
                        });
                        google.maps.event.addListener(infowindow, 'domready', ()=>{
                            var someButton = document.getElementsByClassName('deletePin');
                            /*infowindow.addListener(someButton,'click', ()=>{
                                console.log(`working delete pin PinID: ${currResponse._pinID}`);
                            })*/
                            google.maps.event.addListener(someButton,'dblclick',console.log(`working delete pin PinID: ${currResponse._pinID}`));
                        });
                    })
                    
                }
            }
        });
    }

    function deleteMarkerHandler(pinId){
        console.log("opened infowindow");
        var someButton = document.getElementsByClassName('deletePin');
        console.log(someButton);
        //someButton.addEventListener("dblclick",console.log(`working delete pin PinID: ${pinId}`));
        google.maps.listener.addEventListener(someButton,'click',    
        () => {
            // show something.
            // add something.
            console.log(`working delete pin PinID: ${pinId}`);
        });
        //check is authorized user
        //post to backend
        //refresh map
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

    function placeNewPin(latLng, map) {
        const webServiceUrl = 'https://localhost:7259/Pin';
        errorsDiv.innerHTML = "";
        let pinType = prompt("1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");
        if (!(pinType == "1" || pinType == "2" ||pinType == "3" ||pinType == "4" ||pinType == "5")||pinType == null)
        {
            errorsDiv.innerHTML = "Invalid Pin Input...";
            return;
        };

        let title = prompt("Enter pin title.");
        if (title == null || !titleLimit(title))
        //if (title == null)
        {
            errorsDiv.innerHTML = "Invalid Title Input...";
            return;
        };
        let description = prompt("Enter pin description");
        if (description == null || !descriptionLimit(description))
        //if (description == null)
        {
            errorsDiv.innerHTML = "Invalid Description Input...";
            return;
        };

        let content = `<h1>${title}</h1><p>${description}</p><button class="deletePin">Delete Pin</button>`;

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

        map.em

        map.panTo(latLng);

        const pin = {}
        pin.pinID = 0;
        pin.userID = 0;
        pin.lat = `${latLng.lat()}`
        pin.lng = `${latLng.lng()}`
        pin.pinType = pinType-1;
        pin.completed = 0;
        pin.description = content;
        pin.disabled = 0;

        axios.post(webServiceUrl, pin, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        }).then(function (responseAfter)
        {
        }).catch(function (error)
        {
        });
    }

    root.myApp = root.myApp || {}
   
    window.initMap = function() {
        errorsDiv.innerHTML = "";
        map = new google.maps.Map(document.getElementById('map'), {
            center: CSULB,
            minZoom: 8,
            maxZoom: 18,
            zoom: 15,
            mapId: 'bb1d4678c71528ff',
            restriction: {
                latLngBounds: CALIFORNIA_BOUNDS,
                strictBounds: false
            },
            mapTypeControl: false
        });
        getMarkerHandler();
        
        //checks jwt signature for role
        if (localStorage.getItem("role")==="Admin User"){
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
       
    }

})(window, window.ajaxClient);

