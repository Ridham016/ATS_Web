﻿(function () {
    'use strict';

    angular.module("MVCApp").controller('CalendarCtrl', [
        '$scope', 'uiCalendarConfig', '$rootScope', 'CommonEnums', 'CalendarService', CalendarCtrl
    ]);
    function CalendarCtrl($scope, uiCalendarConfig, $rootScope, CommonEnums, CalendarService) {

        $scope.SelectedEvent = null;
        var isFirstTime = true;

        $scope.events = [];
        $scope.eventSources = [$scope.events];

        $scope.DateRange = {
            StartDate: moment().startOf('month').toDate(),
            EndDate: moment().endOf('month').toDate()
        };
        

        $scope.Mode = CommonEnums.Mode;

        $scope.uiConfig = {
            calendar: {
                height: 500,
                selectable: true,
                selectHelper: true,
                editable: true,
                header: {
                    left: 'month,agendaWeek,agendaDay',
                    center: 'title',
                    right: 'today prev,next'
                }
                , dayClick: function (date) {
                    //debugger
                    var events = $scope.events.filter(function (event) {
                        return moment(event.start).isSame(date, 'day');
                    });
                    console.log(events);
                    $scope.length = Object.keys(events).length;
                    $scope.SelectedEvents = events;
                    $scope.Date = moment(date).format('DD MMM YYYY');
                }
                ,eventAfterAllRender: function (view) {
                    //debugger
                    $scope.DateRange = {
                        StartDate: moment(view.start).startOf('month').format('YYYY-MM-DD'),
                        EndDate: moment(view.end).endOf('month').format('YYYY-MM-DD')
                    };
                    //$scope.events.length = 0;
                    console.log($scope.DateRange);
                    //debugger
                    CalendarService.Calendar($scope.DateRange).then(function (data) {
                        //debugger
                        console.log($rootScope.sessionToken);
                        $scope.events = data.data.Result.map(function (value) {
                            return {
                                id: value.Id,
                                Description: value.Description,
                                Interviewer: value.InterviewerName,
                                Applicant: value.ApplicantName,
                                start: moment(value.ScheduleDateTime).toDate(),
                                Mode: value.Mode,
                                end: moment(value.ScheduleDateTime).add(1, 'hours').toDate(),
                                allDay: false
                            };
                        });
                        $('.fc-day').each(function () {
                            var eventsForDay = $scope.events.filter(function (event) {
                                return moment(event.start).isSame($(this).data('date'), 'day');
                            }.bind(this));
                            if (eventsForDay.length > 0) {
                                $(this).addClass('has-events');
                            }
                        });
                        var today = moment().startOf('day');
                        var events = $scope.events.filter(function (event) {
                            return moment(event.start).isSame(today, 'day');
                        });
                        //debugger
                        $scope.length = Object.keys(events).length;
                        $scope.SelectedEvents = events;
                        $scope.Date = moment(today).format('DD MMM YYYY');
                    });
                }
                //,eventRender: function (event, element) {
                //    return false;
                //}
            }
        };

    }
})();