'use strict';

// The cover is from Vatanak Vong's AJAX Demo in his GitHub. https://github.com/v-vong3/csulb/tree/master/cecs_491/demo/ajax-demo
// Google maps api code follows Google's map api doc tutorials. https://developers.google.com/maps/documentation/javascript/overview#maps_map_simple-javascript
(function (root, ajaxClient) {
    // Dependency check
    const isValid = root && ajaxClient;
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
    
    const CSULB = { lat: 33.7838, lng: -118.1141 };

    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAAfbLnE9etZVZ0_ZqaAPUMl03BfKLN8kI&region=US&language=en&callback=initMap';
    script.async = true;

    let map;

    //const webServiceUrl = 'https://localhost:7079';

    window.getMarkerHandler = function () {

        // Connect to backend to get markers
        /*var request = ajaxClient.get(webServiceUrl); // ('https://reqres.in/api/users?page=2');
        console.log(request);
        request.then(function (response) {
            console.log(response);
        });*/

        // Temp marker
        const marker = new google.maps.Marker({
            position: CSULB,
            map: map,
        });

    }
   
    window.initMap = function() {
        map = new google.maps.Map(document.getElementById('map'), {
            center: CSULB,
            minZoom: 8,
            maxZoom: 18,
            zoom: 15,
            mapId: "bb1d4678c71528ff",
            restriction: {
                latLngBounds: CALIFORNIA_BOUNDS,
                strictBounds: false
            }
        });
        getMarkerHandler();
    }

    document.head.appendChild(script);

})(window, window.ajaxClient);

