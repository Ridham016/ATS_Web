(function () {
    'use strict';

    angular.module("MVCApp").controller('AccountCtrl', [
        '$scope', '$rootScope', '$uibModal', '$timeout', '$window','$interval', 'AccountService', 'CommonFunctions', 'CommonService', AccountCtrl
        ]);

    //BEGIN AccountCtrl
    function AccountCtrl($scope, $rootScope, $uibModal, $timeout, $window, $interval, AccountService, CommonFunctions, CommonService) {

        $scope.applicationLogo = "/Content/images/company-logo.png";
        $scope.isLogoLoaded = true;
        $scope.showForgotPassword = true;
        $scope.showOtp = false;
        $scope.showResetPassword = false;


        $scope.user = {
            Email: ''
        };
        $scope.otp = {};

        //BEGIN Check login
        $scope.CheckLogin = function (isSessionExpired) {
            if (isSessionExpired) {
                toastr.warning(sessionHasBeenExpiredMsg ? sessionHasBeenExpiredMsg : '', warningTitle);
            }
            localStorage.setItem("logout", true);
            localStorage.removeItem("logout");

            var userdata = CommonFunctions.GetCookie("REM", userdata);
            if (userdata) {
                userdata = CommonFunctions.DecryptData(userdata);
                userdata = userdata.split('░');
                if (userdata.length > 1) {
                    $scope.loginFormData = {
                        Email: userdata[0],
                        Password: userdata[1],
                        Remember: true
                    }
                }
            } else {
                $scope.loginFormData = { Remember: false };
            }
        };
        //End Check login

        //BEGIN Do Login
        $scope.DoLogin = function (Login, frmLogin) {
            if (frmLogin.$invalid) {
                if (frmLogin.txtUserName.$error.required) {
                    frmLogin.txtUserName.$dirty = true;
                    $("#txtUserName").focus();
                } else if (frmLogin.txtPassword.$error.required) {
                    frmLogin.txtPassword.$dirty = true;
                    $("#txtPassword").focus();
                }
                return;
            }
            //debugger
            Login.TimeZoneMinutes = CommonFunctions.GetTimeZoneMinutes();
            Login.Password = CommonFunctions.EncryptData(Login.Password);
            console.log(Login.Password);
            AccountService.DoLogin(Login).then(function (res) {
                //debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                        CommonService.CreateSession(data.Result).then(function (response) {
                            //debugger
                            console.log(data.Result);
                            $rootScope.isAjaxLoadingChild = true;
                            if (Login.Remember) {
                                Login.Password = CommonFunctions.DecryptData(Login.Password);
                                var userdata = Login.Email + "░" + Login.Password;
                                userdata = CommonFunctions.EncryptData(userdata);
                                CommonFunctions.SetCookie("REM", userdata);
                            } else {
                                CommonFunctions.SetCookie("REM", "");
                            }
                            //debugger
                            CommonFunctions.RedirectToDefaultUrl();
                        });
                    } else {
                        Login.Password = CommonFunctions.DecryptData(Login.Password);
                        $("#txtUserName").focus();
                        toastr.error(data.Message, errorTitle);
                    }
                }
            });
        };
        //END Do Login


        //Forgot Password

        $scope.passwordModel = {
            NewPassword: "",
            ConfirmPassword: ""
        };
        $scope.showResend = false;

        $scope.SendResetPasswordMail = function (frmForgotPassword,user) {
            if (frmForgotPassword.$valid) {
                AccountService.GenerateCode(user).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success) {
                            toastr.success(data.Message, successTitle);
                            $scope.showForgotPassword = false;
                            $scope.showOtp = true;
                            $scope.UserId = res.data.Result;
                            $scope.CountDown($scope.showOtp);
                            document.getElementById('first').focus();
                        } else {
                            toastr.error(data.Message, errorTitle);
                        }
                    }
                });
            }
        };

        $scope.onPaste = function (event, index) {
            event.preventDefault();
            console.log(event);
            const pastedText = (event.originalEvent || event).clipboardData.getData('text').trim();
            console.log(pastedText);
            $scope.otp.first = pastedText[0];
            $scope.otp.second = pastedText[1];
            $scope.otp.third = pastedText[2];
            $scope.otp.fourth = pastedText[3];
            $scope.otp.fifth = pastedText[4];
            $scope.otp.sixth = pastedText[5];
            $scope.validateOtp($scope.otp);
            document.getElementById('sixth').focus();
        };


        $scope.ResendCode = function () {
            AccountService.GenerateCode($scope.user).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        toastr.success(data.Message, successTitle);
                        $scope.showForgotPassword = false;
                        $scope.showOtp = true;
                        $scope.UserId = res.data.Result;
                        $scope.CountDown($scope.showOtp);
                        $scope.showResend = false;
                        document.getElementById('first').focus();
                    } else {
                        toastr.error(data.Message, errorTitle);
                    }
                }
            });
        }

        $scope.validateOtp = function (otp) {
            if (otp.sixth != null) {
                var otpFull = otp.first + otp.second + otp.third + otp.fourth + otp.fifth + otp.sixth;
                AccountService.ValidateOtp(otpFull).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success) {
                            toastr.success(data.Message, successTitle);
                            $scope.showForgotPassword = false;
                            $scope.showOtp = false;
                            $scope.showResetPassword = true;
                        } else {
                            toastr.error(data.Message, errorTitle);
                        }
                    }
                });
            }
        }

        $scope.ResetPassword = function (frmResetPassword) {
            if (frmResetPassword.$valid) {
                $scope.passwordModel.NewPassword = CommonFunctions.EncryptData($scope.passwordModel.NewPassword);
                var model = {
                    UserId: $scope.UserId,
                    Password: $scope.passwordModel.NewPassword
                };

                AccountService.ResetPassword(model).then(function (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        sessionStorage.setItem("resetPasswordSuccessMessage", data.Message);
                        window.location.href = "/account/login";
                    } else {
                        toastr.error(data.Message, errorTitle);
                    }
                });
            }
        };
        $scope.CountDown = function (showOtp) {
            if (showOtp == true) {
                $scope.countdown = "01:00";

                var totalSeconds = 60;
                var intervalPromise = $interval(function () {
                    totalSeconds--;
                    $scope.countdown = formatTime(totalSeconds);
                    if (totalSeconds === 0) {
                        $interval.cancel(intervalPromise);
                        $scope.showResend = true;
                    }
                }, 1000);

                function formatTime(totalSeconds) {
                    var minutes = Math.floor(totalSeconds / 60);
                    var seconds = totalSeconds % 60;
                    return padZero(minutes) + ":" + padZero(seconds);
                }

                function padZero(num) {
                    return (num < 10 ? "0" : "") + num;
                }
            }
        }

        //End Forgot Password
        $scope.OpenResetPasswordModel = function () {
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'template/ResetPasswordModel.html',
                controller: "ResetPasswordModel",
                size: 'md',
                keyboard: true,
                backdrop: 'static',
                windowTopClass: "modal-center-override",
                resolve: {}
            });
        };

        // Init
        $scope.Init = function () {
            if (sessionStorage.getItem("resetPasswordSuccessMessage")) {
                toastr.success(sessionStorage.getItem("resetPasswordSuccessMessage"), successTitle);
                sessionStorage.removeItem("resetPasswordSuccessMessage");
            }
        } ();
    }
    //END AccountCtrl

    angular.module("MVCApp").controller('ResetPasswordModel', [
           "$scope", "$rootScope", "$filter", "$uibModalInstance", 'AccountService', ResetPasswordModel
        ]);

    //BEGIN AccountCtrl
    function ResetPasswordModel($scope, $rootScope, $filter, $uibModalInstance, AccountService) {
        $scope.user = { UserName: '' };

        $scope.SendResetPasswordMail = function (frmResetPassword) {
            if (frmResetPassword.$valid) {
                AccountService.SendResetPassword($scope.user).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success) {
                            toastr.success(data.Message);
                            $uibModalInstance.dismiss('cancel');
                        } else {
                            toastr.error(data.Message, errorTitle);
                        }
                    }
                });
            }
        };

        $scope.cancelActionToPerform = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();