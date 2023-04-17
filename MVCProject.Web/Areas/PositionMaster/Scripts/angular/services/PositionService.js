angular.module("MVCApp").service('PositionService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.Register = function (positionDetailScope) {
        //debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Position/Register',
            data: JSON.stringify(positionDetailScope)
        })
    }
    list.GetAllPositions = function (positionDetailParams) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Position/GetAllPositions',
            data: JSON.stringify(positionDetailParams)
        });
    }
    list.GetPositionById = function (PositionId) {
        return $http({
            method: 'Get',
            url: $rootScope.apiURL + '/Position/GetPositionById?Id=' + PositionId
        })
    }
    return list;
}])
