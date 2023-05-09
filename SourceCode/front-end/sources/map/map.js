'use strict';

// The wrapper is from Vatanak Vong's AJAX Demo in his GitHub. https://github.com/v-vong3/csulb/tree/master/cecs_491/demo/ajax-demo
// Google maps api code follows Google's map api doc tutorials. https://developers.google.com/maps/documentation/javascript/overview#maps_map_simple-javascript
(function (root) {

    // Dependency check
    const isValid = root;

    root.Utification= root.Utification || {};

    if (!isValid) {
        // Handle missing dependencies
        alert("Missing dependencies");
    }

    var backend = "";
    fetch("./config.json").then((response) => response.json()).then((json) => 
    {
        backend = json.backend;
    })

    var webServiceUrl = "";

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
        if ((latLng.lat() < 42.009517 && latLng.lat() > 39) && (latLng.lng() < -124 || latLng.lng() > -120)) {
            return false;
        }
        else if ((latLng.lat() < 39 && latLng.lat() > 38) && (latLng.lng() < -124 || latLng.lng() > -119)) {
            return false;
        }
        else if ((latLng.lat() < 38 && latLng.lat() > 37) && (latLng.lng() < -123 || latLng.lng() > -118)) {
            return false;
        }
        else if ((latLng.lat() < 37 && latLng.lat() > 36) && (latLng.lng() < -122 || latLng.lng() > -117)) {
            return false;
        }
        else if ((latLng.lat() < 36 && latLng.lat() > 35) && (latLng.lng() < -121 || latLng.lng() > -116)) {
            return false;
        }
        else if ((latLng.lat() < 35 && latLng.lat() > 34) && (latLng.lng() < -120 || latLng.lng() > -115)) {
            return false;
        }
        else if ((latLng.lat() < 34 && latLng.lat() > 33) && (latLng.lng() < -119 || latLng.lng() > -114)) {
            return false;
        }
        else if ((latLng.lat() < 33 && latLng.lat() > 32.528832) && (latLng.lng() < -118 || latLng.lng() > -114)) {
            return false;
        }
        else {
            return true;
        }
    }

    function titleLimit(title) {
        let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]")
        if (title.length < 8 || title.length > 30 || !charactersAllowed.test(title)) {
            return false;
        }
        return true;
    }

    function descriptionLimit(description) {
        let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]")
        if (description.split(' ').length > 150 || !charactersAllowed.test(description)) {
            return false;
        }
        return true;
    }

    function getMarkerHandler(map) {
        webServiceUrl = backend + '/Pin/GetAllPins';

        pinsInfo = []
        pinsMarker = []
        infoWindows = []

        // Connect to backend to get markers as an authorized user
        axios.get(webServiceUrl, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        }).then(function (response) {

            for (let i = 0; i < response.data.length; i++) {
                var currResponse = response.data[i]

                const pin = new google.maps.Marker({
                    position: { lat: parseFloat(currResponse.lat), lng: parseFloat(currResponse.lng) },
                    map: map,
                    icon: {
                        url: PIN_ICONS[currResponse.pinType]
                    }
                });

                //Users can mark a pin complete
                var pinContent = currResponse.description + `<br>Created: ${currResponse.dateCreated}<br>`;
                if (currResponse.userID != localStorage.getItem("id")) {
                    pinContent += `<button id='completePin' onclick='completePinHandler(${i})'>Complete Pin</button>`;

                }

                pinContent += `<button id='downloadPic' onclick='Utification.downloadPicture(${i})'>Download Picture</button>`;

                if (((localStorage.getItem("role")==="Regular User" || localStorage.getItem("role")==="Reputable User") && localStorage.getItem("id") !== currResponse.userID)) {
                    pinContent += `<button id='reputation-view-btn' onclick='window.Utification.reputationView(${currResponse.userID})'>View Reputation</button>`;
                }

                //User can modify/delete their pins and admin can modify/delete anyone's pin
                if (localStorage.getItem("role") == "Admin User" || localStorage.getItem("id") == currResponse.userID) {
                    pinContent += `<button id='modifyPin' onclick='modifyPinHandler(${i})'>Modify Pin</button>`;
                    pinContent += `<button id='uploadPic' onclick='uploadPicture(${i})'>Upload Picture</button>`;
                    pinContent += `<button id='deletePic' onclick='deletePicture(${i})'>Delete Picture</button>`;
                    pinContent += `<button id='updatePic' onclick='updatePicture(${i})'>Update Picture</button>`;
                    pinContent += `<button id='requestService' onclick='requestingService(${i})'>Request Service</button>`;      
                }

                const infowindow = new google.maps.InfoWindow({
                    content: pinContent
                });


                pinsInfo.push(response.data[i])
                pinsMarker.push(pin);
                infoWindows.push(infowindow);

                pin.addListener("click", () => {
                    infowindow.open({
                        anchor: pin,
                        map,
                        shouldFocus: false,
                    });
                });
            }
        });


    }

    // David
    window.uploadPicture = function (pos) {
        let pinID = pinsInfo[pos].pinID;
        let content = pinsInfo[pos].description;
        let fileSelector = document.getElementById("fileSelector");
        let file = fileSelector.files[0];
        if (file === undefined) {
            alert("Use the File Selector on the Top Right");
        }
        else {
            let filename = file.name;
            let length = filename.length;
            let ext = filename.substring(length - 4, length)
            if(file.size > 1000000)
            {
                alert("File too big");
                return 0;
            }
            if (ext != ".jpg" && ext != ".png" && ext != ".JPG" && ext != ".PNG") {
                alert("Incorrect File Extension");
            }
            else {
                let url = URL.createObjectURL(file);
                //rebuild content
                content += `<img  id=\"PinPic\" style=\"height:100%; object-fit:contain\" src=` + file.data + `>`;
                let pic = document.getElementById("picture")
                let params = {
                    fileName: filename,
                    ID: pinID,
                    role: localStorage.getItem("role"),
                    userID: localStorage.getItem("id")
                }
                axios.post(backend + "/File/pinUpload", params).catch(function (error) {
                    let errorAfter = error.response.data;
                    let cleanError = errorAfter.replace(/"/g, "");
                    timeOut(cleanError, 'red', errorsDiv)
                }).then(function (key) {
                    if (key === undefined) {

                    }
                    else if (key.data.length > 0) {
                        uploadToS3(key.data);
                    }
                })
            }
        }
    }

    window.Utification.downloadPicture = function (pos) {
        let pinID = pinsInfo[pos].pinID;
        // Rebuild content
        let content = pinsInfo[pos].description;
        let config = {
            headers: { "ID": pinID }
        };
        // Get the key from the backend SQL Server
        axios.post(backend + "/File/pinDownload", 0, config).catch(function (error) {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g, "");
            timeOut(cleanError, 'red', errorsDiv)
        }).then(function (key) {
            if (key === undefined) {

            }
            else if (key.data.length > 0) {
                // Download file from S3
                axios.get(s3 + "/" + key.data).catch(function (error) {
                    let errorAfter = error.response.data;
                    let cleanError = errorAfter.replace(/"/g, "");
                    timeOut(cleanError, 'red', errorsDiv)
                }).then(function (file) {
                    // Picture stored as a DataURL for easy access
                    content += `<img  id=\"PinPic\" style=\"height:100%; object-fit:contain\" src=` + file.data + `>`;
                    let pic = document.getElementById("picture")
                    pic.innerHTML = content;
                })
            }
        })
    }


    window.updatePicture = function (pos) {
        let pinID = pinsInfo[pos].pinID;
        let content = pinsInfo[pos].description;
        let fileSelector = document.getElementById("fileSelector");
        let file = fileSelector.files[0];
        if (file === undefined) {
            alert("Use the File Selector on the Top Right");
        }
        else {
            let filename = file.name;
            let length = filename.length;
            let ext = filename.substring(length - 4, length)
            if(file.size > 1000000)
            {
                alert("File too big");
                return 0;
            }
            if (ext != ".jpg" && ext != ".png" && ext != ".JPG" && ext != ".PNG") {
                alert("Incorrect File Extension");
            }
            else {
                let url = URL.createObjectURL(file);
                //rebuild content
                // Picture stored as a DataURL for easy access
                content += `<img  id=\"PinPic\" style=\"height:100%; object-fit:contain\" src=` + file.data + `>`;
                let pic = document.getElementById("picture")
                pic.innerHTML = content;
                let params = {
                    fileName: filename,
                    ID: pinID,
                    role: localStorage.getItem("role"),
                    userID: localStorage.getItem("id")
                }
                axios.post(backend + "/File/pinUpdate", params).catch(function (error) {
                    let errorAfter = error.response.data;
                    let cleanError = errorAfter.replace(/"/g, "");
                    timeOut(cleanError, 'red', errorsDiv)
                }).then(function (key) {
                    if (key === undefined) {

                    }
                    else if (key.data.length > 0) {
                        uploadToS3(key.data);
                    }
                })
            }
        }
    }

    window.deletePicture = function (pos) {
        let pinID = pinsInfo[pos].pinID;
        //rebuild content
        let content = pinsInfo[pos].description;
        content += `<br>Created: ${pinsInfo[pos].dateCreated}<br><button id='completePin' onclick='completePinHandler(${pos})'>Complete Pin</button>`
        if (localStorage.getItem("role") == "Admin User" || localStorage.getItem("id") == pinsInfo[pos].userID) {
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
        axios.post(backend + "pinDelete", params).catch(function (error) {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g, "");
            timeOut(cleanError, 'red', errorsDiv)
        }).then(function (key) {
            if (key === undefined) {

            }
            else if (key.data.length > 0) {
                deleteFromS3(key.data);
            }
        })
    }

    window.completePinHandler = function (pos) {
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

        webServiceUrl = backend + "/Reputation/GainReputation";

        axios.post(webServiceUrl, {}, {
            headers: {
                "Authorization": `Bearer ${localStorage.getItem("jwtToken")}`
            }
        }).then(function (response){
            let successMessage = response.data;
            //let formatMessage = successMessage.replace(/"/g,"");
            timeOut(successMessage, "green", errorsDiv);

            webServiceUrl = backend + '/Pin/CompleteUserPin';
        
            axios.post(webServiceUrl, pin, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
                }
            })
            .then(function (responseAfter) {
                initMap();
            })
            .catch(function (error) {
                console.log(error);
                return
            });
        })
        .catch(function (error){
            let errorMessage = error.data;
            let formatMessage = errorMessage.replace(/"/g,"");
            timeOut(formatMessage, "red", errorsDiv);
        });
    }        

    window.modifyPinHandler = function (pos) {
        infoWindows[pos].close();
        pinsMarker[pos].setMap(null);

        let userAction = prompt("1. Modify Pin Type\n2. Modify Pin Content\n3. Delete Pin\nPick Options 1-3: ");
        if (!(userAction == "1" || userAction == "2" || userAction == "3") || userAction == null) {
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
window.requestingService = function(pos) {
   let servicelistContainer = document.querySelector(".servicelist-container");
   let homeContainer = document.querySelector(".home-container");
   homeContainer.style.display = "none";
   servicelistContainer.style.display = "block";

   let distanceInput = document.createElement("input");
   distanceInput.setAttribute("id", "distance");
   distanceInput.setAttribute("type", "number");
distanceInput.setAttribute("max", 20);
distanceInput.setAttribute("min", 1);

   let distContBtn = document.createElement("button");
   distContBtn.setAttribute("id", "distancecontinue");
   distContBtn.innerText = "Continue";

   let cancelBtn = document.createElement("button");
   cancelBtn.setAttribute("id", "requestCancelBtn");
   cancelBtn.innerText = "Exit";

   let distanceLabel = document.createElement("label");
distanceLabel.innerText = "Insert the distance for a list of services";

document.getElementById("ResponseButtons").appendChild(distanceLabel);
document.getElementById("ResponseButtons").appendChild(document.createElement("br"));
document.getElementById("ResponseButtons").appendChild(distanceInput);
document.getElementById("ResponseButtons").appendChild(distContBtn);
document.getElementById("ResponseButtons").appendChild(document.createElement("br"));
   document.getElementById("ResponseButtons").appendChild(cancelBtn);

   document.getElementById("requestCancelBtn").addEventListener('click', () => {

     homeContainer.style.display = "block";
     servicelistContainer.style.display = "none";

     let container = document.querySelector('#ResponseButtons');
     container.innerHTML = "";
     document.getElementById("serviceList").innerHTML = "";
   });

   let data = [];
   let list = document.getElementById("serviceList");
   let choice = "";

   function lister(x, y) {
     x.forEach((service) => {
       let button = document.createElement("button");
       button.classList.add('servButton');
       button.innerText = service.ServiceName;
       y.appendChild(button);
     });
   }

   function buttonlisten() {
     document.querySelectorAll(".servButton").forEach(item => {
       item.addEventListener('click', event => {
         data.forEach((service) => {
           document.getElementById('infoTab').style.visibility = 'visible';
           if (item.innerText == service.ServiceName) {
             document.getElementById('infoTitle').innerText = service.ServiceName;
             document.getElementById('infoDesc').innerText = service.ServiceDescription;
             document.getElementById('infoNum').innerText = service.ServicePhone;
             choice = service;
           }
         })
       });
     });
   }

   function regularRequests() {
     let requestBtn = document.createElement("button");
     requestBtn.setAttribute("id", "requestBtn");
     requestBtn.innerText = "Request";
     document.getElementById("ResponseButtons").appendChild(requestBtn)

     requestBtn.addEventListener("click", event => {
       axios.post('https://localhost:7259/UserServices/CreateRequest', {
         RequestID: 0,
         ServiceID: choice.ServiceID,
         ServiceName: choice.ServiceName,
         Requester: localStorage.getItem("id"),
         RequestLat: pinsInfo[pos].lat,
         RequestLong: pinsInfo[pos].lng,
         PinType: pinsInfo[pos].pinType,
         Accept: 2,
         Distance: document.getElementById("distance").value
       }, {
         headers: {
           'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
         }
       });
     });
   }


   document.getElementById('infoTab').style.visibility = 'hidden';
   let parent = document.getElementById("distanceForm");
   let distance = document.getElementById("distance");
   //distance.value = 20;
   function distanceBtn() {
     document.getElementById("serviceList").innerHTML = "";
     if (distance.value >= 1 && distance.value <= 25) {
let request = axios.post('https://localhost:7259/UserServices/GetServices', {
         RequestID: 0,
         ServiceID: 0,
         ServiceName: "",
         Requester: localStorage.getItem("id"),
         RequestLat: pinsInfo[pos].lat,
         RequestLong: pinsInfo[pos].lng,
         PinType: pinsInfo[pos].pinType,
         Accept: 2,
         Distance: distance.value
       }, {
         headers: {
           'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
         }
       }).then(function(response) {
         data = response.data;
         lister(data, list);
         buttonlisten();
	if(data.length == 0){
	list.innerHTML = "No local services";
}
       });
       //parent.close();

     }
   }
   document.getElementById("distancecontinue").addEventListener('click', distanceBtn);
   //parent.showModal();
   regularRequests();

 }

    function modifyPinTypeHandler(pos) {
       webServiceUrl = backend + '/Pin/ModifyPinType';

        let pinType = prompt("Modifying Pin Type\n1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");
        if (!(pinType == "1" || pinType == "2" || pinType == "3" || pinType == "4" || pinType == "5") || pinType == null) {
            timeOut("Invalid Pin Input...", 'red', errorsDiv)
            return;
        };

        const pin = {}
        pin.PinID = pinsInfo[pos].pinID;
        pin.UserID = pinsInfo[pos].userID;
        pin.Lat = pinsInfo[pos].lat;
        pin.Lng = pinsInfo[pos].lng;
        pin.PinType = pinType - 1;
        pin.Description = pinsInfo[pos].description;
        pin.Userhash = localStorage.getItem("userhash");

        axios.post(webServiceUrl, pin, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
            .then(function (responseAfter) {
            })
            .catch(function (error) {
            });
    }

    function modifyPinContentHandler(pos) {
        webServiceUrl = backend + '/Pin/ModifyPinContent';

        let title = prompt("Modifying Pin Content\nEnter pin title.");
        if (title == null || !titleLimit(title)) {
            timeOut("Invalid Title Input...", 'red', errorsDiv)
            return;
        };

        let description = prompt("Modifying Pin Content\nEnter pin description");
        if (description == null || !descriptionLimit(description)) {
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
            .then(function (responseAfter) {
                infoWindows[pos].close();

                content = content + `<br>Created: ${pinsInfo[pos].dateCreated}<br><button id='completePin' onclick='completePinHandler(${pos});'>Complete Pin</button>`

                //User can delete their pins and admin can delete anyone's pin
                if (localStorage.getItem("role") == "Admin User" || localStorage.getItem("id") == pinsInfo[pos].userID) {
                    content = content + `<button id='modifyPin' onclick='modifyPinHandler(${pos});'>Modify Pin</button>`;
                }

                infoWindows[pos].setContent(content);
            })
            .catch(function (error) {
            });


    }

    function deletePinHandler(pos) {
        webServiceUrl = backend + '/Pin/DisablePin';

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
            .then(function (responseAfter) {
            })
            .catch(function (error) {
            });
    }

    function placeNewPin(latLng, map) {
        webServiceUrl = backend + '/Pin/PostNewPin';

        let pinType = prompt("1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");
        if (!(pinType == "1" || pinType == "2" || pinType == "3" || pinType == "4" || pinType == "5") || pinType == null) {
            timeOut("Invalid Pin Input...", 'red', errorsDiv)
            return;
        };

        if (pinType == "2") {
            return showEventMenu(latLng);
        }

        let title = prompt("Enter pin title.");
        if (title == null || !titleLimit(title)) {
            timeOut("Invalid Title Input...", 'red', errorsDiv)
            return;
        };
        let description = prompt("Enter pin description");
        if (description == null || !descriptionLimit(description)) {
            timeOut('Invalid Description Input...', 'red', errorsDiv)
            return;
        };

        let content = `<h1>${title}</h1><p>${description}</p>`;

        const marker = new google.maps.Marker({
            position: latLng,
            map: map,
            icon: {
                url: PIN_ICONS[pinType - 1]
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
        pin.PinType = pinType - 1;
        pin.Description = content;
        pin.Userhash = localStorage.getItem('userhash')

        axios.post(webServiceUrl, pin, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
            .then(function (responseAfter) {
                initMap();
            })
            .catch(function (error) {
                timeOut(error.data, 'red', errorsDiv)
            });
    }

    root.Utification = root.Utification || {};

    window.initMap = function () {
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

            placeMarker(map, localStorage.getItem("id"));
            getMarkerHandler(map);
        });

        // getMarkerHandler();
        //checks jwt signature for role
        if (localStorage.getItem("role") === "Admin User" || localStorage.getItem("role") === "Reputable User") {
            //user can add pins to map
            map.addListener("click", (e) => {
                if (!pinBounds(e.latLng)) {
                    return timeOut("Pin is placed out of bounds.. ", 'red', errorsDiv)
                }
                placeNewPin(e.latLng, map);
            });
        }
    }
})(window, window.ajaxClient)