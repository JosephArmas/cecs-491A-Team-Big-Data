'use strict';

// Immediately Invoke Function Execution (IIFE or IFE)
// Protects functions from being exposed to the global object
(function (root, ajaxClient) {
    // Dependency check
    const isValid = root && ajaxClient;

    if(!isValid){
        // Handle missing dependencies
        alert("Missing dependencies");
    }

    // Show or Hide private functions
    //root.myApp.getData = getDataHandler;
    //root.myApp.sendData = sendDataHandler;

    // Initialize the current view by attaching event handlers 
   
    window.initMap = function() {
        const CSULB = { lat: 33.7838, lng: -118.1141 };
        const map = new google.maps.Map(document.getElementById("map"), {
            zoom: 15,
            center: CSULB,
        });
        const marker = new google.maps.Marker({
            position: CSULB,
            map: map,
        });
    }

    initMap();

})(window, window.ajaxClient);

