(function () {
    'use strict';

    angular.module("MVCApp").controller('JobListingCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonEnums', '$rootScope', '$location', '$window', 'JobListingService', JobListingCtrl
    ]);

    function JobListingCtrl($scope, ngTableParams, CommonFunctions, CommonEnums, $rootScope, $location, $window, JobListingService) {

        $scope.getJobPostingList = function () {
            JobListingService.GetJobPostingList().then(function (res) {
                $scope.JobList = res.data.Result;
            })
        }
        $scope.calculateDay = function (date) {
            return Math.floor((new Date() - new Date(date)) / (1000 * 3600 * 24));
        }
    }
})();