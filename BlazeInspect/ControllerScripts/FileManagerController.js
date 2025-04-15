var app = angular.module('Homeapp', []);
app.controller('FileManagerController', function ($scope, $http) {
   
    localStorage.setItem('URLIndex', '/Home/')


    $http.get(localStorage.getItem('URLIndex') + 'GetDocumentList').then(function (response) {
        debugger
        $scope.GroupedDocumentList = response.data;
        debugger

    }, function (error) {
        console.error(error);
    });
    $http.get(localStorage.getItem('URLIndex') + 'GetNotification').then(function (i) {
        debugger
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


    $scope.SendAttachment = function () {
        debugger
        $('.page-loader-wrapper').fadeIn();

        var attachment = $("#attachment").get(0).files;
        var fileExtension = ['jpeg','jpg','png','gif','bmp','doc','pdf'];

        if (attachment.length == 0) {
            $("#attachment_errormessage").html("Please upload Image!");
        }
        else if ($.inArray($("#attachment").val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            $("#attachment_errormessage").html("Invalid format!");
        }
        else {
            var data = new FormData;

            for (var j = 0; j < attachment.length; j++) {
                data.append("attachment", attachment[j]);
            }

            $.ajax({
                type: "Post",
                url: localStorage.getItem('URLIndex') + 'SaveFile',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger
                    $('.page-loader-wrapper').fadeOut();
                    
                    $("#addFile").modal('hide');
                    if (result == "Saved")
                    {
                        $http.get(localStorage.getItem('URLIndex') + 'GetDocumentList').then(function (response) {
                            $scope.GroupedDocumentList = response.data;
                        }, function (error) {
                            console.error(error);
                        });
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


    //Forward
    $scope.FileShareFunction = function (modelText, modelID, Heading) {
        debugger

        var test = new Array();
        $("input[name='Checkbox']:checked").each(function () {
            test.push($(this).val());
        });
        localStorage.setItem('Inspector_Forward_ID', JSON.stringify(test));

        if (test == "") {
            showWithTitleMessage("Please select File.. !");
        }
        else {
            $('.page-loader-wrapper').fadeIn();

            var data = new FormData;
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'ShareuserListView',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger
                    $('.page-loader-wrapper').fadeOut();
                    $(modelText).html(result);
                    $("#FileShareHeading").text(Heading);
                    $("#fileshareModalID").modal('show');

                },
                error: function (xhr, status, error) {
                    $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    //alert('Error - ' + errorMessage);
                    $("#ErrorModalID").modal('show');
                    $("#ErrormodeltextId").html('Error - ' + errorMessage);
                }
            })
        }
    }

    $scope.SetShareFileFunction = function (modelID) {
        debugger
        $('.page-loader-wrapper').fadeIn();

        const ID = JSON.parse(localStorage.getItem('Inspector_Forward_ID'));

        //here i want to get multi select box ids and pass thorought localStorage
        // Get selected IDs from the multi-select dropdown
        var selectedIDs = $('#userDropdown').val() || []; // If no selection, default to empty array
        //localStorage.setItem('MultiSelect_Inspector_IDs', JSON.stringify(selectedIDs)); // Save IDs to localStorage

        // Debugging output for selected IDs
        console.log('Selected IDs from Multi-select:', selectedIDs);

        //var cc_Forward_to = $("#Share_to").val();
        //if (cc_Forward_to == "") {
        //    $('.page-loader-wrapper').fadeOut();
        //    $("#cc_Forward_to_errormessage").html("<strong>Name</strong> is a Mandatory Requirement, Please Put it");
        //    $("#cc_Forward_to").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        //}
        if (selectedIDs.length === 0) {
            showWithTitleMessage("Please select an Inspector.. !");
        }
        else {
            var data = new FormData;

            //data.append("inspector_id", cc_Forward_to);
            //data.append("inspector_name", cc_Forward_to_title_edit);

            for (var j = 0; j < ID.length; j++) {
                data.append("checkbox", ID[j]);
            }
            for (var i = 0; i < selectedIDs.length; i++) {
                data.append("shared_id", selectedIDs[i]);
            }
            
            $.ajax(
                {
                    url: localStorage.getItem('URLIndex') + 'SetShareFile',
                    type: "POST",
                    data: data,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        if (result == "Saved") {
                            debugger
                            $('.page-loader-wrapper').fadeOut();
                            $(modelID).modal('hide');
                            //chatHub.server.sendMessage(cc_Forward_to, cc_Forward_to_title_edit);

                            $http.get(localStorage.getItem('URLIndex') + 'GetDocumentList').then(function (response) {
                                $scope.GroupedDocumentList = response.data;
                            }, function (error) {
                                console.error(error);
                            });

                        }
                        else if (result == "DataBaseError") {
                            $('.page-loader-wrapper').fadeOut();
                            $("#ErrorModalID").modal('show');
                            $("#ErrormodeltextId").html("<strong>Database Connectivity Error</strong>, Please check  application database connection...");
                        }
                        else if (result == "NetworkError") {
                            $('.page-loader-wrapper').fadeOut();
                            $("#ErrorModalID").modal('show');
                            $("#ErrormodeltextId").html("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                        }
                        else if (result == "ExceptionError") {
                            $('.page-loader-wrapper').fadeOut();
                            $("#ErrorModalID").modal('show');
                            $("#ErrormodeltextId").html("<strong>Exception Error</strong>, Please Check the Error Log...");
                        }
                        else {
                            $('.page-loader-wrapper').fadeOut();
                            //alert("Session has been expired, Please login again...")
                            $("#ErrorModalID").modal('show');
                            $("#ErrormodeltextId").html(result);
                        }
                    },
                    error: function (xhr, status, error) {
                        $('.page-loader-wrapper').fadeOut();
                        var errorMessage = xhr.status + ': ' + xhr.statusText
                        //alert('Error - ' + errorMessage);
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html('Error - ' + errorMessage);
                    }
                })
        }
    }
    //Forward

    function showWithTitleMessage(msg) {
        swal("Alert !", msg);
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