using Moq;
using NUnit.Framework;
using PersonalCalendar.Data;
using PersonalCalendar.Domain;
using PersonalCalendar.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PersonalCalendar.Tests
{
    [TestFixture]
    public class EventServiceTest
    {
        [Test]
        public void GetOneTimeEventsForCalendar_ForOneDay_ReturnsAllOneTimeEventsForThatDay()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareOneTimeEvent(1, new DateTime(2015, 3, 13), new DateTime(2015, 3, 14), "all_day_event_1"),
                PrepareOneTimeEvent(2, new DateTime(2015, 3, 12), new DateTime(2015, 3, 13), "all_day_event_2"),
                PrepareOneTimeEvent(3, new DateTime(2015, 3, 14), new DateTime(2015, 3, 16), "two_days_event_1"),
                PrepareOneTimeEvent(4, new DateTime(2015, 3, 12), new DateTime(2015, 3, 15), "three_days_event_1"),
                PrepareOneTimeEvent(5, new DateTime(2015, 3, 13, 11, 0, 0), new DateTime(2015, 3, 13, 12, 0, 0), "one_hour_event_1"),
                PrepareOneTimeEvent(6, new DateTime(2015, 3, 14, 11, 0, 0), new DateTime(2015, 3, 14, 12, 0, 0), "one_hour_event_2"),
                PrepareOneTimeEvent(7, new DateTime(2015, 3, 12, 12, 0, 0), new DateTime(2015, 3, 14, 12, 0, 0), "48_hours_event_1"),
                PrepareOneTimeEvent(8, new DateTime(2015, 3, 13, 12, 0, 0), new DateTime(2015, 3, 14, 12, 0, 0), "24_hours_event_1"),
                PrepareOneTimeEvent(9, new DateTime(2015, 3, 12, 12, 0, 0), new DateTime(2015, 3, 13, 12, 0, 0), "24_hours_event_1")
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            IEnumerable<Event> events = service.GetOneTimeEventsForCalendar(1, new DateTime(2015, 3, 13), new DateTime(2015, 3, 14));

            // Assert
            Assert.AreEqual(6, events.Count());
            
            int[] expectedEventIds = { 1, 4, 5, 7, 8, 9 };
            Assert.IsTrue(events.All(e => expectedEventIds.Contains(e.Id)));
        }

        [Test]
        public void GetOneTimeEventsForCalendar_ForOneWeek_ReturnsAllOneTimeEventsForThatWeek()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareOneTimeEvent(1, new DateTime(2015, 3, 13), new DateTime(2015, 3, 14), "all_day_event_1"),
                PrepareOneTimeEvent(2, new DateTime(2015, 3, 7), new DateTime(2015, 3, 8), "all_day_event_2"),
                PrepareOneTimeEvent(3, new DateTime(2015, 3, 16), new DateTime(2015, 3, 17), "all_day_event_3"),
                PrepareOneTimeEvent(4, new DateTime(2015, 3, 7), new DateTime(2015, 3, 7).AddDays(14), "2_weeks_event_1"),
                PrepareOneTimeEvent(5, new DateTime(2015, 3, 10), new DateTime(2015, 3, 10).AddDays(2), "2_days_event_1"),
                PrepareOneTimeEvent(6, new DateTime(2015, 3, 7, 12, 0, 0), new DateTime(2015, 3, 13, 13, 0, 0), "many_hours_event_1"),
                PrepareOneTimeEvent(7, new DateTime(2015, 3, 13, 12, 0, 0), new DateTime(2015, 3, 26, 13, 0, 0), "many_hours_event_2")
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime firstDayOfTheWeek = new DateTime(2015, 3, 9);
            IEnumerable<Event> events = service.GetOneTimeEventsForCalendar(1, firstDayOfTheWeek, firstDayOfTheWeek.AddDays(7));

            // Assert
            Assert.AreEqual(5, events.Count());

            int[] expectedEventIds = { 1, 4, 5, 6, 7 };
            Assert.IsTrue(events.All(e => expectedEventIds.Contains(e.Id)));
        }

        [Test]
        public void GetOneTimeEventsForCalendar_ForOneMonth_ReturnsAllOneTimeEventsForThatMonth()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareOneTimeEvent(1, new DateTime(2015, 3, 13), new DateTime(2015, 3, 14), "all_day_event_1"),
                PrepareOneTimeEvent(2, new DateTime(2015, 2, 7), new DateTime(2015, 2, 8), "all_day_event_2"),
                PrepareOneTimeEvent(3, new DateTime(2015, 3, 25), new DateTime(2015, 3, 25).AddDays(14), "2_weeks_event_1"),
                PrepareOneTimeEvent(4, new DateTime(2015, 2, 25), new DateTime(2015, 2, 25).AddDays(14), "2_weeks_event_2")
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime firstDayOfTheMonth = new DateTime(2015, 3, 1);
            IEnumerable<Event> events = service.GetOneTimeEventsForCalendar(1, firstDayOfTheMonth, firstDayOfTheMonth.AddMonths(1));

            // Assert
            Assert.AreEqual(3, events.Count());

            int[] expectedEventIds = { 1, 3, 4 };
            Assert.IsTrue(events.All(e => expectedEventIds.Contains(e.Id)));
        }

        [Test]
        public void GetOneTimeEventsForCalendar_ForOneYear_ReturnsAllOneTimeEventsForThatYear()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareOneTimeEvent(1, new DateTime(2015, 3, 13), new DateTime(2015, 3, 14), "all_day_event_1"),
                PrepareOneTimeEvent(2, new DateTime(2014, 12, 31), new DateTime(2015, 1, 1), "all_day_event_2"),
                PrepareOneTimeEvent(3, new DateTime(2016, 1, 1), new DateTime(2016, 1, 2), "all_day_event_3"),
                PrepareOneTimeEvent(4, new DateTime(2014, 12, 25), new DateTime(2014, 12, 25).AddDays(14), "2_weeks_event_1"),
                PrepareOneTimeEvent(5, new DateTime(2015, 12, 25), new DateTime(2015, 12, 25).AddDays(14), "2_weeks_event_2")
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime firstDayOfTheYear = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetOneTimeEventsForCalendar(1, firstDayOfTheYear, firstDayOfTheYear.AddYears(1));

            // Assert
            Assert.AreEqual(3, events.Count());

            int[] expectedEventIds = { 1, 4, 5 };
            Assert.IsTrue(events.All(e => expectedEventIds.Contains(e.Id)));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenDailyRecurringEvent_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 3, 8, 10, 0, 0), new DateTime(2015, 3, 8, 12, 0, 0), null, "never_stops", FrequencyType.Daily, FrequencySubtype.None, 2)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 9);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddDays(7));

            // Assert
            Assert.AreEqual(3, events.Count());

            DateTime[] expectedStartDateTimes = 
            {
                new DateTime(2015, 3, 10, 10, 0, 0), 
                new DateTime(2015, 3, 12, 10, 0, 0),
                new DateTime(2015, 3, 14, 10, 0, 0)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 3, 10, 12, 0, 0), 
                new DateTime(2015, 3, 12, 12, 0, 0),
                new DateTime(2015, 3, 14, 12, 0, 0)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenDailyRecurringEventSeriesFinishesDuringGivenPeriod_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 3, 8, 10, 0, 0), new DateTime(2015, 3, 9, 10, 0, 0), new DateTime(2015, 3, 13), "1", FrequencyType.Daily, FrequencySubtype.None, 1)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 9);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddDays(7));

            // Assert
            Assert.AreEqual(5, events.Count());

            DateTime[] expectedStartDateTimes = 
            {
                new DateTime(2015, 3, 8, 10, 0, 0),
                new DateTime(2015, 3, 9, 10, 0, 0),
                new DateTime(2015, 3, 10, 10, 0, 0),
                new DateTime(2015, 3, 11, 10, 0, 0),
                new DateTime(2015, 3, 12, 10, 0, 0)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 3, 9, 10, 0, 0),
                new DateTime(2015, 3, 10, 10, 0, 0), 
                new DateTime(2015, 3, 11, 10, 0, 0),
                new DateTime(2015, 3, 12, 10, 0, 0),
                new DateTime(2015, 3, 13, 10, 0, 0)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenDailyRecurringEventSeriesFinishesBeforeGivenPeriod_ReturnsEmptyLst()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 3, 8, 10, 0, 0), new DateTime(2015, 3, 8, 10, 30, 0), new DateTime(2015, 3, 9), "1", FrequencyType.Daily, FrequencySubtype.None, 1)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 9);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddDays(7));

            // Assert
            Assert.AreEqual(0, events.Count());
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenWeeklyRecurringEvent_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 3, 4), new DateTime(2015, 3, 5), null, "1", FrequencyType.Weekly, FrequencySubtype.None, 1)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddMonths(1));

            // Assert
            Assert.AreEqual(4, events.Count());

            DateTime[] expectedStartDateTimes = 
            { 
                new DateTime(2015, 3, 4),
                new DateTime(2015, 3, 11),
                new DateTime(2015, 3, 18),
                new DateTime(2015, 3, 25)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 3, 5),
                new DateTime(2015, 3, 12),
                new DateTime(2015, 3, 19),
                new DateTime(2015, 3, 26)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenWeeklyRecurringEventWithDayOfTheWeekSpecified_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 3, 4), new DateTime(2015, 3, 5), null, "1", FrequencyType.Weekly, FrequencySubtype.Friday | FrequencySubtype.Saturday, 2)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddMonths(1));

            // Assert
            Assert.AreEqual(4, events.Count());

            DateTime[] expectedStartDateTimes = 
            { 
                new DateTime(2015, 3, 6),
                new DateTime(2015, 3, 7),
                new DateTime(2015, 3, 20),
                new DateTime(2015, 3, 21)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 3, 7),
                new DateTime(2015, 3, 8),
                new DateTime(2015, 3, 21),
                new DateTime(2015, 3, 22)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenWeeklyRecurringEventSeriesFinishesDuringGivenPeriod_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 3, 4), new DateTime(2015, 3, 5), new DateTime(2015, 3, 19), "1", FrequencyType.Weekly, FrequencySubtype.Friday | FrequencySubtype.Saturday, 2)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddMonths(1));

            // Assert
            Assert.AreEqual(2, events.Count());

            DateTime[] expectedStartDateTimes = 
            { 
                new DateTime(2015, 3, 6),
                new DateTime(2015, 3, 7)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 3, 7),
                new DateTime(2015, 3, 8)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenWeeklyRecurringEventSeriesFinishesBeforeGivenPeriod_ReturnsEmptyLst()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 2, 25), new DateTime(2015, 2, 26), new DateTime(2015, 3, 1), "1", FrequencyType.Weekly, FrequencySubtype.None, 1)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddMonths(1));

            // Assert
            Assert.AreEqual(0, events.Count());
        }

        [Test]
        public void GetRecurringEventForCalendar_WhenWeeklyRecurringEventSeriesFinishesBeforeGivenPeriodButLastEventOverlaps_ReturnsOneOccurrence()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 2, 28), new DateTime(2015, 3, 2), new DateTime(2015, 3, 1), "1", FrequencyType.Weekly, FrequencySubtype.None, 1)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 3, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddMonths(1));

            // Assert
            Assert.AreEqual(1, events.Count());

            DateTime[] expectedStartDateTimes = { new DateTime(2015, 2, 28) };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = { new DateTime(2015, 3, 2) };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventForCalendar_WhenMonthlyRecurringEvent_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 1, 5), new DateTime(2015, 1, 6), null, "1", FrequencyType.Monthly, FrequencySubtype.DayOfTheMonth, 2)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddYears(1));

            // Assert
            Assert.AreEqual(6, events.Count());

            DateTime[] expectedStartDateTimes = 
            { 
                new DateTime(2015, 1, 5),
                new DateTime(2015, 3, 5),
                new DateTime(2015, 5, 5),
                new DateTime(2015, 7, 5),
                new DateTime(2015, 9, 5),
                new DateTime(2015, 11, 5)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 1, 6),
                new DateTime(2015, 3, 6),
                new DateTime(2015, 5, 6),
                new DateTime(2015, 7, 6),
                new DateTime(2015, 9, 6),
                new DateTime(2015, 11, 6)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventForCalendar_WhenMonthlyRecurringEventWithDayOfTheWeekSpecified_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 1, 15, 10, 0, 0), new DateTime(2015, 1, 16, 11, 0, 0), null, "1", FrequencyType.Monthly, FrequencySubtype.DayOfTheWeek, 3)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddYears(1));

            // Assert
            Assert.AreEqual(4, events.Count());

            // Third Thursday of every month.
            DateTime[] expectedStartDateTimes = 
            { 
                new DateTime(2015, 1, 15, 10, 0, 0),
                new DateTime(2015, 4, 16, 10, 0, 0),
                new DateTime(2015, 7, 16, 10, 0, 0),
                new DateTime(2015, 10, 15, 10, 0, 0)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 1, 16, 11, 0, 0),
                new DateTime(2015, 4, 17, 11, 0, 0),
                new DateTime(2015, 7, 17, 11, 0, 0),
                new DateTime(2015, 10, 16, 11, 0, 0)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventForCalendar_WhenMonthlyRecurringEventSeriesFinishesDuringGivenPeriod_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 1, 5), new DateTime(2015, 1, 6), new DateTime(2015, 8, 1), "1", FrequencyType.Monthly, FrequencySubtype.DayOfTheMonth, 2)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddYears(1));

            // Assert
            Assert.AreEqual(4, events.Count());

            DateTime[] expectedStartDateTimes = 
            { 
                new DateTime(2015, 1, 5),
                new DateTime(2015, 3, 5),
                new DateTime(2015, 5, 5),
                new DateTime(2015, 7, 5)
            };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = 
            {
                new DateTime(2015, 1, 6),
                new DateTime(2015, 3, 6),
                new DateTime(2015, 5, 6),
                new DateTime(2015, 7, 6)
            };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenMonthlyRecurringEventSeriesFinishesBeforeGivenPeriod_ReturnsEmptyLst()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 1, 5), new DateTime(2015, 1, 6), new DateTime(2015, 1, 5), "1", FrequencyType.Monthly, FrequencySubtype.DayOfTheMonth, 1)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddYears(1));

            // Assert
            Assert.AreEqual(0, events.Count());
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenYearlyRecurringEvent_ReturnsProperNumberOfOccurrences()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2014, 3, 8, 10, 0, 0), new DateTime(2014, 3, 8, 12, 0, 0), null, "1", FrequencyType.Yearly, FrequencySubtype.None, 1)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddYears(1));

            // Assert
            Assert.AreEqual(1, events.Count());

            DateTime[] expectedStartDateTimes = { new DateTime(2015, 3, 8, 10, 0, 0) };

            CollectionAssert.AreEqual(expectedStartDateTimes, events.OrderBy(e => e.StartDateTimeUTC).Select(e => e.StartDateTimeUTC));

            DateTime[] expectedEndDateTimes = { new DateTime(2015, 3, 8, 12, 0, 0) };

            CollectionAssert.AreEqual(expectedEndDateTimes, events.OrderBy(e => e.EndDateTimeUTC).Select(e => e.EndDateTimeUTC));
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenYearlyRecurringEventNotOccuringInGivenPeriod_ReturnsEmptyList()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2014, 3, 8, 10, 0, 0), new DateTime(2014, 3, 8, 12, 0, 0), null, "1", FrequencyType.Yearly, FrequencySubtype.None, 2)
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddYears(1));

            // Assert
            Assert.AreEqual(0, events.Count());
        }

        [Test]
        public void GetRecurringEventsForCalendar_WhenDifferentlyRecurringEvents_ReturnsAllRecurringEventsForGivenTimePeriod()
        {
            // Arrange
            IQueryable<Event> mockEvents = new List<Event>
            {
                PrepareRecurringEvent(1, new DateTime(2015, 3, 13), new DateTime(2015, 3, 14), new DateTime(2015, 3, 20), "daily", FrequencyType.Daily, FrequencySubtype.None, 1),
                PrepareRecurringEvent(2, new DateTime(2015, 3, 5), new DateTime(2015, 3, 6), new DateTime(2015, 3, 20), "weekly", FrequencyType.Weekly, FrequencySubtype.None, 1),
                PrepareRecurringEvent(3, new DateTime(2015, 1, 1), new DateTime(2015, 1, 2), null, "monthly", FrequencyType.Monthly, FrequencySubtype.None, 1),
                PrepareRecurringEvent(4, new DateTime(2015, 1, 1), new DateTime(2015, 1, 2), null, "yearly", FrequencyType.Yearly, FrequencySubtype.None, 1),
            }.AsQueryable();

            CalendarDB calendarDB = PrepareCalendarDB(mockEvents);
            EventService service = new EventService(calendarDB);

            // Act
            DateTime startDateTime = new DateTime(2015, 1, 1);
            IEnumerable<Event> events = service.GetRecurringEventsForCalendar(1, startDateTime, startDateTime.AddYears(1));

            // Assert
            Assert.AreEqual(7, events.Count(e => e.Id == 1));
            Assert.AreEqual(3, events.Count(e => e.Id == 2));
            Assert.AreEqual(12, events.Count(e => e.Id == 3));
            Assert.AreEqual(1, events.Count(e => e.Id == 4));
        }

        private CalendarDB PrepareCalendarDB(IQueryable<Event> events)
        {
            var eventsSetMock = new Mock<DbSet<Event>>();
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.Provider).Returns(events.Provider);
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.Expression).Returns(events.Expression);
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.ElementType).Returns(events.ElementType);
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.GetEnumerator()).Returns(events.GetEnumerator());

            var calendarDBMock = new Mock<CalendarDB>();
            calendarDBMock.Setup(c => c.Events).Returns(eventsSetMock.Object);

            return calendarDBMock.Object;
        }

        private Event PrepareOneTimeEvent(int id, DateTime startDateTimeUTC, DateTime endDateTimeUTC, string title)
        {
            return new Event
            {
                Id = id,
                CalendarId = 1,
                StartDateTimeUTC = startDateTimeUTC,
                EndDateTimeUTC = endDateTimeUTC,
                Title = title,
                FreqType = FrequencyType.OnlyOnce,
                FreqSubtype = FrequencySubtype.None,
                FreqInterval = 0,
                SeriesEndDateUTC = null
            };
        }

        private Event PrepareRecurringEvent(
            int id, 
            DateTime startDateTimeUTC, 
            DateTime endDateUTC, 
            DateTime? seriesEndDateTimeUTC, 
            string title, 
            FrequencyType freqType, 
            FrequencySubtype freqSubtype,
            int freqInterval)
        {
            return new Event
            {
                Id = id,
                CalendarId = 1,
                StartDateTimeUTC = startDateTimeUTC,
                EndDateTimeUTC = endDateUTC,
                Title = title,
                FreqType = freqType,
                FreqSubtype = freqSubtype,
                FreqInterval = freqInterval,
                SeriesEndDateUTC = seriesEndDateTimeUTC
            };
        }
    }
}
