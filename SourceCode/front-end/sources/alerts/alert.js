(function (root) {
    
    // Dependency check
    const isValid = root;
    if(!isValid){
        // Handle missing dependencies
        alert("Missing dependencies");
    }
    function descriptionLimit(description){
        let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]")
        if (description.split(' ').length > 50 || !charactersAllowed.test(description))
        {
            return false;
        }
        return true;
    }
    function getAlerts() {
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
                

                
                var alertContent = currResponse._description + `<br>Created: ${currResponse._dateTime}<br><button id='markRead' onclick='getAlert(${i})'>Get Alerts</button>`

                alertInfo.push(response.data[i])
                alertMade.push(alerts)
                
                // Do not display disabled or complete pins
                if (currResponse._completed === 1)
                {
                    alertMade[i].setAlert(null);
                }
                else
                {
                    pin.addListener("click", () => {
                        alertContent.push(alerts)
                    });
                }
            }
        });
    }


    root.readAlertHandler = function(pos)
    {
        infoWindows[pos].close();

        const webServiceUrl = 'https://localhost:7259/Alert/ReadUserAlert';
        
        const alerts = {}
        alerts.alertID = alertInfo[pos]._alertID;
        alerts.userID = alertInfo[pos]._userID;
        alerts.description = alertInfo[pos]._description;
        alerts.read = 1;
        alerts.dateTime = alertInfo[pos]._dateTime;

        axios.post(webServiceUrl, pin, {
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
    }
})(window, window.ajaxClient);