(function () {
    'use strict';

    angular.module("MVCApp").controller('ImportCtrl', [
        '$scope', '$http', 'ngTableParams', '$rootScope', 'CommonFunctions', 'ImportService', ImportCtrl
    ]);

    function ImportCtrl($scope, $http, ngTableParams, $rootScope, CommonFunctions, ImportService) {
        $scope.applicantDetailScope = [];
        $scope.currentApplicantIndex = -1;
        $scope.showNextApplicant = function () {
            $scope.currentApplicantIndex++;
            if ($scope.currentApplicantIndex >= $scope.applicants.length) {
                $scope.currentApplicantIndex = 0;
            }
            $scope.applicant = $scope.applicants[$scope.currentApplicantIndex];
        };

        // $scope.showNextApplicant();


        $scope.submitForm = function () {
            var file = $scope.file;
            var formData = new FormData();
            formData.append('file', file);
            $scope.selectedFile = null;
            $scope.msg = "";
        }


        $scope.loadFile = function (files) {

            $scope.$apply(function () {

                $scope.selectedFile = files[0];

            })

        }

        $http.post('/api/Import/ImportData', formData, {
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        })
            .then(function successCallback(response) {
                // handle success
                $scope.message = response.data.message;
            }, function errorCallback(response) {
                // handle error
                $scope.message = response.data.message;
            });
    }
})();