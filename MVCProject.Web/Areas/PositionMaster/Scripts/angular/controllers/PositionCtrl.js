(function () {
    'use strict';

    angular.module("MVCApp").controller('PositionCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonEnums', '$rootScope', '$location', '$window', 'PositionService', PositionCtrl
    ]);

    function PositionCtrl($scope, ngTableParams, CommonFunctions, CommonEnums, $rootScope, $location, $window, PositionService) {
        var positionDetailParams = {};
        $scope.positionDetailScope = {
            PositionId: 0,
            PositionName: '',
            Description: '',
            IsActive: true
        };

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { EntryDate: 'desc' }
        }, {
            getData: function ($defer, params) {
                debugger
                if (positionDetailParams == null) {
                    positionDetailParams = {};
                }
                positionDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                positionDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                PositionService.GetAllPositions(positionDetailParams.Paging).then(function (res) {
                    debugger
                    var data = res.data;
                    $scope.position = res.data.Result;
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

        $scope.ClearFormData = function (frmRegister) {
            $scope.positionDetailScope = {
                PositionId: 0,
                PositionName: '',
                Description: '',
                IsActive: true
            };
            $scope.frmRegister.$setPristine();
            CommonFunctions.ScrollToTop();
        };

        $scope.SavePositionDetails = function (positionDetailScope) {
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
                PositionService.Register(positionDetailScope).then(function (res) {
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
        $scope.EditPositionDetails = function (PositionId) {
            //debugger
            PositionService.GetPositionById(PositionId).then(function (res) {
                debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.positionDetailScope = res.data.Result;
                        $scope.positionDetailScope.PositionId = JSON.stringify($scope.positionDetailScope.PositionId);
                        $scope.positionDetailScope.CompanyId = JSON.stringify($scope.positionDetailScope.CompanyId);
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