var EventsNew = {
    initialize: function () {
        $(function () {
            EventsNew.toggleRecurringEventPanel($('#FreqType'));

            $('#StartDateTime, #EndDateTime').datetimepicker({ format: 'Y-m-d H:i:s' });
            $('#SeriesEndDate').datetimepicker({ format: 'Y-m-d' });

            $('#FreqType').change(function () {
                EventsNew.toggleRecurringEventPanel($(this));
            });

            $('input[name="EventSeriesFinish"]').change(function () {
                switch ($(this).val()) {
                    case 'never':
                        $('#OccurencesCount').prop('disabled', true);
                        $('#SeriesEndDate').prop('disabled', true);
                        break;

                    case 'occurences':
                        $('#OccurencesCount').prop('disabled', false);
                        $('#SeriesEndDate').prop('disabled', true);
                        break;

                    case 'date':
                        $('#OccurencesCount').prop('disabled', true);
                        $('#SeriesEndDate').prop('disabled', false);
                        break;
                }
            });
        });
    },

    toggleRecurringEventPanel: function ($freqTypeDropdown) {
        if ($freqTypeDropdown.val() == 0) {
            $('.recurring-event-panel').hide();
        } else {
            $('.recurring-event-panel').show();
        }
    }
};