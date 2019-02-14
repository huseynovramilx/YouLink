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
    $.ajax({
        url: "Dashboard/Home/GetClicksLastHourAsync",
        data:
        {
            "linkId":linkId
        },
        method: "GET",
        success: function (data) {
            let timeNow = data.now;
            $.each(data.clicks, function () {
                let time_this = this.time;
                let m_now =Number.parseInt(timeNow.substr(timeNow.indexOf(":") + 1, 2));
                let m_this =Number.parseInt(time_this.substr(time_this.indexOf(":") + 1, 2));
                let i = (m_this + (59 - m_now))%60;
                labels[i] = this.time;
                values[i] = this.count;
            });
            fillChart(labels, values);
        }
    });

    
}
$(document).ready(function () {
    'use strict';
    getStats(null);
}());
