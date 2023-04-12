(function () {
    'use strict';

    angular.module("MVCApp").controller('ScheduleCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonEnums', '$rootScope', '$location','$window', 'ScheduleService', ScheduleCtrl
    ]);

    function ScheduleCtrl($scope, ngTableParams, CommonFunctions, CommonEnums, $rootScope, $location, $window, ScheduleService) {
        var applicantDetailParams = {};
        $scope.Mode = CommonEnums.Mode;
        $scope.scheduleDetailScope = {
            Id: 0,
            ScheduleDateTime: null,
            ScheduleLink: '',
            Description: '',
            Mode: '',
            IsActive: true
        };

        $scope.minDate = new Date().toISOString().slice(0, 16);

        $scope.Reason = {
            ReasonId: '',
            CancelReason: ''
        };

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { EntryDate: 'desc' }
        }, {
            getData: function ($defer, params) {
                if (applicantDetailParams == null) {
                    applicantDetailParams = {};
                }
                applicantDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                ScheduleService.GetApplicantList(applicantDetailParams.Paging).then(function (res) {
                    //debugger
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
            //debugger
            var param = $location.search();
            $scope.applicantId = param.ApplicantId;
            ScheduleService.GetApplicant($scope.applicantId).then(function (res) {
                //debugger
                $scope.applicants = res.data.Result;
                $scope.Level = $scope.applicants.Level;
                $scope.getButtons($scope.applicants.StatusId);
            })
        }

        $scope.getButtons = function (StatusId) {
            //debugger
            ScheduleService.GetButtons(StatusId).then(function (res) {
                $scope.buttons = res.data.Result;
                console.log($scope.buttons);
            })
        }

        $scope.showModal = function (ButtonId) {
            if (ButtonId == 3 || ButtonId == 4 || ButtonId == 7) {
                var modalOptions = {
                    backdrop: 'static'
                };
                //debugger
                $scope.StatusId = ButtonId + 1;
                $('#Modal' + ButtonId).modal(modalOptions);
                $('#Modal' + ButtonId).modal('show');
            }
            else {
                var modalOptions = {
                    backdrop: 'static'
                };
                $scope.StatusId = ButtonId + 1;
                $('#Modal').modal(modalOptions);
                $('#Modal').modal('show');
            }
            
        }

        $scope.updateStatus = function (scheduleDetailScope, StatusId, ApplicantId) {
            //debugger
            ScheduleService.UpdateButton(StatusId, ApplicantId).then(function (res) {
                //debugger
                if (res) {
                    $scope.Action = res.data.Result;
                    $scope.ActionId = $scope.Action[1];
                    //debugger
                    ScheduleService.Comment(scheduleDetailScope, $scope.ActionId).then(function (res) {
                        var data = res.data;
                        $window.location.href = '../../ScheduleManagement/Schedule';
                    })
                }
            })
            
        }
        $scope.getAllInterviewers = function () {
            //debugger
            ScheduleService.GetAllInterviewers().then(function (res) {
                var data = res.data;
            $scope.interviewers = res.data.Result;
            });
        };

        $scope.SaveSchduleDetails = function (scheduleDetailScope, StatusId, ApplicantId) {
            ScheduleService.UpdateButton(StatusId, ApplicantId).then(function (res) {
                //debugger
                if (res) {
                    $scope.Action = res.data.Result;
                    //debugger
                    $scope.scheduleDetailScope['ActionId'] = $scope.Action[1];
                    $scope.scheduleDetailScope.ScheduleDateTime = angular.copy(moment($scope.scheduleDetailScope.ScheduleDateTime).format($rootScope.apiDateFormat));
                    ScheduleService.Schedule(scheduleDetailScope).then(function (res) {
                        if (res) {
                            var schedule = res.data;
                            $window.location.href = '../../ScheduleManagement/Schedule';
                        }
                    })
                }
            })
        }

        $scope.getOtherReasons = function () {
            //debugger
            ScheduleService.GetOtherReasons().then(function (res) {
                //debugger
                var data = res.data;
                $scope.reasons = res.data.Result;
            });
        };

        $scope.updateOtherReason = function (Reason, StatusId, ApplicantId) {
            ScheduleService.UpdateButton(StatusId, ApplicantId).then(function (res) {
                //debugger
                if (res) {
                    $scope.Action = res.data.Result;
                    $scope.ActionId = $scope.Action[1];
                    //debugger
                    ScheduleService.UpdateOtherReason(Reason,$scope.ActionId).then(function (res) {
                        var data = res.data;
                        $window.location.href = '../../ScheduleManagement/Schedule';
                    })
                }
            })
        }
    }
})();