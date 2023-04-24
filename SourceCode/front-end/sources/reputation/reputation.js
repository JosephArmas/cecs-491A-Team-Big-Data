'use strict';

const reputationBox = document.querySelector(".reputation");

function createReportView()
{
    
}


function reputationView(id)
{
    const reputationUrl = "https://localhost:7259/Reputation/GetReputation";
    const reportsUrl = "https://localhost:7259/Reputation/ViewReports";
    
    reputationBox.style.border = "1px solid";
    reputationBox.style.height = "50px";
    reputationBox.style.margin = "50px";
    reputationBox.style.marginLeft = "700px";
    reputationBox.style.width = "25%";


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

        let reputationRequest = axios.post(reputationUrl, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("id")}`
            }
        });
        reputationRequest.then(function(response){
            console.log(response.data);
        })
    }

    reportsView();

    document.querySelector(".home-container").style.display = 'none';
    document.querySelector(".reputation-reports-container").style.display = 'block';
}

function reportsView()
{
    const reportsContainer = document.querySelector(".reports-box");
    reportsContainer.style.border = "2px solid";
    reportsContainer.style.backgroundColor = "gray";
    reportsContainer.style.height = "1000px";
    reportsContainer.style.width = "50%";
    reportsContainer.style.marginLeft = "25%";
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
}
/*(function (userID){


    
})(currResponse._userID);*/