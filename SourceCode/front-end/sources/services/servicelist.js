function serviceClicked() {
  var userDrivenServices = document.querySelector(".services-container");
  var homeContainer = document.querySelector(".home-container");
  homeContainer.style.display = "none";
  userDrivenServices.style.display = "block";

  document.getElementById("servReq").addEventListener('click', requestsClicked);

  document.getElementById("servCrea").addEventListener('click', creationClicked);

  document.getElementById("servicesBack-home").addEventListener('click', servicesBackClicked);

}


//---------------------------------------------------------


function requestsClicked() {
  var servicelistContainer = document.querySelector(".servicelist-container");
  var userDrivenServices = document.querySelector(".services-container");

  userDrivenServices.style.display = "none";
  servicelistContainer.style.display = "block";
  document.getElementById("requestsBack-services").addEventListener('click', requestsBackClicked);


let data = [];


  let list = document.getElementById("serviceList");
list.innerText = "Loading"
let choice = "";

  function lister(x, y) {
document.getElementById("serviceList").innerText = "";
    x.forEach((service) => {
      let button = document.createElement("button");
      button.classList.add('servButton');
      button.innerText = service.ServiceName;
      y.appendChild(button);
    });
  }

  function buttonlisten() {
    document.querySelectorAll(".servButton").forEach(item => {
      item.addEventListener('click', event => {
        data.forEach((service) => {
          document.getElementById('infoTab').style.visibility = 'visible';
          if (item.innerText == service.ServiceName) {
            document.getElementById('infoTitle').innerText = service.ServiceName;
            document.getElementById('infoDesc').innerText = service.RequestLat + " , " + service.RequestLong;
let requestAccept = "";
		if(service.Accept == 1){
requestAccept = "Accepted";
}
else{
requestAccept = "Pending";
}
            document.getElementById('infoNum').innerText = requestAccept;
		choice = service;
          }
        })
      });
    });
  }



  function regularRequests() {
    let cancelBtn = document.createElement("button");
    cancelBtn.setAttribute("id", "requestCancelBtn");
    cancelBtn.innerText = "Cancel";
    document.getElementById("ResponseButtons").appendChild(cancelBtn)
cancelBtn.addEventListener("click", event => {
    var request = axios.post('https://localhost:7259/UserServices/DenyRequest',choice,{
      headers: {
        'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
      }
    });
    request.then(function(response) {
      data = response.data
    });
}
);
  }

  function providerRequests() {
    let denyBtn = document.createElement("button");
    denyBtn.setAttribute("id", "denyBtn");
    denyBtn.innerText = "Deny";
    document.getElementById("ResponseButtons").appendChild(denyBtn)
    let acceptBtn = document.createElement("button");
    acceptBtn.setAttribute("id", "acceptBtn");
    acceptBtn.innerText = "Accept";
    document.getElementById("ResponseButtons").appendChild(acceptBtn)
acceptBtn.addEventListener("click", event => {
    var request = axios.post('https://localhost:7259/UserServices/AcceptRequest',{RequestID: choice.RequestID, ServiceID: choice.ServiceID, ServiceName: choice.ServiceName,
Requester: localStorage.getItem("id"), RequestLat: choice.RequestLat,RequestLong: choice.RequestLong,PinType: choice.PinTypes,Accept: choice.Accept, Distance: choice.Distance},{
      headers: {
        'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
      }
    });
}
);
denyBtn.addEventListener("click", event => {
    var request = axios.post('https://localhost:7259/UserServices/DenyRequest',{RequestID: choice.RequestID, ServiceID: choice.ServiceID, ServiceName: choice.ServiceName,
Requester: localStorage.getItem("id"), RequestLat: choice.RequestLat,RequestLong: choice.RequestLong,PinType: choice.PinTypes,Accept: choice.Accept, Distance: choice.Distance},{
      headers: {
        'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
      }
    });
}
);
  }

  if (localStorage.getItem("role") == "Regular User" || localStorage.getItem("role") == "Reputable User") {
    regularRequests();
  }
  if (localStorage.getItem("role") == "Service User") {
    providerRequests();
  }

  document.getElementById('infoTab').style.visibility = 'hidden';
	let listem = {};
    var request = axios.get('https://localhost:7259/UserServices/GetRequests',{
      headers: {
        'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
      }
    });
    request.then(function(response) {
      data = response.data
  lister(data, list);
  buttonlisten();
    });

}

//---------------------------------------------------------------------

function creationClicked() {
  let webServiceUrl = 'https://localhost:7259/UserServices/CreateService'
  var creationContainer = document.querySelector(".servicecreation-container");
  var userDrivenServices = document.querySelector(".services-container");
  userDrivenServices.style.display = "none";
  creationContainer.style.display = "block";
  document.getElementById("servicecreationBack-services").addEventListener('click', creationBackClicked);
  let responseBox = document.getElementById("responseBox");
  let responseBoxAcc = document.getElementById("continue");

  //Borrowed from map.js for pin verifications
  function servicetitleLimit(title) {
    let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]")
    let err = "";
    if (title.length > 30) {
      err = "Syntax Error. Title over 30 Character Limit"
    } else if (title.length < 4) {
      err = "Syntax Error. Title under 4 characters"
    } else if (!charactersAllowed.test(title)) {
      err = "Syntax Error. Invalid Title Characters";
    }
    return err;
  }

  //Borrowed from map.js for pin verifications
  function descriptionLimit(description) {
    let charactersAllowed = new RegExp("^[a-zA-Z0-9áéíóúüñ¿¡ÁÉÍÓÚÜÑ@.,!\s-]");
    let err = "";
    if (description.split(' ').length > 150) {
      err = "Syntax Error. Description is over 150 word limit";
    } else if (!charactersAllowed.test(description)) {
      err = "Syntax Error. Invalid Description Characters";
    }
    return err;
  }

  function GetPinTypes(){
let types = "";
if(document.getElementById("servicePintype1").checked == true){
types += document.getElementById("servicePintype1").value;
}
if(document.getElementById("servicePintype2").checked == true){
types += document.getElementById("servicePintype2").value;
}
if(document.getElementById("servicePintype3").checked == true){
types += document.getElementById("servicePintype3").value;
}
if(document.getElementById("servicePintype4").checked == true){
types += document.getElementById("servicePintype4").value;
}
if(document.getElementById("servicePintype5").checked == true){
types += document.getElementById("servicePintype5").value;
}
return types;
}

  function CreateUserService() {
    var service = {};
    service.ServiceName = document.getElementById("serviceTitle").value;
    service.ServiceDescription = document.getElementById("serviceDesc").value;
    service.ServicePhone = document.getElementById("servicePhone").value;
    service.ServiceURL = document.getElementById("serviceWebsite").value;
    service.ServiceLat = document.getElementById("serviceLat").value
    service.ServiceLong = document.getElementById("serviceLong").value
    service.PinTypes = GetPinTypes();
    service.Distance = document.getElementById("servicedistanceValue").value;
    service.CreatedBy = localStorage.getItem("id");
    service.ServiceID = 0;
    let titleTest = servicetitleLimit(service.ServiceName);
    let descTest = descriptionLimit(service.ServiceDescription);
    if (titleTest == '' && descTest == '') {


      var request = axios.post(webServiceUrl, service, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
        }
      });
      request.then(function(response) {
        document.getElementById("Parentcon").innerText = response.data
      });
    } else if (titleTest != '') {
      document.getElementById("Parentcon").innerText = titleTest;
    } else if (descTest != '') {
      document.getElementById("Parentcon").innerText = descTest;
    }
    openResponseBox();
  }

  function DeleteService() {
    var service = {};
    service.ServiceName = document.getElementById("serviceTitle").value;
    service.ServiceDescription = document.getElementById("serviceDesc").value;
    service.ServicePhone = document.getElementById("servicePhone").value;
    service.ServiceURL = document.getElementById("serviceWebsite").value;
    service.PinTypes = GetPinTypes();
    service.Distance = document.getElementById("servicedistanceValue").value;
    service.CreatedBy = localStorage.getItem("id");
    var request = axios.post('https://localhost:7259/UserServices/DeleteService', service, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
      }
    });
    request.then(function(response) {
      document.getElementById("Parentcon").innerText = response.data
    });
    openResponseBox();
  }

  function UpdateService() {
    var service = {};
    service.ServiceName = document.getElementById("serviceTitle").value;
    service.ServiceDescription = document.getElementById("serviceDesc").value;
    service.ServicePhone = document.getElementById("servicePhone").value;
    service.ServiceURL = document.getElementById("serviceWebsite").value;
    service.ServiceLat = document.getElementById("serviceLat").value
    service.ServiceLong = document.getElementById("serviceLong").value
    service.PinTypes = GetPinTypes();
    service.Distance = document.getElementById("servicedistanceValue").value;
    service.CreatedBy = localStorage.getItem("id");
    var request = axios.post('https://localhost:7259/UserServices/UpdateService', service, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem("jwtToken")}`
      }
    });
    request.then(function(response) {
      document.getElementById("Parentcon").innerText = response.data
    });
    openResponseBox();
  }

  function openResponseBox() {
    responseBox.showModal();
  }

  function closeResponseBox() {
    responseBox.close();
  }
  if (localStorage.getItem("role") == "Regular User" || localStorage.getItem("role") == "Reputable User") {
    document.getElementById("submitBtn").addEventListener('click', CreateUserService);
  }
  if (localStorage.getItem("role") == "Service User") {
    document.getElementById("submitBtn").innerText = 'Update Information'
    let deleteBtn = document.createElement("button");
    deleteBtn.setAttribute("id", "serviceDeleteBtn");
    deleteBtn.innerText = "Delete Service";
    document.getElementById("serviceCreationBtns").appendChild(deleteBtn);
    document.getElementById("serviceDeleteBtn").addEventListener('click', DeleteService);
    document.getElementById("submitBtn").addEventListener('click', UpdateService);
  }

  document.getElementById("creationcontinue").addEventListener('click', closeResponseBox);
}

//---------------------------------------------------------

function creationBackClicked() {
  var creationContainer = document.querySelector(".servicecreation-container");
  var userDrivenServices = document.querySelector(".services-container");
  userDrivenServices.style.display = "block";
  creationContainer.style.display = "none";
  if (localStorage.getItem("role") == "Service User") {
    var delBtn = document.getElementById("serviceCreationBtns")
    //delBtn.removeChild(delBtn.firstChild);
	delBtn.innerHTML = "";
  }
}

//---------------------------------------------------------


function requestsBackClicked() {
  var servicelistContainer = document.querySelector(".servicelist-container");
  var userDrivenServices = document.querySelector(".services-container");
  userDrivenServices.style.display = "block";
  servicelistContainer.style.display = "none";

  //Borrowed from https://www.javascripttutorial.net/dom/manipulating/remove-all-child-nodes/
  function removeAllChildNodes(parent) {
    while (parent.firstChild) {
      parent.removeChild(parent.firstChild);
    }
  }
  let container = document.querySelector('#ResponseButtons');
  //removeAllChildNodes(container);
  container.innerHTML = "";
  document.getElementById("serviceList").innerHTML = "";
}

//---------------------------------------------------------


function servicesBackClicked() {
  var userDrivenServices = document.querySelector(".services-container");
  var homeContainer = document.querySelector(".home-container");
  homeContainer.style.display = "block";
  userDrivenServices.style.display = "none";
}


//---------------------------------------------------------

