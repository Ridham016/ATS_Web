(function () {
    'use strict';

    angular.module("MVCApp").controller('DashboardCtrl', [
        '$scope', 'uiCalendarConfig', 'DashboardService', DashboardCtrl
    ]);
    function DashboardCtrl($scope, uiCalendarConfig, DashboardService) {

        $scope.SelectedEvent = null;
        var isFirstTime = true;

        $scope.events = [];
        $scope.eventSources = [$scope.events];

        $scope.DateRange = {
            StartDate: moment().startOf('month').toDate(),
            EndDate: moment().endOf('month').toDate()
        };

        //$scope.Calendar = function () {
        //    debugger
        //    DashboardService.Calendar().then(function (data) {
        //        console.log(data.data.Result);
        //        angular.forEach(data.data.Result, function (value) {
        //            debugger
        //            $scope.events.push({
        //                title : 'Title',
        //                description: value.Description,
        //                start: moment(value.ScheduleDateTime).toDate(),
        //                end: moment(value.ScheduleDateTime).toDate()
        //            });
        //        });
        //        console.log($scope.events);
        //    });
        //}
        
        DashboardService.Calendar($scope.DateRange).then(function (data) {
            $scope.events = data.data.Result.map(function (value) {
                return {
                    id: value.Id,
                    Description: value.Description,
                    Interviewer: value.InterviewerName,
                    Applicant: value.ApplicantName,
                    start: moment(value.ScheduleDateTime).toDate(),
                    end: moment(value.ScheduleDateTime).add(1, 'hours').toDate()
                };
            });
        });
            
        $scope.uiConfig = {
            calendar: {
                height: 500,
                selectable: true,
                selectHelper: true,
                editable: true,
                displayEventTime: false,
                header: {
                    left: 'month,agendaWeek,agendaDay',
                    center: 'title',
                    right: 'today prev,next'
                }
                ,events: function (info) {
                    $scope.DateRange = {
                        StartDate: moment(info.start).startOf('month').toDate(),
                        EndDate: moment(info.end).endOf('month').toDate()
                    };
                    DashboardService.Calendar($scope.DateRange).then(function (data) {
                        debugger
                        $scope.events = data.data.Result.map(function (value) {
                            return {
                                id: value.Id,
                                Description: value.Description,
                                Interviewer: value.InterviewerName,
                                Applicant: value.ApplicantName,
                                start: moment(value.ScheduleDateTime).toDate(),
                                end: moment(value.ScheduleDateTime).add(1, 'hours').toDate()
                            };
                        });
                        $('.fc').fullCalendar('render');
                        $('.fc').fullCalendar('option', 'eventAfterAllRender')(null, null, null);
                        console.log($scope.events);
                    })
                }
                , viewRender: function (view, element) {
                    $scope.DateRange = {
                        StartDate: view.start.toDate(),
                        EndDate: view.end.toDate()
                    };
                    console.log($scope.DateRange);
                    DashboardService.Calendar($scope.DateRange).then(function (data) {
                        debugger
                        $scope.events = data.data.Result.map(function (value) {
                            return {
                                id: value.Id,
                                Description: value.Description,
                                Interviewer: value.InterviewerName,
                                Applicant: value.ApplicantName,
                                start: moment(value.ScheduleDateTime).toDate(),
                                end: moment(value.ScheduleDateTime).add(1, 'hours').toDate()
                            };
                        });
                    });
                }
                ,dayClick: function (date, jsEvent, view) {
                    debugger
                    var events = $scope.events.filter(function (event) {
                        return moment(event.start).isSame(date, 'day');
                    });
                    console.log(events);
                    $scope.SelectedEvents = events;
                }
                ,eventAfterAllRender: function (view) {
                    debugger
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
                    console.log(events);
                    $scope.SelectedEvents = events;
                }
                ,eventRender: function (event, element) {
                    return false;
                }
            }
        };

    }
})();