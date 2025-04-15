var app = angular.module('Homeapp', []);
app.controller('QuestionController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Home/')
    const notiSound = new Audio('/admin_assets/sound/notification.wav');

    //localStorage.setItem('Search', "done");

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


    $http.get(localStorage.getItem('URLIndex') + 'GetQuestionList').then(function (i) {
        debugger
        $scope.GetQuestionList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetQuestionList = error;
        });


    //Insertion Funtion
    $scope.QuestionInsertionFunction = function () {
        debugger
        $('.page-loader-wrapper').fadeIn();

        var Question_txt = $("#Question_ID").val();
        var First_ID = $("#First_ID").val();
        var Second_ID = $("#Second_ID").val();
        
        if (Question_txt == "") {
            $("#Question_txt").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            showWithTitleMessage(" Question is mandatory");
        }
        else if (First_ID == "") {
            showWithTitleMessage("option is mandatory");
            $("#First_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Second_ID == "") {
            showWithTitleMessage("option is mandatory ");
            $("#Second_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else {
            //debugger
            var data = new FormData;
            data.append("QuestionText", Question_txt);
            data.append("first_opt", First_ID);
            data.append("second_opt", Second_ID);

            $.ajax({
                type: "Post",
                url: localStorage.getItem('URLIndex') + 'QuestionInsertionAndUpdation',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger
                    if (result == "Saved")
                    {
                        $('.page-loader-wrapper').fadeOut();
                        $http.get(localStorage.getItem('URLIndex') + 'GetQuestionList').then(function (i) {
                            debugger
                            $scope.GetQuestionList = i.data;
                        },
                        function (error) {
                            alert(error);
                            $scope.GetQuestionList = error;
                        });
                    }
                    else if (result == "DataBaseError") {
                            $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Database Connectivity Error</strong>, Please check  application database connection...");

                    }
                    else if (result == "NetworkError") {
                            $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                    }
                    else if (result == "ExceptionError") {
                            $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Exception Error</strong>, Please Check the Error Log...");
                    }
                    else
                    {
                        $('.page-loader-wrapper').fadeOut();
                        alert(result);
                    }
                },
                error: function (xhr, status, error) {
                    $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }
    //Insertion Funtion

    function showWithTitleMessage(msg) {
        swal("Alert !", msg);
    } 
    //Delete Funtion
    $scope.QDeleteFunction = function ()
    {
        debugger
        var ID = new Array();
        $("input[name='Checkbox']:checked").each(function () {
            ID.push($(this).val());
        });
        if (ID == "") {
            showWithTitleMessage("Please select Question.. !");
        }
        else {
            showConfirmMessage(ID);
        }
    }
    function showConfirmMessage(ID) {
        swal({
            title: "Are you sure?",
            text: "You want to delete this Question",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#dc3545",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        }, function () {
            debugger

            var data = new FormData;
            for (var j = 0; j < ID.length; j++) {
                data.append("checkbox", ID[j]);
            }
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'QuestionDeletion',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    $(".loader").hide();
                    $(".imageLoadinng").hide();
                    debugger
                    if (result == 'Saved') {
                        swal("Deleted!", "Your file has been deleted.", "success");

                        $http.get(localStorage.getItem('URLIndex') + 'GetQuestionList').then(function (i) {
                            debugger
                            $scope.GetQuestionList = i.data;
                        },
                            function (error) {
                                alert(error);
                                $scope.GetQuestionList = error;
                            });
                    }
                    else if (result == "DataBaseError") {
                            $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Database Connectivity Error</strong>, Please check  application database connection...");

                    }
                    else if (result == "NetworkError") {
                            $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");
                    }
                    else if (result == "ExceptionError") {
                            $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage("<strong>Exception Error</strong>, Please Check the Error Log...");
                    }
                    else {
                            $('.page-loader-wrapper').fadeOut();
                        showWithTitleMessage(result);
                    }
                },
                error: function (xhr, status, error) {
                            $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            });

        });
    }  
    //Delete Funtion

    //EditedFunction
    $scope.QuestionEditedFunction = function (modelText, modelID, Heading) {
        debugger
        
        var GetID = new Array();
        $("input[name='Checkbox']:checked").each(function () {
            GetID.push($(this).val());
        });

        var PID = JSON.stringify(GetID)
        var IDD = JSON.parse(PID)

        if (IDD.length > 1) {
            showWithTitleMessage("Select Only One Question.. !");
        }
        else if (IDD == "") {
            showWithTitleMessage("Please Select Question.. !");
        }

        else {
            var ID = IDD.toString();

            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'EditQuestion',
                data: { ID: ID },
                success: function (result) {
                    debugger
                    $("#QuestionHeading").text(Heading);
                    $(modelText).html(result);
                    $(modelID).modal('show');
                },
                error: function (xhr, status, error) {
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }
    $scope.SetQuestionEditedFunction = function (modelID) {
        debugger

        var question_edit_id = $("#question_edit_id").val();
        var Question_edited = $("#question_edit").val();
        var first_edit = $("#first_edit").val();
        var second_edit = $("#second_edit").val();

        if (Question_edited == "") {
            $("#question_errormessage").html("<strong> Question</strong> is a Mandatory Requirement, Please Put it");
            $("#question_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (first_edit == "") {
            $("#first_edit_errormessage").html("<strong> Question</strong> is a Mandatory Requirement, Please Put it");
            $("#first_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (second_edit == "") {
            $("#second_edit_errormessage").html("<strong> Question</strong> is a Mandatory Requirement, Please Put it");
            $("#second_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else {
            var data = new FormData;
            data.append("QuestionID", question_edit_id);
            data.append("QuestionText", Question_edited);
            data.append("first_opt", first_edit);
            data.append("second_opt", second_edit);

            $.ajax(
                {
                    url: localStorage.getItem('URLIndex') + 'QuestionInsertionAndUpdation',
                    type: "POST",
                    data: data,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        $(modelID).modal('hide');
                        if (result == "Saved") {
                            debugger

                            $http.get(localStorage.getItem('URLIndex') + 'GetQuestionList').then(function (i) {
                                debugger
                                $scope.GetQuestionList = i.data;
                            },
                                function (error) {
                                    alert(error);
                                    $scope.GetQuestionList = error;
                                });
                        }
                        else if (result == "DataBaseError") {
                            $('.page-loader-wrapper').fadeOut();
                            showWithTitleMessage("<strong>Database Connectivity Error</strong>, Please check  application database connection...");
                        }
                        else if (result == "NetworkError") {
                            $('.page-loader-wrapper').fadeOut();
                            showWithTitleMessage("<strong>Internet Connectivity Error</strong>, Please check the Internet connection...");

                        }
                        else if (result == "ExceptionError") {
                            $('.page-loader-wrapper').fadeOut();
                            showWithTitleMessage("<strong>Exception Error</strong>, Please Check the Error Log...");

                        }
                        else {
                            $('.page-loader-wrapper').fadeOut();
                            showWithTitleMessage(result);
                        }
                    },
                    error: function (xhr, status, error) {
                            $('.page-loader-wrapper').fadeOut();
                        var errorMessage = xhr.status + ': ' + xhr.statusText
                        alert('Error - ' + errorMessage);
                    }
                })
        }
    }
    //EditedFunction
});




