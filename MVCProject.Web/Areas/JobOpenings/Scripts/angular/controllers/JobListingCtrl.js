(function () {
    'use strict';

    angular.module("MVCApp").controller('JobListingCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonService', '$rootScope', '$location', '$window', 'JobListingService', JobListingCtrl
    ]);

    function JobListingCtrl($scope, ngTableParams, CommonFunctions, CommonService, $rootScope, $location, $window, JobListingService) {
        $scope.getJobPostingList = function () {
            JobListingService.GetJobPostingList().then(function (res) {
                $scope.JobList = res.data.Result;
            })
        }
    }
})();