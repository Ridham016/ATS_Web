(function () {
    'use strict';

    angular.module("MVCApp").controller('AdvancedSearchCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', '$location', '$window', 'AdvancedSearchService', AdvancedSearchCtrl
    ]);

    function AdvancedSearchCtrl($scope, ngTableParams, CommonFunctions, $rootScope, $location, $window, AdvancedSearchService) {
        var searchDetailParams = {};
        $scope.searchScope = {
            StatusId: null,
            StartDate: null,
            EndDate: null
        };
        $scope.statusDetailScope = {
            StatusId: null,
            StatusName: null
        };

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { FirstName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (applicantDetailParams == null) {
                    applicantDetailParams = {};
                }
                applicantDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                ScheduleService.AdvancedSearch(searchScope,searchDetailParams.Paging).then(function (res) {
                    debugger
                    var data = res.data;
                    $scope.applicants = res.data.Result;
                    if (res.data.MessageType == messageTypes.Success) {// Success
                        $defer.resolve(res.data.Result);
                        if (res.data.Result.length == 0) {
                        } else {
                            params.total(res.data.Result[0].TotalRecords);
                        }
                    } else if (res.data.MessageType == messageTypes.Error) {// Error
                        toastr.error(res.data.Message, errorTitle);
                    }
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                    $scope.accordionGroup_1 = true;
                    $scope.accordionGroup_2 = false;
                });
            }
        });


        $scope.advancedsearch = function (searchScope) {
            debugger
            AdvancedSearchService.AdvancedSearch(searchScope).then(function (res) {
                var data = res.data;
                $scope.searchScope = res.data.Result;
            });
        };
        $scope.GetStatus = function () {
            debugger
            AdvancedSearchService.GetStatus().then(function (res) {
                var data = res.data;
                $scope.status = res.data.Result;
            });
        };
    }
})();