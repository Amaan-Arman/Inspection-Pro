var app = angular.module('Homeapp', []);
app.controller('LoginController', function ($scope, $http) {

    localStorage.setItem('URLIndex', '/Home/')
    //localStorage.setItem('URLCCS', '/Visitors/')
    $scope.LoginFunction = function (formId) {
        //$(".imageLoadinng").show();
        debugger
        var login_id = $("#login_id").val();
        var Password_id = $("#Password_id").val();

        if (login_id == "") {
            $("#login_id_errormessage").html("Login id is a Mandatory Requirement, Please Put it..");
            $("#login_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $(".imageLoadinng").hide();
        }
        else if (Password_id == "") {
            $("#Password_id_errormessage").html("Password is a Mandatory Requirement, Please Put it..");
            $("#Password_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
            $(".imageLoadinng").hide();
        }
        else {
            var data = $(formId).serialize();

            $.ajax({
                type: "POST",
                url: localStorage.getItem('URLIndex') + 'LoginAndPassword',
                data: data,
                success: function (result) {
                    debugger
                    //$(".imageLoadinng").hide();

                    if (result == "Invalid Login Id") {
                        $("#login_id_errormessage").html("Invalid Login Id");
                        $("#login_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
                    }
                    else if (result == "Invalid Password Id") {
                        $("#Password_id_errormessage").html("Invalid Password");
                        $("#Password_id").css({ "border-color": "#fb7c72", "border-weight": "5px", "border-style": "solid" });
                    }
                    else if (result == "Admin") {
                        window.location.href = localStorage.getItem('URLIndex') + ""
                    }
                    else if (result == "Inspector") {
                        window.location.href = localStorage.getItem('URLIndex') + ""
                    }
                    //else if (result == "Cashier") {
                    //    window.location.href = localStorage.getItem('URLIndex') + "CashCounter"
                    //}
                    else {
                        alert(result)
                    }
                },
                error: function (xhr, status, error) {
                    //$(".imageLoadinng").hide();
                    var errorMessage = xhr.status + ': ' + xhr.statusText
                    alert('Error - ' + errorMessage);
                }
            })
        }
    }





});


