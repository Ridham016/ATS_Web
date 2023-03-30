(function () {
    'use strict';

    angular.module("MVCApp").controller('AdvancedSearchCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', 'AdvancedSearchService', AdvancedSearchCtrl
    ]);

    function AdvancedSearchCtrl($scope, ngTableParams, CommonFunctions, $rootScope, AdvancedSearchService) {
        var searchDetailParams = {};
        $scope.CurrentDate = new Date();
        //$scope.searchDetail.daterange.startDate = null;
        //$scope.searchDetail.daterange.endDate = null;
        $scope.CurrentDate = angular.copy(moment($scope.CurrentDate).format("YYYY-MM-DD"));
        $scope.searchDetail = {
            StatusId: null,
            StartDate: null,
            EndDate: null,
            daterange: { startDate: null, endDate: null }
        };
        
        $scope.statusDetailScope = {
            StatusId: null,
            StatusName: null
        };

        $scope.Init = function () {
            $scope.advancedsearch = function (searchDetail) {
                debugger
                $scope.searchDetail = searchDetail;
                $scope.searchDetail.StartDate = angular.copy(moment(searchDetail.daterange.startDate).format($rootScope.apiDateFormat));
                $scope.searchDetail.EndDate = angular.copy(moment(searchDetail.daterange.endDate).format($rootScope.apiDateFormat));
                $scope.tableParams.reload();
            };
        }

        $scope.Init();

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
            debugger
            $scope.searchDetail = {
                StatusId: null,
                StartDate: null,
                EndDate: null,
                daterange: { startDate: null, endDate: null }
            };
            //$scope.searchDetail.daterange.startDate = null;
            //$scope.searchDetail.daterange.endDate = null;
            $("#DateRange").val("");
            frmRegister.$setPristine();
        };

        $scope.advancedsearch = function (searchDetail) {
            debugger
            console.log(searchDetail);
            if (moment.isDate(searchDetail.daterange.startDate) && moment.isDate(searchDetail.daterange.endDate)) {
                $scope.searchDetail = searchDetail;
                $scope.searchDetail.StartDate = angular.copy(moment(searchDetail.daterange.startDate).format($rootScope.apiDateFormat));
                $scope.searchDetail.EndDate = angular.copy(moment(searchDetail.daterange.endDate).format($rootScope.apiDateFormat));
                $scope.tableParams.reload();
            }
            else {
                $scope.searchDetail = searchDetail;
                $scope.searchDetail.StartDate = null;
                $scope.searchDetail.EndDate = null;
                $scope.tableParams.reload();
            }
            
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