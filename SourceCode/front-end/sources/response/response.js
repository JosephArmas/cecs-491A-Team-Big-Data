function timeOut(errorMsg,color,divElement)
{
    divElement.style.display = "block";
    divElement.innerHTML = errorMsg;
    divElement.style.color = color;
    // Shows the message for 3 seconds then disappears
    setTimeout(function(){
        divElement.style.display = "none";
    }, 3000);
}