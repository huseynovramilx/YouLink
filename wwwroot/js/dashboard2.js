/* globals Chart:false, feather:false */
function fillChart(labels, values) {
    //feather.replace();

    // Graphs
    var ctx = document.getElementById('myChart');
    // eslint-disable-next-line no-unused-vars
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                lineTension: 0,
                backgroundColor: 'transparent',
                borderColor: '#007bff',
                borderWidth: 4,
                pointBackgroundColor: '#007bff'
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: false
                    }
                }]
            },
            legend: {
                display: false
            }
        }
    });
}

function getStats(linkId){
    let labels = [];
    let values = [];
    for (var i = 0; i < 60; i++) {
        labels.push("");
        values.push(0);
    }

    let moneyDiv = $("#money");

    $.ajax({
        url: "/Dashboard/Home/GetBalanceAsync",
        method: "GET",
        async:true,
        success: function (data) {
            moneyDiv.html(data + "$");
        },
        error: function (error) {
            console.log(error.status);
            console.log(error.statusText);
        }
    });

    $.ajax({
        url: "/Dashboard/Home/GetLastClicksAsync",
        data:
        {
            "linkId":linkId
        },
        method: "GET",
        success: function (data) {
            let timeNow = data.now;
            $.each(data.clicks, function () {
                let time_this = this.time;
                let m_now =parseInt(timeNow.substr(timeNow.indexOf(":") + 1, 2));
                let m_this =parseInt(time_this.substr(time_this.indexOf(":") + 1, 2));
                let i = (m_this + (59 - m_now))%60;
                labels[i] = this.time;
                values[i] = this.count;
            });
            fillChart(labels, values);
        },
        error: function (error) {
            console.log(error);
        }
    });

    
}
$(document).ready(function () {
    'use strict';
    getStats(null);
}());
