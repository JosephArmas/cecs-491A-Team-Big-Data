'use strict';

const reputationBox = document.querySelector(".reputation");
const reportsContainer = document.querySelector(".reports-box");

function createReportView()
{
    
}


function reputationView(id)
{
    const reputationUrl = "https://localhost:7259/Reputation/GetReputation";
    
    reputationBox.style.border = "1px solid";
    reputationBox.style.height = "50px";
    reputationBox.style.margin = "50px";
    reputationBox.style.marginLeft = "700px";
    reputationBox.style.width = "450px";
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
        
        let reputationRequest = axios.post(reputationUrl, userProfile, {});
        reputationRequest.then(function(response){

            if(response.data < 2.0)
            {
                document.getElementById("star-1").style.color = "orange";
            }
            else if (response.data < 3.0)
            {
                document.getElementById("star-1").style.color = "orange";
                document.getElementById("star-2").style.color = "orange";
            }
            else if (response.data < 4.0)
            {
                document.getElementById("star-1").style.color = "orange";
                document.getElementById("star-2").style.color = "orange";
                document.getElementById("star-3").style.color = "orange";
            }
            else if (response.data < 5.0)
            {
                document.getElementById("star-1").style.color = "orange";
                document.getElementById("star-2").style.color = "orange";
                document.getElementById("star-3").style.color = "orange";
                document.getElementById("star-4").style.color = "orange";
            }
            else if (response.data === 5.0)
            {
                document.getElementById("star-1").style.color = "orange";
                document.getElementById("star-2").style.color = "orange";
                document.getElementById("star-3").style.color = "orange";
                document.getElementById("star-4").style.color = "orange";
                document.getElementById("star-5").style.color = "orange";
            }

            let reputationTitle = document.createElement("span");
            reputationTitle.style.fontSize = "35px";
            reputationTitle.id = "reputation-title";
            reputationTitle.innerHTML = "Reputation: ";
            reputationTitle.style.marginLeft = "20%";
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
    const reputationContainer = document.querySelector(".reputation-reports-container");
    homeContainer.style.display = "none";
    reputationContainer.style.display = "block";
}

function reportsView(id)
{    
    const reportsUrl = "https://localhost:7259/Reputation/ViewReports";

    const userProfile = {}
    userProfile.UserID = id;
    userProfile.Rating = 0.0;
    userProfile.Feedback = "";
    userProfile.CreateDate = "";
    userProfile.ReportingUserID = localStorage.getItem("id");

    
    reportsContainer.style.border = "2px solid";
    reportsContainer.style.backgroundColor = "gray";
    reportsContainer.style.height = "1000px";
    reportsContainer.style.width = "900px";
    reportsContainer.style.marginLeft = "475px";    
    reportsContainer.style.overflow = "hidden";

    let reportsRequest = axios.post(reportsUrl, userProfile, {});
    reportsRequest.then(function(response)
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
            report.style.marginLeft = "115px";
    
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
            ratingTitle.innerHTML = "Rating:  ";
            ratingTitle.style.color = "blue";
            ratingTitle.style.marginLeft = "25%";
            reportNumber.appendChild(ratingTitle);

            const ratingNumber = document.createElement("span");
            ratingNumber.id = "rating-number-" + i;
            ratingNumber.style.fontSize = "25px";            
            ratingNumber.style.color = "orange";
            ratingNumber.innerHTML = response.data[i - 1].rating;
            reportNumber.appendChild(ratingNumber);

            const creationDate = document.createElement("span");
            creationDate.id = "report-date-" + i;
            creationDate.style.color = "red";
            creationDate.style.fontSize = "25px";
            creationDate.style.marginLeft = "24%";
            creationDate.innerHTML = response.data[i - 1].createDate;
            reportNumber.appendChild(creationDate);

            let feedback = document.createElement("p");
            feedback.innerHTML = response.data[i - 1].feedback;
            feedback.style.marginLeft = "30px";
            feedback.style.marginRight = "30px";
            feedback.style.fontSize = "25px";
            reportNumber.appendChild(feedback);

            if(response.data[i - 1].rating < 2.0)
            {
                document.getElementById("star" + i + "-" + 1).style.color = "orange";
            }
            else if (response.data[i - 1].rating < 3.0)
            {
                document.getElementById("star" + i + "-" + 1).style.color = "orange";
                document.getElementById("star" + i + "-" + 2).style.color = "orange";
            }
            else if (response.data[i - 1].rating < 4.0)
            {
                document.getElementById("star" + i + "-" + 1).style.color = "orange";
                document.getElementById("star" + i + "-" + 2).style.color = "orange";
                document.getElementById("star" + i + "-" + 3).style.color = "orange";
            }
            else if (response.data[i - 1].rating< 5.0)
            {
                document.getElementById("star" + i + "-" + 1).style.color = "orange";
                document.getElementById("star" + i + "-" + 2).style.color = "orange";
                document.getElementById("star" + i + "-" + 3).style.color = "orange";
                document.getElementById("star" + i + "-" + 4).style.color = "orange";
            }
            else if (response.data[i - 1].rating === 5.0)
            {
                document.getElementById("star" + i + "-" + 1).style.color = "orange";
                document.getElementById("star" + i + "-" + 2).style.color = "orange";
                document.getElementById("star" + i + "-" + 3).style.color = "orange";
                document.getElementById("star" + i + "-" + 4).style.color = "orange";
                document.getElementById("star" + i + "-" + 5).style.color = "orange";
            }
        }               
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

    for(let i = 1; i < 6; i++)
    {
        let resetReport = document.getElementById("report-" + i);
        reportsContainer.removeChild(resetReport); 
    }
}
/*(function (userID){


    
})(currResponse._userID);*/