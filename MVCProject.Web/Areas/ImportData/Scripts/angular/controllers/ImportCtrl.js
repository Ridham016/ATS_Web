(function () {
    'use strict';

    angular.module("MVCApp").controller('ImportCtrl', [
        '$scope', '$http', 'ngTableParams', '$rootScope', 'CommonFunctions', 'ImportService', ImportCtrl
    ]);

    function ImportCtrl($scope, $http, ngTableParams, $rootScope, CommonFunctions, ImportService) {

        //$scope.currentApplicant = null;
        //$scope.applicantDetailScope = [];
        //$scope.showSubmitButton = false;

        //$scope.showNextApplicant = function () {
        //    if ($scope.currentApplicant === null) {
        //        $scope.currentApplicant = 0;
        //    } else {
        //        $scope.currentApplicant += 1;
        //    }
        //    $scope.applicantDetailscope = $scope.applicantDetails[$scope.currentApplicant];
        //};

        //$scope.submitApplicants = function () {
        //    $http({
        //        method: 'POST',
        //        url: 'api/applicants/add',
        //        data: $scope.applicantDetails
        //    }).then(function successCallback(response) {
        //        alert('Applicants added successfully');
        //        $scope.showSubmitButton = false;
        //    }, function errorCallback(response) {
        //        alert('Error while adding applicants');
        //        console.log(response);
        //    });
        //};

        //$scope.uploadFile = function () {
        //    var file = $scope.myFile;
        //    var formData = new FormData();
        //    formData.append('file', file);

        //    $http({
        //        method: 'POST',
        //        url: 'api/applicants/upload',
        //        headers: { 'Content-Type': undefined },
        //        data: formData,
        //        transformRequest: function (data, headersGetter) {
        //            return data;
        //        }
        //    }).then(function successCallback(response) {
        //        $scope.applicantDetails = response.data;
        //        $scope.currentApplicant = null;
        //        $scope.showNextApplicant();
        //        $scope.showSubmitButton = true;
        //    }, function errorCallback(response) {
        //        alert('Error while uploading file');
        //        console.log(response);
        //    });
        //};
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
        $scope.Updatedapplicant = [];
        $scope.currentApplicantIndex = 0;
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

            //$scope.showNextApplicant = function () {
            //    // Check if there are more applicants to display
            //    if ($scope.currentApplicantIndex < $scope.applicantDetails.length) {
            //        // Get next applicant data from list
            //        var nextApplicant = $scope.applicantDetails[$scope.currentApplicantIndex];

            //        // Increment applicant index for next iteration
            //        $scope.currentApplicantIndex++;

            //        // Show submit button if all applicants have been displayed
            //        if ($scope.currentApplicantIndex === $scope.applicantDetails.length) {
            //            $scope.showSubmitButton = true;
            //        }
            //    }
            //};
            // Send HTTP POST request to upload file and retrieve data
            //$http.post(uploadUrl, formData, {
            //    transformRequest: angular.identity,
            //    headers: { 'Content-Type': undefined }
            //})
            ImportService.uploadFile(payload).then(function (response) {
                $scope.applicant = response.data.Result;
                $scope.applicantDetailScope = $scope.applicant[$scope.currentApplicantIndex];
                $scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth); 
                debugger
                $scope.showNextApplicant();
            });

        };
        $scope.submitApplicants = function (applicantDetailScope) {
            //$http({
            //    method: 'POST',
            //    url: '/api/Import/AddApplicants',
            //    data: applicantDetailScope
            $scope.Updatedapplicant[$scope.currentApplicantIndex] = applicantDetailScope;
            debugger
            ImportService.AddApplicants($scope.Updatedapplicant).then(function (response) {
                // Handle success response
                console.log(response.data.Result);
            }, function (error) {
                // Handle error response
                console.log(error.data);
            });
        };
    }

})();

