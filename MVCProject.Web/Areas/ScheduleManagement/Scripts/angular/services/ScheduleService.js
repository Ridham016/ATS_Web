angular.module("MVCApp").service('ScheduleService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetApplicantList = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Schedules/GetApplicantList',
            data: JSON.stringify(applicantDetailScope)
        });
    };
    list.GetApplicant = function (applicantId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Schedules/GetApplicant?ApplicantId=' + applicantId
        });
    }
    list.GetButtons = function (statusId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Schedules/GetButtons?StatusId=' + statusId
        });
    }
    list.UpdateButton = function (statusId,applicantId) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Schedules/UpdateStatus?ApplicantId=' + applicantId + '&StatusId=' + statusId,
            data: JSON.stringify(statusId, applicantId)
        });
    }
    list.GetAllInterviewers = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Schedules/GetInterviewers'
        })
    }
    list.Schedule = function (scheduleDetailScope) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Schedules/ScheduleInterview',
            data: JSON.stringify(scheduleDetailScope)
        })
    }
    list.GetOtherReasons = function () {
        debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Schedules/GetReasons'
        })
    }
    list.UpdateOtherReason = function (reasonId, actionId) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Schedules/UpdateReason?ActionId=' + actionId,
            data: JSON.stringify(reasonId, actionId)
        })
    }
    list.HoldReason = function (Hold, actionId) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Schedules/HoldReason?ActionId=' + actionId,
            data: JSON.stringify(Hold, actionId)
        })
    }
    return list;
}])
