(function () {
    'use strict';

    angular.module("MVCApp").controller('AdvancedSearchCtrl', [
        '$scope', 'ngTableParams', 'CommonFunctions', '$rootScope', 'AdvancedSearchService', AdvancedSearchCtrl
    ]);

    function AdvancedSearchCtrl($scope, ngTableParams, CommonFunctions, $rootScope, AdvancedSearchService) {
        var searchDetailParams = {};
        $scope.headers = ["ApplicantId", "FirstName", "MiddleName", "LastName", "Email", "Phone", "Address", "DateOfBirth", "CurrentCompany", "CurrentDesignation", "ApplicantDate", "TotalExperience", "DetailedExperience", "CurrentCTC", "ExpectedCTC", "NoticePeriod", "ReasonForChange", "CurrentLocation", "PreferedLocation", "SkillDescription", "PortfolioLink", "LinkedinLink", "OtherLink", "ExpectedJoiningDate", "StatusName", "Comment", "Reason", "EntryDate"];
        $scope.CurrentDate = new Date();
        //$scope.searchDetail.daterange.startDate = null;
        //$scope.searchDetail.daterange.endDate = null;
        $scope.CurrentDate = angular.copy(moment($scope.CurrentDate).format("YYYY-MM-DD"));
        $scope.searchDetail = {
            StatusId: null,
            StartDate: null,
            EndDate: null,
            daterange: { startDate: null, endDate: null }
        };
        angular.extend($scope.searchDetail.daterange, { startDate: null, endDate: null });

        $scope.statusDetailScope = {
            StatusId: null,
            StatusName: null
        };

        $scope.expandSelected = function (applicant, ApplicantId) {
            AdvancedSearchService.ApplicantTimeline(ApplicantId).then(function (res) {
                //debugger
                $scope.applicantDetail = res.data.Result;
            })
            $scope.applicants.forEach(function (val) {
                val.expanded = false;
            })
            //debugger
            applicant.expanded = true;
            console.log(applicant.expanded);

        }
        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize
        }, {
            getData: function ($defer, params) {
                debugger
                if (searchDetailParams == null) {
                    searchDetailParams = {};
                }
                //searchDetailParams.paging = CommonFunctions.GetPagingParams(params);
                searchDetailParams.paging = CommonFunctions.GetPagingParams(params);
                searchDetailParams.paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //designationDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                AdvancedSearchService.AdvancedSearch(searchDetailParams.paging, $scope.searchDetail).then(function (res) {
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

        $scope.ClearFormData = function (frmRegister) {
            //debugger
            $scope.searchDetail = {
                StatusId: null,
                StartDate: null,
                EndDate: null,
                daterange: { startDate: null, endDate: null }
            };
            $("#DateRange").val("");
            frmRegister.$setPristine();
        };

        $scope.advancedsearch = function (searchDetail) {
            debugger
            if (searchDetail.daterange.startDate && searchDetail.daterange.endDate) {
                if (moment.isMoment(searchDetail.daterange.startDate) && moment.isMoment(searchDetail.daterange.endDate)) {
                    searchDetail.daterange.startDate = null;
                    searchDetail.daterange.endDate = null;
                } else {
                    searchDetail.StartDate = angular.copy(moment(searchDetail.daterange.startDate).format($rootScope.apiDateFormat));
                    searchDetail.EndDate = angular.copy(moment(searchDetail.daterange.endDate).format($rootScope.apiDateFormat));
                }
            } else {
                searchDetail.StartDate = null;
                searchDetail.EndDate = null;
            }
            $scope.searchDetail = searchDetail;
            debugger
            $scope.tableParams.page(1);
            $scope.tableParams.reload();

        };
        $scope.GetStatus = function () {
            //debugger
            AdvancedSearchService.GetStatus().then(function (res) {
                var data = res.data;
                $scope.status = res.data.Result;
            });
        };
        $scope.exportData = function (searchDetail) {
            debugger
            AdvancedSearchService.Export($scope.headers, searchDetail)
                .then(function (res) {
                    debugger
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        debugger
                        var filename = res.data.Result;
                        var params = { filename: filename };
                        var form = document.createElement("form");
                        form.setAttribute("method", "POST");
                        form.setAttribute("action", "/AdvancedSearch/Search/Download");
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
        };
    }
})();