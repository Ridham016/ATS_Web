(function () {
    'use strict';

    angular.module("MVCApp").controller('JobPostingCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonEnums', '$rootScope', '$location', '$window', 'JobPostingService', JobPostingCtrl
    ]);

    function JobPostingCtrl($scope, ngTableParams, CommonFunctions, CommonEnums, $rootScope, $location, $window, JobPostingService) {
        var jobpostingDetailParams = {};
        $scope.jobpostingDetailScope = {
            PostingId: 0,
            PositionId: '',
            CompanyId: '',
            Experience: '',
            Salary: '',
            IsActive: true
        };

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { EntryDate: 'desc' }
        }, {
            getData: function ($defer, params) {
                debugger
                if (jobpostingDetailParams == null) {
                    jobpostingDetailParams = {};
                }
                jobpostingDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                jobpostingDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                JobPostingService.GetAllPostings(jobpostingDetailParams.Paging).then(function (res) {
                    debugger
                    var data = res.data;
                    $scope.postings = res.data.Result;
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

        $scope.getCompanyDetails = function () {
            debugger
            JobPostingService.GetCompanyDetails().then(function (res) {
                $scope.companyDetails = res.data.Result;
            })
        }

        $scope.getPositionDetails = function () {
            debugger
            JobPostingService.GetPositionDetails().then(function (res) {
                $scope.positionDetails = res.data.Result;
            })
        }

        $scope.ClearFormData = function (frmRegister) {
            $scope.jobpostingDetailScope = {
                PostingId: 0,
                PositionId: '',
                CompanyId: '',
                Experience: '',
                Salary: '',
                IsActive: true
            };
            $scope.frmRegister.$setPristine();
            CommonFunctions.ScrollToTop();
        };

        $scope.SavePostingDetails = function (jobpostingDetailScope) {
            if (!$scope.frmRegister.$valid) {
                angular.forEach($scope.frmRegister.$error.required, function (field) {
                    field.$setTouched();
                    field.$setValidity('required', true);
                });
                toastr.error('Please Check Form for errors', errorTitle)
                return false;
            }
            else {
                JobPostingService.Register(jobpostingDetailScope).then(function (res) {
                    if (res) {
                        if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                            toastr.success(res.data.Message, successTitle);
                            $scope.ClearFormData(frmRegister);
                            $scope.tableParams.reload();
                        } else if (res.data.MessageType == messageTypes.Error) {// Error
                            toastr.error(res.data.Message, errorTitle);
                        } else if (res.data.MessageType == messageTypes.Warning) {// Warning
                            toastr.warning(res.data.Message, warningTitle);
                        }
                    }

                });
            }

        }
        $scope.EditPostingDetails = function (PostingId) {
            //debugger
            JobPostingService.GetPostingById(PostingId).then(function (res) {
                debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.jobpostingDetailScope = res.data.Result;
                        $scope.jobpostingDetailScope.PositionId = JSON.stringify($scope.jobpostingDetailScope.PositionId);
                        $scope.jobpostingDetailScope.CompanyId = JSON.stringify($scope.jobpostingDetailScope.CompanyId);
                        CommonFunctions.ScrollToTop();
                    } else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            })
        }
    }
})();