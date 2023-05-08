'use strict';

const reputationBox = document.querySelector(".reputation");
const reputationContainer = document.querySelector(".reputation-reports-container");
const reportsContainer = document.querySelector(".reports-box");
const createReportBtn = document.getElementById("create-report-btn")
const createReportView = document.querySelector(".create-report-container");

createReportBtn.addEventListener('click', function()
{   
    reputationContainer.style.display = "none";
    createReportView.style.display = "block";
});


function organizeReports(response)
{
    for(let i = 1; i < response.data.length + 1; i++)
    {
        let report = document.createElement("div");
        report.id = "report-" + i;
        report.style.border = "2px solid";
        report.style.backgroundColor = "white";
        report.style.height = "170px";
        report.style.width = "75%";
        report.style.marginTop = "20px";
        report.style.marginLeft = "12%";
        report.style.overflow = "hidden";

        reportsContainer.appendChild(report);

        let reportNumber = document.getElementById("report-" + i);
        for(let j = 1; j < 6; j++)
        {
            let star = document.createElement("span");
            star.id = "star" + i + "-" + j;
            star.innerHTML = "&starf;";
            star.style.fontSize = "200%";
            star.style.color = "gray";

            reportNumber.appendChild(star);
        }

        const ratingTitle = document.createElement("span");
        ratingTitle.style.fontSize = "25px";
        ratingTitle.id = "reputation-title-" + i;
        ratingTitle.innerHTML = "Rating: " + response.data[i - 1].rating.toString();
        ratingTitle.style.color = "blue";
        ratingTitle.style.marginLeft = "20%";
        reportNumber.appendChild(ratingTitle);

        /*const ratingNumber = document.createElement("span");
        ratingNumber.id = "rating-number-" + i;
        ratingNumber.style.fontSize = "25px";            
        ratingNumber.style.color = "orange";
        ratingNumber.innerHTML = response.data[i - 1].rating;
        reportNumber.appendChild(ratingNumber);*/

        const creationDate = document.createElement("span");
        creationDate.id = "report-date-" + i;
        creationDate.style.color = "red";
        creationDate.style.fontSize = "25px";
        creationDate.style.marginLeft = "20%";
        creationDate.innerHTML = response.data[i - 1].createDate;
        reportNumber.appendChild(creationDate);

        let feedback = document.createElement("p");
        feedback.innerHTML = response.data[i - 1].feedback;
        feedback.style.marginLeft = "30px";
        feedback.style.marginRight = "30px";
        feedback.style.fontSize = "22px";
        reportNumber.appendChild(feedback);

        for(let j = 0; j < Math.floor(response.data[i - 1].rating); j++)
        {
            document.getElementById("star" + i + "-" + (j + 1)).style.color = "orange";
        }
    }    
}

function reputationView(id)
{
    const reputationUrl = "https://localhost:7259/Reputation/GetReputation";
    
    reputationBox.style.border = "1px solid";
    reputationBox.style.height = "50px";
    reputationBox.style.margin = "50px";
    reputationBox.style.marginLeft = "40%";
    reputationBox.style.width = "25%";
    reputationBox.style.overflow = "hidden";


    for(let i = 1; i < 6; i++)
    {
        const displayReputation = document.createElement("span");
        displayReputation.id = "star-" + i;
        displayReputation.innerHTML = "&starf;";
        displayReputation.style.color = "gray";
        displayReputation.style.fontSize = "200%";
        reputationBox.appendChild(displayReputation);
    }

    if(id !== "")
    {
        const userProfile = {}
        userProfile.UserID = id;
        userProfile.Rating = 0.0;
        userProfile.Feedback = "";
        userProfile.CreateDate = "";
        userProfile.ReportingUserID = localStorage.getItem("id");
        userProfile.ButtonCommand = "";
        
        let reputationRequest = axios.post(reputationUrl, userProfile, {});
        reputationRequest.then(function(response)
        {
            for(let i = 0; i < Math.floor(response.data); i++)
            {
                document.getElementById("star-" + (i + 1)).style.color = "orange";
            }

            let reputationTitle = document.createElement("span");
            reputationTitle.style.fontSize = "35px";
            reputationTitle.id = "reputation-title";
            reputationTitle.innerHTML = "Reputation: ";
            reputationTitle.style.marginLeft = "10%";
            reputationBox.appendChild(reputationTitle);

            let reputationNumber = document.createElement("span");
            reputationNumber.id = "reputation-number";
            reputationNumber.style.fontSize = "35px";            
            reputationNumber.style.color = "orange";
            reputationNumber.innerHTML = response.data;
            reputationBox.appendChild(reputationNumber);
        })
    }

    reportsView(id);

    const homeContainer = document.querySelector(".home-container")
    homeContainer.style.display = "none";
    reputationContainer.style.display = "block";
}

function reportsView(id)
{    
    const reportsUrl = "https://localhost:7259/Reputation/ViewReports";

    const userReport = {};
    userReport.UserID = id;
    userReport.Rating = 0.0;
    userReport.Feedback = "";
    userReport.CreateDate = "";
    userReport.ReportingUserID = localStorage.getItem("id");
    userReport.ButtonCommand = "";

    
    reportsContainer.style.border = "2px solid";
    reportsContainer.style.backgroundColor = "gray";
    reportsContainer.style.height = "1000px";
    reportsContainer.style.width = "50%";
    reportsContainer.style.marginLeft = "27%";    
    reportsContainer.style.overflow = "hidden";

    let reportsRequest = axios.post(reportsUrl, userReport, {});
    reportsRequest.then(function(response)
    {   
        localStorage.setItem("numberOfReports", response.data.length);     
        organizeReports(response);
    });

    // Partitions and displays the previous set of 5 reports
    const previousReportsBtn = document.getElementById("previous-reports");
    previousReportsBtn.addEventListener('click', function()
    {
            userReport.ButtonCommand = "Previous";
            const previousReports = axios.post(reportsUrl, userReport, {});
            previousReports.then(function(response)
            {
                resetReports();
                localStorage.setItem("numberOfReports", response.data.length);                
                organizeReports(response);
            });
    });

    // Partitions and displays the next set of 5 reports
    const nextReportsBtn = document.getElementById("next-reports");
    nextReportsBtn.addEventListener('click', function()
    {
        
        userReport.ButtonCommand = "Next";           
        const previousReports = axios.post(reportsUrl, userReport, {});
        previousReports.then(function(response)
        {            
            resetReports();
            localStorage.setItem("numberOfReports", response.data.length);            
            organizeReports(response);
        });    
    });
}


const homeReturnBtn = document.querySelector("#map-return-button");
homeReturnBtn.addEventListener("click", function ()
{
    const reputationContainer = document.querySelector(".reputation-reports-container");
    const homeContainer = document.querySelector(".home-container");
    reputationContainer.style.display = "none";
    homeContainer.style.display = "block";
    resetReportsView();
});

function resetReports()
{    
    for(let i = 0; i < localStorage.getItem("numberOfReports"); i++)
    {
        let resetReport = document.getElementById("report-" + (i + 1));
        reportsContainer.removeChild(resetReport); 
    }
}

function resetReportsView()
{    
    for(let i = 1; i < 6; i++)
    {
        let removeStars = document.getElementById("star-" + i);
        reputationBox.removeChild(removeStars);
    }

    let resetReputationTitle = document.getElementById("reputation-title");
    reputationBox.removeChild(resetReputationTitle);
    let resetReputationNumber = document.getElementById("reputation-number");
    reputationBox.removeChild(resetReputationNumber);

    resetReports();
}
/*(function (userID){


    
})(currResponse._userID);*/