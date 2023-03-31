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
        '$scope', 'ngTableParams', 'CommonFunctions','$timeout', '$rootScope','FileService', 'RegistrationService', RegistrationCtrl
    ]);

    function RegistrationCtrl($scope, ngTableParams, CommonFunctions, $timeout, $rootScope, FileService, RegistrationService) {
        var applicantDetailParams = {};
        $scope.applicantDetailScope = {
            ApplicantId: 0,
            FirstName: '',
            MiddleName: '',
            LastName: '',
            Email: '',
            Phone: '',
            Address: '',
            DateOfBirth: new Date(new Date().setFullYear(new Date().getFullYear() - 18)),
            CurrentCompany: '',
            CurrentDesignation: '',
            TotalExperience: '',
            DetailedExperience: '',
            CurrentCTC: '',
            ExpectedCTC: '',
            NoticePeriod: '',
            CurrentLocation: '',
            PreferedLocation: '',
            ReasonForChange: '',
            IsActive: true
        };


        checkValidationRules();

        function checkValidationRules() {
            debugger
            $timeout(function () {
                if ($scope.applicantDetailScope.TotalExperience > 50) {
                    $scope.frmRegister.TotalExperience.$setValidity('max', false);
                }
                if ($scope.applicantDetailScope.DetailedExperience > 50) {
                    $scope.frmRegister.DetailedExperience.$setValidity('max', false);
                }
            })
        }

        $scope.dateOptions = {
            maxDate: new Date(new Date().setFullYear(new Date().getFullYear() - 18)),
            datepickerMode: 'day'
        };
        $scope.setDefaultDate = function () {
            $scope.applicantDetailScope.DateOfBirth = new Date(new Date().setFullYear(new Date().getFullYear() - 18));
        }
        $scope.isOpen = false;

        //$scope.AllData = true;
        $scope.today = new Date();

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

        //var table = $('#datatable').DataTable({
        //    fixedColumns: {
        //        left: 2,
        //        right: 2
        //    }
        //});
        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { ApplicantDate: 'desc' }
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
                        console.log($scope.applicants);
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
                DateOfBirth: new Date(new Date().setFullYear(new Date().getFullYear() - 18)),
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
            $("#file").val("");
            $scope.files = null;
            $scope.Selectedfile = null;
            $scope.frmRegister.$setPristine();
            CommonFunctions.ScrollToTop();
            $('#accordionExample').find('#personal_details').addClass('show').find('.accordion-collapse').addClass('show');
            $('#accordionExample').find('#company_details').removeClass('show').find('.accordion-collapse').removeClass('show');
            $("#FirstName").focus();
        };
        $scope.SaveApplicantDetails = function (applicantDetailScope) {
            debugger
            applicantDetailScope.DateOfBirth = angular.copy(moment(applicantDetailScope.DateOfBirth).format($rootScope.apiDateFormat));
            RegistrationService.Register(applicantDetailScope).then(function (res) {
                if (res) {
                    var applicants = res.data;
                    $scope.applicantId = res.data.Result;
                    debugger
                    if ($scope.filedata) {
                         RegistrationService.AddFile($scope.filedata, $scope.applicantId).then(function (res) {
                             if (applicants.MessageType == messageTypes.Success && applicants.IsAuthenticated) {
                                 toastr.success(applicants.Message, successTitle);
                                 $scope.ClearFormData(frmRegister);
                                 $scope.tableParams.reload();
                             } else if (applicants.MessageType == messageTypes.Error) {// Error
                                 toastr.error(applicants.Message, errorTitle);
                             } else if (applicants.MessageType == messageTypes.Warning) {// Warning
                                 toastr.warning(applicants.Message, warningTitle);
                             }
                    })
                    }
                    else {
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
}
                
            });
        }

        $scope.getFiles = function (ApplicantId) {
            debugger
            RegistrationService.GetFiles(ApplicantId).then(function (res) {
                debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.files = res.data.Result;
                        $scope.Selectedfile = res.data.Result[0];
                    } else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
                $scope.files = res.data.Result;
            })
        }

        $scope.EditApplicantDetails = function (ApplicantId) {
            debugger
            RegistrationService.GetApplicantsById(ApplicantId).then(function (res) {
                debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.applicantDetailScope = res.data.Result;
                        //$scope.applicantDetailScope.DateOfBirth = angular.copy(moment($scope.applicantDetailScope.DateOfBirth).format($rootScope.apiDateFormat));
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
                debugger
                $scope.ClearFormData(frmRegister);
                $("#file").val("");
                $scope.tableParams.reload();
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

        $scope.downloadPDF = function (data, filename, mimeType) {
            debugger
            FileService.SaveBlob(data, filename, mimeType).then(function (res) {
                res.data;
            })
        }
    }
})();

