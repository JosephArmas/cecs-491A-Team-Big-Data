const backend = "https://localhost:7259/File/";
const s3 = "https://utificationbucket.s3.amazonaws.com/";

//Gets File From AWS Bucket
function s3Upload(key)
{
    if(key.length > 0)
    {
        let box = document.getElementById("file-container");
        let fileSelector = document.getElementById("fileSelect");
        let file = fileSelector.files[0];
        let url = URL.createObjectURL(file);
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = (function (x)
        {
            axios.put(host + key.data, reader.result, config).catch(function (error)
            {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                errorsDiv.innerHTML = cleanError; 
            });
        })
    }
}

function s3Download(key)
{
    axios.get(host + key).catch(function (error)
    {
        let errorAfter = error.response.data;
        let cleanError = errorAfter.replace(/"/g,"");
        errorsDiv.innerHTML = cleanError; 
    }).then(function(file)
    {
        //picture stored as a DataURL for easy access
        return file.data;
    })
}

function fileDownload(pinID)
{
    //check if pinId is an int
    if(1 + pinID === pinID + 1 && pinID % 1 === 0)
    {
        let params = {
            fileName: "",
            ID: pinID,
            role: "Regular User",
            //Get Login Information Here
            userID: 1054
        }
        axios.post(backend + "pinDownload", params).catch(function (error)
        {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g,"");
            errorsDiv.innerHTML = cleanError; 
        }).then(function(key)
        {
            let box = document.getElementById("file-container");
            let image = s3Download(key.data);
            box.innerHTML = image;
        })
    }
}

//Uploads File to AWS Bucket
async function uploadPinPic(pinID)
{
    let fileSelector = document.getElementById("fileSelector");
    let file = fileSelector.files[0];
    let filename = file.name;
    let worked = false;
    let url = URL.createObjectURL(file);
    let reader = new FileReader();
    let params = {
        fileName: filename,
        ID: pinID,
        role: localStorage.getItem("role"),
        userID: localStorage.getItem("id")
    }
    await axios.post(backend + "pinUpload", params).catch(function (error)
    {
        let errorAfter = error.response.data;
        let cleanError = errorAfter.replace(/"/g,"");
        errorsDiv.innerHTML = cleanError; 
    }).then(function(key)
    {
        if(url.length > 0)
        {
            infoWindows[pos].close();
            let content = infoWindows[pos].content;

            content = content + "<img style=\"height:100%; width:100%; object-fit:contain\" src=" + s + ">";

            infoWindows[pos].setContent(content);
        }
        reader.readAsDataURL(file);
        reader.onload = (function (x)
        {
            axios.put(s3 + key.data, reader.result).catch(function (error)
            {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                errorsDiv.innerHTML = cleanError; 
            }).then(function (x) {
                worked = true;});
        })
    })
    if(worked)
    {
        alert(url);
        return url;
    }
}