(function () {
    'use strict';

    angular.module("MVCApp").controller('ScheduleCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', 'ScheduleService', ScheduleCtrl
    ]);

    function ScheduleCtrl($scope, ngTableParams, CommonFunctions, $rootScope, ScheduleService) {
        //$scope.getApplicantList = function (isGetAll) {
        //    ScheduleService.GetApplicantList(isGetAll).then(function (res) {
        //        $scope.applicants = res.data.Result;
        //    });
        //}
        var applicantDetailParams = {};

        $scope.tableActiveParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize
        }, {
            getData: function ($defer, params) {
                if (applicantDetailParams == null) {
                    applicantDetailParams = {};
                }
                applicantDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                debugger
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
                });
            }
        });
    }
})();