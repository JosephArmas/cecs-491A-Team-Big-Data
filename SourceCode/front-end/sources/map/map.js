'use strict';

// The wrapper is from Vatanak Vong's AJAX Demo in his GitHub. https://github.com/v-vong3/csulb/tree/master/cecs_491/demo/ajax-demo
// Google maps api code follows Google's map api doc tutorials. https://developers.google.com/maps/documentation/javascript/overview#maps_map_simple-javascript
(function (root) {
    
    // Dependency check
    const isValid = root;

    root.Utification = root.Utification || {};

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

    let pinsInfo = []
    let pinsMarker = []
    let infoWindows = []


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

    function getMarkerHandler(map) {
        const webServiceUrl = 'https://localhost:7259/Pin/GetAllPins';

        pinsInfo = []
        pinsMarker = []
        infoWindows = []

        // Connect to backend to get markers as an authorized user
        var request = axios.get(webServiceUrl,{
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        });
        request.then(function (response) {
        
            for (var i = 0; i < response.data.length; i++) {
                var currResponse = response.data[i]
                
                const pin = new google.maps.Marker({
                    position: {lat:parseFloat(currResponse.lat),lng:parseFloat(currResponse.lng)},
                    map: map,
                    icon: {
                        url: PIN_ICONS[currResponse.pinType]
                    }
                });

                //Users can mark a pin complete
                var pinContent = currResponse.description + `<br>Created: ${currResponse.dateCreated}<br>`;
                if (currResponse.userID != localStorage.getItem("id"))
                {
                    pinContent += `<button id='completePin' onclick='completePinHandler(${i})'>Complete Pin</button>`;
                }

                if(localStorage.getItem("role")=="Regular User")
                {
                    pinContent += `<button id='reputation-view-btn' onclick='reputationView(${currResponse.userID})'>View Reputation</button>`;
                }
                
                //User can modify/delete their pins and admin can modify/delete anyone's pin
                if (localStorage.getItem("role")== "Admin User" || localStorage.getItem("id") == currResponse.userID)
                {
                    pinContent = pinContent + `<button id='modifyPin' onclick='modifyPinHandler(${i})'>Modify Pin</button>`;
                    pinContent = pinContent + `<button id='uploadPic' onclick='uploadPicture(${i})'>Upload Picture</button>`;
                    pinContent = pinContent + `<button id='deletePic' onclick='deletePicture(${i})'>Delete Picture</button>`;                    
                }
    
                const infowindow = new google.maps.InfoWindow({
                    content: pinContent
                });
                

                pinsInfo.push(response.data[i])
                pinsMarker.push(pin);
                infoWindows.push(infowindow);
                
                // Do not display disabled or complete pins
                if (currResponse.disabled === 1)
                {
                    pinsMarker[i].setMap(null);
                }
                else
                {
                    let x = i;
                    pin.addListener("click", () => {
                        infowindow.open({
                            anchor: pin,
                            map,
                            shouldFocus: false,
                        });
                        downloadPicture(x, (updateContent) =>
                        {
                            infoWindows[i].setContent(updateContent);
                        });
                    });
                }
            }
        });

        
    }

    // David
    window.uploadPicture = function(pos)
    {
        let pinID = pinsInfo[pos].pinID;
        let content = pinsInfo[pos].description;
        let fileSelector = document.getElementById("fileSelector");
        let file = fileSelector.files[0];
        if(file === undefined)
        {
            alert("Use the File Selector on the Top Right");
        }
        else
        {
            let filename = file.name;
            let length = filename.length;
            let ext = filename.substring(length - 4, length)
            if(ext != ".jpg" && ext != ".png" && ext != ".JPG" && ext != ".PNG")
            {
                alert("Incorrect File Extension");
            }
            else
            {
                let url = URL.createObjectURL(file);
                //rebuild content
                content += "<img id=\"PinPic\" style=\"height:100%; width:100%; object-fit:contain\" src=" + url + ">";
                content += `<br>Created: ${pinsInfo[pos].dateTime}<br><button id='completePin' onclick='completePinHandler(${pos})'>Complete Pin</button>`
                if (localStorage.getItem("role")=="Admin User" || localStorage.getItem("role")=="Regular User" || localStorage.getItem("role")== "Reputable User"|| localStorage.getItem("id") == pinsInfo[pos].userID)
                {
                    content += `<button id='modifyPin' onclick='modifyPinHandler(${pos})'>Modify Pin</button>`;
                    content += `<button id='updatePic' onclick='updatePicture(${pos})'>Update Picture</button>`;
                    content += `<button id='deletePic' onclick='deletePicture(${pos})'>Delete Picture</button>`;
                }
                infoWindows[pos].setContent(content);
                let params = {
                    fileName: filename,
                    ID: pinID,
                    role: localStorage.getItem("role"),
                    userID: localStorage.getItem("id")
                }
                axios.post(backend + "pinUpload", params).catch(function (error)
                {
                    let errorAfter = error.response.data;
                    let cleanError = errorAfter.replace(/"/g,"");
                    timeOut(cleanError, 'red', errorsDiv)
                }).then(function(key)
                {
                    if(key === undefined)
                    {
        
                    }
                    else if(key.data.length > 0)
                    {
                        uploadToS3(key.data);
                    }
                })
            }
        }
    }

    window.downloadPicture = function(pos, updateContent)
    {
        let pinID = pinsInfo[pos].pinID;
        // Rebuild content
        let content = pinsInfo[pos].description;
        let config = {
            headers : {"ID": pinID}
        };
        // Get the key from the backend SQL Server
        axios.post(backend + "pinDownload", 0, config).catch(function (error)
        {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g,"");
            timeOut(cleanError, 'red', errorsDiv)
        }).then(function(key)
        {
            if(key === undefined)
            {

            }
            else if(key.data.length > 0)
            {
                // Download file from S3
                axios.get(s3 + key.data).catch(function (error)
                {
                    let errorAfter = error.response.data;
                    let cleanError = errorAfter.replace(/"/g,"");
                    timeOut(cleanError, 'red', errorsDiv)
                }).then(function(file)
                {
                    // Picture stored as a DataURL for easy access
                    content += "<img  id=\"PinPic\" style=\"height:100%; width:100%; object-fit:contain\" src=" + file.data + ">";
                    content += `<br>Created: ${pinsInfo[pos].dateTime}<br><button id='completePin' onclick='completePinHandler(${pos})'>Complete Pin</button>`
                    if (localStorage.getItem("role")=="Admin User"  || localStorage.getItem("id") == pinsInfo[pos].userID)
                    {
                        content += `<button id='modifyPin' onclick='modifyPinHandler(${pos})'>Modify Pin</button>`;
                        content += `<button id='updatePic' onclick='updatePicture(${pos})'>Update Picture</button>`;
                        content += `<button id='deletePic' onclick='deletePicture(${pos})'>Delete Picture</button>`;
                    }
                    infoWindows[pos].setContent(content);
                    updateContent(content);
                })
            }
            else{
                updateContent(content);
            }
        })
    }

    window.updatePicture = function(pos)
    {
        let pinID = pinsInfo[pos].pinID;
        let content = pinsInfo[pos].description;
        let fileSelector = document.getElementById("fileSelector");
        let file = fileSelector.files[0];
        if(file === undefined)
        {
            alert("Use the File Selector on the Top Right");
        }
        else
        {
            let filename = file.name;
            let length = filename.length;
            let ext = filename.substring(length - 4, length)
            if(ext != ".jpg" && ext != ".png" && ext != ".JPG" && ext != ".PNG")
            {
                alert("Incorrect File Extension");
            }
            else
            {
                let url = URL.createObjectURL(file);
                //rebuild content
                content += "<img id=\"PinPic\"style=\"height:100%; width:100%; object-fit:contain\" src=" + url + ">";
                content += `<br>Created: ${pinsInfo[pos].dateTime}<br><button id='completePin' onclick='completePinHandler(${pos})'>Complete Pin</button>`
                if (localStorage.getItem("role")=="Admin User" || localStorage.getItem("id") == pinsInfo[pos].userID)
                {
                    content += `<button id='modifyPin' onclick='modifyPinHandler(${pos})'>Modify Pin</button>`;
                    content += `<button id='updatePic' onclick='updatePicture(${pos})'>Update Picture</button>`;
                }
                infoWindows[pos].setContent(content);
                let params = {
                    fileName: filename,
                    ID: pinID,
                    role: localStorage.getItem("role"),
                    userID: localStorage.getItem("id")
                }
                axios.post(backend + "pinUpdate", params).catch(function (error)
                {
                    let errorAfter = error.response.data;
                    let cleanError = errorAfter.replace(/"/g,"");
                    timeOut(cleanError, 'red', errorsDiv)
                }).then(function(key)
                {
                    if(key === undefined)
                    {
        
                    }
                    else if(key.data.length > 0)
                    {
                        uploadToS3(key.data);
                    }
                })
            }
        }
    }

    window.deletePicture = function(pos)
    {
        let pinID = pinsInfo[pos].pinID;
        //rebuild content
        let content = pinsInfo[pos].description;
        content += `<br>Created: ${pinsInfo[pos].dateTime}<br><button id='completePin' onclick='completePinHandler(${pos})'>Complete Pin</button>`
        if (localStorage.getItem("role")=="Admin User" || localStorage.getItem("id") == pinsInfo[pos].userID)
        {
            content += `<button id='modifyPin' onclick='modifyPinHandler(${pos})'>Modify Pin</button>`;
            content += `<button id='uploadPic' onclick='uploadPicture(${pos})'>Upload Picture</button>`;
            content += `<button id='deletePic' onclick='deletePicture(${pos})'>Delete Picture</button>`;
        }
        infoWindows[pos].setContent(content);
        let params = {
            fileName: "",
            ID: pinID,
            role: localStorage.getItem("role"),
            userID: localStorage.getItem("id")
        }
        axios.post(backend + "pinDelete", params).catch(function (error)
        {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g,"");
            timeOut(cleanError, 'red', errorsDiv)
        }).then(function(key)
        {
            if(key === undefined)
            {

            }
            else if(key.data.length > 0)
            {            
                deleteFromS3(key.data);
            }
        })
    }

    window.completePinHandler = function(pos)
    {
        infoWindows[pos].close();
        pinsMarker[pos].setMap(null);

        const webServiceUrl = 'https://localhost:7259/Pin/CompleteUserPin';
        
        const pin = {}
        pin.PinID = pinsInfo[pos].pinID;
        pin.UserID = pinsInfo[pos].userID;
        pin.Lat = pinsInfo[pos].lat;
        pin.Lng = pinsInfo[pos].lng;
        pin.PinType = pinsInfo[pos].pinType;
        pin.Description = pinsInfo[pos].description;
        pin.Userhash = localStorage.getItem("userhash");

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

        let userAction = prompt("1. Modify Pin Type\n2. Modify Pin Content\n3. Delete Pin\nPick Options 1-3: ");
        if (!(userAction == "1" || userAction == "2" || userAction == "3")||userAction == null)
        {
            timeOut("Invalid Pin Input...", 'red', errorsDiv)
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

        let pinType = prompt("Modifying Pin Type\n1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");
        if (!(pinType == "1" || pinType == "2" ||pinType == "3" ||pinType == "4" ||pinType == "5")||pinType == null)
        {
            timeOut("Invalid Pin Input...", 'red', errorsDiv)
            return;
        };

        const pin = {}
        pin.PinID = pinsInfo[pos].pinID;
        pin.UserID = pinsInfo[pos].userID;
        pin.Lat = pinsInfo[pos].lat;
        pin.Lng = pinsInfo[pos].lng;
        pin.PinType = pinsInfo[pos].pinType;
        pin.Description = pinsInfo[pos].description;
        pin.Userhash = localStorage.getItem("userhash");

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
            timeOut("Invalid Title Input...", 'red', errorsDiv)
            return;
        };

        let description = prompt("Modifying Pin Content\nEnter pin description");
        if (description == null || !descriptionLimit(description))
        {
            timeOut('Invalid Description Input...', 'red', errorsDiv)
            return;
        };

        let content = `<h1>${title}</h1><p>${description}</p>`;

        const pin = {}
        pin.PinID = pinsInfo[pos].pinID;
        pin.UserID = pinsInfo[pos].userID;
        pin.Lat = pinsInfo[pos].lat;
        pin.Lng = pinsInfo[pos].lng;
        pin.PinType = pinsInfo[pos].pinType;
        pin.Description = content;
        pin.Userhash = localStorage.getItem("userhash");

        axios.post(webServiceUrl, pin, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
            infoWindows[pos].close();

            content = content + `<br>Created: ${pinsInfo[pos].dateTime}<br><button id='completePin' onclick='completePinHandler(${pos});'>Complete Pin</button>`

            //User can delete their pins and admin can delete anyone's pin
            if (localStorage.getItem("role")=="Admin User" || localStorage.getItem("id") == pinsInfo[pos].userID)
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
        pin.PinID = pinsInfo[pos].pinID;
        pin.UserID = pinsInfo[pos].userID;
        pin.Lat = pinsInfo[pos].lat;
        pin.Lng = pinsInfo[pos].lng;
        pin.PinType = pinsInfo[pos].pinType;
        pin.Description = pinsInfo[pos].description;
        pin.Userhash = localStorage.getItem("userhash");

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

        let pinType = prompt("1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");
        if (!(pinType == "1" || pinType == "2" ||pinType == "3" ||pinType == "4" ||pinType == "5")||pinType == null)
        {
            timeOut("Invalid Pin Input...", 'red', errorsDiv)
            return;
        };

        if (pinType == "2")
        {
            return showEventMenu(latLng);
        }

        let title = prompt("Enter pin title.");
        if (title == null || !titleLimit(title))
        {
            timeOut("Invalid Title Input...", 'red', errorsDiv)
            return;
        };
        let description = prompt("Enter pin description");
        if (description == null || !descriptionLimit(description))
        {
            timeOut('Invalid Description Input...', 'red', errorsDiv)
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

        const pin = {}
        pin.PinID = 0;
        pin.UserID = localStorage.getItem('id');
        pin.Lat = `${latLng.lat()}`
        pin.Lng = `${latLng.lng()}`
        pin.PinType = pinType-1;
        pin.Description = content;
        pin.Userhash = localStorage.getItem('userhash')

        axios.post(webServiceUrl, pin, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
            initMap();
        })
        .catch(function (error){
            timeOut(error.data, 'red', errorsDiv)
        });
    }

    root.Utification = root.Utification || {};
   
    window.initMap = function() {
        const mapElements = document.querySelectorAll('.map');
        mapElements.forEach((mapElement) => {
        map = new google.maps.Map(mapElement, 
            {
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
            
            placeMarker(map,localStorage.getItem("id"));
            getMarkerHandler(map);
        });

        // getMarkerHandler();
        //checks jwt signature for role
        if (localStorage.getItem("role")==="Admin User" || localStorage.getItem("role")==="Reputable User"){
            //user can add pins to map
            map.addListener("click", (e) => 
            {
                if (!pinBounds(e.latLng)){
                    return timeOut("Pin is placed out of bounds.. ", 'red',errorsDiv)
                }
                placeNewPin(e.latLng, map);
            });
        }
    }
})(window, window.ajaxClient);

