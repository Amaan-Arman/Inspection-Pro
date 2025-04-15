var app = angular.module('Homeapp', []);
app.controller('ProductController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Home/')
    const notiSound = new Audio('/admin_assets/sound/notification.wav');

    localStorage.setItem('Search', "done");

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


    $http.get(localStorage.getItem('URLIndex') + 'GetProductList').then(function (i) {
        debugger
        $scope.GetProductList = i.data;
    },
    function (error) {
        alert(error);
        $scope.GetProductList = error;
    });

    $scope.ProductInsertionFunction = function () {
        debugger
        $('.page-loader-wrapper').fadeIn();

        var SerialNumber = $("#SerialNumber_ID").val();
        var Location_ID = $("#Location_ID").val();
        var Type_ID = $("#Type_ID").val();
        var Capacity_ID = $("#Capacity_ID").val();
        var manufacturer_ID = $("#manufacturer_ID").val();
        var Date_of_Manufacture = $("#Date_of_Manufacture").val();
        //var Last_Inspection_Date = $("#Last_Inspection_Date").val();
        //var Next_Inspection_Date = $("#Next_Inspection_Date").val();
        //var Maintenance_Schedule = $("#Maintenance_Schedule").val();
        //var Status = $("#Status").val();
        var Remarks = $("#Remarks").val();
        
        //var Item_picture = $("#Item_picture").get(0).files;
        //var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
        
        if (SerialNumber == "") {
            $('.page-loader-wrapper').fadeOut();
            $("#SerialNumber_error_ID").html("<strong>Serial Number </strong> is a Mandatory, Please Put it..");
            $("#SerialNumber_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Location_ID == "") {
                        $('.page-loader-wrapper').fadeOut();
            $("#Location_ID_error_id").html("<strong>Location </strong> is a Mandatory, Please Put it..");
            $("#Location_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (manufacturer_ID == "") {
                        $('.page-loader-wrapper').fadeOut();
            $("#manufacturer_error_ID").html("<strong> manufacturer</strong> is a Mandatory, Please Put it..");
            //$(modelID).modal('show');
            $("#manufacturer_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Type_ID == "") {
                        $('.page-loader-wrapper').fadeOut();
            $("#Type_ID_error_Id").html("<strong>Type</strong> is a Mandatory, Please Put it..");
            //$(modelID).modal('show');
            $("#Type_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Capacity_ID == "") {
                        $('.page-loader-wrapper').fadeOut();
            $("#Capacity_ID_error_ID").html("<strong>Capacity </strong> is a Mandatory, Please Put it..");
            //$(modelID).modal('show');
            $("#Capacity_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Date_of_Manufacture == "") {
                        $('.page-loader-wrapper').fadeOut();
            $("#Date_of_Manufacture_error_ID").html("<strong>Date_of_Manufacture</strong> is a Mandatory, Please Put it..");
            //$(modelID).modal('show');
            $("#Date_of_Manufacture").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        //else if (Last_Inspection_Date == "") {
        //    $("#Last_Inspection_Date_error_ID").html("<strong>Last_Inspection_Date</strong> is a Mandatory, Please Put it..");
        //    //$(modelID).modal('show');
        //    $("#Last_Inspection_Date").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        //}
        //else if (Next_Inspection_Date == "") {
        //    $("#Next_Inspection_Date_error_ID").html("<strong>Last_Inspection_Date</strong> is a Mandatory, Please Put it..");
        //    //$(modelID).modal('show');
        //    $("#model_ID").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        //}
        //else if (Maintenance_Schedule == "") {
        //    $("#Maintenance_Schedule_error_ID").html("<strong>Maintenance_Schedule</strong> is a Mandatory, Please Put it..");
        //    //$(modelID).modal('show');
        //    $("#Maintenance_Schedule").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        //}
        //else if (Status == "") {
        //                $('.page-loader-wrapper').fadeOut();
        //    $("#Status_error_ID").html("<strong>Status</strong> is a Mandatory, Please Put it..");
        //    //$(modelID).modal('show');
        //    $("#Status").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        //}
        else if (Remarks == "") {
            $("#Remarks_error_ID").html("<strong>Remarks</strong> is a Mandatory, Please Put it..");
            //$(modelID).modal('show');
            $("#Remarks").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else {
            //    debugger
            //var Location_ = $("#Location_ID").find("option:selected").text();

            var data = new FormData;
            data.append("SerialNumber", SerialNumber);
            data.append("Location_", Location_ID);
            data.append("Manufacturer", manufacturer_ID);
            data.append("Type", Type_ID);
            data.append("Capacity", Capacity_ID);
            data.append("Date_of_Manufacture", Date_of_Manufacture);
            //data.append("Last_Inspection_Date", Last_Inspection_Date);
            //data.append("Next_Inspection_Date", Next_Inspection_Date); 
            //data.append("Maintenance_Schedule", Maintenance_Schedule); 
            //data.append("Status", Status);
            data.append("Remarks", Remarks);

            $.ajax({
                type: "Post",
                url: localStorage.getItem('URLIndex') + 'ProductInsertionAndUpdation',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger
                    if (result == "Saved")
                    {
                        $('.page-loader-wrapper').fadeOut();
                        $http.get(localStorage.getItem('URLIndex') + 'GetProductList').then(function (i) {
                            debugger
                            $scope.GetProductList = i.data;
                        },
                            function (error) {
                                alert(error);
                                $scope.GetProductList = error;
                            });

                        $("#SerialNumber_ID").get(0).value = "";
                        $("#Location_ID").get(0).value = "";
                        $("#Type_ID").get(0).value = "";
                        $("#Capacity_ID").get(0).value = "";
                        $("#manufacturer_ID").get(0).value = "";
                        //$("#Date_of_Manufacture").get(0).value = "";
                        //$("#Last_Inspection_Date").get(0).value = "";
                        //$("#Next_Inspection_Date").get(0).value = "";
                        //$("#Maintenance_Schedule").get(0).value = "";
                        //$("#Status").get(0).value = "";
                        $("#Remarks").get(0).value = "";

                        //$("#product_id").get(0).value = result;
                        $http.get(localStorage.getItem('URLIndex') + 'GetProductList').then(function (i) {
                            debugger
                            $scope.GetProductList = i.data;
                        },
                            function (error) {
                                alert(error);
                                $scope.GetProductList = error;
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
                    //$(".loader").hide();
                        $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }

    $scope.ReportListFunction = function (ID, Heading) {
        debugger
        $('.page-loader-wrapper').fadeIn();
        localStorage.setItem('ProductID', ID)

        $.ajax({
            type: "POST",
            url: localStorage.getItem('URLIndex') + 'Report_List',
            data: { ID: ID },
            success: function (result) {
                debugger
                $('.page-loader-wrapper').fadeOut();

                $("#inspectionreportHeading").text(Heading);
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
    $scope.ShowQRCode = function (serial_no, Heading) {
        debugger
        $('.page-loader-wrapper').fadeIn();

        $("#QRCodeHeading").text(Heading);
        $("#QRCodeModalBodyId").html('<div class= "container"><div class="row"><div style="height:150px;" class="col d-flex justify-content-center"><img class="img" src="/QRCodeImages/'+serial_no+'.png" /></div></div></div >');
        $("#QRCodeModalID").modal('show');
        $('.page-loader-wrapper').fadeOut();

        //$.ajax({
        //    type: "POST",
        //    url: localStorage.getItem('URLIndex') + 'ReportList',
        //    data: { ID: ID },
        //    success: function (result) {
        //        debugger
        //        $('.page-loader-wrapper').fadeOut();
        //        $("#inspectionreportHeading").text(Heading);
        //        $("#inspectionreportModalBodyId").html(result);
        //        $("#inspectionreportModalID").modal('show');
        //    },
        //    error: function (xhr, status, error) {
        //        debugger
        //        $('.page-loader-wrapper').fadeOut();
        //        var errorMessage = xhr.status + ': ' + xhr.statusText
        //        alert('Error - ' + errorMessage);
        //    }
        //})
    }
    $scope.StartInspectionFuntion = function () {
        debugger
        $('.page-loader-wrapper').fadeIn();

        window.open('Inspection_Form/'+ localStorage.getItem('ProductID'), '_blank');
        $('.page-loader-wrapper').fadeOut();
    }


    //Forward
    $scope.IspectorForwardFunction = function (modelText, modelID, Heading) {
        debugger

        var test = new Array();
        $("input[name='Checkbox']:checked").each(function () {
            test.push($(this).val());
        });

        localStorage.setItem('Inspector_Forward_ID', JSON.stringify(test));

        if (test == "") {
            showWithTitleMessage("Please select Product.. !");
        }
        else {
            $('.page-loader-wrapper').fadeIn();

            var data = new FormData;
            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'inspectionForwardView',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger
                    $('.page-loader-wrapper').fadeOut();

                    $("#InspectorforwardHeading").text(Heading);
                    $(modelText).html(result);
                    $(modelID).modal('show');
                },
                error: function (xhr, status, error) {
                     $('.page-loader-wrapper').fadeOut();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }
    $scope.SetInspectorForwardFunction = function (modelID) {
        debugger
        $('.page-loader-wrapper').fadeIn();

        const ID = JSON.parse(localStorage.getItem('Inspector_Forward_ID'));

        var cc_Forward_to = $("#cc_Forward_to").val();
        var Set_inspection_date = $("#Set_inspection_date").val();

        if (cc_Forward_to == "") {
            $('.page-loader-wrapper').fadeOut();
            $("#cc_Forward_to_errormessage").html("<strong> Name</strong> is a Mandatory Requirement, Please Put it");
            $("#cc_Forward_to").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Set_inspection_date == "") {
            $('.page-loader-wrapper').fadeOut();
            $("#set_inspection_date_errormessage").html("<strong>Date</strong> is a Mandatory Requirement, Please Put it");
            $("#Set_inspection_date").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else {
            var data = new FormData;
            var cc_Forward_to_title_edit = $("#cc_Forward_to").find("option:selected").text();

            data.append("inspector_id", cc_Forward_to);
            data.append("inspector_name", cc_Forward_to_title_edit);
            data.append("Set_inspection_date", Set_inspection_date);

            for (var j = 0; j < ID.length; j++) {
                data.append("checkbox", ID[j]);
            }

            $.ajax(
                {
                    url: localStorage.getItem('URLIndex') + 'setinspectionForward',
                    type: "POST",
                    data: data,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        if (result == "Saved") {
                            debugger
                            $('.page-loader-wrapper').fadeOut();
                            $(modelID).modal('hide');
                            chatHub.server.sendMessage(cc_Forward_to, cc_Forward_to_title_edit);

                            $http.get(localStorage.getItem('URLIndex') + 'GetProductList').then(function (i) {
                                debugger
                                $scope.GetProductList = i.data;
                            },
                                function (error) {
                                    alert(error);
                                    $scope.GetProductList = error;
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
                            //alert("Session has been expired, Please login again...")
                            showWithTitleMessage(result);
                        }
                    },
                    error: function (xhr, status, error) {
                        $('.page-loader-wrapper').fadeOut();
                        var errorMessage = xhr.status + ': ' + xhr.statusText
                        alert('Error - ' + errorMessage);
                        //$("#ErrorModalID").modal('show');
                        //$("#ErrormodeltextId").html('Error - ' + errorMessage);
                    }
                })
        }
    }
    //Forward

    function showWithTitleMessage(msg) {
        swal("Alert !", msg);
    }

    //Delete Funtion
    $scope.ProductDeleteFunction = function () {
        debugger
        var ID = new Array();
        $("input[name='Checkbox']:checked").each(function () {
            ID.push($(this).val());
        });
        if (ID == "") {
            showWithTitleMessage("Please select Product.. !");
        }
        else {
            showConfirmMessage(ID);
        }
    }
    function showConfirmMessage(ID) {
        swal({
            title: "Are you sure?",
            text: "You want to delete this Product",
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
                url: localStorage.getItem('URLIndex') + 'ProductDeletion',
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    debugger
                    if (result == 'Saved') {
                        swal("Deleted!", "Your file has been deleted.", "success");

                        if (localStorage.getItem('Search') == "Search") {
                            $http.get(localStorage.getItem('URLIndex') + "ProductSearchList?id=" + localStorage.getItem('id') + "&start_date_id=" + localStorage.getItem('start_date_id') + "&end_date_id=" + localStorage.getItem('end_date_id')).then(function (cp) {
                                debugger
                                $scope.ProductSearchList = cp.data;
                            },
                            function (error) {
                                alert(error)
                                $scope.ProductSearchList = error;
                            });
                        }
                        else {
                            $http.get(localStorage.getItem('URLIndex') + 'GetProductList').then(function (i) {
                                debugger
                                $scope.GetProductList = i.data;
                            },
                            function (error) {
                                alert(error);
                                $scope.GetProductList = error;
                            });
                        }
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
                        showWithTitleMessage(result);
                    }
                },
                error: function (xhr, status, error) {
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            });

        });
    }
    //Delete Funtion

    //EditedFunction
    $scope.ProductEditedFunction = function (modelText, modelID, Heading) {
        debugger

        var GetID = new Array();
        $("input[name='Checkbox']:checked").each(function () {
            GetID.push($(this).val());
        });

        var PID = JSON.stringify(GetID)
        var IDD = JSON.parse(PID)

        if (IDD.length > 1) {
            showWithTitleMessage("Select Only One Product.. !");
        }
        else if (IDD == "") {
            showWithTitleMessage("Please Select Product.. !");
        }

        else {
            var ID = IDD.toString();

            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'EditProduct',
                data: { ID: ID },
                success: function (result) {
                    debugger
                    $("#PrductHeading").text(Heading);
                    $(modelText).html(result);
                    $(modelID).modal('show');
                },
                error: function (xhr, status, error) {
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    $("#ErrorModalID").modal('show');
                    $("#ErrormodeltextId").html('Error - ' + errorMessage);
                }
            })
        }
    }
    $scope.SetProductEditedFunction = function (modelID) {
        debugger

        var Extinguisher_ID_edit = $("#Extinguisher_ID_edit").val();
        var Location_edit = $("#Location_edit").val();
        var Type_edit = $("#Type_edit").val();
        var Capacity_edit = $("#Capacity_edit").val();
        var Manufacturer_edit = $("#Manufacturer_edit").val();
        var Date_of_Manufacture_edit = $("#Date_of_Manufacture_edit").val();
        var Status_edit = $("#Status_edit").val();
        var Remarks_edit = $("#Remarks_edit").val();
        
        if (Location_edit == "") {
            $("#Location_errormessage").html("<strong> Location </strong> is a Mandatory Requirement, Please Put it");
            $("#Location_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Type_edit == "") {
            $("#Type_errormessage").html("<strong> Type</strong> is a Mandatory Requirement, Please Put it");
            $("#Type_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Capacity_edit == "") {
            $("#Capacity_errormessage").html("<strong> Capacity </strong> is a Mandatory Requirement, Please Put it");
            $("#Capacity_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Manufacturer_edit == "") {
            $("#Manufacturer_errormessage").html("<strong> Question</strong> is a Mandatory Requirement, Please Put it");
            $("#Manufacturer_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Date_of_Manufacture_edit == "") {
            $("#Date_of_Manufacture_errormessage").html("<strong> Date of Manufacture </strong> is a Mandatory Requirement, Please Put it");
            $("#Date_of_Manufacture_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Status_edit == "") {
            $("#Status_errormessage").html("<strong> Status </strong> is a Mandatory Requirement, Please Put it");
            $("#Status_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else if (Remarks_edit == "") {
            $("#Remarks_errormessage").html("<strong> Remarks </strong> is a Mandatory Requirement, Please Put it");
            $("#Remarks_edit").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
        }
        else {
            var data = new FormData;
            data.append("Extinguisher_ID", Extinguisher_ID_edit);
            data.append("Location_", Location_edit);
            data.append("Type", Type_edit);
            data.append("Capacity", Capacity_edit);
            data.append("Manufacturer", Manufacturer_edit);
            data.append("Date_of_Manufacture", Date_of_Manufacture_edit);
            data.append("Status", Status_edit);
            data.append("Remarks", Remarks_edit);

            $.ajax(
                {
                    url: localStorage.getItem('URLIndex') + 'ProductInsertionAndUpdation',
                    type: "POST",
                    data: data,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        $(modelID).modal('hide');
                        if (result == "Saved") {
                            debugger
                            if (localStorage.getItem('Search') == "Search") {
                                $http.get(localStorage.getItem('URLIndex') + "ProductSearchList?id=" + localStorage.getItem('id') + "&start_date_id=" + localStorage.getItem('start_date_id') + "&end_date_id=" + localStorage.getItem('end_date_id')).then(function (cp) {
                                    debugger
                                    $scope.ProductSearchList = cp.data;
                                },
                                function (error) {
                                    alert(error)
                                    $scope.ProductSearchList = error;
                                });
                            }
                            else {
                                $http.get(localStorage.getItem('URLIndex') + 'GetProductList').then(function (i) {
                                    debugger
                                    $scope.GetProductList = i.data;
                                },
                                function (error) {
                                    alert(error);
                                    $scope.GetProductList = error;
                                });
                            }
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
                            showWithTitleMessage(result);
                        }
                    },
                    error: function (xhr, status, error) {
                        var errorMessage = xhr.status + ': ' + xhr.statusText
                        $("#ErrorModalID").modal('show');
                        $("#ErrormodeltextId").html('Error - ' + errorMessage);
                    }
                })
        }
    }
    //EditedFunction

    //SearchFunction
    $scope.ProductSearchFunction = function () {
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
            $http.get(localStorage.getItem('URLIndex') + "ProductSearchList?id=" + id + "&start_date_id=" + start_date_id + "&end_date_id=" + end_date_id).then(function (cp) {
                debugger
                $scope.ProductSearchList = cp.data;
            },
            function (error) {
                alert(error)
                $scope.ProductSearchList = error;
            });
        }
    }

    //SearchFunction
});
GetReport = function (Heading,serial_no,date,time) {
    debugger
    $('.page-loader-wrapper').fadeIn();
    var getdatetime = date + time;
    var datetime = getdatetime.replace(" ","-");
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