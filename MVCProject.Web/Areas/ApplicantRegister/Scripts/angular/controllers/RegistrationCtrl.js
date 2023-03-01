(function () {
    'use strict';

    angular.module("MVCApp").directive('fileModel', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.bind('change', function () {
                    $parse(attrs.fileModel).assign(scope, element[0].files)
                    scope.$apply();
                });
            }
        };
    }]).controller('RegistrationCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions','$rootScope', 'RegistrationService', RegistrationCtrl
    ]);

    function RegistrationCtrl($scope, ngTableParams, CommonFunctions, $rootScope, RegistrationService) {
        var applicantDetailParams = {};
        $scope.applicantDetailScope = {
            ApplicantId: 0,
            FirstName: '',
            MiddleName: '',
            LastName: '',
            Email: '',
            Phone: '',
            Address: '',
            DateOfBirth: null,
            CurrentCompany: '',
            CurrentDesignations: '',
            TotalExperience: null,
            DetailedExperince: null,
            CurrentCTC: null,
            ExpectedCTC: null,
            NoticePeriod: null,
            CurrentLocation: '',
            PreferedLocation: '',
            ReasonForChange : '',
            IsActive: true
        };

        //$scope.AllData = true;

        $scope.getApplicants = function (IsGetAll) {
            $scope.IsGetAll = IsGetAll;
            $scope.tableParams.reload();
        }

        $scope.getAllApplicants = function () {
            RegistrationService.GetAllApplicants().then(function (res) {
                $scope.applicants = res.data.Result;
                console.log($scope.applicants);
                debugger
            });
        };
        //Load Designation List
        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize
        }, {
            getData: function ($defer, params) {
                if (applicantDetailParams == null) {
                    applicantDetailParams = {};
                }
                applicantDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                debugger
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                if ($scope.IsGetAll) {
                    RegistrationService.GetApplicantList(applicantDetailParams.Paging, $scope.IsGetAll).then(function (res) {
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
                    else {
                    RegistrationService.GetAllApplicants(applicantDetailParams.Paging).then(function (res) {
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
            }
        });

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
                RegistrationService.GetApplicantList(applicantDetailParams.Paging).then(function (res) {
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

        //$scope.getApplicantList = function (isGetAll) {
        //    RegistrationService.GetApplicantList(isGetAll).then(function (res) {
        //        $scope.applicants = res.data.Result;
        //    });
        //}

        $scope.ClearFormData = function (frmRegister) {
            $scope.applicantDetailScope = {
                ApplicantId: 0,
                FirstName: '',
                MiddleName: '',
                LastName: '',
                Email: '',
                Phone: '',
                Address: '',
                DateOfBirth: null,
                CurrentCompany: '',
                CurrentDesignations: '',
                TotalExperience: null,
                DetailedExperince: null,
                CurrentCTC: null,
                ExpectedCTC: null,
                NoticePeriod: null,
                CurrentLocation: '',
                PreferedLocation: '',
                ReasonForChange: '',
                IsActive: true
            };
            debugger
            $scope.frmRegister.$setPristine();
            $("FirstName").focus();
            CommonFunctions.ScrollToTop();
        };
        $scope.SaveApplicantDetails = function (applicantDetailScope) {
            debugger
            RegistrationService.Register(applicantDetailScope).then(function (res) {
                if (res) {
                    debugger
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
            RegistrationService.GetApplicantsById(ApplicantId).then(function (res) {
                if (res) {
                    debugger
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.applicantDetailScope = res.data.Result;
                        $scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth);
                        CommonFunctions.ScrollUpAndFocus("FirstName");
                    } else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            })
        }

        //$scope.UploadFile = function (files) {
        //    $scope.SelectedFiles = files;
        //    if ($scope.SelectedFiles && $scope.SelectedFiles.lenght) {
        //        Upload.upload({
        //            url: 'http://localhost:56562/api/Registrations/Upload',
        //            data: {
        //                files: $scope.SelectedFiles
        //            }
        //        }).then(function (res) {
        //            if (res.status > 0) {
        //                var errormsg = res.status + ":" + res.data.Result;
        //            }
        //        })
        //    }
        //}
        $scope.uploadFile = function () {
            var fd = new FormData();
            console.log($scope.files);
            angular.forEach($scope.files, function (file) {
                fd.append('file', file);
            });
            RegistrationService.Upload(fd).success(function (d) {
                console.log(d);
            })  
            debugger
        }
    }
})();