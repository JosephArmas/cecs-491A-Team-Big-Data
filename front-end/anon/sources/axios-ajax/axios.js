/*
vong example: https://github.com/v-vong3/csulb/blob/master/cecs_491/demo/ajax-demo/front-end/resources/ajax/thirdparty-ajax.js
* using for testing and to get familar with library
*/

function get(url)
{
    const ajax = window.axios
    // creating configs obj
    const configs = {
        method: 'get' // property of configs obj 
    };
    return ajax.get(url,configs)
}

