using PersonalCalendar.Data;
using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalCalendar.Service
{
    public class EventService
    {
        protected readonly CalendarDB _calendarDB;

        public EventService(CalendarDB context)
        {
            _calendarDB = context;
        }

        public IEnumerable<Event> GetOneTimeEventsForCalendar(long calendarId, DateTime startDateUTC, DateTime endDateUTC)
        {
            IEnumerable<Event> events = _calendarDB.Events.Where(
                e => e.FreqType == FrequencyType.OnlyOnce
                && ((e.StartDateUTC >= startDateUTC && e.EndDateUTC <= endDateUTC)
                || (e.StartDateUTC <= startDateUTC && e.EndDateUTC >= endDateUTC)
                || (e.StartDateUTC >= startDateUTC && e.StartDateUTC < endDateUTC)
                || (e.EndDateUTC > startDateUTC && e.EndDateUTC <= endDateUTC))
            );

            return events;
        }

        public IEnumerable<Event> GetRecurringEventsForCalendar(long calendarId, DateTime startDateUTC, DateTime endDateUTC)
        {
            IList<Event> resultEvents = new List<Event>();

            IEnumerable<Event> events = _calendarDB.Events.Where(
                e => e.FreqType != FrequencyType.OnlyOnce
                && ((e.StartDateUTC >= startDateUTC && e.StartDateUTC < endDateUTC ) || (e.StartDateUTC <= startDateUTC))
                && (!e.SeriesEndDateUTC.HasValue 
                    || (e.SeriesEndDateUTC.HasValue && e.SeriesEndDateUTC.Value > startDateUTC ) 
                    || (e.SeriesEndDateUTC.HasValue && e.SeriesEndDateUTC.Value >= e.StartDateUTC && e.SeriesEndDateUTC.Value < e.EndDateUTC && e.EndDateUTC > startDateUTC))
            );

            foreach(var evt in events)
            {
                IEnumerable<DateTime> occurences = evt.GetSeriesOccurences(endDateUTC);

                foreach(DateTime occurence in occurences)
                {
                    DateTime eventStartDateUTC = occurence;
                    DateTime eventEndDateUTC = occurence.Add(evt.EndDateUTC - evt.StartDateUTC);

                    if (eventStartDateUTC < endDateUTC && eventEndDateUTC > startDateUTC)
                    {
                        Event e = (Event)evt.Clone();
                        
                        e.StartDateUTC = eventStartDateUTC;
                        e.EndDateUTC = eventEndDateUTC;

                        resultEvents.Add(e);
                    }
                }

            }

            return resultEvents;
        }
    }
}
