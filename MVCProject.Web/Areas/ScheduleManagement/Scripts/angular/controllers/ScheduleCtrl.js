(function () {
    'use strict';

    angular.module("MVCApp").controller('ScheduleCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', 'ScheduleService', ScheduleCtrl
    ]);

    function ScheduleCtrl($scope, ngTableParams, CommonFunctions, $rootScope, ScheduleService) {
        var applicantDetailParams = {};
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
                debugger
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                ScheduleService.GetApplicantList(applicantDetailParams.Paging).then(function (res) {
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
    }
})();