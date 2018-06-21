'use strict';
var d ;
var date = new Date();
var m = date.getMonth();
var y = date.getFullYear();

$.ajaxSetup({ cache: false });
$.getJSON('/Entry/json', function (date) {
    d = date;
    console.log(date);
});

/*-----------------------------------------------------
    Main Calendar
------------------------------------------------------*/
$(document).ready(function () {
    if ($('#calendar')[0]) {

        var target = $('#calendar');

        target.fullCalendar({
            header: {
                right: '',
                center: '',
                left: ''
            },
            buttonIcons: {
                prev: 'calendar__prev',
                next: 'calendar__next'
            },
            
            theme: true,
            selectable: true,
            selectHelper: true,
            editable: true,

            dayClick: function (date) {
                $('#new-event').modal('show');
                $('#addNew-event input:text').val('');
                var isoDate = moment(date).toISOString();
                $('.new-event__start').val(isoDate);
                $('.new-event__end').val(isoDate);
            },

            viewRender: function (view) {
                var calendarDate = $("#calendar").fullCalendar('getDate');
                var calendarMonth = calendarDate.month();

                //Set data attribute for header. This is used to switch header images using css
                $('#calendar .fc-toolbar').attr('data-calendar-month', calendarMonth);

                //Set title in page header
                $('.content__header--calendar > h2').html(view.title);
            },

            eventClick: function (event, element) {

                $('.edit-event__id').val(event.id);
                $('.edit-event__title').val(event.title);
                $('.edit-event__description').val(event.description);
                $('#edit-event').modal('show');
            }
        });
        console.log(d);
        target.fullCalendar('renderEvent', d, 'stick');

        //Add new Event
        $('body').on('click', '#addEvent', function () {
            var eventTitle = $('.new-event__title').val();

            var GenRandom = {
                Stored: [],
                Job: function () {
                    var newId = Date.now().toString().substr(6); // or use any method that you want to achieve this string

                    if (!this.Check(newId)) {
                        this.Stored.push(newId);
                        return newId;
                    }
                    return this.Job();
                },
                Check: function (id) {
                    for (var i = 0; i < this.Stored.length; i++) {
                        if (this.Stored[i] == id) return true;
                    }
                    return false;
                }
            };

            if (eventTitle != '') {
                $('#calendar').fullCalendar('renderEvent', {
                    id: GenRandom.Job(),
                    title: eventTitle,
                    start: $('.new-event__start').val(),
                    end: $('.new-event__end').val(),
                    allDay: true,
                    className: $('.color-tag input:checked').val()

                }, true);

                $('.new-event__form')[0].reset()
                $('#new-event').modal('hide');
            }
            else {
                $('.new-event__title').closest('.form-group').addClass('has-error');
                $('.new-event__title').focus();
            }
        });


        //Update/Delete an Event
        $('body').on('click', '[data-calendar]', function () {
            var calendarAction = $(this).data('calendar');
            var currentId = $('.edit-event__id').val();
            var currentTitle = $('.edit-event__title').val();
            var currentDesc = $('.edit-event__description').val();
            var currentEvent = $("#calendar").fullCalendar('clientEvents', currentId);

            //Update
            if (calendarAction === 'update') {
                if (currentTitle != '') {
                    currentEvent[0].title = currentTitle;
                    currentEvent[0].description = currentDesc;

                    $('#calendar').fullCalendar('updateEvent', currentEvent[0]);
                    $('#edit-event').modal('hide');
                }
                else {
                    $('.edit-event__title').closest('.form-group').addClass('has-error');
                    $('.edit-event__title').focus();
                }
            }

            //Delete
            if (calendarAction === 'delete') {
                $('#edit-event').modal('hide');
                setTimeout(function () {
                    swal({
                        title: 'Удалить?',
                        text: "You won't be able to revert this!",
                        type: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes, delete it!'
                    }).then(function () {
                        target.fullCalendar('removeEvents', currentId);
                        swal(
                            'Deleted!',
                            'Your list has been deleted.',
                            'success'
                        );
                    })
                }, 200);
            }
        });


        //Calendar views switch
        $('body').on('click', '[data-calendar-view]', function (e) {
            e.preventDefault();

            $('[data-calendar-view]').removeClass('active');
            $(this).addClass('active');
            var calendarView = $(this).attr('data-calendar-view');
            target.fullCalendar('changeView', calendarView);
        });


        //Calendar Next
        $('body').on('click', '.actions__calender-next', function (e) {
            e.preventDefault();
            target.fullCalendar('next');
        });


        //Calendar Prev
        $('body').on('click', '.actions__calender-prev', function (e) {
            e.preventDefault();
            target.fullCalendar('prev');
        });
    }



    
});