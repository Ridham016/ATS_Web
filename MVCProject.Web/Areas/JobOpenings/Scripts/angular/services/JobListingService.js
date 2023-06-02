angular.module("MVCApp").service('JobListingService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetJobPostingList = function (jobDetailParams,searchDetail) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/JobListing/GetJobPostingList_WEB?StartDate=' + searchDetail.StartDate + '&EndDate=' + searchDetail.EndDate,
            data: JSON.stringify(jobDetailParams)
        })
    }

    list.GetDescription = function (PostingId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/JobListing/GetDescription?PostingId=' + PostingId
        })
    }
    return list;
}])
