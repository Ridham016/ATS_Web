(function () {
    'use strict';

    angular.module("MVCApp").controller('AdvancedSearchCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', 'AdvancedSearchService', AdvancedSearchCtrl
    ]);

    function AdvancedSearchCtrl($scope, ngTableParams, CommonFunctions, $rootScope, AdvancedSearchService) {
        var searchDetailParams = {};
        $scope.CurrentDate = new Date();
        $scope.searchDetail = {
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
            count: $rootScope.pageSize
        }, {
            getData: function ($defer, params) {
                debugger
                if (searchDetailParams == null) {
                    searchDetailParams = {};
                }
                searchDetailParams.paging = CommonFunctions.GetPagingParams(params);
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                AdvancedSearchService.AdvancedSearch(searchDetailParams.paging, $scope.searchDetail).then(function (res) {
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
                });
            }
        });

        $scope.ClearFormData = function (frmRegister) {
            $scope.searchDetail = {
                StatusId: null,
                StartDate: null,
                EndDate: null
            };
            $scope.frmRegister.$setPristine();
        };

        $scope.advancedsearch = function (searchDetail) {
            debugger
            $scope.searchDetail = searchDetail;
            $scope.searchDetail.StartDate = angular.copy(moment($scope.searchDetail.StartDate).format($rootScope.apiDateFormat));
            $scope.searchDetail.EndDate = angular.copy(moment($scope.searchDetail.EndDate).format($rootScope.apiDateFormat));
            $scope.tableParams.reload();
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