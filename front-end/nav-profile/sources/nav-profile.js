
function secClicked()
{
    var btnContainer = document.querySelector(".container");
    if (btnContainer.style.display === "none")
    {
        btnContainer.style.display = "block";
    }
    else
    {
        btnContainer.style.display = "none";
    }

}

function updateClicked()
{
    var updateContainer = document.querySelector(".nav-update");
    var navContainer = document.querySelector(".nav-home");
    navContainer.style.display = "none";
    updateContainer.style.display = "block";

}

function homeClicked()
{
    var navContainer = document.querySelector(".nav-home");
    var updateContainer = document.querySelector(".nav-update");
    updateContainer.style.display = "none";
    navContainer.style.display = "block";

}