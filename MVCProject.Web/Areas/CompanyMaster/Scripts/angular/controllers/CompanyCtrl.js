﻿(function () {
    'use strict';

    angular.module("MVCApp").controller('CompanyCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonEnums', '$rootScope', '$location', '$window', 'CompanyService', CompanyCtrl
    ]);

    function CompanyCtrl($scope, ngTableParams, CommonFunctions, CommonEnums, $rootScope, $location, $window, CompanyService) {
        var companyDetailParams = {};
        $scope.companyDetailScope = {
            Id: 0,
            CompanyName: '',
            Venue: '',
            ContactPersonName: '',
            ContactPersonPhone: '',
            ContactPersonPositionId: '',
            IsActive: true
        };

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { EntryDate: 'desc' }
        }, {
            getData: function ($defer, params) {
                debugger
                if (companyDetailParams == null) {
                    companyDetailParams = {};
                }
                companyDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                companyDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                CompanyService.GetAllCompany(companyDetailParams.Paging).then(function (res) {
                    debugger
                    var data = res.data;
                    $scope.company = res.data.Result;
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

        $scope.getPositionDetails = function () {
            debugger
            CompanyService.GetPositionDetails().then(function (res) {
                $scope.positionDetails = res.data.Result;
            })
        }

        $scope.ClearFormData = function (frmRegister) {
            $scope.companyDetailScope = {
                Id: 0,
                CompanyName: '',
                Venue: '',
                ContactPersonName: '',
                ContactPersonPhone: '',
                ContactPersonPositionId: '',
                IsActive: true
            };
            $scope.frmRegister.$setPristine();
            CommonFunctions.ScrollToTop();
        };

        $scope.SaveCompanyDetails = function (companyDetailScope) {
            if (!$scope.frmRegister.$valid) {
                angular.forEach($scope.frmRegister.$error.required, function (field) {
                    field.$setTouched();
                    field.$setValidity('required', true);
                });
                toastr.error('Please Check Form for errors', errorTitle)
                return false;
            }
            else {
                CompanyService.Register(companyDetailScope).then(function (res) {
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
        $scope.EditCompanyDetails = function (CompanyId) {
            //debugger
            CompanyService.GetCompanyById(CompanyId).then(function (res) {
                debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.companyDetailScope = res.data.Result;
                        $scope.companyDetailScope.ContactPersonPositionId = JSON.stringify($scope.companyDetailScope.ContactPersonPositionId);
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