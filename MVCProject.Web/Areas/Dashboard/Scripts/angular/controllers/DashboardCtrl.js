(function () {
    'use strict';

    angular.module("MVCApp").controller('DashboardCtrl', [
        '$scope', 'uiCalendarConfig', '$timeout','$interval','$rootScope','CommonEnums', 'DashboardService', DashboardCtrl
    ]);
    function DashboardCtrl($scope, uiCalendarConfig, $timeout, $interval, $rootScope, CommonEnums, DashboardService) {
        var colorPalette = ['#007bff', '#008a9b', '#a66b55', '#4680ff', '#6c757d', '#0e9e4a', '#ff2c2c', '#ffa21d'];

        $scope.getCounts = function (time) {
            $scope.timeFrame = time;
            DashboardService.GetCount(time).then(function (res) {
                if (res) {
                    if (res.data.MessageType = messageTypes.Success) {
                        $scope.Counts = res.data.Result;
                        $.fn.jQuerySimpleCounter = function (options) {
                            var settings = $.extend({
                                start: 0,
                                end: 100,
                                easing: 'swing',
                                duration: 400,
                                complete: ''
                            }, options);

                            var thisElement = $(this);

                            $({ count: settings.start }).animate({ count: settings.end }, {
                                duration: settings.duration,
                                easing: settings.easing,
                                step: function () {
                                    var mathCount = Math.ceil(this.count);
                                    thisElement.text(mathCount);
                                },
                                complete: settings.complete
                            });
                        };
                        $('#number1').jQuerySimpleCounter({ end: $scope.Counts.MeetingsScheduled, duration: 2500 });
                        $('#number2').jQuerySimpleCounter({ end: $scope.Counts.JobOpenings, duration: 3200 });
                        $('#number3').jQuerySimpleCounter({ end: $scope.Counts.ApplicantsRegistered, duration: 2500 });
                        $('#number4').jQuerySimpleCounter({ end: $scope.Counts.ApplicantsHired, duration: 3000 });

                        var optionDonut = {
                            chart: {
                                type: 'donut',
                                width: '100%',
                                height: 400
                            },
                            dataLabels: {
                                enabled: false,
                            },
                            plotOptions: {
                                pie: {
                                    customScale: 0.8,
                                    donut: {
                                        size: '60%',
                                    },
                                    offsetY: 20,
                                },
                                stroke: {
                                    colors: undefined
                                }
                            },
                            colors: colorPalette,
                            title: {
                                text: 'Applicant Status',
                                align: 'center',
                                style: {
                                    fontSize: '16px',
                                    fontWeight: 'bold',
                                    color: '#000'
                                },
                            },
                            series: [$scope.Counts.Registered, $scope.Counts.Shortlisted, $scope.Counts.Discarded, $scope.Counts.InterviewScheduled, $scope.Counts.Hold, $scope.Counts.ApplicantsHired, $scope.Counts.Rejected, $scope.Counts.InterviewCancelled],
                            labels: ['Registered', 'Shortlisted', 'Discarded', 'Interview Scheduled', 'Hold', 'Hired', 'Rejected', 'Interview Cancelled'],
                            legend: {
                                position: 'bottom'
                            }
                        }
                        var donut = new ApexCharts(
                            document.querySelector("#donut"),
                            optionDonut
                        )

                        $timeout(function () {
                        }, 200).then(function () {
                            donut.render();
                        })
                        //donut.render();
                        //window.dispatchEvent(new Event('resize'));
                    } else if (res.data.MessageType == messageTypes.Error) {
                        toastr.error(res.data.Message, errorTitle);
                    } else if (res.data.MessageType == messageTypes.Warning) {
                        toastr.warning(res.data.Message, warningTitle);
                    }
                }
            })
        }

        $scope.getStackedCount = function () {
            DashboardService.GetStackedCount().then(function (res) {
                if (res) {
                    if (res.data.MessageType = messageTypes.Success) {
                        $scope.StackedCounts = res.data.Result;
                        var labels = [],registeredData = [], hiredData = [], rejectedData = [];

                        for (var i = 0; i < $scope.StackedCounts.length; i++) {
                            labels.push($scope.StackedCounts[i].Months);
                            registeredData.push($scope.StackedCounts[i].ApplicantsRegistered);
                            hiredData.push($scope.StackedCounts[i].ApplicantsHired);
                            rejectedData.push($scope.StackedCounts[i].ApplicantsRejected);
                        }
                        console.log($scope.StackedCounts)
                        var optionsBar = {
                            chart: {
                                type: 'bar',
                                height: 380,
                                width: '100%',
                                stacked: true,
                            },
                            plotOptions: {
                                bar: {
                                    columnWidth: '45%',
                                }
                            },
                            fill: {
                                opacity: 1,
                            },
                            colors: ['#007bff', '#3ecd5e', '#e44022'],
                            series: [{
                                name: "Applicants Registered",
                                data: registeredData
                            }, {
                                name: "Accepted",
                                data: hiredData,
                            }, {
                                name: "Rejected",
                                data: rejectedData,
                            }],
                            labels: labels,
                            xaxis: {
                                labels: {
                                    show: true
                                },
                                axisBorder: {
                                    show: false
                                },
                                axisTicks: {
                                    show: false
                                },
                            },
                            yaxis: {
                                axisBorder: {
                                    show: false
                                },
                                axisTicks: {
                                    show: false
                                },
                                labels: {
                                    style: {
                                        colors: '#78909c'
                                    }
                                }
                            },
                            title: {
                                text: 'Number of Offers',
                                align: 'center',
                                style: {
                                    fontSize: '16px'
                                }
                            }

                        }

                        var chartBar = new ApexCharts(document.querySelector('#bar'), optionsBar);
                        $timeout(function () {
                        }, 200).then(function () {
                            chartBar.render();
                        })
                        //chartBar.render();

                    } else if (res.data.MessageType == messageTypes.Error) {
                        toastr.error(res.data.Message, errorTitle);
                    } else if (res.data.MessageType == messageTypes.Warning) {
                        toastr.warning(res.data.Message, warningTitle);
                    }
                }
            })
        }
    }
})();