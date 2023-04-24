//(function () {
//    'use strict';

//    angular.module("MVCApp").controller('ImportCtrl', [
//        '$scope', '$http', 'ngTableParams', '$rootScope', 'CommonFunctions', 'ImportService', ImportCtrl
//    ]);

//    function ImportCtrl($scope, $http, ngTableParams, $rootScope, CommonFunctions, ImportService) {

//        //$scope.currentApplicant = null;
//        //$scope.applicantDetailScope = [];
//        //$scope.showSubmitButton = false;

//        //$scope.showNextApplicant = function () {
//        //    if ($scope.currentApplicant === null) {
//        //        $scope.currentApplicant = 0;
//        //    } else {
//        //        $scope.currentApplicant += 1;
//        //    }
//        //    $scope.applicantDetailscope = $scope.applicantDetails[$scope.currentApplicant];
//        //};

//        //$scope.submitApplicants = function () {
//        //    $http({
//        //        method: 'POST',
//        //        url: 'api/applicants/add',
//        //        data: $scope.applicantDetails
//        //    }).then(function successCallback(response) {
//        //        alert('Applicants added successfully');
//        //        $scope.showSubmitButton = false;
//        //    }, function errorCallback(response) {
//        //        alert('Error while adding applicants');
//        //        console.log(response);
//        //    });
//        //};

//        //$scope.uploadFile = function () {
//        //    var file = $scope.myFile;
//        //    var formData = new FormData();
//        //    formData.append('file', file);

//        //    $http({
//        //        method: 'POST',
//        //        url: 'api/applicants/upload',
//        //        headers: { 'Content-Type': undefined },
//        //        data: formData,
//        //        transformRequest: function (data, headersGetter) {
//        //            return data;
//        //        }
//        //    }).then(function successCallback(response) {
//        //        $scope.applicantDetails = response.data;
//        //        $scope.currentApplicant = null;
//        //        $scope.showNextApplicant();
//        //        $scope.showSubmitButton = true;
//        //    }, function errorCallback(response) {
//        //        alert('Error while uploading file');
//        //        console.log(response);
//        //    });
//        //};
//        debugger

//        $scope.applicantDetailScope = {
//            ApplicantId: 0,
//            FirstName: '',
//            MiddleName: '',
//            LastName: '',
//            Email: '',
//            Phone: '',
//            Address: '',
//            DateOfBirth: null,
//            CurrentCompany: '',
//            CurrentDesignation: '',
//            TotalExperience: '',
//            DetailedExperience: '',
//            CurrentCTC: '',
//            ExpectedCTC: '',
//            NoticePeriod: '',
//            CurrentLocation: '',
//            PreferedLocation: '',
//            ReasonForChange: '',
//            SkillDescription: '',
//            PortfolioLink: '',
//            LinkedinLink: '',
//            OtherLink: '',
//            ExpectedJoiningDate: null,
//            IsActive: true
//        };
//        $scope.validateApplicant = function (applicantDetailScope) {
//            var errors = [];
//            var onlyNumbersRegex = /^\d+$/;
//            if (!applicantDetailScope.FirstName) {
//                errors.push('First Name is required');
//            }
//            if (!applicantDetailScope.LastName) {
//                errors.push('Last Name is required');
//            }
//            if (!applicantDetailScope.Email) {
//                errors.push('Email is required');
//            } else if (!isValidEmail(applicantDetailScope.Email)) {
//                errors.push('Invalid email format');
//            }
//            if (!applicantDetailScope.Phone) {
//                errors.push('Phone is required');
//            } else if (!isValidPhone(applicantDetailScope.Phone)) {
//                errors.push('Invalid phone number format');
//            }
//            if (!applicantDetailScope.DateOfBirth) {
//                errors.push('Date Of Birth is required');
//            }
//            if (onlyNumbersRegex.test(applicantDetailScope.CurrentCompany)) {
//                return 'CurrentCompany field cannot contain only numbers.';
//            }
//            if (onlyNumbersRegex.test(applicantDetailScope.CurrentDesignation)) {
//                return 'CurrentDesignation field cannot contain only numbers.';
//            }
//            if (onlyNumbersRegex.test(applicantDetailScope.NoticePeriod)) {
//                return 'NoticePeriod field cannot contain only numbers.';
//            }

//            return errors;
//        };
//        $scope.isValidEmail = function (email) {
//            var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
//            return regex.test(email);
//        };
//        //$scope.applicant = [];
//        $scope.Updatedapplicant = [];
//        $scope.currentApplicantIndex = 0;
//        $scope.showNextApplicant = function () {
//            debugger
//            $scope.Updatedapplicant[$scope.currentApplicantIndex] = $scope.applicantDetailScope;
//            $scope.currentApplicantIndex++;
//            if ($scope.currentApplicantIndex >= $scope.applicant.length) {
//                $scope.currentApplicantIndex = 0;
//            }
//            console.log($scope.applicant[0]);
//            $scope.applicantDetailScope = $scope.applicant[$scope.currentApplicantIndex];
//            $scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth);
//        };
//        //$scope.showNextApplicant();
//        $scope.uploadFile = function () {
//            debugger
//            var fileInput = document.getElementById('file');
//            if (fileInput.files.length === 0) return;

//            var file = fileInput.files[0];

//            var payload = new FormData();
//            payload.append("file", file);

//            //$scope.showNextApplicant = function () {
//            //    // Check if there are more applicants to display
//            //    if ($scope.currentApplicantIndex < $scope.applicantDetails.length) {
//            //        // Get next applicant data from list
//            //        var nextApplicant = $scope.applicantDetails[$scope.currentApplicantIndex];

//            //        // Increment applicant index for next iteration
//            //        $scope.currentApplicantIndex++;

//            //        // Show submit button if all applicants have been displayed
//            //        if ($scope.currentApplicantIndex === $scope.applicantDetails.length) {
//            //            $scope.showSubmitButton = true;
//            //        }
//            //    }
//            //};
//            // Send HTTP POST request to upload file and retrieve data
//            //$http.post(uploadUrl, formData, {
//            //    transformRequest: angular.identity,
//            //    headers: { 'Content-Type': undefined }
//            //})
//            ImportService.uploadFile(payload).then(function (response) {
//                $scope.applicantDetailScope = response.data;
//                //$scope.applicantDetailScope = $scope.applicant[$scope.currentApplicantIndex];
//                $scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth);
//                debugger
//                //$scope.showNextApplicant();
//            });

//        };
//        $scope.submitApplicants = function (applicantDetailScope) {
//            var applicantsJson = JSON.stringify($scope.applicantDetailScope);
//            var requestBody = {
//                applicantsJson: applicantsJson
//            };
//            //$http({
//            //    method: 'POST',
//            //    url: '/api/Import/AddApplicants',
//            //    data: applicantDetailScope
//            //$scope.Updatedapplicant[$scope.currentApplicantIndex] = applicantDetailScope;
//            debugger
//            ImportService.AddApplicants(requestBody).then(function (response) {
//                // Handle success response
//                console.log(response.data.Result);
//            }, function (error) {
//                // Handle error response
//                console.log(error.data);
//            });
//        };
//    }

//})();


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

        $scope.Updatedapplicant = [];
        $scope.currentApplicantIndex = 0;
        $scope.validation = [];
        //$scope.validateApplicant = function (applicant) {
        //    debugger
        //    var errors = [];
        //    var onlyNumbersRegex = /^\d+$/;
        //    if (!applicant.FirstName) {
        //        errors.push('First Name is required');
        //    }
        //    if (!applicant.LastName) {
        //        errors.push('Last Name is required');
        //    }
        //    if (!applicant.Email) {
        //        errors.push('Email is required');
        //    } else if (!isValidEmail(applicant.Email)) {
        //        errors.push('Invalid email format');
        //    }
        //    if (!applicant.Phone) {
        //        errors.push('Phone is required');
        //    } else if (!isValidPhone(applicant.Phone)) {
        //        errors.push('Invalid phone number format');
        //    }
        //    if (!applicant.DateOfBirth) {
        //        errors.push('Date Of Birth is required');
        //    }
        //    if (onlyNumbersRegex.test(applicant.CurrentCompany)) {
        //        return 'CurrentCompany field cannot contain only numbers.';
        //    }
        //    if (onlyNumbersRegex.test(applicant.CurrentDesignation)) {
        //        return 'CurrentDesignation field cannot contain only numbers.';
        //    }
        //    if (onlyNumbersRegex.test(applicant.NoticePeriod)) {
        //        return 'NoticePeriod field cannot contain only numbers.';
        //    }

        //    return errors;
        //};
        //$scope.isValidEmail = function (email) {
        //    var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        //    return regex.test(email);
        //};
        $scope.showNextApplicant = function () {
            debugger
            $scope.Updatedapplicant[$scope.currentApplicantIndex] = $scope.applicantDetailScope;
            $scope.currentApplicantIndex++;
            if ($scope.currentApplicantIndex >= $scope.applicant.length) {
                $scope.currentApplicantIndex = 0;
            }
            console.log($scope.applicant[0]);
            $scope.applicantDetailScope = $scope.applicant[$scope.currentApplicantIndex];
            $scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth);
        };
        //$scope.showNextApplicant();
        $scope.uploadFile = function () {
            debugger
            var fileInput = document.getElementById('file');
            if (fileInput.files.length === 0) return;

            var file = fileInput.files[0];

            var payload = new FormData();
            payload.append("file", file);
            ImportService.uploadFile(payload).then(function (response) {
                $scope.applicant = response.data.Result.Data;
                $scope.errors = response.data.Result.Errors;
                for (var i = 0; i < $scope.errors.length; i++) {
                    var errorRow = parseInt($scope.errors[i].split(":")[0]);
                    var errorMessage = $scope.errors[i].split(":")[1];
                    $scope.applicant[errorRow - 1].Error = errorMessage;
                    console.log($scope.errors);
                }
                //$scope.validation = response.data.validation;
                //$scope.validateApplicant($scope.applicant);
                //$scope.errors = $scope.validateApplicant($scope.applicant);
                //$scope.applicantDetailScope = $scope.applicant[$scope.currentApplicantIndex];
                //$scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth);
                debugger
                //$scope.showNextApplicant();
            });
            //.catch (function (response) {
            //    $scope.errors = response.data;

            //});

        };
        $scope.submitApplicants = function (applicant) {
            debugger
            ImportService.AddApplicants($scope.applicant).then(function (response) {
                // Handle success response
                toastr.success(response.Message, successTitle);
                console.log(response.data.Result);
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
    }

})();


