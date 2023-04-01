'use strict';

// const { LineChart } = require("chartist");

// const { LineChart } = require("chartist");

// import { LineChart } from '../../node_modules/chartist/dist/index.js';
// import { homeClicked } from '../main.js';
// * Event listerners for analytics view
// document.querySelector("#usage-dashboard").addEventListener('click', showAnalytics);
// document.querySelector("#analytics-home").addEventListener('click', adminView);
// document.querySelector("#analytics-logout").addEventListener('click', homeClicked);
const analyticsHealth = 'https://localhost:7259/analysis/health';
const analyticsRegistration = 'https://localhost:7259/analysis/registrations';
const analyticsLogins = 'https://localhost:7259/analysis/logins';
const analyticsMaps = 'https://localhost:7259/analysis/pins';
const analyticsPins = 'https://localhost:7259/analysis/maps';
// document.querySelector("#analytics-registrations").addEventListener('click', showAnalyticsRegistrations);
// document.querySelector("#analytics-logins").addEventListener('click', showAnalyticsLogins);
// document.querySelector("#analytics-maps").addEventListener('click', showAnalyticsMaps);
// document.querySelector("#analytics-pins").addEventListener('click', showAnalyticsPins);

let analyticsBuild = false;
let analyticRegBuild = false;
let analyticsBtn = {};


function getData(server, chartClassname)
{
    // * Days
    let xAxis = [];
    // * Registered users
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
        // errorsDiv.style.color = "red";
        // errorsDiv.innerHTML= "Server down. Please check server status";
        timeOut("Server down. Please check server status", 'red', responseDiv)
    });

}


function showAnalyticsRegistrations()
{
    
    showAnalyticsRegistrationView()
    getData(analyticsRegistration, '.analytics-chart-registration')
    setInterval(showAnalyticsRegistrations, 60 * 1000);
    
    
}

function showAnalyticsLogins()
{
    getData(analyticsLogins, '.analytics-chart-logins')
    setInterval(showAnalyticsLogins, 60 * 1000);

}


function showAnalyticsMaps()
{
    getData(analyticsMaps, '.analytics-chart-maps')
    setInterval(showAnalyticsMaps, 60 * 1000);
}

function showAnalyticsPins()
{
    getData(analyticsPins, '.analytics-chart-pins')
    setInterval(showAnalyticsPins, 60 * 1000);
}



function showAnalytics()
{
    var analyticsView = document.querySelector(".analytics-container");
    var analyticsHome = document.querySelector(".analytics-home");
    var homeContainer = document.querySelector(".home-admin-container");
    let analyticsChart = document.querySelector(".charts");
    buildAnalytics();
    analyticsView.style.display = "block";
    analyticsChart.style.display = "none";
    analyticsHome.style.display = "block";
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
        analyticsBtn.analyticsHomeBtn = analyticsHomeBtn;
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
        let analyticRegisterBtn = document.createElement("button");
        analyticRegisterBtn.setAttribute("type", "button");
        analyticRegisterBtn.id = "analytics-registrations";
        analyticRegisterBtn.textContent = "Registrations";
        analyticRegisterBtn.addEventListener('click', showAnalyticsRegistrations);
        let analyticMapBtn = document.createElement("button");
        analyticMapBtn.setAttribute("type", "button");
        analyticMapBtn.textContent = "Maps";
        analyticMapBtn.id = "analytics-maps";
        let analyticPinBtn = document.createElement("button");
        analyticPinBtn.setAttribute("type", "button");
        analyticPinBtn.id = "analytics-pins";
        analyticPinBtn.textContent = "Pins";
        chartOptionsDiv.appendChild(analyticLoginBtn);
        chartOptionsDiv.appendChild(analyticRegisterBtn);
        chartOptionsDiv.appendChild(analyticPinBtn);
        chartOptionsDiv.appendChild(analyticMapBtn);
        analyticsBuild = true;
    }
}


function showAnalyticsRegistrationView()
{
        let analyticsHome = document.querySelector(".analytics-home");
        let analyticCharts = document.querySelector(".charts");
        let analyticsRegistration = document.querySelector(".analytics-registration-view");
        let build = buildAnalytics(); 
        console.log(build.analyticsHomeBtn);
        // build.analyticsHomeBtn.text = "Back";
        // build.analyticsHomeBtn.addEventListener('click', showAnalytics);
        let title = document.querySelector(".analytics-registration-view h1");
        title.style.textAlign = "center";
        analyticCharts.style.display = "block";
        analyticsRegistration.style.display = "block";
        analyticsHome.style.display = "none";
        analyticRegBuild = true;
    
}

function showAnalyticsLoginsView()
{
    let a
}

// * Test Graph using LineChart
/*
function testGraph()
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
*/

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
    new Chartist.Line('.analytics-chart-registration', data, configs);
}

// * Test if server is running
function health()
{
    axios.get(analyticsHealth).then(function (response)
    {
        console.log(response.data);
        // errorsDiv.innerHTML = response.data;
        timeOut(response.data,'red', responseDiv)
    });

}

//health()
//showAnalyticsRegistrations()
// testGraph()
// testGraph2()