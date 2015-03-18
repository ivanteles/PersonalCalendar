using PersonalCalendar.Domain;
using PersonalCalendar.Service;
using PersonalCalendar.Web.ViewModels.Events;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

namespace PersonalCalendar.Web.Controllers
{
    public class EventsController : Controller
    {
        protected readonly IEventService _service;

        public EventsController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public PartialViewResult Index(int calendarId, string calendarView = "month")
        {
            DateTime now = DateTime.UtcNow;

            // Only "month" calendar view implemented.
            DateTime firstDayUTC = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayUTC = firstDayUTC.AddMonths(1);

            var oneTimeEvents = _service.GetOneTimeEventsForCalendar(calendarId, firstDayUTC, lastDayUTC);
            var recurringEvents = _service.GetRecurringEventsForCalendar(calendarId, firstDayUTC, lastDayUTC);

            var allGroupedEvents = oneTimeEvents.Concat(recurringEvents).OrderBy(e => e.StartDateTimeUTC).GroupBy(e => e.StartDateTimeUTC.ToLocalTime().Date);

            List<EventViewModel> eventsViewModel = new List<EventViewModel>();

            foreach(var group in allGroupedEvents)
            {
                var eventViewModel = new EventViewModel { OccurenceDateTime = group.Key.ToLocalTime() };

                List<EventDetailsViewModel> eventDetails = new List<EventDetailsViewModel>();

                foreach(var evt in group)
                {
                    eventDetails.Add(Mapper.Map<EventDetailsViewModel>(evt));
                }

                eventViewModel.Details = eventDetails;

                eventsViewModel.Add(eventViewModel);
            }

            return PartialView("_Index", eventsViewModel);
        }

        [HttpGet]
        public ActionResult New(int calendarId)
        {
            var eventFormViewModel = new EventFormViewModel { CalendarId = calendarId };

            return View(eventFormViewModel);
        }

        [HttpPost]
        public ActionResult Create(EventFormViewModel eventFormViewModel)
        {
            if(eventFormViewModel.EndDateTime < eventFormViewModel.StartDateTime)
            {
                ModelState.AddModelError("EndDateTime", "Event's end time cannot be earlier than start time!");
            }

            if(ModelState.IsValid)
            {
                Event evt = new Event
                {
                    CalendarId = eventFormViewModel.CalendarId,
                    Title = eventFormViewModel.Title,
                    Location = eventFormViewModel.Location,
                    Description = eventFormViewModel.Description,
                    Color = eventFormViewModel.Color,
                    StartDateTimeUTC = eventFormViewModel.StartDateTime.ToUniversalTime(),
                    EndDateTimeUTC = eventFormViewModel.EndDateTime.ToUniversalTime(),
                    SeriesEndDateUTC = GetSeriesEndDateUTC(eventFormViewModel),
                    FreqType = eventFormViewModel.FreqType,
                    FreqSubtype = GetFrequencySubtype(eventFormViewModel),
                    FreqInterval = eventFormViewModel.FreqType != FrequencyType.OnlyOnce ? eventFormViewModel.FreqInterval : 0
                };

                _service.Create(evt);

                TempData["Message"] = "Event successfully created!";

                return RedirectToAction("Details", "Calendars", new { id = eventFormViewModel.CalendarId });
            }

            return View("New", eventFormViewModel);
        }

        private DateTime? GetSeriesEndDateUTC(EventFormViewModel eventFormViewModel)
        {
            DateTime? seriesEndDateUTC = null;

            if(eventFormViewModel.SeriesEndDate.HasValue)
            {
                seriesEndDateUTC = eventFormViewModel.SeriesEndDate.Value.ToUniversalTime();
            }
            else if(eventFormViewModel.OccurencesCount.HasValue && eventFormViewModel.OccurencesCount.Value > 0)
            {
                switch(eventFormViewModel.FreqType)
                {
                    case FrequencyType.Weekly:
                        WeeklyScheduler scheduler = new WeeklyScheduler(GetFrequencySubtype(eventFormViewModel), eventFormViewModel.FreqInterval);
                        seriesEndDateUTC = scheduler.GetOccurenceDateTime(eventFormViewModel.StartDateTime.ToUniversalTime(), eventFormViewModel.OccurencesCount.Value);
                        
                        seriesEndDateUTC = seriesEndDateUTC.Value.AddDays(1);
                        break;
                }
            }

            return seriesEndDateUTC;
        }

        private FrequencySubtype GetFrequencySubtype(EventFormViewModel eventFormViewModel)
        {
            FrequencySubtype freqSubtype = FrequencySubtype.None;

            if(eventFormViewModel.FreqType == FrequencyType.Weekly && eventFormViewModel.SelectedWeekdays != null)
            {
                int weekdays = 0;
                
                foreach(int weekday in eventFormViewModel.SelectedWeekdays)
                {
                    weekdays |= weekday;
                }

                freqSubtype = (FrequencySubtype)weekdays;
            }

            return freqSubtype;
        }
    }
}