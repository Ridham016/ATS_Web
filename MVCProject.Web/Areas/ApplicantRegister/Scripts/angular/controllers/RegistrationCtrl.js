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
        '$scope', 'ngTableParams', 'CommonFunctions', 'CommonService', '$location','$window', '$rootScope','FileService', 'RegistrationService', RegistrationCtrl
    ]);

    function RegistrationCtrl($scope, ngTableParams, CommonFunctions, CommonService, $location, $window, $rootScope, FileService, RegistrationService) {
        var applicantDetailParams = {};
        $scope.files = [];
        $scope.headers = ["ApplicantId", "FirstName", "MiddleName", "LastName", "Email", "Phone", "Address", "DateOfBirth", "CurrentCompany", "CurrentDesignation", "ApplicantDate", "TotalExperience", "DetailedExperience", "CurrentCTC", "ExpectedCTC", "NoticePeriod", "ReasonForChange", "CurrentLocation", "PreferedLocation", "IsActive", "SkillDescription", "PortfolioLink", "LinkedinLink", "OtherLink", "FileName", "FilePath", "FileRelativePath", "Comment", "EntryBy", "EntryDate", "UpdatedBy", "UpdateDate"];
        $scope.Selectedfile = null;
        $scope.Files = null;

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
            CurrentDesignation: '',
            TotalExperience: '',
            DetailedExperience: '',
            CurrentCTC: '',
            ExpectedCTC: '',
            NoticePeriod: '',
            CurrentLocation: '',
            PreferedLocation: '',
            ReasonForChange: '',
            SkillDescription: '',
            PortfolioLink: '',
            LinkedinLink: '',
            OtherLink: '',
            ExpectedJoiningDate: null,
            PostingId: '',
            IsActive: true
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
        console.log($scope.applicantDetailScope.PostingId);
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

        $scope.Check = function (textInput, applicantDetailScope) {
            debugger
            if (textInput.disabled == 'fresher') {
                $scope.applicantDetailScope = {
                    ApplicantId: applicantDetailScope.ApplicantId,
                    FirstName: applicantDetailScope.FirstName,
                    MiddleName: applicantDetailScope.MiddleName,
                    LastName: applicantDetailScope.LastName,
                    Email: applicantDetailScope.Email,
                    Phone: applicantDetailScope.Phone,
                    Address: applicantDetailScope.Address,
                    DateOfBirth: applicantDetailScope.DateOfBirth,
                    CurrentCompany: '',
                    CurrentDesignation: '',
                    TotalExperience: '',
                    DetailedExperience: '',
                    CurrentCTC: '',
                    ExpectedCTC: applicantDetailScope.ExpectedCTC,
                    NoticePeriod: '',
                    CurrentLocation: applicantDetailScope.CurrentLocation,
                    PreferedLocation: applicantDetailScope.PreferedLocation,
                    ReasonForChange: '',
                    SkillDescription: applicantDetailScope.SkillDescription,
                    PortfolioLink: applicantDetailScope.PortfolioLink,
                    LinkedinLink: applicantDetailScope.LinkedinLink,
                    OtherLink: applicantDetailScope.OtherLink,
                    ExpectedJoiningDate: applicantDetailScope.ExpectedJoiningDate,
                    PostingId: applicantDetailScope.PostingId,
                    IsActive: true
                };
            }
        }

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
                applicantDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
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
                        //debugger
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
                TotalExperience: '',
                DetailedExperince: '',
                CurrentCTC: '',
                ExpectedCTC: '',
                NoticePeriod: '',
                CurrentLocation: '',
                PreferedLocation: '',
                ReasonForChange: '',
                SkillDescription: '',
                PortfolioLink: '',
                LinkedinLink: '',
                OtherLink: '',
                ExpectedJoiningDate: null,
                PostingId: '',
                IsActive: true
            };
            $("#file").val("");
            $scope.files = null;
            $scope.Selectedfile = null;
            $scope.Files = null;
            $scope.frmRegister.$setPristine();
            $location.search({});
            CommonFunctions.ScrollToTop();
            $('#accordionExample').find('#personal_details').addClass('show').find('.accordion-collapse').addClass('show');
            $('#accordionExample').find('#company_details').removeClass('show').find('.accordion-collapse').removeClass('show');
            $("#FirstName").focus();
        };



        $scope.SaveApplicantDetails = function (applicantDetailScope) {
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
                applicantDetailScope.DateOfBirth = angular.copy(moment(applicantDetailScope.DateOfBirth).format($rootScope.apiDateFormat));
                applicantDetailScope.ExpectedJoiningDate = angular.copy(moment(applicantDetailScope.ExpectedJoiningDate).format($rootScope.apiDateFormat));
                //applicantDetailScope.PostingId = $scope.PostingId;
                console.log(applicantDetailScope.PostingId);
                RegistrationService.Register(applicantDetailScope).then(function (res) {
                    if (res) {
                        var applicants = res.data;
                        $scope.applicantId = res.data.Result;
                        //debugger
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
            
        }

        $scope.deleteFile = function (FileId) {
            //debugger
            RegistrationService.DeleteFile(FileId).then(function (res) {
                if (res) {
                    if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                        toastr.success(res.data.Message, successTitle);
                        //debugger
                        var index = $scope.files.indexOf(FileId);
                        $scope.files.splice(index, 1);
                        $scope.Files = $scope.files.length;
                    }
                }
            })
        }

        $scope.today = new Date().toISOString().split('T')[0];
        $scope.maxDate = new Date(new Date().setFullYear(new Date().getFullYear() - 18)).toISOString().split('T')[0];


        $scope.$watch('Files', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.getFiles($scope.applicantDetailScope.ApplicantId);
            }
        }, true);

        $scope.getFiles = function (ApplicantId) {
            //debugger
            RegistrationService.GetFiles(ApplicantId).then(function (res) {
                //debugger
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
                $scope.Files = $scope.files.length;
            })
        }

        $scope.EditApplicantDetails = function (ApplicantId) {
            $location.search({});
            $scope.getFiles(ApplicantId);
            RegistrationService.GetApplicantsById(ApplicantId).then(function (res) {
                //debugger
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.applicantDetailScope = res.data.Result;
                        //$scope.applicantDetailScope.DateOfBirth = angular.copy(moment($scope.applicantDetailScope.DateOfBirth).format($rootScope.apiDateFormat));
                        $scope.applicantDetailScope.DateOfBirth = new Date($scope.applicantDetailScope.DateOfBirth);
                        $scope.applicantDetailScope.ExpectedJoiningDate = new Date($scope.applicantDetailScope.ExpectedJoiningDate);
                        $scope.applicantDetailScope.PostingId = JSON.parse($scope.applicantDetailScope.PostingId);
                        $scope.frmRegister.$setSubmitted();
                        angular.forEach($scope.frmRegister.$error, function (controls) {
                            angular.forEach(controls, function (control) {
                                control.$setDirty(); 
                            });
                        });
                        
                        CommonFunctions.ScrollUpAndFocus("FirstName");
                    } else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            })
        }

        $scope.getJobPostingList = function () {
            RegistrationService.GetJobPostingList().then(function (res) {
                $scope.JobList = res.data.Result;
                console.log($scope.JobList);
            })
        }

        $scope.AddFileToDb = function () {
            RegistrationService.AddFile($scope.filedata, $scope.applicantId).then(function (res) {
                //debugger
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
            //debugger
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
        $scope.textInput = {
            disabled: ''
        };
        $scope.downloadPDF = function (data, filename, mimeType) {
            //debugger
            FileService.SaveBlob(data, filename, mimeType).then(function (res) {
                res.data;
            })
        }

        $scope.Export = function () {
            debugger
            RegistrationService.Export($scope.headers).then(function (res) {
                //var blob = new Blob([res.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                //var filename = 'data.xlsx';
                ////(blob, filename);
                debugger
                var data = res.data;
                if (data.MessageType == messageTypes.Success) {
                    debugger
                    var filename = res.data.Result;
                    var params = { filename: filename };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "/ApplicantRegister/Registration/Download");
                    form.setAttribute("target", "_blank");

                    for (var key in params) {
                        if (params.hasOwnProperty(key)) {
                            var hiddenField = document.createElement("input");
                            hiddenField.setAttribute("type", "hidden");
                            hiddenField.setAttribute("name", key);
                            hiddenField.setAttribute("value", params[key]);
                            form.appendChild(hiddenField);
                        }
                    }
                    document.body.appendChild(form);
                    form.submit();

                    //$defer.resolve(res.data.Result);
                    if (res.data.Result.length == 0) { }
                    else {
                        params.total(res.data.Result[0].TotalRecords);
                    }
                }
                else if (res.data.MessageType == MessageType.Error) {
                    toastr.error(res.data.Message, errorTitle);
                }

                //CommonFunctions.DownloadReport('/AdvancedSearch/ExportToXl', filename);
                $rootScope.isAjaxLoadingChild = false;
                CommonFunctions.SetFixHeader();

            });
        }

        $scope.getCompanyDetails = function () {
            RegistrationService.GetCompanyDetails().then(function (res) {
                $scope.companyDetails = res.data.Result;
            })
        }

        $scope.getPostingStatus = function () {
            RegistrationService.GetPostingStatus().then(function (res) {
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
            RegistrationService.GetPositionDetails().then(function (res) {
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
                RegistrationService.PositionRegister($scope.positionDetail).then(function (res) {
                    jobpostingDetailScope.PositionId = res.data.Result;
                    RegistrationService.PostingRegister(jobpostingDetailScope).then(function (res) {
                        if (res) {
                            if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                                sessionStorage.setItem("JobPostingSuccessMessage", res.data.Message);
                                window.location.href = '../../ApplicantRegister/Registration#?PostingId=' + res.data.Result;
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
                RegistrationService.PostingRegister(jobpostingDetailScope).then(function (res) {
                    if (res) {
                        if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                            sessionStorage.setItem("JobPostingSuccessMessage", res.data.Message);
                            window.location.href = '../../ApplicantRegister/Registration#?PostingId=' + res.data.Result;
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



const optionFormat = (item) => {
    if (!item.id) {
        return item.text;
    }

    var span = document.createElement('span');
    var template = '';

    template += '<div class="d-flex align-items-center">';
    template += '<div class="d-flex flex-column">'
    template += '<span class="font-16 fw-bold lh-1">' + item.text + '</span>';
    template += '<span class="text-muted font-12">' + item.element.getAttribute('data-kt-rich-content-subcontent') + '</span>';
    template += '</div>';
    template += '</div>';

    span.innerHTML = template;

    return $(span);
}

// Init Select2 --- more info: https://select2.org/
$('#kt_docs_select2_rich_content').select2({
    minimumResultsForSearch: Infinity,
    templateSelection: optionFormat,
    templateResult: optionFormat
});
