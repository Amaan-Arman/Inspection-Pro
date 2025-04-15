var app = angular.module('Homeapp', []);
app.controller('ChatAppController', function ($scope, $http) {
   
    localStorage.setItem('URLIndex', '/Home/')
    localStorage.setItem('Getchatmsg', "");
    const notiSound = new Audio('/admin_assets/sound/notification.wav');

    $http.get(localStorage.getItem('URLIndex') + 'GetList').then(function (i) {
        $scope.GetList = i.data;
    }, function (error) {
        alert(error);
        $scope.GetList = error;
    });

    //Notification
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


    $scope.GetChat = function (user_id) {
        $('.page-loader-wrapper').fadeIn();
        var ID = user_id;
        localStorage.setItem('Getchatmsg', user_id);
        $http.get(localStorage.getItem('URLIndex') + 'GetuserData?ID=' + ID).then(function (i) {
            $('.page-loader-wrapper').fadeOut();
            $scope.GetuserData = i.data;
        }, function (error) {
            alert(error);
            $scope.GetuserData = error;
        });

        $http.get(localStorage.getItem('URLIndex') + 'GetChatbox?ID=' + ID).then(function (i) {
            $('.page-loader-wrapper').fadeOut();
            $scope.GetChatbox = i.data;
        }, function (error) {
            alert(error);
            $scope.GetChatbox = error;
        });
    };
    $scope.sendmessage = function (user_id, user_name) {
        var ID = user_id;
        var msg = $("#message_txt").val();

        if (msg !== "") {
            var data = new FormData();
            data.append("receiver_user_id", ID);
            data.append("messege_text", msg);

            $.ajax({
                type: "Post",
                url: localStorage.getItem('URLIndex') + 'Sendmessage',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    $("#message_txt").get(0).value = "";
                    if (result === "Saved")
                    {
                        $http.get(localStorage.getItem('URLIndex') + 'GetChatbox?ID=' + ID).then(function (i) {
                            debugger
                            $scope.GetChatbox = i.data;
                        },
                        function (error) {
                            alert(error)
                            $scope.GetChatbox = error;
                            });
                        // Call the SignalR hub method to send the message
                        //chatHub.server.sendMessage(ID, user_name, msg);
                        chatHub.server.sendMessage(user_id, user_name, msg,"message");
                    }
                    else {
                        handleError(result);
                    }
                },
                error: function (xhr, status, error) {
                    $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText;
                    $("#ErrorModalID").modal('show');
                    $("#ErrormodeltextId").html('Error - ' + errorMessage);
                }
            });
        }
    };
    $scope.SendAttachment = function (user_id) {
        debugger
        $('.page-loader-wrapper').fadeIn();

        var ID = user_id;
        var attachment = $("#attachment").get(0).files;
        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];

        if (attachment.length == 0) {
            $("#attachment_errormessage").html("Please upload Image!");
        }
        else if ($.inArray($("#attachment").val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            $("#attachment_errormessage").html("Invalid format!");
        }
        else {
            var data = new FormData;
            data.append("receiver_user_id", ID);
            for (var j = 0; j < attachment.length; j++) {
                data.append("attachment", attachment[j]);
            }

            $.ajax({
                type: "Post",
                url: localStorage.getItem('URLIndex') + 'Sendmessage',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger
                    $('.page-loader-wrapper').fadeOut();
                    
                    $("#addtask").modal('hide');
                    if (result == "Saved")
                    {
                        $http.get(localStorage.getItem('URLIndex') + 'GetChatbox?ID=' + localStorage.getItem('Getchatmsg') ).then(function (i) {
                            $scope.GetChatbox = i.data;
                        },
                        function (error) {
                            alert(error)
                            $scope.GetChatbox = error;
                            });
                        // Call the SignalR hub method to send the message
                        //alert('Attempting to send message');
                        chatHub.server.sendMessage(ID, user_name, msg);
                        //alert('Message sent');
                    }
                    else if (result == "DataBaseError") {
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html("<strong>Database Connectivity Error</strong>, Please check  application database connection...");
                    }
                    else if (result == "NetworkError") {
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                    }
                    else if (result == "ExceptionError") {
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html("<strong>Exception Error</strong>, Please Check the Error Log...");
                    }
                    else {
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html(result);
                    }
                },
                error: function (xhr, status, error) {
                    $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    $("#ErrorModalID").modal('show');
                    $("#ErrormodeltextId").html('Error - ' + errorMessage);
                }
            })
        }
    }
    function handleError(result) {
        if (result === "DataBaseError") {
            $("#ErrorModalID").modal('show');
            $("#ErrormodeltextId").html("<strong>Database Connectivity Error</strong>, Please check application database connection...");
        } else if (result === "NetworkError") {
            $("#ErrorModalID").modal('show');
            $("#ErrormodeltextId").html("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
        } else if (result === "ExceptionError") {
            $("#ErrorModalID").modal('show');
            $("#ErrormodeltextId").html("<strong>Exception Error</strong>, Please Check the Error Log...");
        } else {
            $("#ErrorModalID").modal('show');
            $("#ErrormodeltextId").html(result);
        }
    }
});