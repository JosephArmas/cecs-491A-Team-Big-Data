var backend = "";
fetch("./config.json").then((response) => response.json()).then((json) => 
{
backend = json.backend;
})

function addProfilePic()
{
    let userID = localStorage.getItem("id");
    let fileSelector = document.getElementById("p-fileSelector");
    let file = fileSelector.files[0];
    if(file === undefined)
    {
        alert("Use the File Selector on the Top Right");
    }
    else
    {
        let filename = file.name;
        let length = filename.length;
        let ext = filename.substring(length - 4, length)
        if(file.size > 1000000)
            {
                alert("File too big");
                return 0;
            }
        if(ext != ".jpg" && ext != ".png" && ext != ".JPG" && ext != ".PNG")
        {
            alert("Incorrect File Extension");
        }
        else
        {
            let url = URL.createObjectURL(file);
            let picture = document.getElementById("profilePicture")
            picture.innerHTML = "<img id=\"profilePic\" style=\"height:400px; width:100%; object-fit:contain\" src=" + url + ">";
            let params = {
                fileName: filename,
                ID: userID,
                role: localStorage.getItem("role"),
                userID: userID
            }
            axios.post(backend + "/File/profileUpload", params).catch(function (error)
            {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                timeOut(cleanError, 'red', errorsDiv)
            }).then(function(key)
            {
                if(key === undefined)
                {

                }
                else if(key.data.length > 0)
                {
                    uploadProfileToS3(key.data);
                }
            })
        }
    }
}

function updateProfilePic()
{
    let userID = localStorage.getItem("id");
    let fileSelector = document.getElementById("p-fileSelector");
    let file = fileSelector.files[0];
    if(file === undefined)
    {
        alert("Use the File Selector on the Top Right");
    }
    else
    {
        let filename = file.name;
        let length = filename.length;
        let ext = filename.substring(length - 4, length)
        if(file.size > 1000000)
            {
                alert("File too big");
                return 0;
            }
        if(ext != ".jpg" && ext != ".png" && ext != ".JPG" && ext != ".PNG")
        {
            alert("Incorrect File Extension");
        }
        else
        {
            let url = URL.createObjectURL(file);
            let picture = document.getElementById("profilePicture")
            picture.innerHTML = "<img id=\"profilePic\" style=\"height:400px; width:100%; object-fit:contain\" src=" + url + ">";
            let params = {
                fileName: filename,
                ID: userID,
                role: localStorage.getItem("role"),
                userID: userID
            }
            axios.post(backend + "/File/profileUpdate", params).catch(function (error)
            {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                timeOut(cleanError, 'red', errorsDiv)
            }).then(function(key)
            {
                if(key === undefined)
                {

                }
                else if(key.data.length > 0)
                {
                    uploadProfileToS3(key.data);
                }
            })
        }
    }
}

function downloadProfilePic()
{
    let userID = localStorage.getItem("id");
    let config = {
        headers : {"ID": userID}
    };
    // Get the key from the backend SQL Server
    axios.post(backend + "/File/profileDownload", 0, config).catch(function (error)
    {
        let errorAfter = error.response.data;
        let cleanError = errorAfter.replace(/"/g,"");
        timeOut(cleanError, 'red', errorsDiv)
    }).then(function(key)
    {
        if(key === undefined)
        {

        }
        else if(key.data.length > 0)
        {
            // Download file from S3
            axios.get(s3 + "/" + key.data).catch(function (error)
            {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                timeOut(cleanError, 'red', errorsDiv)
            }).then(function(file)
            {
                let picture = document.getElementById("profilePicture");
                picture.innerHTML = "<img id=\"profilePic\" style=\"height:400px; width:100%; object-fit:contain\" src=" + file.data + ">";
            })
        }
    })
}

function deleteProfilePic()
{
    let userID = localStorage.getItem("id");
    let picture = document.getElementById("profilePicture")
    picture.innerHTML = "<img id=\"profilePic\" style=\"height:400px; width:100%; object-fit:contain\" src=\"blank image.jpg\">";
    let params = {
        fileName: "",
        ID: userID,
        role: localStorage.getItem("role"),
        userID: userID
    }
    axios.post(backend + "/profileDelete", params).catch(function (error)
    {
        let errorAfter = error.response.data;
        let cleanError = errorAfter.replace(/"/g,"");
        timeOut(cleanError, 'red', errorsDiv)
    }).then(function(key)
    {
        deleteFromS3(key.data);
    })
}