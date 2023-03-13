(function () {
    'use strict';

    angular.module("MVCApp").directive('fileOnChange', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var onChangeHandler = scope.$eval(attrs.fileOnChange);
                element.bind('change', onChangeHandler);
            }
        };
    }).controller('RegistrationCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', 'FileService', 'RegistrationService', RegistrationCtrl
    ]);

    function RegistrationCtrl($scope, ngTableParams, CommonFunctions, $rootScope, FileService, RegistrationService) {
        var applicantDetailParams = {};
        $scope.accordionGroup_1 = true;
        $scope.accordionGroup_2 = false;
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

        //$scope.AllData = true;

        $scope.getApplicants = function (IsGetAll) {
            $scope.IsGetAll = IsGetAll;
            $scope.tableParams.reload();
        }

        $scope.getAllApplicants = function () {
            RegistrationService.GetAllApplicants().then(function (res) {
                $scope.applicants = res.data.Result;
                console.log($scope.applicants);
            });
        };
        //Load Designation List
        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { FirstName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (applicantDetailParams == null) {
                    applicantDetailParams = {};
                }
                applicantDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                if ($scope.IsGetAll) {
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
                else {
                    RegistrationService.GetAllApplicants(applicantDetailParams.Paging).then(function (res) {
                        var data = res.data;
                        $scope.applicants = res.data.Result;
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
            $scope.frmRegister.$setPristine();
            CommonFunctions.ScrollToTop();
            $("FirstName").focus();
        };
        $scope.SaveApplicantDetails = function (applicantDetailScope) {
            debugger
            console.log(applicantDetailScope.DateOfBirth);
            RegistrationService.Register(applicantDetailScope).then(function (res) {
                if (res) {
                    var applicants = res.data;
                    $scope.applicantId = res.data.Result;
                    debugger
                    RegistrationService.AddFile($scope.filedata, $scope.applicantId).then(function (res) {
                        debugger
                        console.log(res.data.Result);
                    })
                    if (applicants.MessageType == messageTypes.Success && applicants.IsAuthenticated) {
                        toastr.success(applicants.Message, successTitle);
                        $scope.ClearFormData(frmRegister);
                        $("#file").val("");
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

        $scope.AddFileToDb = function () {
            RegistrationService.AddFile($scope.filedata, $scope.applicantId).then(function (res) {
                console.log(res.data.Result);
            })
        }

        $scope.checkFile = function (event) {
            var fileInput = event.target.files[0];

            var allowedExtensions =
                /(\.pdf)$/i;

            if (!allowedExtensions.exec(fileInput)) {
                alert('Invalid file type');
                $('#file').val("");
            }
        }

        $scope.uploadFile = function () {
            debugger
            var fileInput = document.getElementById('file');
            if (fileInput.files.length === 0) return;

            var file = fileInput.files[0];

            var payload = new FormData();
            payload.append("file", file);


            RegistrationService.uploadFile(payload).then(function (response) {
                console.log(response);
                $scope.filedata = response.data.Result;
            });
        }
    }
})();

