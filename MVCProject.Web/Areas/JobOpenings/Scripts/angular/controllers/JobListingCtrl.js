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

        $scope.searchDetail = {
            StartDate: null,
            EndDate: null,
            daterange: { startDate: null, endDate: null }
        };

        $scope.sortBy = function (SortBy,searchDetail) {
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
            if (searchDetail.daterange.startDate && searchDetail.daterange.endDate) {
                if (moment.isMoment(searchDetail.daterange.startDate) && moment.isMoment(searchDetail.daterange.endDate)) {
                    searchDetail.daterange.startDate = null;
                    searchDetail.daterange.endDate = null;
                } else {
                    searchDetail.StartDate = angular.copy(moment(searchDetail.daterange.startDate).format($rootScope.apiDateFormat));
                    searchDetail.EndDate = angular.copy(moment(searchDetail.daterange.endDate).format($rootScope.apiDateFormat));
                }
            } else {
                searchDetail.StartDate = null;
                searchDetail.EndDate = null;
            }
            $scope.searchDetail = searchDetail;

            JobListingService.GetJobPostingList($scope.jobDetailParams, $scope.searchDetail).then(function (res) {
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


        $scope.ClearFormData = function () {
            $scope.searchDetail = {
                StartDate: null,
                EndDate: null,
                daterange: { startDate: moment(), endDate: moment() }
            };
            $("#DateRange").val("");
        };


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