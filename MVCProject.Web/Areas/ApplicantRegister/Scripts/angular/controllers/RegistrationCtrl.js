(function () {
    'use strict';

    angular.module("MVCApp").controller('RegistrationCtrl', [
        '$scope', 'RegistrationService', RegistrationCtrl
    ]);

    function RegistrationCtrl($scope, RegistrationService) {
        $scope.data = {
            Id: 0,
            Name: '',
            Email: '',
            Phone: '',
            Address: '',
            IsActive: true
        };
        $scope.getAllApplicants = function () {
            RegistrationService.GetAllApplicants().then(function (res) {
                $scope.applicants = res.data;
            });
        }
        $scope.ClearFormData = function (frmRegister) {
            $scope.data = {
                DesignationId: 0,
                DesignationName: '',
                IsActive: true
            };
            frmRegister.$setPristine();
            $("#txtName").focus();
        };
        $scope.SaveApplicantDetails = function (data) {
            RegistrationService.Register(data).then(function (res) {
                if (res) {
                    var applicants = res.data;
                    if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                        toastr.success(data.Message, successTitle);
                        location.reload();
                    } else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    } else if (data.MessageType == messageTypes.Warning) {// Warning
                        toastr.warning(data.Message, warningTitle);
                    }
                }
            });
        }
    }
})();
