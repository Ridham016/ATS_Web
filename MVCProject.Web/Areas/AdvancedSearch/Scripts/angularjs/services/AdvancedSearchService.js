angular.module("MVCApp").service('AdvancedSearchService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    //list.AdvancedSearch = function (applicantDetailScope) {
    //    return $http({
    //        method: 'GET',
    //        url: $rootScope.apiURL + '/AdvancedSearch/AdvancedSearch?CurrentStatusId=' + StatusId + '&StartDate=' + Entryby + '&EndDate=' + EndDate,
    //        /*data: JSON.stringify(applicantDetailParams)*/
    //    })
    //}
    list.AdvancedSearch = function (searchScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/AdvancedSearch/AdvancedActionSearch?StatusId=' + searchScope.StatusId + '&StartDate=' + searchScope.StartDate + '&EndDate=' +searchScope.EndDate ,
            /*data: JSON.stringify(applicantDetailParams)*/
            data: JSON.stringify(searchScope)
        })
    }
    list.GetStatus = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/AdvancedSearch/GetStatus'
        });
    }
    return list;
}])
