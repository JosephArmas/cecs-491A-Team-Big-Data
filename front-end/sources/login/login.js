'use strict';
// working dispaly data to div elementkj
function displayData()
{
    let testElement = document.getElementById('test-data');
    axios.get('https://jsonplaceholder.typicode.com/posts').then( function (response) {
        testElement.innerHTML = JSON.stringify(response.data);
    })
    
}

// displayData();