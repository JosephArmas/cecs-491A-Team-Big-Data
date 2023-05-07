'use strict';

let analyticsBuild = false;
let analyticRegBuild = false;
let buttonBuilt = false;

// Get data from server and div class name to make chart
function makeChart(server, chartClassname)
{
    // Days
    let xAxis = [];
    // Registered users
    let yAxis = [];
    axios.get(server).then(function (response)
    {
        //console.log(response.data);
        let reponseAfter = response.data;
        for (let k in reponseAfter)
        {
            xAxis.push(k);
            yAxis.push(reponseAfter[k])
        }

        let dataPoints = {
            labels: xAxis,
            series: [yAxis],
        }
        let configs = {
            low: 0,
            height: '90vh'
        }
        new Chartist.Line(chartClassname, dataPoints, configs);
    }).catch(function (error)
    {
        timeOut("Server down. Please check server status", 'red', errorsDiv)
    });

}


function showAnalyticsRegistrations()
{
    const endPoint = getEndPoint();
    showAnalyticsRegistrationView()
    makeChart(endPoint.analyticsRegistration, '.analytics-chart-registration')
    setInterval(showAnalyticsRegistrations, 60 * 1000);
}

function showAnalyticsLogins()
{
    const endPoint = getEndPoint();
    showAnalyticsLoginsView();
    makeChart(endPoint.analyticsLogins, '.analytics-chart-logins')
    // Miliseconds to Seconds
    setInterval(showAnalyticsLogins, 60 * 1000);

}


function showAnalyticsMaps()
{
    showAnalyticsMapsView();
    const endPoint = getEndPoint();
    makeChart(endPoint.analyticsMaps, '.analytics-chart-maps')
    // Miliseconds to Seconds
    setInterval(showAnalyticsMaps, 60 * 1000);
}

function showAnalyticsPins()
{
    const endPoint = getEndPoint();
    showAnalyticsPinsView();
    makeChart(endPoint.analyticsPins, '.analytics-chart-pins')
    // Miliseconds to Seconds
    setInterval(showAnalyticsPins, 60 * 1000);
}



function showAnalytics()
{
    let analyticsView = document.querySelector(".analytics-container");
    let analyticsHome = document.querySelector(".analytics-home");
    let homeContainer = document.querySelector(".home-admin-container");
    let analyticsChart = document.querySelector(".charts");
    buildAnalytics();
    analyticsView.style.display = "block";
    analyticsHome.style.display = "block";
    analyticsChart.style.display = "none";
    homeContainer.style.display = "none";
}

function buildAnalytics()
{
    if(!analyticsBuild)
    {
        let analyticsHeader = document.querySelector(".analytics-header");
        let analyticsHomeBtn = document.createElement("button");
        let analyticsLogoutBtn = document.createElement("button");
        let analyticsHomeDiv = document.querySelector('.analytics-home');
        let chartOptionsDiv = document.querySelector('.chart-options');
        analyticsHomeBtn.setAttribute("type", "button");
        analyticsHomeBtn.id = "analytics-home";
        analyticsHomeBtn.textContent = "Home";
        analyticsLogoutBtn.setAttribute("type", "button");
        analyticsLogoutBtn.id = "analytics-logout";
        analyticsLogoutBtn.textContent = "Logout";
        analyticsLogoutBtn.addEventListener('click', homeClicked);
        analyticsHomeBtn.addEventListener('click', adminView);
        analyticsHeader.appendChild(analyticsHomeBtn);
        analyticsHeader.appendChild(analyticsLogoutBtn);
        let title = document.createElement("h1");
        title.textContent = "Analytics";
        title.id = 'home-title';
        analyticsHomeDiv.insertBefore(title,chartOptionsDiv);
        let analyticLoginBtn = document.createElement("button");
        analyticLoginBtn.setAttribute("type", "button");
        analyticLoginBtn.textContent = "Logins";
        analyticLoginBtn.id = "analytics-logins";
        analyticLoginBtn.addEventListener('click', showAnalyticsLogins);
        let analyticRegisterBtn = document.createElement("button");
        analyticRegisterBtn.setAttribute("type", "button");
        analyticRegisterBtn.id = "analytics-registrations";
        analyticRegisterBtn.textContent = "Registrations";
        analyticRegisterBtn.addEventListener('click', showAnalyticsRegistrations);
        let analyticMapBtn = document.createElement("button");
        analyticMapBtn.setAttribute("type", "button");
        analyticMapBtn.textContent = "Maps";
        analyticMapBtn.id = "analytics-maps";
        analyticMapBtn.addEventListener('click', showAnalyticsMaps);
        let analyticPinBtn = document.createElement("button");
        analyticPinBtn.setAttribute("type", "button");
        analyticPinBtn.id = "analytics-pins";
        analyticPinBtn.textContent = "Pins";
        analyticPinBtn.addEventListener('click', showAnalyticsPins);
        chartOptionsDiv.appendChild(analyticLoginBtn);
        chartOptionsDiv.appendChild(analyticRegisterBtn);
        chartOptionsDiv.appendChild(analyticPinBtn);
        chartOptionsDiv.appendChild(analyticMapBtn);
        analyticsBuild = true;
    }
}
function buildBackButton() 
{
        let headerDiv = document.querySelector(".charts .analytics-header");
        let analyticsBackBtn = document.createElement("button");
        let analyticsLogoutBtn = document.createElement("button");
        analyticsBackBtn.setAttribute("type", "button");
        analyticsBackBtn.id = "analytics-home";
        analyticsBackBtn.textContent = "Back";
        analyticsLogoutBtn.setAttribute("type", "button");
        analyticsLogoutBtn.id = "analytics-logout";
        analyticsLogoutBtn.textContent = "Logout";
        analyticsLogoutBtn.addEventListener('click', homeClicked);
        analyticsBackBtn.addEventListener('click', showAnalytics);
        headerDiv.appendChild(analyticsBackBtn);
        headerDiv.appendChild(analyticsLogoutBtn);
        buttonBuilt = true;
}

function showAnalyticsRegistrationView()
{
    let analyticsHome = document.querySelector(".analytics-home");
    let analyticCharts = document.querySelector(".charts");
    let registrations = document.querySelector(".analytics-registration-view");
    let logins = document.querySelector(".analytics-logins-view");
    let maps = document.querySelector(".analytics-maps-view");
    let pins = document.querySelector(".analytics-pins-view");
    if(!buttonBuilt)
    {
        buildBackButton();
    }
    let title = document.querySelector(".analytics-registration-view h1");
    title.style.textAlign = "center";
    analyticCharts.style.display = "block";
    registrations.style.display = "block";
    logins.style.display = "none";
    maps.style.display = "none";
    pins.style.display = "none";
    analyticsHome.style.display = "none";
}

function showAnalyticsLoginsView()
{
    let analyticsHome = document.querySelector(".analytics-home"); 
    let analyticCharts = document.querySelector(".charts");
    let registrations = document.querySelector(".analytics-registration-view");
    let logins = document.querySelector(".analytics-logins-view");
    let maps = document.querySelector(".analytics-maps-view");
    let pins = document.querySelector(".analytics-pins-view");
    if(!buttonBuilt)
    {
        buildBackButton();
    }
    let title = document.querySelector(".analytics-logins-view h1");
    title.style.textAlign = "center";
    analyticCharts.style.display = "block";
    logins.style.display = "block";
    maps.style.display = "none";
    pins.style.display = "none";
    registrations.style.display = "none";
    analyticsHome.style.display = "none";
    
}

function showAnalyticsMapsView()
{
    let analyticsHome = document.querySelector(".analytics-home"); 
    let analyticCharts = document.querySelector(".charts");
    let registrations = document.querySelector(".analytics-registration-view");
    let logins = document.querySelector(".analytics-logins-view");
    let maps = document.querySelector(".analytics-maps-view");
    let pins = document.querySelector(".analytics-pins-view");
    if(!buttonBuilt)
    {
        buildBackButton();
    }
    let title = document.querySelector(".analytics-maps-view h1");
    title.style.textAlign = "center";
    analyticCharts.style.display = "block";
    maps.style.display = "block";
    logins.style.display = "none";
    pins.style.display = "none";
    registrations.style.display = "none";
    analyticsHome.style.display = "none";
    
}

function showAnalyticsPinsView()
{
    let analyticsHome = document.querySelector(".analytics-home"); 
    let analyticCharts = document.querySelector(".charts");
    let registrations = document.querySelector(".analytics-registration-view");
    let logins = document.querySelector(".analytics-logins-view");
    let maps = document.querySelector(".analytics-maps-view");
    let pins = document.querySelector(".analytics-pins-view");
    if(!buttonBuilt)
    {
        buildBackButton();
    }
    let title = document.querySelector(".analytics-pins-view h1");
    title.style.textAlign = "center";
    analyticCharts.style.display = "block";
    pins.style.display = "block";
    maps.style.display = "none";
    logins.style.display = "none";
    registrations.style.display = "none";
    analyticsHome.style.display = "none";
    
}

// * Test Graph using LineChart
function testGraph2()
{

new LineChart
(
  '.test',
  {
    labels: [1, 2, 3, 4, 5, 6, 7, 8],
    series: [[5, 9, 7, 8, 5, 3, 5, 4]]
  },
  {
    low: 0,
    showArea: true
  }
);
}

// * Test Graph importing { LineChart }
function testGraph()
{
    let data = {
        labels: [1, 2, 3, 4, 5, 6, 7, 8],
        series: [[5, 9, 7, 8, 5, 3, 5, 4]]
    }
    let configs = {
        low: 0,
        showArea: true
    }
    new Chartist.Line('.test', data, configs);
}

// * Test if server is running
function health()
{
    // const analyticsHealth = 'https://localhost:7259/analysis/health';
    let server = getServer();
    axios.get(server.analyticsHealth).then(function (response)
    {
        console.log(response.data);
        // errorsDiv.innerHTML = response.data;
        timeOut(response.data,'red', responseDiv)
    });

}
function testG()
{
    let endPoint = getEndPoint();
    axios.get(endPoint.analyticsLogins).then(function (response)
    {
        console.log(response.data);

    });

}
// testG();
//health()
//showAnalyticsRegistrations()
// testGraph()
// testGraph2()