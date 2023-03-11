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

    const PIN_ICONS = [
        "http://maps.google.com/mapfiles/ms/icons/blue-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/yellow-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/green-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
        "http://maps.google.com/mapfiles/ms/icons/purple-dot.png"
    ]

    const TEST_PINS = [
        // blue, yellow, green, red, purple
        // Litter, Group, Junk, Abandoned, Vandalism
        // { title, lat, lng, pinType, }
        [
            `<h1>Litter Pin</h1>
            <p>Displays Blue Pin</p>`,
            // 33.779287, -118.111032
            33.779287,
            -118.111032,
            0
        ],
        [
            `<h1>Group Event Pin</h1>
            <p>Displays Yellow Pin</p>`,
            // 33.781173, -118.115297
            33.781173,
            -118.115297,
            1
        ],
        [
            `<h1>Junk Pin</h1>
            <p>Displays Green Pin</p>`,
            // 33.782191, -118.107339
            33.782191,
            -118.107339,
            2
        ],
        [
            `<h1>Abandoned Pin</h1>
            <p>Displays Red Pin</p>`,
            // 33.788414, -118.115537
            33.788414,
            -118.115537,
            3
        ],
        [
            `<h1>Vadalism Pin</h1>
            <p>Displays Purple Pin</p>`,
            // 33.787690, -118.120698
            33.787690,
            -118.120698,
            4
        ],
    ]
        


    
    
    const CSULB = { lat: 33.7838, lng: -118.1141 };

    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAAfbLnE9etZVZ0_ZqaAPUMl03BfKLN8kI&region=US&language=en&callback=initMap';
    script.async = true;

    let map;

    //const webServiceUrl = 'https://localhost:7079';

    function getMarkerHandler() {

        // Connect to backend to get markers
        /*var request = ajaxClient.get(webServiceUrl); // ('https://reqres.in/api/users?page=2');
        console.log(request);
        request.then(function (response) {
            console.log(response);
        });*/

        // Temp pins
        for (let i = 0; i < TEST_PINS.length; i++) {
            const currPin = TEST_PINS[i]

            const pin = new google.maps.Marker({
                position: {lat:currPin[1],lng:currPin[2]},
                map: map,
                icon: {
                    url: PIN_ICONS[currPin[3]]
                }
            });

            const infowindow = new google.maps.InfoWindow({
                content: currPin[0]
            });
    
            pin.addListener("click", () => {
                infowindow.open({
                  anchor: pin,
                  map,
                  shouldFocus: true,
                });
            });
        }
    }

    function placeMarkerAndPanTo(latLng, map) {
        let pinType = prompt("1. Litter\n2. Group Event\n3. Junk\n4. Abandoned\n5. Vandalism\nWhich Pin Type?");

        if (pinType == null)
        {
            return
        }
        if (pinType < 1 || pinType > 5)
        {
            alert("Invalid Input...");
            return;
        };

        const pin = new google.maps.Marker({
            position: latLng,
            map: map,
            icon: {
                url: PIN_ICONS[pinType-1]
            }
        });

        let title = prompt("Enter pin title.");

        const infowindow = new google.maps.InfoWindow({
            content: title
        });

        pin.addListener("click", () => {
            infowindow.open({
              anchor: pin,
              map,
              shouldFocus: true,
            });
        });
        map.panTo(latLng);
      }
   
    window.initMap = function() {
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

        // if user is authenticated to make a pin enable this        
        map.addListener("click", (e) => {
            placeMarkerAndPanTo(e.latLng, map);
        });
    }

    document.head.appendChild(script);

})(window, window.ajaxClient);

