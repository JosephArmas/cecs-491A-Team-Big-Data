'use strict';
//(function (root) {
    
    // Dependency check
    const isValid = window;
    if(!isValid){
        // Handle missing dependencies
        alert("Missing dependencies");
    }
    let alertInfo = []
    function descriptionLimit(description){
        let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]")
        if (description.split(' ').length > 50 || !charactersAllowed.test(description))
        {
            return false;
        }
        return true;
    }
    function postAlert( lat, lng, pinType) {
        const endPoint = getEndPoint();
        let userID = localStorage.getItem('id');
      
        getZipcode(lat, lng).then(function (zipcode) {
            const dateTime = new Date();
            //this is to get the date in readable format
            const dateFormat = dateTime.toLocaleDateString('en-US', { year: 'numeric', month: '2-digit', day: '2-digit' });
            let description = `${dateFormat}: ${userID} has posted a ${pinType} at ${zipcode}`;
            if (!descriptionLimit(description)) {
                console.error("Invalid description.");
                return;
            }
            const alert = {
              userID: userID,
              lat: lat,
              lng: lng,
              description: description,
              dateTime: time,
              pinType: pinType,
              read: 0,
              zipcode: zipcode,
            };
      
            try {
              return axios.post(endPoint.PostNewAlert, alert).then(function (response) {
                console.log("Alert posted:", response.data);
              })
            } catch (error) {
              console.error("Error posting alert:", error);
            }
          })
      }

    /*function getAlerts() {
        const webServiceUrl = 'https://localhost:7259/Alert/GetAllAlerts';
        alertInfo = []
        alertMade = []
        var request = axios.get(webServiceUrl, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        });
        request.then(function (response) {
    
            for (var i = 0; i < response.data.length && i<100; i++) {
                var currResponse = response.data[i]
    
                var alertContent = currResponse._description + `<br>Created: ${currResponse._dateTime}<br><button id='read-button' onclick='readAlertHandler(${i})'>Read Alerts</button>`
    
                alertInfo.push(response.data[i])
                alertMade.push(alerts)
                if (currResponse._read === 1)
                {
                    alertMade[i].setAlert(null);
                }
                else
                {
                    const alerts = {}
                    alerts.alertID = alertInfo[pos]._alertID;
                    alerts.userID = alertInfo[pos]._userID;
                    alerts.lat = alertInfo[pos]._lat;
                    alerts.lng = alertInfo[pos]._lng;
                    alerts.description = alertInfo[pos]._description;
                    alerts.read = 1;
                    alerts.dateTime = alertInfo[pos]._dateTime;
    
                    alertMade[i] = new CustomAlert(alert);
    
                    // Add a listener to create an alert when a pin is clicked
                    const marker = new google.maps.Marker({
                        position: { lat: alert.lat, lng: alert.lng },
                        map: map
                    });
                    marker.addListener('click', function() {
                        alertMade[i].createAlert();
                    });
                }
            }
        });
    }*/
    /*let userLatLng = getLatLng();
    let userLat = userLatLng.lat;
    let userLng = userLatLng.lng;*/
    async function alertHealth() {
        try {
          const response = await axios.get('https://localhost:7259/alert/health');
          if (response.status === 200) {
            console.log('Application is healthy');
          } else {
            console.error('Application is unhealthy');
          }
          //let alertH = response.data;
          //return alertH
        } catch (error) {
          console.error('Error checking application health:', error);
        }
      }
      function getAlerts(userLat, userLng) {
        const endPoint = getEndPoint();
        var request = axios.get(endPoint, {
          headers: {
            'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
          }
        });
        request.then(function (response) {
          const alertContainer = document.createElement("div");
          alertContainer.classList.add("alert-container");
      
          const alertBox = document.createElement("div");
          alertBox.classList.add("alert");
      
          const backButton = document.createElement("div");
          backButton.classList.add("alertback-button");
          const backBtn = document.createElement("button");
          backBtn.innerText = "Back";
          backButton.appendChild(backBtn);
          alertBox.appendChild(backButton);
      
          const notificationView = document.createElement("div");
          notificationView.classList.add("notification-view");
          const notificationText = document.createElement("p");
          notificationText.innerText = "Alerts";
          notificationView.appendChild(notificationText);
          alertBox.appendChild(notificationView);
      
          const alertMessages = document.createElement("div");
          alertMessages.classList.add("alert-messages");

          const readButton  = document.createElement("button");
          readButton.classList.add("read-button");
          readButton.innerText = "Mark as Read";
          alertBox.appendChild(readButton);
      
          const readAllButton = document.createElement("button");
          readAllButton.classList.add("readAll-button");
          readAllButton.innerText = "Mark All as Read";
          alertBox.appendChild(readAllButton);
      
          alertInfo = [];
          for (var i = 0; i < response.data.length && i<100; i++) {
            var currResponse = response.data[i];
            if (calculateDistance(userLat, userLng, currResponse.lat, currResponse.lng) > 20.0) {
              continue;
            }
            if (currResponse._read === 1) {
                readButton.disabled = true;
              } else {
                readButton.addEventListener("click", function() {
                    onclick= `readAlertHandler(${i})`
                });
              }
      
            alertInfo.push(currResponse);
          }
      
          localStorage.setItem('alerts', JSON.stringify(alertInfo));
      
          displayAlerts();
      
          alertContainer.appendChild(alertBox);
      
          document.body.appendChild(alertContainer);
        });
      }
    //code found HERE: https://stackoverflow.com/questions/5585957/get-latlng-from-zip-code-google-maps-api
    //and also HERE: https://stackoverflow.com/questions/8372223/get-zip-code-based-on-lat-long 
    // was used inorder to determine how to code the geocodeAddress
    var geocoder;
    var latlng;

    function initMap() {
        geocoder = new google.maps.Geocoder();
        geocoder2 = new google.maps.Geocoder();
        latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    }
    function reverseGeocode(lat, lng) {
        var geocoder = new google.maps.Geocoder();
        var latlng = { lat, lng };
      
        geocoder.geocode({ location: latlng }, function(results, status) {
          if (status === 'OK') {
            if (results[0]) {
              var address = results[0].formatted_address;
              document.getElementById("address").innerHTML = address;
            } else {
              alert('No results found');
            }
          } else {
            alert('Geocoder failed due to: ' + status);
          }
        });
      }

    function geocodeAddress() {
        //var geocoder = new google.maps.Geocoder();
        navigator.geolocation.getCurrentPosition(reverseGeocode);
        //var address = document.getElementById('address').value;
    
        geocoder.geocode({ 'address': address }, function(results, status) {
          if (status === 'OK') {
            var lat = results[0].geometry.location.lat();
            var lng = results[0].geometry.location.lng();
    
            //this geocoder is going to be used to get the zipcode
            //while the first geocode obtained the lat and lng
            //var geocoder2 = new google.maps.Geocoder();
            latlng = new google.maps.LatLng(lat, lng);
            geocoder2.geocode({ 'latLng': latlng }, function(results2, status2) {
              if (status2 === 'OK') {
                for (var i = 0; i < results2[0].address_components.length; i++) {
                  var component = results2[0].address_components[i];
                  if (component.types.indexOf('postal_code') !== -1) {
                    document.getElementById('zipcode').innerHTML = component.long_name;
                  }
                }
              } else {
                alert('Geocode was not successful: ' + status2);
              }
            });
          } else {
            alert('Geocode was not successful: ' + status);
          }
        });
    }
    function getLatLng(){
        //var geocoder = new google.maps.Geocoder();
        navigator.geolocation.getCurrentPosition(function(position) {
            latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            geocoder.geocode({ 'latLng': latlng }, function(results, status) {
                results[0].geometry.location;
            });
        });
    }

    function getZipcode(lat, lng) {
        const geocoder = new google.maps.Geocoder();
        const latlng = new google.maps.LatLng(lat, lng);
      
        geocoder.geocode({ 'latLng': latlng }, function(results, status) {
          if (status === 'OK') {
            for (var i = 0; i < results[0].address_components.length; i++) {
              var component = results[0].address_components[i];
              if (component.types.indexOf('postal_code') !== -1) {
                const zipcode = component.long_name;
                // Use the retrieved zipcode as needed
                console.log(zipcode);
              }
            }
          } else {
            console.error('Geocode was not successful: ' + status);
          }
        });
      }
        //var address = document.getElementById('address').value;
        /*geocoder.geocode({ 'address': address }, function(results, status) {
            if (status === 'OK') {
                var lat = results[0].geometry.location.lat();
                var lng = results[0].geometry.location.lng();
                latlng = new google.maps.LatLng(lat, lng);
                return latlng;
            }
        })*/
    
    
    function calculateDistance(lat1,lat2, lng1, lng2)
    {
        //This code was provided by GeeksforGeeks
        //https://www.geeksforgeeks.org/program-distance-two-points-earth/ 
        // The math module contains a function
        // named toRadians which converts from
        // degrees to radians.
        lng1 =  lng1 * Math.PI / 180;
        lng2 = lng2 * Math.PI / 180;
        lat1 = lat1 * Math.PI / 180;
        lat2 = lat2 * Math.PI / 180;
   
        // Haversine formula
        let dlng = lng2 - lng1;
        let dlat = lat2 - lat1;
        let a = Math.pow(Math.sin(dlat / 2), 2)
                 + Math.cos(lat1) * Math.cos(lat2)
                 * Math.pow(Math.sin(dlng / 2),2);
               
        let c = 2 * Math.asin(Math.sqrt(a));
        let r = 3956;
        distance= c*r;
        return distance;
    }
    function filterAlertsByDistance(alerts, userLat, userLng) {
        const filteredAlerts = alerts.filter(function(alert) {
          const distance = calculateDistance(userLat, userLng, alert.lat, alert.lng);
          return distance <= 20.0; // Within 20 miles
        });
        return filteredAlerts;
    }
    function readAlert(alertElement) {
        var alertContainer = alertElement.closest('.alert-container');
        alertElement.style.display = 'none';

        if (alertContainer.querySelectorAll('.alert:not([style="display: none;"])').length === 0) {
          alertContainer.style.display = 'none';
        }
    }
    function displayAlerts() {
        var alertContainer = document.getElementById('alert-container');
        var alerts = JSON.parse(localStorage.getItem('alerts'));
      
        if (alerts && alerts.length > 0) {
          var alertBox = document.getElementById('alert-box');
          // generate a list of alerts
          for (var i = 0; i < alerts.length; i++) {
            var alert = alerts[i];
            var message = 'UserID ' + alert.userID + ' has created a pin of type ' + alert.pinID.pinType;
            var timestamp = new Date(alert.timestamp).toLocaleString();
            var alertElement = document.createElement('div');
            alertElement.className = 'alert-message';
            alertElement.innerHTML = '<p>' + message + '</p><p class="alert-timestamp">' + timestamp + '</p>';
            alertBox.appendChild(alertElement);
          }
          alertContainer.style.display = 'block';
        } else {
          // this code will display "No Notifications" when there
          // are no alerts to be viewed.
          var noAlerts = document.createElement('div');
          noAlerts.className = 'no-alerts-message';
          noAlerts.innerHTML = 'No Notifications';
          var alertBox = document.getElementById('alert-box');
          alertBox.innerHTML = '';
          alertBox.appendChild(noAlerts);

          alertContainer.style.display = 'block';
        }
    }
    function delAlerts() {
        const alerts = JSON.parse(localStorage.getItem("alerts"));
        const now = new Date();
        let deleteIndex = -1;
        for (let i = 0; i < alerts.length; i++) {
          const alertDate = new Date(alerts[i].timestamp);
          const diffTime = Math.abs(now - alertDate);
          const diffDays = Math.ceil(diffTime / (86400000));
          if (diffDays >= 7 && alerts[i].read) {
            deleteIndex = i;
            break;
          }
        }
        if (deleteIndex !== -1) {
          alerts.splice(deleteIndex, 1);
        } else if (alerts.length > 100) {
          alerts.shift();
        }
        localStorage.setItem("alerts", JSON.stringify(alerts));
    }
      

    
    window.readAlertHandler = function(pos) {
        alertInfo = []
    
        const webServiceUrl = 'https://localhost:7259/Alert/ReadUserAlert';
        
        const alerts = {}
        alerts.alertID = alertInfo[pos]._alertID;
        alerts.userID = alertInfo[pos]._userID;
        alerts.lat = alertInfo[pos]._lat;
        alerts.lng = alertInfo[pos]._lng;
        alerts.description = alertInfo[pos]._description;
        alerts.read = 1;
        alerts.dateTime = alertInfo[pos]._dateTime;
    
        axios.post(webServiceUrl, alerts, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
            }
        })
        .then(function (responseAfter){
            delAlert();
        })
        .catch(function (error){
            console.log(error);
            return
        });
        
        function delAlert() {
            const currentTimestamp = new Date().getTime();
            let i = 0;
            
            while(i < alertInfo.length) {
                const timestamp = alertInfo[i]._dateTime.getTime();
                //need days to tell wheter 7 days have passed
                const differenceInDays = (currentTimestamp - timestamp) / (86400000);
                if(differenceInDays >= 7 && alertInfo[i]._read === 1) {
                    alertInfo.splice(i, 1);
                }
                else {
                    i++;
                }
            }
            
            if(alertInfo.length > 100) {
                alertInfo.shift();
            }
            displayAlerts();
        }
    }
    function markAllAsRead() {
        const alertMessages = document.getElementsByClassName("alert-message");
        for (let i = 0; i < alertMessages.length; i++) {
          alertMessages[i].classList.add("read");
          alertInfo[i]._read = 1;
        }
    }
      const readAllButton = document.querySelector(".readAll-button");
      readAllButton.addEventListener("click", markAllAsRead);
      /*function showAlertView() {
        var alertContainer = document.querySelector(".alert-container");
        var homeContainer = document.quereySelector(".home-container");
        alertContainer.style.display = "block";
        homeContainer.style.display = "none";
    }*/
    //alertBtn.addEventListener("click",showAlertView());
    //document.querySelector(".alertBtn").addEventListener("click", getAlerts(userLat,userLng));
//})(window, window.ajaxClient);