angular.module("MVCApp").service('JobListingService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetJobPostingList = function (jobpostingDetailScope) {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/JobListing/GetJobPostingList'
        })
    }
    return list;
}])
