'use strict';
import * as Chartist from '../../node_modules/chartist/dist/index.js';
// * Event listerners for analytics view
//document.getElementById("usage-dashboard").addEventListener('click', showAnalytics);
//document.getElementById("analytics-home").addEventListener('click', adminView);
document.getElementById("analytics-logout").addEventListener('click', homeClicked);
const analyticsHealth = 'https://localhost:7259/analysis/health';
const analyticsRegistration = 'https://localhost:7259/analysis/registrations';
const analyticsLogins = 'https://localhost:7259/analysis/logins';
// var chartDoctor = document.querySelector(".test");
// errorsDiv.innerHTML = "test";
document.getElementById("analytics-registrations").addEventListener('click', showAnalyticsRegistrations);





function showAnalyticsRegistrations()
{
    alert("hi");
    // let analyticsHome = document.querySelector(".analytics-home");
    // let analyticsRegistrationView = document.querySelector(".analytics-registration-view");
    // analyticsHome.style.display = "none";
    // analyticsRegistrationView.style.display = "block";
    showAnalyticsRegistrationView()

    // Days
    let xAxis = [];
    // Registered users
    let yAxis = [];
    axios.get(analyticsRegistration).then(function (response)
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
            height: "80vh",
            axisY: {
                onlyInteger: true
            },
            axisX: {
                showLabel: false
            }
        }
        new Chartist.LineChart('.analytics-chart-registration', dataPoints, configs);
    }).catch(function (error)
    {
        alert(error);
        errorsDiv.style.color = "red";
        errorsDiv.innerHTML= "Server down. Please check server status";
    });
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

function testGraph2()
{
    let xAxis = [];
    // Registered users
    let yAxis = [];
    axios.get(analyticsRegistration).then(function (response)
    {
        // console.log(response.data);
        let reponseAfter = response.data;
        for (let k in reponseAfter)
        {
            xAxis.push(k);
            yAxis.push(reponseAfter[k])
        }
    new LineChart(
        '.analytics-registration-view',
        {
            labels: xAxis,
            series: [yAxis]
        },
        {
            low: 0,
            // showArea: true
            height: '80vh'
        }
    );
    });
}

// health()
// showAnalyticsRegistrations()
// testGraph()
// testGraph2()