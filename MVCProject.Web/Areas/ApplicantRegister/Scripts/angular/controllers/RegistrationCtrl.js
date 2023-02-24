(function () {
    'use strict';

    angular.module("MVCApp").controller('RegistrationCtrl', [
        '$scope', 'CommonFunctions', 'RegistrationService', RegistrationCtrl
    ]);

    function RegistrationCtrl($scope, CommonFunctions, RegistrationService) {
        $scope.applicantDetailScope = {
            ApplicantId: 0,
            Name: '',
            Email: '',
            Phone: '',
            Address: '',
            DateOfBirth: null,
            CurrentCompany: '',
            CurrentDesignations: '',
            TotalExperience: null,
            DetailedExperince: null,
            CurrentCTC: null,
            ExpectedCTC: null,
            NoticePeriod: null,
            CurrentLocation: '',
            PreferedLocation: '',
            ReasonForChange : '',
            IsActive: true
        };

        $scope.getApplicants = function (IsGetAll) {
            if (IsGetAll) {
                debugger
                $scope.getApplicantList(IsGetAll);
            }
            else {
                debugger
                $scope.getAllApplicants();
            }
        }

        $scope.getAllApplicants = function () {
            RegistrationService.GetAllApplicants().then(function (res) {
                $scope.applicants = res.data.Result;
                console.log($scope.applicants);
                debugger
            });
        };

        $scope.getApplicantList = function (isGetAll) {
            RegistrationService.GetApplicantList(isGetAll).then(function (res) {
                $scope.applicants = res.data.Result;
            });
        }

        $scope.ClearFormData = function (frmRegister) {
            $scope.applicantDetailScope = {
                ApplicantId: 0,
                Name: '',
                Email: '',
                Phone: '',
                Address: '',
                DateOfBirth: null,
                CurrentCompany: '',
                CurrentDesignations: '',
                IsActive: true
            };
            frmRegister.$setPristine();
            $("Name").focus();
        };
        $scope.SaveApplicantDetails = function (applicantDetailScope) {
            RegistrationService.Register(applicantDetailScope).then(function (res) {
                if (res) {
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
            RegistrationService.GetApplicantsById(ApplicantId).then(function (res) {
                if (res) {
                    $scope.applicantDetailScope = res.data.Result;
                    $scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth);
                    CommonFunctions.ScrollUpAndFocus("Name");
                }
            })
        }
    }
})();