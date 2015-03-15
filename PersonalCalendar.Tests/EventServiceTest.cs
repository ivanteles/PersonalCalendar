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
        protected Mock<CalendarDB> _calendarDBMock;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            DateTime date = new DateTime(2015,3,13);

            IQueryable<Event> events = new List<Event>
            {
                new Event 
                { 
                    CalendarId = 1, 
                    Title = "one_time_one_day_event", 
                    StartDateUTC = date,
                    EndDateUTC = date,
                    SeriesEndDateUTC = null,
                    FreqType = FrequencyType.OnlyOnce,
                    FreqSubtype = FrequencySubtype.None,
                    FreqInterval = 0
                },
                
                new Event 
                { 
                    CalendarId = 1, 
                    Title = "ont_time_two_days_event", 
                    StartDateUTC = date, 
                    EndDateUTC = date.AddDays(1), 
                    FreqType = FrequencyType.OnlyOnce, 
                    FreqSubtype = FrequencySubtype.None, 
                    FreqInterval = 0 
                }
            }.AsQueryable();

            var eventsSetMock = new Mock<DbSet<Event>>();
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.Provider).Returns(events.Provider);
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.Expression).Returns(events.Expression);
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.ElementType).Returns(events.ElementType);
            eventsSetMock.As<IQueryable<Event>>().Setup(m => m.GetEnumerator()).Returns(events.GetEnumerator());

            _calendarDBMock = new Mock<CalendarDB>();
            _calendarDBMock.Setup(c => c.Events).Returns(eventsSetMock.Object);
        }

        [Test]
        public void GetAllTest()
        {
            EventService service = new EventService(_calendarDBMock.Object);
            
            IEnumerable<Event> events = service.GetAllForCalendar(1);

            Assert.AreEqual(2, events.Count());
        }
    }
}
