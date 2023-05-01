(function () {
    'use strict';

    angular.module("MVCApp").controller('JobListingCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonService', '$rootScope', '$location', '$window', 'JobListingService', JobListingCtrl
    ]);

    function JobListingCtrl($scope, ngTableParams, CommonFunctions, CommonService, $rootScope, $location, $window, JobListingService) {
        $scope.getJobPostingList = function () {
            JobListingService.GetJobPostingList().then(function (res) {
                if (res) {
                    if (res.data.MessageType == messageTypes.Success) {
                        $scope.JobList = res.data.Result;
                    } else if (res.data.MessageType == messageTypes.Error) {
                        toastr.error(res.data.Message, errorTitle);
                    }
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                }
            })
        }

        $scope.getDescription = function () {
            var params = $location.search();
            debugger
            if (params.PostingId == null) {
                debugger
                $window.location.href = "../../JobOpenings/JobOpenings"
            }
            $scope.PostingId = params.PostingId;
            JobListingService.GetDescription($scope.PostingId).then(function (res) {
                if (res) {
                    if (res.data.MessageType == messageTypes.Success) {
                        $scope.jd = res.data.Result;
                    } else if (res.data.MessageType == messageTypes.Error) {
                        toastr.error(res.data.Message, errorTitle);
                    }
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                }
            })
        }
    }
})();