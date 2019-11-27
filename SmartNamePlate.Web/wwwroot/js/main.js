"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/smartNamePlateHub").build();

//Disable send button until connection is established 
document.getElementById("divAvailable").style.display = 'flex';
document.getElementById("divAway").style.display = 'none';


connection.on("ReceiveMessage", function (user, message) {
    if (message === 'SessionUnlock'
        || message === 'SessionLogon'
        || message === 'RemoteConnect') {
        document.getElementById("divAvailable").style.display = 'flex';
        document.getElementById("divAway").style.display = 'none';
    }
    else {
        document.getElementById("divAvailable").style.display = 'none';
        document.getElementById("divAway").style.display = 'flex';
    }
    document.getElementById("divTest").innerText = message;
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(function () {
    $.get("api/image/getimageoftheday", function (data) {
        var i = "data: image/png;base64," + data;
        document.body.style.backgroundImage =i;
    });
});