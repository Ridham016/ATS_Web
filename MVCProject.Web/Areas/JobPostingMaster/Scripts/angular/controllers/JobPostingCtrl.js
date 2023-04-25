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
            PostingStatusId: '',
            Salary: '',
            IsActive: true
        };
        $scope.jobpostingDetail = {};

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { EntryDate: 'desc' }
        }, {
            getData: function ($defer, params) {
                if (jobpostingDetailParams == null) {
                    jobpostingDetailParams = {};
                }
                jobpostingDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                jobpostingDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                JobPostingService.GetAllPostings(jobpostingDetailParams.Paging).then(function (res) {
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
            JobPostingService.GetCompanyDetails().then(function (res) {
                $scope.companyDetails = res.data.Result;
            })
        }

        $scope.getPostingStatus = function () {
            JobPostingService.GetPostingStatus().then(function (res) {
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
        };

        $scope.getPositionDetails = function () {
            JobPostingService.GetPositionDetails().then(function (res) {
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

        $scope.ClearFormData = function (frmRegister) {
            $scope.jobpostingDetailScope = {
                PostingId: 0,
                PositionId: '',
                CompanyId: '',
                Experience: '',
                PostingStatusId: '',
                Salary: '',
                IsActive: true
            };
            $scope.$broadcast('angucomplete-alt:clearInput')
            $scope.frmRegister.$setPristine();
            CommonFunctions.ScrollToTop();
        };

        $scope.onSearchCompletedCallback = function (str) {
            if ($scope.jobpostingDetail.findIndex(x => x.PositionName === str) === -1) {
                $scope.positionDetail.push({
                    Id: '',
                    PositionName: str
                });
            }
        };


        $scope.SavePostingDetails = function (jobpostingDetailScope) {
            if (!$scope.frmRegister.$valid) {
                angular.forEach($scope.frmRegister.$error, function (controls) {
                    angular.forEach(controls, function (control) {
                        control.$setDirty();
                    });
                });
                toastr.error('Please Check Form for errors', errorTitle)
                return false;
            }
            else {
                debugger
                if (jobpostingDetailScope.Id == null && $scope.positionDetail != null) {
                    JobPostingService.PositionRegister($scope.positionDetail).then(function (res) {
                        jobpostingDetailScope.PositionId = res.data.Result;
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
                    })
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

        }
        $scope.EditPostingDetails = function (PostingId) {
            //debugger
            JobPostingService.GetPostingById(PostingId).then(function (res) {
                debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.jobpostingDetailScope = res.data.Result;
                        $scope.selectedPosition = {
                            Id: $scope.jobpostingDetailScope.PositionId,
                            PositionName: $scope.jobpostingDetailScope.PositionName
                         }
                        $scope.$broadcast('angucomplete-alt:changeInput', 'Position', $scope.selectedPosition);
                        $scope.jobpostingDetailScope.CompanyId = JSON.stringify($scope.jobpostingDetailScope.CompanyId);
                        $scope.jobpostingDetailScope.PostingStatusId = JSON.stringify($scope.jobpostingDetailScope.PostingStatusId);
                        $scope.frmRegister.$setSubmitted();
                        angular.forEach($scope.frmRegister.$error, function (controls) {
                            angular.forEach(controls, function (control) {
                                control.$setDirty();
                            });
                        });
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