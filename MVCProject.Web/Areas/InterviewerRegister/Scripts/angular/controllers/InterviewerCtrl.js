(function () {
    'use strict';

    angular.module("MVCApp").controller('InterviewerCtrl', [
        '$scope', 'ngTableParams', '$rootScope', 'CommonFunctions', 'InterviewerService', InterviewerCtrl
    ]);

    function InterviewerCtrl($scope, ngTableParams, $rootScope, CommonFunctions, InterviewerService) {
        var interviewerDetailParams = {};
        $scope.interviewerDetailScope = {
            InterviewerId: 0,
            InterviewerName: '',
            InterviewerEmail: '',
            InterviewerPhone: '',
            Is_Active: true
        };

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize
        }, {
            getData: function ($defer, params) {
                if (interviewerDetailParams == null) {
                    interviewerDetailParams = {};
                }
                interviewerDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                debugger
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                InterviewerService.GetAllInterviewers(interviewerDetailParams.Paging).then(function (res) {
                        var data = res.data;
                    $scope.interviewers = res.data.Result;
                    debugger
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


        //$scope.getAllInterviewers = function () {
        //    InterviewerService.GetAllInterviewers(interviewerDetailParams.Paging).then(function (res) {
        //        var data = res.data;
            //$scope.interviewers = res.data.Result;
            //if (res.data.MessageType == messageTypes.Success) {// Success
            //    $defer.resolve(res.data.Result);
            //    if (res.data.Result.length == 0) {
            //    } else {
            //        params.total(res.data.Result[0].TotalRecords);
            //    }
            //} else if (res.data.MessageType == messageTypes.Error) {// Error
            //    toastr.error(res.data.Message, errorTitle);
            //}
        //    });
        //};

        $scope.ClearFormData = function (frmRegister) {
            $scope.interviewerDetailScope = {
                InterviewertId: 0,
                InterviewerName: '',
                InterviewerEmail: '',
                InterviewerPhone: '',
                Is_Active: true
            };
            $scope.frmRegister.$setPristine();
            $("InterviewerName").focus();
            CommonFunctions.ScrollToTop();
        };
        $scope.SaveInterviewerDetails = function (interviewerDetailScope) {
            InterviewerService.Register(interviewerDetailScope).then(function (res) {
                if (res) {
                    var interviewers = res.data;
                    if (interviewers.MessageType == messageTypes.Success && interviewers.IsAuthenticated) {
                        toastr.success(interviewers.Message, successTitle);
                        $scope.ClearFormData(frmRegister);
                        $scope.tableParams.reload();
                    } else if (interviewers.MessageType == messageTypes.Error) {// Error
                        toastr.error(interviewers.Message, errorTitle);
                    } else if (interviewers.MessageType == messageTypes.Warning) {// Warning
                        toastr.warning(interviewers.Message, warningTitle);
                    }
                }
            });
        }
        $scope.EditInterviewersDetails = function (InterviewerId) {
            InterviewerService.GetInterviewersById(InterviewerId).then(function (res) {
                if (res) {
                    debugger
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.interviewerDetailScope = res.data.Result;
                        CommonFunctions.ScrollUpAndFocus("InterviewerName");
                    } else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            })
        }
    }
})();