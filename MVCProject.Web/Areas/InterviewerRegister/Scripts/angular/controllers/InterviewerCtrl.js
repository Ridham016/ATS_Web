(function () {
    'use strict';

    angular.module("MVCApp").controller('InterviewerCtrl', [
        '$scope', 'CommonFunctions', 'InterviewerService', InterviewerCtrl
    ]);

    function InterviewerCtrl($scope, CommonFunctions, InterviewerService) {
        $scope.interviewerDetailScope = {
            InterviewerId: 0,
            InterviewerName: '',
            InterviewerEmail: ''
        };


        $scope.getAllInterviewers = function () {
            InterviewerService.GetAllInterviewers().then(function (res) {
                $scope.interviewers = res.data.Result;
            });
        };

        $scope.ClearFormData = function (frmRegister) {
            $scope.interviewerDetailScope = {
                InterviewertId: 0,
                InterviewerName: '',
                InterviewerEmail: ''
            };
            frmRegister.$setPristine();
            $("InterviewerName").focus();
        };
        $scope.SaveInterviewerDetails = function (interviewerDetailScope) {
            InterviewerService.Register(interviewerDetailScope).then(function (res) {
                if (res) {
                    var interviewers = res.data;
                    if (interviewers.MessageType == messageTypes.Success && interviewers.IsAuthenticated) {
                        toastr.success(interviewers.Message, successTitle);
                        location.reload();
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
                    $scope.interviewerDetailScope = res.data.Result;
                    CommonFunctions.ScrollUpAndFocus("InterviewerName");

                }
            })
        }
    }
})();