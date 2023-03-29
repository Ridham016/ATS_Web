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
        DashboardService.Calendar().then(function (data) {
            angular.forEach(data.data.Result, function (value) {
                $scope.events.push({
                    Description: value.Description,
                    Interviewer: value.InterviewerName,
                    Applicant: value.ApplicantName,
                    start: moment(value.ScheduleDateTime).toDate(),
                    end: moment(value.ScheduleDateTime).add(2,'hours').toDate()
                });
            });
        });

        $scope.uiConfig = {
            calendar: {
                height: 500,
                editable: false,
                displayEventTime: false,
                header: {
                    left: '',
                    center: "March",
                    right: ''
                }
                //,eventClick: function (event) {
                //    $scope.SelectedEvent = event;
                //}
                ,viewRender: function (view) {
                    $('.fc-day').removeClass('has-events');
                    $('.fc-day-number').removeClass('event-count');
                    $('.fc-day-number').removeAttr('data-events');
                    $scope.events.forEach(function (event) {
                        var start = moment(event.start).startOf('day');
                        var end = moment(event.end).startOf('day');
                        var date = start;
                        while (date.isSameOrBefore(end)) {
                            var cell = $('.fc-day[data-date="' + date.format('YYYY-MM-DD') + '"]');
                            if (cell.length > 0) {
                                cell.addClass('has-events');
                                if (!cell.find('.fc-day-number').hasClass('event-count')) {
                                    cell.find('.fc-day-number').addClass('event-count');
                                }
                                var eventsCount = cell.find('.fc-day-number').attr('data-events');
                                if (eventsCount) {
                                    eventsCount++;
                                } else {
                                    eventsCount = 1;
                                }
                                cell.find('.fc-day-number').attr('data-events', eventsCount);
                                cell.find('.fc-day-number').text(eventsCount);
                            }
                            date.add(1, 'days');
                        }
                    });
                },
                dayClick: function (date, jsEvent, view) {
                    // Filter the events for the clicked day
                    var events = $scope.events.filter(function (event) {
                        return moment(event.start).isSame(date, 'day');
                    });
                    // Display the events however you like
                    console.log(events);
                    $scope.SelectedEvents = events;
                },
                eventRender: function (event, element) {
                    // Hide the events from the calendar
                    return false;
                }
            }
        };

    }
})();