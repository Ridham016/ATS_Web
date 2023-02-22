(function () {
    'use strict';

    angular.module("MVCApp").controller('RegistrationCtrl', [
        '$scope', 'RegistrationService', RegistrationCtrl
    ]);

    function RegistrationCtrl($scope, RegistrationService) {
        $scope.applicantDetailScope = {
            ApplicantId: 0,
            Name: '',
            Email: '',
            Phone: '',
            Address: '',
            IsActive: true,
            EntryDate: new Date(),
            UpdateDate: new Date()
        };
        $scope.getAllApplicants = function () {
            RegistrationService.GetAllApplicants().then(function (res) {
                $scope.applicants = res.data;
            });
        };

        $scope.ClearFormData = function (frmRegister) {
            $scope.applicantDetailScope = {
                ApplicantId: 0,
                Name: '',
                Email: '',
                Phone: '',
                Address: '',
                IsActive: true,
                EntryDate: new Date(),
                UpdateDate: new Date()
            };
            frmRegister.$setPristine();
            $("Name").focus();
        };
        $scope.SaveApplicantDetails = function (applicantDetailScope) {
            debugger
            RegistrationService.Register(applicantDetailScope).then(function (res) {
                if (res) {
                    debugger
                    var applicants = res.data;
                    if (applicants.MessageType == messageTypes.Success && applicants.IsAuthenticated) {
                        toastr.success(applicants.Message, successTitle);
                        location.reload();
                    } else if (applicants.MessageType == messageTypes.Error) {// Error
                        toastr.error(applicants.Message, errorTitle);
                    } else if (applicants.MessageType == messageTypes.Warning) {// Warning
                        toastr.warning(applicants.Message, warningTitle);
                    }
                }
            });
        }
        $scope.EditApplicantDetails = function (ApplicantId) {
            debugger
            RegistrationService.GetApplicantsById(ApplicantId).then(function (res) {
                if (res) {
                    $scope.applicantDetailScope = res.data.Result;
                }
            })
        }
    }
})();