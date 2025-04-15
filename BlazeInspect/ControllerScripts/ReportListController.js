var app = angular.module('Homeapp', []);
app.controller('ReportListController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Home/')
    const notiSound = new Audio('/admin_assets/sound/notification.wav');

    $http.get(localStorage.getItem('URLIndex') + 'GetNotification').then(function (i) {
        debugger
        $scope.GetNotification = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetNotification = error;
    });

    $http.get(localStorage.getItem('URLIndex') + 'GetReportList').then(function (i) {
        debugger
        $scope.GetReportList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetReportList = error;
    });
    $http.get(localStorage.getItem('URLIndex') + 'GetMessage').then(function (i) {
        //debugger
        $scope.GetMessage = i.data;
    },
        function (error) {
            alert(error);
            $scope.GetMessage = error;
        });


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
    $scope.GetReport = function (Heading, serial_no, date, time) {
        debugger
        $('.page-loader-wrapper').fadeIn();
        let timeString = time;
        let newTimeString = timeString.split(':').slice(0, 2).join(':');
        //console.log(newTimeString); // Outputs: "06:35"

        var datetime = date.concat("-", newTimeString)

        //var getdatetime = date + time;
        //var datetime = getdatetime.replace(" ", "-");

        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'GetReport',
            data: { SN: serial_no, DT: datetime },
            success: function (result) {
                debugger
                $('.page-loader-wrapper').fadeOut();

                $("#GetreportHeading").text(Heading);
                $("#GetreportModalBodyId").html(result);
                $("#GetreportModalID").modal('show');
            },
            error: function (xhr, status, error) {
                $('.page-loader-wrapper').fadeOut();
                var errorMessage = xhr.status + ': ' + xhr.statusText
                alert('Error - ' + errorMessage);
            }
        })
    }

    $scope.ReportSearchFunction = function () {
        debugger
        var id = $("#filter_id").val();
        var start_date_id = $("#start_date_id").val();
        var end_date_id = $("#end_date_id").val();

        if (id == "" && start_date_id == "" && end_date_id == "") {
            showWithTitleMessage("Select on Search Box or Data Arrange is a Mandatory Requirement, Please Put it..");
        }
        else {
            debugger
            localStorage.setItem('Search', "Search");
            localStorage.setItem('id', id);
            localStorage.setItem('start_date_id', start_date_id);
            localStorage.setItem('end_date_id', end_date_id);
            $http.get(localStorage.getItem('URLIndex') + "ReportSearchList?id=" + id + "&start_date_id=" + start_date_id + "&end_date_id=" + end_date_id).then(function (cp) {
                debugger
                $scope.ReportSearchList = cp.data;
            },
            function (error) {
                alert(error)
                $scope.ReportSearchList = error;
            });
        }
    }

}); 