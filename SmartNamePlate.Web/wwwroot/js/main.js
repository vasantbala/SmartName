﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/smartNamePlateHub").build();

var clientsCount = 0;

var lastRefreshDate = new Date().toDateString();
//console.log('Last refresh date is init to ' + lastRefreshDate);

//Disable send button until connection is established 
//document.getElementById("divAvailable").style.display = 'flex';
//document.getElementById("divAway").style.display = 'none';


connection.on("ReceiveMessage", function (user, message) {
    //console.log('Checking last refresh date ' + new Date().toDateString());
    if (lastRefreshDate < new Date().toDateString()) {
        $.get("api/image/getimageoftheday", function (data) {

            $('body').css('background', 'url(data:image/jpg;base64,' + data + ')');
            $('body').css("background-size", "cover");
            $('body').css("background-repeat", "no-repeat");
            lastRefreshDate = new Date().toDateString();
        });
    }

    if (message === 'SessionUnlock'
        || message === 'SessionLogon'
        || message === 'RemoteConnect') {
        $("#imgstatus").attr("src", "./images/status_available.png");
        $("#txtStatus").text('Available');
    }
    else {
        $("#imgstatus").attr("src", "./images/status_away.png");
        $("#txtStatus").text('Away');
    }
});

connection.on("ConnectionMessage", function (user, message) {
    if (connection.connectionId !== user) {
        console.log('ConnectionMessage from ' + user);
        clientsCount++;
        if (clientsCount > 0) {
            $('#divstatus').show();
        }
    }
    
});

connection.on("DisconnectionMessage", function (user, message) {
    if (connection.connectionId !== user) {
        console.log('DisconnectionMessage from ' + user);
        clientsCount--;
        if (clientsCount < 0) {
            $('#divstatus').hide();
        }
    }
    
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(function () {

    $('#divstatus').hide();

    $.get("api/image/getimageoftheday", function (data) {

        $('body').css('background', 'url(data:image/jpg;base64,' + data + ')');
        $('body').css("background-size", "cover");
        $('body').css("background-repeat", "no-repeat");
        /*var url = './images/' + data + "?preventcache=" + $.now();
        console.log(url);
        $('body').css("background-image", "url('" + url + "')");
        $('body').css("background-size", "cover");
        //$('body').css("opacity", "0.5");
        $('body').css("background-repeat", "no-repeat");*/
    });
});