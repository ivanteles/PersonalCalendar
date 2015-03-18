using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace PersonalCalendar.Web.ViewModels.Events
{
    public class EventFormViewModel
    {
        public int Id { get; set; }

        public int CalendarId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        [Required]
        [DisplayName("Start")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DisplayName("End")]
        public DateTime EndDateTime { get; set; }

        [DisplayName("Frequency")]
        public FrequencyType FreqType { get; set; }

        public IEnumerable<SelectListItem> FrequencyTypes { get; private set; }

        [DisplayName("Interval")]
        public int FreqInterval { get; set; }

        public IEnumerable<SelectListItem> FrequencyIntervals { get; private set; }

        public IList<int> SelectedWeekdays { get; set; }

        public IEnumerable<SelectListItem> Weekdays { get; private set; }

        public int? OccurrencesCount { get; set; }

        public DateTime? SeriesEndDate { get; set; }

        public EventFormViewModel()
        {
            DateTime now = DateTime.Now.Date;

            StartDateTime = now;
            EndDateTime = now.AddDays(1);

            FrequencyTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Once", Value = "0" },
                new SelectListItem { Text = "Weekly", Value = "16" }
            };

            var freqIntervals = new List<SelectListItem>();
            var range = Enumerable.Range(1, 30);

            foreach(int item in range)
            {
                freqIntervals.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            FrequencyIntervals = freqIntervals;

            Weekdays = new List<SelectListItem>
            {
                new SelectListItem { Text = "Monday", Value = "1" },
                new SelectListItem { Text = "Tuesday", Value = "2" },
                new SelectListItem { Text = "Wednesday", Value = "4" },
                new SelectListItem { Text = "Thursday", Value = "8" },
                new SelectListItem { Text = "Friday", Value = "16" },
                new SelectListItem { Text = "Saturday", Value = "32" },
                new SelectListItem { Text = "Sunday", Value = "64" }
            };
        }
    }
}