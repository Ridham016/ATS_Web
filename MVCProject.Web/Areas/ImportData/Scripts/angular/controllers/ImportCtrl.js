

(function () {
    'use strict';

    angular.module("MVCApp").controller('ImportCtrl', [
        '$scope', '$http', 'ngTableParams', '$rootScope', 'CommonFunctions', 'ImportService', ImportCtrl
    ]);

    function ImportCtrl($scope, $http, ngTableParams, $rootScope, CommonFunctions, ImportService) {
        debugger

        $scope.applicantDetailScope = {
            ApplicantId: 0,
            FirstName: '',
            MiddleName: '',
            LastName: '',
            Email: '',
            Phone: '',
            Address: '',
            DateOfBirth: null,
            CurrentCompany: '',
            CurrentDesignation: '',
            TotalExperience: '',
            DetailedExperience: '',
            CurrentCTC: '',
            ExpectedCTC: '',
            NoticePeriod: '',
            CurrentLocation: '',
            PreferedLocation: '',
            ReasonForChange: '',
            SkillDescription: '',
            PortfolioLink: '',
            LinkedinLink: '',
            OtherLink: '',
            ExpectedJoiningDate: null,
            IsActive: true
        };
        $scope.applicant = [];
        $scope.errors = '';

        $scope.uploadFile = function () {
            //debugger
            var fileInput = document.getElementById('file');
            if (fileInput.files.length === 0) return;

            var file = fileInput.files[0];

            var payload = new FormData();
            payload.append("file", file);
            ImportService.uploadFile(payload).then(function (response) {
                debugger
                if (response) {
                    if (response.data.MessageType == messageTypes.Success) {
                        toastr.success(response.data.Message, successTitle);
                        debugger
                        $scope.applicant = response.data.Result.Data;
                        $scope.errors = response.data.Result.Errors;
                        if ($scope.errors) {
                            for (var i = 0; i < $scope.errors.length; i++) {
                                var errorRow = parseInt($scope.errors[i].split(":")[0]);
                                var errorMessage = $scope.errors[i].split(":")[1];
                                $scope.applicant[errorRow - 1].Error = errorMessage;
                                console.log($scope.errors);
                            }
                        }
                    }
                    else if (response.data.MessageType == messageTypes.Error) {
                        $scope.applicant = response.data.Result.Data;
                        $scope.errors = response.data.Result.Errors;
                        if ($scope.errors) {
                            for (var i = 0; i < $scope.errors.length; i++) {
                                var errorRow = parseInt($scope.errors[i].split(":")[0]);
                                var errorMessage = $scope.errors[i].split(":")[1];
                                $scope.applicant[errorRow - 1].Error = errorMessage;
                                console.log($scope.errors);
                            }
                        }
                        toastr.error(response.data.Message, errorTitle);
                    }
                    else {
                        $scope.applicant = response.data.Result.Data;
                    }
                }
                else {
                    toastr.error("No Response", errorTitle);
                }
                
            });
           

        };
        $scope.submitApplicants = function (applicant) {
            //debugger
            ImportService.AddApplicants($scope.applicant).then(function (response) {
                // Handle success response
                if (response.data.MessageType == messageTypes.Success) {
                    toastr.success(response.data.Message, successTitle);
                    console.log(response.data.Result);
                }
                else {
                    toastr.error(response.Message, errorTitle);
                }
            }, function (error) {
                toastr.error(response.Message, errorTitle);
                // Handle error response
                console.log(error.data);
            });
        };

        $scope.getSampleExcel = function () {
            ImportService.GetSample($scope.applicant).then(function (response) {
                var data = new Blob([response.data], { type: 'application/octet-stream' });
                var downloadLink = document.createElement('a');
                downloadLink.href = window.URL.createObjectURL(data);
                downloadLink.download = 'SampleExcel.xlsx';
                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);
            });
        }
        $scope.reset = function () {
            $('#file').val('');
            $scope.applicant = [];
            $scope.errors = [];
        };
    }

})();


