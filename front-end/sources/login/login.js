'use strict';
// working dispaly data to div elementkj
function displayData()
{
    let testElement = document.getElementById('test-data');
    axios.get('https://jsonplaceholder.typicode.com/posts').then( function (response) {
        console.log(response.data)
        testElement.innerHTML = JSON.stringify(response.data);
    })
    
}

// displayData();
function postDisplayData()
{
    let testElement = document.getElementById('test-data');
    var dataa = 
            {
                "title": "foo",
                "body": "bar",
                "userId": 1
          };
    axios.post('https://jsonplaceholder.typicode.com/posts', dataa).then(function (response) {
        console.log(response.data);
        var dataAfter = response.data
        testElement.innerHTML = JSON.stringify(response.data);
        console.log(dataAfter.title);
    })
}
postDisplayData();