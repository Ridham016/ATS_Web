angular.module("MVCApp").service('AdvancedSearchService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    //list.AdvancedSearch = function (applicantDetailScope) {
    //    return $http({
    //        method: 'GET',
    //        url: $rootScope.apiURL + '/AdvancedSearch/AdvancedSearch?CurrentStatusId=' + StatusId + '&StartDate=' + Entryby + '&EndDate=' + EndDate,
    //        /*data: JSON.stringify(applicantDetailParams)*/
    //    })
    //}
    list.AdvancedSearch = function (searchDetailParams, searchDetail) {
        //debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/AdvancedSearch/AdvancedActionSearch?StatusId=' + searchDetail.StatusId + '&StartDate=' + searchDetail.StartDate + '&EndDate=' +
                searchDetail.EndDate + '&CompanyId=' + searchDetail.CompanyId + '&PositionId=' + searchDetail.PositionId,
            data: JSON.stringify(searchDetailParams)
        })
    }
    list.GetCompanyDetails = function () {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/AdvancedSearch/GetCompanyDetails'
        });
    }
    list.GetPositionDetails = function () {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/AdvancedSearch/GetPositionDetails'
        });
    }
    list.GetStatus = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/AdvancedSearch/GetStatus'
        });
    }
    list.ApplicantTimeline = function (ApplicantId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/AdvancedSearch/ApplicantTimeline?ApplicantId=' + ApplicantId
        });
    }
    list.Export = function (headers, searchDetail) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/AdvancedSearch/ExportToXl?headers=' + headers.join(',') + '&StatusId=' + searchDetail.StatusId + '&StartDate=' + searchDetail.StartDate + '&EndDate=' + searchDetail.EndDate +
                searchDetail.EndDate + '&CompanyId=' + searchDetail.CompanyId + '&PositionId=' + searchDetail.PositionId,
            data: JSON.stringify(searchDetail)
        });
    }
    return list;
}])
