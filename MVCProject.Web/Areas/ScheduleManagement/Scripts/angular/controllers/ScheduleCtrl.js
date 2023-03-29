(function () {
    'use strict';

    angular.module("MVCApp").controller('ScheduleCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', '$location','$window', 'ScheduleService', ScheduleCtrl
    ]);

    function ScheduleCtrl($scope, ngTableParams, CommonFunctions, $rootScope, $location, $window, ScheduleService) {
        var applicantDetailParams = {};
        $scope.scheduleDetailScope = {
            Id: 0,
            ScheduleDateTime: null,
            ScheduleLink: '',
            Description : '',
            IsActive: true
        };

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { ApplicantDate: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (applicantDetailParams == null) {
                    applicantDetailParams = {};
                }
                applicantDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                ScheduleService.GetApplicantList(applicantDetailParams.Paging).then(function (res) {
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

        $scope.GetData = function () {
            debugger
            var param = $location.search();
            $scope.applicantId = param.ApplicantId;
            ScheduleService.GetApplicant($scope.applicantId).then(function (res) {
                debugger
                $scope.applicants = res.data.Result;
                $scope.getButtons($scope.applicants.StatusId);
            })
        }

        $scope.getButtons = function (StatusId) {
            debugger
            ScheduleService.GetButtons(StatusId).then(function (res) {
                $scope.buttons = res.data.Result;
            })
        }

        $scope.updateStatus = function (StatusId, ApplicantId) {
            debugger
            ScheduleService.UpdateButton(StatusId, ApplicantId).then(function (res) {
                debugger
                if (res) {
                    var status = res.data;
                    $window.location.href = '../../ScheduleManagement/Schedule';
                    //if (status.MessageType == messageTypes.Success && status.IsAuthenticated) {
                    //    toastr.success(status.Message, successTitle);
                    //} else if (status.MessageType == messageTypes.Error) {// Error
                    //    toastr.error(status.Message, errorTitle);
                    //} else if (status.MessageType == messageTypes.Warning) {// Warning
                    //    toastr.warning(applicants.Message, warningTitle);
                    //}
                }
            })
        }
        $scope.getAllInterviewers = function () {
            debugger
            ScheduleService.GetAllInterviewers().then(function (res) {
                var data = res.data;
            $scope.interviewers = res.data.Result;
            });
        };

        $scope.SaveSchduleDetails = function (scheduleDetailScope, StatusId, ApplicantId) {
            ScheduleService.UpdateButton(StatusId, ApplicantId).then(function (res) {
                debugger
                if (res) {
                    $scope.Action = res.data.Result;
                    debugger
                    $scope.scheduleDetailScope['ActionId'] = $scope.Action[1];
                    //$scope.SaveSchduleDetails(scheduleDetailScope);
                    debugger
                    $scope.scheduleDetailScope.ScheduleDateTime = angular.copy(moment($scope.scheduleDetailScope.ScheduleDateTime).format($rootScope.apiDateFormat));
                    ScheduleService.Schedule(scheduleDetailScope).then(function (res) {
                        if (res) {
                            var schedule = res.data;
                            $window.location.href = '../../ScheduleManagement/Schedule';
                            //if (schedule.MessageType == messageTypes.Success && schedule.IsAuthenticated) {
                            //    toastr.success(schedule.Message, successTitle);
                            //} else if (schedule.MessageType == messageTypes.Error) {// Error
                            //    toastr.error(schedule.Message, errorTitle);
                            //} else if (schedule.MessageType == messageTypes.Warning) {// Warning
                            //    toastr.warning(schedule.Message, warningTitle);
                            //}
                        }
                    })
                }
            })
        }

        $scope.getOtherReasons = function () {
            debugger
            ScheduleService.GetOtherReasons().then(function (res) {
                debugger
                var data = res.data;
                $scope.reasons = res.data.Result;
            });
        };

        $scope.updateOtherReason = function (reasonId, StatusId, ApplicantId) {
            ScheduleService.UpdateButton(StatusId, ApplicantId).then(function (res) {
                debugger
                if (res) {
                    $scope.Action = res.data.Result;
                    $scope.ActionId = $scope.Action[1];
                    debugger
                    ScheduleService.UpdateOtherReason(reasonId,$scope.ActionId).then(function (res) {
                        var data = res.data;
                        $window.location.href = '../../ScheduleManagement/Schedule';
                    })
                }
            })
        }

        $scope.holdReason = function (scheduleDetailScope, StatusId, ApplicantId) {
            ScheduleService.UpdateButton(StatusId, ApplicantId).then(function (res) {
                debugger
                if (res) {
                    $scope.Action = res.data.Result;
                    $scope.ActionId = $scope.Action[1];
                    debugger
                    ScheduleService.HoldReason(scheduleDetailScope, $scope.ActionId).then(function (res) {
                        var data = res.data;
                        $window.location.href = '../../ScheduleManagement/Schedule';
                    })
                }
            })
        }
    }
})();