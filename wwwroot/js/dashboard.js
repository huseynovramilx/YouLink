/* globals Chart:false, feather:false */
let select;
let linkId='';



function fillChart(labels, values, id) {
    //feather.replace();

    // Graphs
    var ctx = document.getElementById(id);
    // eslint-disable-next-line no-unused-vars
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                steppedLine: false,
                pointRadius: 0,
                backgroundColor: 'transparent',
                borderColor: '#8BBB3E',
                borderWidth: 2,
                pointBackgroundColor: '#FFF09B'
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: false
                    }
                }],
                xAxes: [{
                    display:false
                }]
            },
            legend: {
                display: false
            },
            tooltips: {
                mode: 'nearest',
                intersect:false
            },
            events:['click', 'mousemove']
        }
    });
}

function getLastHourStats() {
        $.ajax({
        url: "/Dashboard/Home/GetLastHourClicksAsync",
        data:
        {
            "linkId": linkId
        },
        method: "GET",
        success: function (data) {
            fillChart(data.labels, data.values, 'chartHour'+linkId);
        },
        error: function (error) {
            console.log(error);
        }
        });
}

function getLastDayStats() {
    $.ajax({
        url: "/Dashboard/Home/GetLastDayClicksAsync",
        data:
        {
            "linkId": linkId
        },
        method: "GET",
        success: function (data) {
            fillChart(data.labels, data.values, 'chartDay'+linkId);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function getLastWeekStats() {
    $.ajax({
        url: "/Dashboard/Home/GetLastWeekClicksAsync",
        data:
        {
            "linkId": linkId
        },
        method: "GET",
        success: function (data) {
            fillChart(data.labels, data.values, 'chartWeek'+linkId);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function getLastMonthStats() {
    $.ajax({
        url: "/Dashboard/Home/GetLastMonthClicksAsync",
        data:
        {
            "linkId": linkId
        },
        method: "GET",
        success: function (data) {
            fillChart(data.labels, data.values, 'chartMonth'+linkId);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function getLastYearStats() {
    $.ajax({
        url: "/Dashboard/Home/GetLastYearClicksAsync",
        data:
        {
            "linkId": linkId
        },
        method: "GET",
        success: function (data) {
            fillChart(data.labels, data.values, 'chartYear' + linkId);
        },
        error: function (error) {
            console.log(error);
        }
    });
}


function getMoneyInfo() {
    let moneyDiv = $("#money");

    $.ajax({
        url: "/Dashboard/Home/GetBalanceAsync",
        method: "GET",
        async: true,
        success: function (data) {
            moneyDiv.html(data + "$");
        },
        error: function (error) {
            console.log(error.status);
            console.log(error.statusText);
        }
    });
}


function getGraph() {
    $('canvas:not(.d-none)').addClass('d-none');
    let element = $('.div');
    if (select === 1) {
        element = $('#chartHour'+linkId);
        if(element.attr('data-loaded')==='0'){
            getLastHourStats();
        }
    }
    else if (select === 2) {
        element = $('#chartDay'+linkId);
        if(element.attr('data-loaded')==='0'){
            getLastDayStats();
        }
    }
    else if (select === 3) {
        element = $('#chartWeek'+linkId);
        if(element.attr('data-loaded')==='0'){
            getLastWeekStats();
        }
    }
    else if (select === 4) {
        element = $('#chartMonth'+linkId);
        if(element.attr('data-loaded')==='0'){
            getLastMonthStats();
        }
    }
    else if (select === 5) {
        element = $('#chartYear'+linkId);
        if(element.attr('data-loaded')==='0'){
            getLastYearStats();
        }
    }
    element.data('loaded', 1);
    element.removeClass('d-none');
}


function getAllCharts() {
    getLastHourStats();
    getLastDayStats();
    getLastWeekStats();
    getLastMonthStats();
    getLastYearStats();
}


function getChart(LinkId){
    linkId = LinkId;
    getGraph();
}




$(document).ready(function () {
    'use strict';
    $(".dropdown-item").click(function () {

        $("#ddText").html($(this).text());
        $('.dropdown-item').removeClass('active');
        $(this).addClass('active');

        select = $(this).data('val');
        getGraph();
    });
    getMoneyInfo();
    getAllCharts();
}());