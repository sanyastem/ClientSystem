'use strict';
var obd;

if (location.protocol + '//' + location.host + '/Home/Static' === location.href) {



        $.ajaxSetup({ cache: false });
        $.getJSON(location.protocol + '//' + location.host + "/Home/json", function (date) {
            all(date);
            console.log(obd);
        }).done(function () {
            if ($('#chart-pie')[0]) {
                $.plot($("#chart-pie"), obd, {
                    series: {
                        shadowSize: 0
                    },
                    grid: {
                        borderWidth: 1,
                        borderColor: '#31424b',
                        show: true,
                        hoverable: false
                    },

                    yaxis: {
                        tickColor: '#31424b',
                        tickDecimals: 0,
                        font: {
                            lineHeight: 13,
                            style: "normal",
                            color: "#98a7ac"
                        },
                        shadowSize: 0
                    },

                    xaxis: {
                        tickColor: '#31424b',
                        tickDecimals: 0,
                        font: {
                            lineHeight: 13,
                            style: "normal",
                            color: "#98a7ac"
                        },
                        shadowSize: 0
                    },

                    legend: {
                        container: '.flot-chart-legend--line',
                        noColumns: 5
                    }
                });
                
            }

            var pieData = d;
            });
        // Make some sample data
        var sd = new Array();


        function all(a) {
            for (var i = 0; i < a.length; i++) {
                sd.push(
                    [a[i].data1, a[i].data2]
                );
            }
            obd = [{
                color: '#edeff0',
                data: sd
            }];

        };
}
var ad;
// Manager
if (location.protocol + '//' + location.host + '/Home/Index' === location.href || location.protocol + '//' + location.host+ '/' === location.href) {


$.getJSON(location.protocol + '//' + location.host + "/Home/JsonManager", function (showing) {
    ad = showing;
}).done(function () {
    console.log(ad);
    if ($('#chart-pie')[0]) {
        $.plot('#chart-pie', ad, {
            series: {
                pie: {
                    show: true,
                    stroke: {
                        width: 0
                    }
                }
            },
            legend: {
                container: '.flot-chart-legend--pie',
                noColumns: 5
            }
        });

    }

});
}