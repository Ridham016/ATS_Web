(function () {
    'use strict';

    angular.module("MVCApp").controller('PostingCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonEnums', '$rootScope', '$timeout', '$location', '$window', 'PostingService', PostingCtrl
    ]);

    function PostingCtrl($scope, ngTableParams, CommonFunctions, CommonEnums, $rootScope, $timeout, $location, $window, PostingService) {
        var applicantDetailParams = {};

        $scope.applicantDetailScope = {
            ApplicantId: 0,
            ApplicantName: '',
            PostingId: '',
            StatusId: ''
        };

        $scope.jobpostingDetailScope = {
            PostingId: 0,
            PositionId: '',
            CompanyId: '',
            PostingStatusId: 1,
            Salary: '',
            IsActive: true
        };
        $scope.jobpostingDetail = {};

        var params = $location.search();
        if (params.PostingId != null) {
            $scope.applicantDetailScope.PostingId = JSON.parse(params.PostingId);
            console.log($scope.applicantDetailScope);
        }
        else {
            $scope.applicantDetailScope.PostingId = '';
        }

        $scope.ClearFormData = function (frmRegister) {
            $scope.applicantDetailScope = {
                ApplicantId: 0,
                ApplicantName: '',
                PostingId: '',
                StatusId: ''
            };
            $scope.frmRegister.$setPristine();
        };

        $scope.getJobPostingList = function () {
            PostingService.GetJobPostingList().then(function (res) {
                $scope.JobList = res.data.Result;
                console.log($scope.JobList);
            })
        }

        $scope.Init = function () {
            if (sessionStorage.getItem("JobPostingErrorMessage")) {
                toastr.error(sessionStorage.getItem("JobPostingErrorMessage"), errorTitle);
                sessionStorage.removeItem("JobPostingErrorMessage");
            }
            if (sessionStorage.getItem("JobPostingSuccessMessage")) {
                toastr.success(sessionStorage.getItem("JobPostingSuccessMessage"), successTitle);
                sessionStorage.removeItem("JobPostingSuccessMessage");
            }
        }();

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
                applicantDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                applicantDetailParams.Paging.StatusId = $scope.StatusId;
                //Load Employee List
                PostingService.GetApplicantList(applicantDetailParams.Paging).then(function (res) {
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


        $scope.getStatus = function () {
            //debugger
            PostingService.GetStatus().then(function (res) {
                $scope.status = res.data.Result;
            })
        }
        $scope.SaveApplicantDetails = function (applicantDetailScope) {
            PostingService.Register(applicantDetailScope).then(function (res) {
                if (res) {
                    var applicants = res.data;
                    if (applicants.MessageType == messageTypes.Success && applicants.IsAuthenticated) {
                        toastr.success(applicants.Message, successTitle);
                        $scope.ClearFormData(frmRegister);
                        $scope.tableParams.reload();
                    } else if (applicants.MessageType == messageTypes.Error) {// Error
                        toastr.error(applicants.Message, errorTitle);
                    } else if (applicants.MessageType == messageTypes.Warning) {// Warning
                        toastr.warning(applicants.Message, warningTitle);
                    }
                }

            });

        }
        $scope.EditApplicantDetails = function (ApplicantId) {
            $location.search({});
            PostingService.GetApplicantsById(ApplicantId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.applicantDetailScope = res.data.Result;
                        $scope.frmRegister.$setSubmitted();
                        CommonFunctions.ScrollUp();
                    } else if (data.MessageType == messageTypes.Error) {
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            })
        }

        $scope.getCompanyDetails = function () {
            PostingService.GetCompanyDetails().then(function (res) {
                $scope.companyDetails = res.data.Result;
            })
        }

        $scope.getPostingStatus = function () {
            PostingService.GetPostingStatus().then(function (res) {
                $scope.postingStatusDetails = res.data.Result;
            })
        }

        //$scope.getPositionDetails = JobPostingService.GetPositionDetails();

        //$scope.$watch('Position', function (newValue, oldValue) {
        //    if (newValue !== oldValue && newValue != null) {
        //        debugger
        //        $scope.jobpostingDetailScope.PositionId = newValue.description.Id;
        //        console.log($scope.jobpostingDetailScope);
        //        console.log($scope.jobpostingDetail);
        //    }
        //});

        $scope.Position = function (selected) {
            if (selected) {
                if (selected.originalObject.Id) {
                    $scope.jobpostingDetailScope.PositionId = selected.originalObject.Id;
                    console.log($scope.jobpostingDetailScope);
                }
                else {
                    $scope.positionDetail = {
                        Id: '',
                        PositionName: selected.originalObject,
                        IsActive: true
                    };
                }
            }
        };

        $scope.getPositionDetails = function () {
            PostingService.GetPositionDetails().then(function (res) {
                if (res) {
                    $scope.jobpostingDetail = res.data.Result.map(function (item) {
                        return {
                            Id: item.Id,
                            PositionName: item.PositionName
                        };
                    });
                }
            })
        }

        $scope.onSearchCompletedCallback = function (str) {
            if ($scope.jobpostingDetail.findIndex(x => x.PositionName === str) === -1) {
                $scope.positionDetail.push({
                    Id: '',
                    PositionName: str
                });
            }
        };


        $scope.SavePostingDetails = function (jobpostingDetailScope) {
            debugger
            if (jobpostingDetailScope.Id == null && $scope.positionDetail != null) {
                PostingService.PositionRegister($scope.positionDetail).then(function (res) {
                    jobpostingDetailScope.PositionId = res.data.Result;
                    PostingService.PostingRegister(jobpostingDetailScope).then(function (res) {
                        if (res) {
                            if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                                sessionStorage.setItem("JobPostingSuccessMessage", res.data.Message);
                                window.location.href = '../../PostingManagement/Posting#?PostingId=' + res.data.Result;
                                location.reload();
                            } else if (res.data.MessageType == messageTypes.Error) {
                                sessionStorage.setItem("JobPostingErrorMessage", res.data.Message);
                            } else if (res.data.MessageType == messageTypes.Warning) {
                                toastr.warning(res.data.Message, warningTitle);
                            }
                        }

                    });
                })
            }
            else {
                PostingService.PostingRegister(jobpostingDetailScope).then(function (res) {
                    if (res) {
                        if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                            sessionStorage.setItem("JobPostingSuccessMessage", res.data.Message);
                            window.location.href = '../../PostingManagement/Posting#?PostingId=' + res.data.Result;
                            location.reload();
                        } else if (res.data.MessageType == messageTypes.Error) {
                            sessionStorage.setItem("JobPostingErrorMessage", res.data.Message);
                        } else if (res.data.MessageType == messageTypes.Warning) {
                            toastr.warning(res.data.Message, warningTitle);
                        }
                    }

                });
            }

        }
    }
})();