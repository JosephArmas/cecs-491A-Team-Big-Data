'use strict';
import { LineChart } from '../../node_modules/chartist/dist/index.js';
// * Event listerners for analytics view
document.querySelector("#usage-dashboard").addEventListener('click', showAnalytics);
document.querySelector("#analytics-home").addEventListener('click', adminView);
document.querySelector("#analytics-logout").addEventListener('click', homeClicked);
const analyticsHealth = 'https://localhost:7259/analysis/health';
const analyticsRegistration = 'https://localhost:7259/analysis/registrations';
const analyticsLogins = 'https://localhost:7259/analysis/logins';
// var chartDoctor = document.querySelector(".test");
// errorsDiv.innerHTML = "test";
document.querySelector("#analytics-registrations").addEventListener('click', showAnalyticsRegistrations);
document.querySelector("#analytics-logins").addEventListener('click', showAnalyticsLogins);
document.querySelector("#analytics-maps").addEventListener('click', showAnalyticsMaps);
document.querySelector("#analytics-pins").addEventListener('click', showAnalyticsLogins);



function getData(server, chartClassname)
{
    // Days
    let xAxis = [];
    // Registered users
    let yAxis = [];
    axios.get(server).then(function (response)
    {
        // console.log(response.data);
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
        new LineChart(chartClassname, dataPoints, configs);
    }).catch(function (error)
    {
        errorsDiv.style.color = "red";
        errorsDiv.innerHTML= "Server down. Please check server status";
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
    getData(analyticsLogins, '.analytics-chart-maps')
    setInterval(showAnalyticsMaps, 60 * 1000);
}

// * Test if server is running
function health()
{
    axios.get(analyticsHealth).then(function (response)
    {
        console.log(response.data);
        errorsDiv.innerHTML = response.data;
    });

}

// * Test Graph using LineChart
function testGraph()
{

new LineChart(
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


// health()
// showAnalyticsRegistrations()
// testGraph()
// testGraph2()