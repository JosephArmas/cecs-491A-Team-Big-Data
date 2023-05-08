var backend = "";
var s3 = "";
fetch("./config.json").then((response) => response.json()).then((json) => 
{
backend = json.backend;
s3 = json.s3;
})

//Uploads File to AWS Bucket
function uploadToS3(key)
{
    if(key.length > 0)
    {
        let fileSelector = document.getElementById("fileSelector");
        let file = fileSelector.files[0];
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = (function (x)
        {
            axios.put(s3 + "/" + key, reader.result).catch(function (error)
            {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                errorsDiv.innerHTML = cleanError; 
            })
        })
    }
}

function uploadProfileToS3(key)
{
    if(key.length > 0)
    {
        let fileSelector = document.getElementById("p-fileSelector");
        let file = fileSelector.files[0];
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = (function (x)
        {
            axios.put(s3 + "/" + key, reader.result).catch(function (error)
            {
                let errorAfter = error.response.data;
                let cleanError = errorAfter.replace(/"/g,"");
                errorsDiv.innerHTML = cleanError; 
            })
        })
    }
}

function deleteFromS3(key)
{
    if(key.length > 0)
    {
        axios.delete(s3 + "/" + key).catch(function (error)
        {
            let errorAfter = error.response.data;
            let cleanError = errorAfter.replace(/"/g,"");
            errorsDiv.innerHTML = cleanError; 
        })
    }
}