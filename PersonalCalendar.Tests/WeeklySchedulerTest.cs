using NUnit.Framework;
using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalCalendar.Tests
{

    [TestFixture]
    public class WeeklySchedulerTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOccurrences_WhenStartDateIsLaterThanEndDate_ThrowsArgumentException()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            IEnumerable<DateTime> occurrences = scheduler.GetOccurrences(new DateTime(2015, 3, 13), new DateTime(2015, 3, 11));
        }

        [Test]
        public void GetOccurrences_WhenFrequencySubtypeIsNone_ReturnsProperNumberOfOccurrences()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            IEnumerable<DateTime> occurrences = scheduler.GetOccurrences(new DateTime(2015, 3, 1), new DateTime(2015, 4, 1)).OrderBy(o => o);

            Assert.AreEqual(5, occurrences.Count());

            IEnumerable<DateTime> expectedOccurrences = new List<DateTime>
            {
                new DateTime(2015, 3, 1),
                new DateTime(2015, 3, 8),
                new DateTime(2015, 3, 15),
                new DateTime(2015, 3, 22),
                new DateTime(2015, 3, 29)
            };

            CollectionAssert.AreEqual(expectedOccurrences, occurrences);
        }

        [Test]
        public void GetOccurrences_WhenFrequencySubtypeIsDefined_ReturnsProperNumberOfOccurrences()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.Monday | FrequencySubtype.Tuesday, 2);

            IEnumerable<DateTime> occurrences = scheduler.GetOccurrences(new DateTime(2015, 3, 1), new DateTime(2015, 4, 1)).OrderBy(o => o);

            Assert.AreEqual(6, occurrences.Count());

            IEnumerable<DateTime> expectedOccurrences = new List<DateTime>
            {
                new DateTime(2015, 3, 2),
                new DateTime(2015, 3, 3),
                new DateTime(2015, 3, 16),
                new DateTime(2015, 3, 17),
                new DateTime(2015, 3, 30),
                new DateTime(2015, 3, 31)
            };

            CollectionAssert.AreEqual(expectedOccurrences, occurrences);
        }

        [Test]
        public void GetOccurrences_WhenDatesRangeIsSmallerThanInterval_ReturnsOnlyOneElement()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            IEnumerable<DateTime> occurrences = scheduler.GetOccurrences(new DateTime(2015, 3, 1), new DateTime(2015, 3, 5)).OrderBy(o => o);

            Assert.AreEqual(1, occurrences.Count());

            IEnumerable<DateTime> expectedOccurrences = new List<DateTime>
            {
                new DateTime(2015, 3, 1)
            };

            CollectionAssert.AreEqual(expectedOccurrences, occurrences);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetOccurrenceDateTime_WhenOccurrencesCountIsSmallerThanOne_ThrowsArgumentOutOfRangeException()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            DateTime occurrenceDateTime = scheduler.GetOccurrenceDateTime(new DateTime(2015, 3, 13), 0);
        }

        [Test]
        public void GetOccurrenceDateTime_FrequencySubtypeIsNone_ReturnsProperDateTime()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            DateTime occurrenceDateTime = scheduler.GetOccurrenceDateTime(new DateTime(2015, 3, 1), 3);

            Assert.AreEqual(new DateTime(2015, 3, 15), occurrenceDateTime);
        }

        [Test]
        public void GetOccurrenceDateTime_FrequencySubtypeIsDefined_ReturnsProperDateTime()
        {
            // Check for different variants.
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.Monday | FrequencySubtype.Saturday | FrequencySubtype.Sunday, 1);
            DateTime occurrenceDateTime = scheduler.GetOccurrenceDateTime(new DateTime(2015, 3, 1), 5);
            
            Assert.AreEqual(new DateTime(2015, 3, 9), occurrenceDateTime);

            scheduler = new WeeklyScheduler(FrequencySubtype.Monday | FrequencySubtype.Wednesday, 2);
            occurrenceDateTime = scheduler.GetOccurrenceDateTime(new DateTime(2015, 3, 3), 3);

            Assert.AreEqual(new DateTime(2015, 3, 18), occurrenceDateTime);
        }
    }
}
