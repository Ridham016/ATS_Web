(function () {
    'use strict';

    angular.module("MVCApp").controller('JobListingCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonService', '$rootScope', '$location', '$window', 'JobListingService', JobListingCtrl
    ]);

    function JobListingCtrl($scope, ngTableParams, CommonFunctions, CommonService, $rootScope, $location, $window, JobListingService) {
        $scope.SortBy = 'date';
        $scope.jobDetailParams = {
            OrderByColumn: 'EntryDate',
            IsAscending: false
        };
        $scope.sortBy = function (SortBy) {
            if (SortBy == 'date') {
                $scope.jobDetailParams = {
                    OrderByColumn: 'EntryDate',
                    IsAscending: false
                };
            }
            else if (SortBy == 'a-z') {
                $scope.jobDetailParams = {
                    OrderByColumn: 'PositionName',
                    IsAscending: true
                };
            }
            else if (SortBy == 'z-a') {
                $scope.jobDetailParams = {
                    OrderByColumn: 'PositionName',
                    IsAscending: false
                };
            }

            JobListingService.GetJobPostingList($scope.jobDetailParams).then(function (res) {
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

        $scope.options = [
            { id: 1, name: 'Option 1', html: 'Option <span class="highlight">1</span>' },
            { id: 2, name: 'Option 2', html: 'Option <span class="highlight">2</span>' },
            { id: 3, name: 'Option 3', html: 'Option <span class="highlight">3</span>' }
        ];

        $scope.getDescription = function () {
            var params = $location.search();
            if (params.PostingId == null) {
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