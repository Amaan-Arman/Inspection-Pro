var app = angular.module('Homeapp', []);
app.controller('IndexController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Home/')
    const notiSound = new Audio('/admin_assets/sound/notification.wav');

    $(document).ready(function () {
        setTimeout(function () {
            $("#intromodel").modal('show');
        }, 5000);
    });

    
    $http.get(localStorage.getItem('URLIndex') + 'GetSubscription').then(function (i) {
        //debugger
        $scope.GetSubscription = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetSubscription = error;
        });
    $http.get(localStorage.getItem('URLIndex') + 'GetMessage').then(function (i) {
        //debugger
        $scope.GetMessage = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetMessage = error;
        });
    $http.get(localStorage.getItem('URLIndex') + 'GetNotification').then(function (i) {
        //debugger
        $scope.GetNotification = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetNotification = error;
    });

    // Request notification permission on load
    requestNotificationPermission();
    function requestNotificationPermission() {
        if (Notification.permission === "denied" || Notification.permission === "default") {
            Notification.requestPermission().then(function (permission) {
                if (permission === "granted") {
                    console.log("Notification permission granted.");
                } else {
                    console.log("Notification permission denied.");
                }
            });
        }
    }

    // Reference the auto-generated proxy for the hub.
    var chatHub = $.connection.chatHub;
    // Create a function that the hub can call to broadcast messages.
    chatHub.client.broadcastMessage = function (userId, user_name, message, type) {
        debugger
        //if (localStorage.getItem('Getchatmsg') == userId) {
        if (type === "message") {
            if (localStorage.getItem('Getchatmsg') !== "") {
                $http.get(localStorage.getItem('URLIndex') + 'GetChatbox?ID=' + localStorage.getItem('Getchatmsg')).then(function (i) {
                    $scope.GetChatbox = i.data;
                    //showNotification("New Message form " + user_name, {
                    //    body: message,  // Customize this to show a relevant message
                    //    icon: "path/to/icon.png"  // Optional
                    //});
                },
                    function (error) {
                        alert(error)
                        $scope.GetChatbox = error;
                    });
            }
        }
        else if (type === "notification") {
            $http.get(localStorage.getItem('URLIndex') + 'GetNotification').then(function (response) {
                $scope.GetNotification = response.data;
                // Show a notification if there are new notifications
                //showNotification("Inspection Alert..!", {
                //    body: response.data[0].messege_text,  // Customize this to show a relevant message
                //    icon: "path/to/icon.png"  // Optional
                //});
            },
                function (error) {
                    alert(error);
                    $scope.GetNotification = error;
                });
        }
        else {
            console.log("undefined" + type);
        }
        //}
    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        console.log('SignalR connected');
    }).fail(function (error) {
        console.log('Could not connect to SignalR: ' + error);
    });

    //Notification

    $http.get(localStorage.getItem('URLIndex') + 'Gettotalproduct').then(function (i) {
        /*debugger*/
        $scope.Gettotalproduct = i.data;
    },
    function (error) {
        alert(error);
        $scope.Gettotalproduct = error;
        });
    $http.get(localStorage.getItem('URLIndex') + 'Gettotalinspector').then(function (i) {
        /*debugger*/
        $scope.Gettotalinspector = i.data;
    },
        function (error) {
            alert(error);
            $scope.Gettotalinspector = error;
        });
    $http.get(localStorage.getItem('URLIndex') + 'GetpendingInspection').then(function (i) {
        /*debugger*/
        $scope.GetpendingInspection = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetpendingInspection = error;
        });
    $http.get(localStorage.getItem('URLIndex') + 'GetInspectionHistory').then(function (i) {
        /*debugger*/
        $scope.GetInspectionHistory = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetInspectionHistory = error;
        });

    $http.get(localStorage.getItem('URLIndex') + 'GetInspectionStatus').then(function (pie) {
        //debugger
        $scope.GetInspectionStatus = pie.data;
        var options = {
            series: [pie.data[0].green, pie.data[0].orange, pie.data[0].red],
            chart: {
                width: 330,
                height: 250,
                type: 'pie',
            },
            labels: ['Green', 'Orange', 'red'],
            colors: ['#53e73c', '#f2711c', '#f60a0a'],
            responsive: [{
                breakpoint: 380,
                options: {
                    chart: {
                        width: 250
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }]
        };

        var chart = new ApexCharts(document.querySelector("#chart"), options);
        chart.render();
    },
        function (error) {
            alert(error);
            $scope.GetInspectionStatus = error;
        });

    $http.get(localStorage.getItem('URLIndex') + 'Getinspectionanalyse').then(function (cdata) {
        debugger
        $scope.Getinspectionanalyse = cdata.data;
        var seriesData = $scope.Getinspectionanalyse.map(function (item) {
            return [new Date(item.Date).getTime(), item.todayinspection];
        });
        var options = {
            //annotations: {
            //    yaxis: [{
            //        y: 30,
            //        borderColor: '#E74C3C',
            //        label: {
            //            show: true,
            //            text: 'Support',
            //            style: {
            //                color: "#fff",
            //                background: '#E74C3C'
            //            }
            //        }
            //    }],
            //    xaxis: [{
            //        x: new Date('14 Nov 2012').getTime(),
            //        borderColor: '#E74C3C',
            //        yAxisIndex: 0,
            //        label: {
            //            show: true,
            //            text: 'Sales',
            //            style: {
            //                color: "#fff",
            //                background: '#E74C3C'
            //            }
            //        }
            //    }]
            //},
            chart: {
                type: 'area',
                height: 350,
                toolbar: {
                    show: false,
                },
            },
            colors: ['#E74C3C'],
            dataLabels: {
                enabled: false
            },

            series: [{
                data: seriesData // Use the mapped data here
                //data: [ 
                //    //[new Date('01 Dec 2012').getTime(), 31.34],
                //    //[new Date('01 Dec 2012').getTime(), 31.18],
                //]
            },

            ],
            markers: {
                size: 0,
                style: 'hollow',
            },
            xaxis: {
                type: 'datetime',
                min: new Date('"' + cdata.data[0].Date + '"').getTime(),
                tickAmount: 6,
                show: false,
            },
            tooltip: {
                x: {
                    format: 'yyyy MMM dd'
                }
            },
            stroke: {
                show: true,
                curve: 'smooth',
                width: 1,
            },
            grid: {
                yaxis: {
                    lines: {
                        show: false,
                    }
                },
            },
        }
        var chart = new ApexCharts(
            document.querySelector("#apex-timeline-chart"),
            options
        );
        chart.render();
        var resetCssClasses = function (activeEl) {
            var els = document.querySelectorAll("button");
            Array.prototype.forEach.call(els, function (el) {
                el.classList.remove('active');
            });
            activeEl.target.classList.add('active')
        }
        document.querySelector("#one_month").addEventListener('click', function (e) {
            resetCssClasses(e)
            const now = new Date().getTime(); // Current time in milliseconds
            const thirtyDaysAgo = now - (30 * 24 * 60 * 60 * 1000); // 30 days ago in milliseconds

            chart.updateOptions({
                xaxis: {
                    //min: new Date('28 Jan 2013').getTime(),
                    //max: new Date('27 Feb 2013').getTime(),
                    min: thirtyDaysAgo,
                    max: now,
                }
            })
        })
        document.querySelector("#six_months").addEventListener('click', function (e) {
            resetCssClasses(e);

            const now = new Date().getTime(); // Current time in milliseconds
            const sixMonthsAgo = now - (6 * 30 * 24 * 60 * 60 * 1000); // 6 months ago (approx.)

            chart.updateOptions({
                xaxis: {
                    min: sixMonthsAgo,
                    max: now
                }
            });
        });

        document.querySelector("#one_year").addEventListener('click', function (e) {
            resetCssClasses(e);

            const now = new Date().getTime(); // Current time in milliseconds
            const oneYearAgo = now - (365 * 24 * 60 * 60 * 1000); // 1 year ago

            chart.updateOptions({
                xaxis: {
                    min: oneYearAgo,
                    max: now
                }
            });
        });

        document.querySelector("#ytd").addEventListener('click', function (e) {
            resetCssClasses(e);

            const now = new Date(); // Current date
            const startOfYear = new Date(now.getFullYear(), 0, 1).getTime(); // Start of the current year
            const currentTime = now.getTime(); // Current time in milliseconds

            chart.updateOptions({
                xaxis: {
                    min: startOfYear,
                    max: currentTime
                }
            });
        });
        document.querySelector("#all").addEventListener('click', function (e) {
            resetCssClasses(e)
            chart.updateOptions({
                xaxis: {
                    min: undefined,
                    max: undefined,
                }
            })
        })
    },
        function (error) {
            alert(error);
            $scope.Getinspectionanalyse = error;
        });

    $scope.MarkRead = function () {
        $('.page-loader-wrapper').fadeIn();

        debugger
        var value = $("#user_ID").val();

        if (value == "") {
            var ID = $("#M_user_ID").val();
            var type = "message"
        }
        else 
        {
            var ID = $("#user_ID").val();
            var type = "notification"
        }
        
        $.ajax(
            {
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'MarkasRead',
                data: { ID: ID , type:type},
                //contentType: false,
                //processData: false,
                success: function (status) {
                    $('.page-loader-wrapper').fadeOut();

                    //debugger
                    if (status == 'Saved') {
                        $http.get(localStorage.getItem('URLIndex') + 'GetNotification').then(function (i) {
                            //debugger
                            $scope.GetNotification = i.data;
                        },
                        function (error) {
                            alert(error);
                            $scope.GetNotification = error;
                            });
                        $http.get(localStorage.getItem('URLIndex') + 'GetMessage').then(function (i) {
                            //debugger
                            $scope.GetMessage = i.data;
                        },
                            function (error) {
                                alert(error);
                                $scope.GetMessage = error;
                            });
                    }
                    else if (status == "DataBaseError") {
                        $(".imageLoadinng").hide();
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html("<strong>Database Connectivity Error</strong>, Please check  application database connection...");
                    }
                    else if (status == "NetworkError") {
                        $(".imageLoadinng").hide();
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                    }
                    else if (status == "ExceptionError") {
                        $(".imageLoadinng").hide();
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html("<strong>Exception Error</strong>, Please Check the Error Log...");
                    }
                    else {
                        //alert("Record not Delete...")
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html(status);
                    }

                },
                error: function (xhr, status, error) {
                    //$(".loader").hide();
                    $(".imageLoadinng").hide();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    //alert('Error - ' + errorMessage);
                    $("#ErrorModalID").modal('show');
                    $("#ErrormodeltextId").html('Error - ' + errorMessage);
                }
            });
    }
    $scope.DetailFuntion = function () {
        //debugger
        $('.page-loader-wrapper').fadeIn();

        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'DetailFuntion',
            //data: { ID: ID },
            success: function (result) {
                debugger
                $('.page-loader-wrapper').fadeOut();

                $("#inspectionreportHeading").text("Product Detail");
                $("#inspectionreportModalBodyId").html(result);
                $("#inspectionreportModalID").modal('show');
            },
            error: function (xhr, status, error) {
                debugger
                $('.page-loader-wrapper').fadeOut();
                var errorMessage = xhr.status + ': ' + xhr.statusText
                alert('Error - ' + errorMessage);
            }
        })
    }



    //$http.get(localStorage.getItem('URLIndex') + 'Getyesterdayinspection').then(function (i) {
    //    debugger
    //    $scope.Getyesterdayinspection = i.data;
    //},
    //    function (error) {
    //        alert(error);
    //        $scope.Getyesterdayinspection = error;
    //    });
    //$http.get(localStorage.getItem('URLIndex') + 'GetWeeklyinspection').then(function (i) {
    //    debugger
    //    $scope.GetWeeklyinspection = i.data;
    //},
    //    function (error) {
    //        alert(error);
    //        $scope.GetWeeklyinspection = error;
    //    });
    //$http.get(localStorage.getItem('URLIndex') + 'GetMonthlyinspection').then(function (i) {
    //    debugger
    //    $scope.GetMonthlyinspection = i.data;
    //},
    //    function (error) {
    //        alert(error);
    //        $scope.GetMonthlyinspection = error;
    //    });
}); 

