angular.module("MVCApp").service('JobListingService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetJobPostingList = function (jobDetailParams) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/JobListing/GetJobPostingList',
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
