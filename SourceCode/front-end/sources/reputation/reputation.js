'use strict';

(function (root)
{
    const reputationBox = document.querySelector(".reputation");
    const reputationContainer = document.querySelector(".reputation-reports-container");
    const reportsContainer = document.querySelector(".reports-box");
    const createReportBtn = document.getElementById("create-report-btn")
    const createReportView = document.querySelector(".create-report-container");
    const submitReportBtn = document.getElementById("submit-report-btn");
    const viewOwnReportsBtn = document.getElementById("view-own-reports");
    const reportsViewReturnBtn = document.getElementById("cancel-report-creation");
    const profileView = document.querySelector(".profileContainer");
    const reportForm = document.querySelector(".new-report");
    const reputationControls = document.querySelector(".reputation-controls");

    var backend = "";
    fetch("./config.json").then((response) => response.json()).then((json) => 
    {
    backend = json.backend;
    })

    viewOwnReportsBtn.addEventListener('click', function()
    {
        profileView.style.display = "none";
        reputationView(localStorage.getItem("id"));
    });

    
    reportsViewReturnBtn.addEventListener('click', function()
    {
        createReportView.style.display = "none";
        reputationControls.innerHTML = "";
        resetReportsView();
        reputationView(localStorage.getItem("reportedUserID"));
    });

    
    submitReportBtn.addEventListener('click', function()
    {
        const insertReportURL = backend + "/Reputation/PostNewReport";
        let newReport = {}
        newReport.Rating = document.getElementById("new-rating").value;
        newReport.Feedback = document.getElementById("new-feedback").value;
        newReport.UserID = localStorage.getItem("reportedUserID");
        newReport.CreateDate = "";
        newReport.ReportingUserID = localStorage.getItem("id");
        newReport.ButtonCommand = "";

        var newReportRequest = axios.post(insertReportURL, newReport, {
            headers: {
                "Authorization": `Bearer ${localStorage.getItem("jwtToken")}`
            }
        });
        newReportRequest.then(function(response)
        {
            createReportView.style.display = "none";
            reputationControls.innerHTML = "";
            reportForm.reset();
            resetReportsView();
            reputationView(localStorage.getItem("reportedUserID"));
        })
        .catch(function(error)
        {
            document.getElementById("errors").innerHTML = error;
        })
    });

    
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
            ratingTitle.style.marginLeft = "10%";
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
            creationDate.style.marginLeft = "10%";
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
        if(id === localStorage.getItem("id"))
        {
            createReportBtn.style.display = "none";
        }
        else
        {
            createReportBtn.style.display = "block";
        }

        localStorage.setItem("reportedUserID", id);

        const reputationURL = backend + "/Reputation/GetReputation";
        
        reputationBox.style.border = "1px solid";
        reputationBox.style.height = "50px";
        reputationBox.style.margin = "50px";
        reputationBox.style.marginLeft = "37%";
        reputationBox.style.width = "29%";
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

        const previousReportBtn = document.createElement("button");
        previousReportBtn.id = "previous-reports";
        previousReportBtn.innerHTML = "Previous";
        previousReportBtn.style.backgroundColor = "#00ACB7";
        previousReportBtn.style.color = "white";
        previousReportBtn.style.fontSize = "25px";
        reputationControls.appendChild(previousReportBtn);

        const nextReportBtn = document.createElement("button");
        nextReportBtn.id = "next-reports";
        nextReportBtn.innerHTML = "Next";
        nextReportBtn.style.backgroundColor = "#00ACB7";
        nextReportBtn.style.color = "white";
        nextReportBtn.style.fontSize = "25px";
        nextReportBtn.style.marginLeft = "54%";
        reputationControls.appendChild(nextReportBtn);

        if(id !== "")
        {
            const userProfile = {}
            userProfile.UserID = id;
            userProfile.Rating = 0.0;
            userProfile.Feedback = "";
            userProfile.CreateDate = "";
            userProfile.ReportingUserID = localStorage.getItem("id");
            userProfile.ButtonCommand = "";
            userProfile.Partition = 0;
            
            let reputationRequest = axios.post(reputationURL, userProfile, {
                headers: {
                    "Authorization": `Bearer ${localStorage.getItem("jwtToken")}`
                }
            });
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
        const reportsURL = backend + "/Reputation/ViewReports";

        let userReport = {};
        userReport.UserID = Number(id);
        userReport.Rating = 0.0;
        userReport.Feedback = "";
        userReport.CreateDate = "";
        userReport.ReportingUserID = localStorage.getItem("id");
        userReport.ButtonCommand = "";
        userReport.Partition = 0;

        reportsContainer.style.border = "2px solid";
        reportsContainer.style.backgroundColor = "gray";
        reportsContainer.style.height = "1000px";
        reportsContainer.style.width = "50%";
        reportsContainer.style.marginLeft = "27%";    
        reportsContainer.style.overflow = "hidden";

        let reportsRequest = axios.post(reportsURL, userReport, {
            headers: {
                "Authorization": `Bearer ${localStorage.getItem("jwtToken")}`
            }
        });
        reportsRequest.then(function(response)
        {   
            localStorage.setItem("numberOfReports", response.data.length);     
            organizeReports(response);
        });

        // Partitions and displays the previous set of 5 reports
        const previousReportsBtn = document.getElementById("previous-reports");
        previousReportsBtn.addEventListener('click', function()
        {
            userReport.UserID = Number(id);
            userReport.ButtonCommand = "Previous";
            const previousReports = axios.post(reportsURL, userReport, {
                headers: {
                    "Authorization": `Bearer ${localStorage.getItem("jwtToken")}`
                }
            });
            previousReports.then(function(response)
            {
                if(response.data.length > 0 && userReport.Partition != 0)
                {
                    userReport.Partition -= 5;
                    resetReports();
                    localStorage.setItem("numberOfReports", response.data.length);                
                    organizeReports(response);
                }
            });
        });

        // Partitions and displays the next set of 5 reports
        const nextReportsBtn = document.getElementById("next-reports");
        nextReportsBtn.addEventListener('click', function()
        {
            userReport.UserID = Number(id);
            userReport.ButtonCommand = "Next";           
            const nextReports = axios.post(reportsURL, userReport, {
                headers: {
                    "Authorization": `Bearer ${localStorage.getItem("jwtToken")}`
                }
            });
            nextReports.then(function(response)
            {   
                if(response.data.length > 0)        
                {
                    userReport.Partition += 5;
                    resetReports();
                    localStorage.setItem("numberOfReports", response.data.length);            
                    organizeReports(response);
                }
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
        reputationControls.innerHTML = "";
        resetReportsView();
    });

    function resetReports()
    {    
        reportsContainer.innerHTML = "";
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

    root.Utification = root.Utification || {};
    root.Utification.reputationView = reputationView;
})(window);