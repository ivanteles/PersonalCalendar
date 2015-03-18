using PersonalCalendar.Data;
using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalCalendar.Service
{
    public interface IEventService : ICrudService<Event>
    {
        IEnumerable<Event> GetOneTimeEventsForCalendar(long calendarId, DateTime startDateTimeUTC, DateTime endDateTimeUTC);

        IEnumerable<Event> GetRecurringEventsForCalendar(long calendarId, DateTime startDateTimeUTC, DateTime endDateTimeUTC);
    }

    public class EventService : CrudService<Event>, IEventService
    {
        public EventService(CalendarDB context)
            : base(context) { }

        public IEnumerable<Event> GetOneTimeEventsForCalendar(long calendarId, DateTime startDateTimeUTC, DateTime endDateTimeUTC)
        {
            IEnumerable<Event> events = _calendarDB.Events.Where(
                e => e.FreqType == FrequencyType.OnlyOnce
                && ((e.StartDateTimeUTC >= startDateTimeUTC && e.EndDateTimeUTC <= endDateTimeUTC)
                || (e.StartDateTimeUTC <= startDateTimeUTC && e.EndDateTimeUTC >= endDateTimeUTC)
                || (e.StartDateTimeUTC >= startDateTimeUTC && e.StartDateTimeUTC < endDateTimeUTC)
                || (e.EndDateTimeUTC > startDateTimeUTC && e.EndDateTimeUTC <= endDateTimeUTC))
            );

            return events;
        }

        public IEnumerable<Event> GetRecurringEventsForCalendar(long calendarId, DateTime startDateTimeUTC, DateTime endDateTimeUTC)
        {
            IEnumerable<Event> events = _calendarDB.Events.Where(
                e => e.FreqType != FrequencyType.OnlyOnce
                && ((e.StartDateTimeUTC >= startDateTimeUTC && e.StartDateTimeUTC < endDateTimeUTC ) || (e.StartDateTimeUTC <= startDateTimeUTC))
                && (!e.SeriesEndDateUTC.HasValue 
                    || (e.SeriesEndDateUTC.HasValue && e.SeriesEndDateUTC.Value > startDateTimeUTC ) 
                    || (e.SeriesEndDateUTC.HasValue 
                        && e.SeriesEndDateUTC.Value >= e.StartDateTimeUTC 
                        && e.SeriesEndDateUTC.Value < e.EndDateTimeUTC 
                        && e.EndDateTimeUTC > startDateTimeUTC))
            );

            IEnumerable<Event> resultEvents = CreateRecurringEventsOccurrences(events, startDateTimeUTC, endDateTimeUTC);

            return resultEvents;
        }

        private IEnumerable<Event> CreateRecurringEventsOccurrences(IEnumerable<Event> events, DateTime startDateTimeUTC, DateTime endDateTimeUTC)
        {
            IList<Event> resultEvents = new List<Event>();

            foreach (var evt in events)
            {
                IEnumerable<DateTime> occurrences = evt.GetSeriesOccurrences(endDateTimeUTC);

                foreach (DateTime occurrence in occurrences)
                {
                    DateTime eventStartDateTimeUTC = occurrence;
                    DateTime eventEndDateTimeUTC = occurrence.Add(evt.EndDateTimeUTC - evt.StartDateTimeUTC);

                    if (eventStartDateTimeUTC < endDateTimeUTC && eventEndDateTimeUTC > startDateTimeUTC)
                    {
                        Event e = (Event)evt.Clone();

                        e.StartDateTimeUTC = eventStartDateTimeUTC;
                        e.EndDateTimeUTC = eventEndDateTimeUTC;

                        resultEvents.Add(e);
                    }
                }
            }

            return resultEvents;
        }
    }
}
