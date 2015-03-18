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
        public void GetOccurences_WhenStartDateIsLaterThanEndDate_ThrowsArgumentException()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            IEnumerable<DateTime> occurences = scheduler.GetOccurences(new DateTime(2015, 3, 13), new DateTime(2015, 3, 11));
        }

        [Test]
        public void GetOccurences_WhenFrequencySubtypeIsNone_ReturnsProperNumberOfOccurences()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            IEnumerable<DateTime> occurences = scheduler.GetOccurences(new DateTime(2015, 3, 1), new DateTime(2015, 4, 1)).OrderBy(o => o);

            Assert.AreEqual(5, occurences.Count());

            IEnumerable<DateTime> expectedOccurences = new List<DateTime>
            {
                new DateTime(2015, 3, 1),
                new DateTime(2015, 3, 8),
                new DateTime(2015, 3, 15),
                new DateTime(2015, 3, 22),
                new DateTime(2015, 3, 29)
            };

            CollectionAssert.AreEqual(expectedOccurences, occurences);
        }

        [Test]
        public void GetOccurences_WhenFrequencySubtypeIsDefined_ReturnsProperNumberOfOccurences()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.Monday | FrequencySubtype.Tuesday, 2);

            IEnumerable<DateTime> occurences = scheduler.GetOccurences(new DateTime(2015, 3, 1), new DateTime(2015, 4, 1)).OrderBy(o => o);

            Assert.AreEqual(6, occurences.Count());

            IEnumerable<DateTime> expectedOccurences = new List<DateTime>
            {
                new DateTime(2015, 3, 2),
                new DateTime(2015, 3, 3),
                new DateTime(2015, 3, 16),
                new DateTime(2015, 3, 17),
                new DateTime(2015, 3, 30),
                new DateTime(2015, 3, 31)
            };

            CollectionAssert.AreEqual(expectedOccurences, occurences);
        }

        [Test]
        public void GetOccurences_WhenDatesRangeIsSmallerThanInterval_ReturnsOnlyOneElement()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            IEnumerable<DateTime> occurences = scheduler.GetOccurences(new DateTime(2015, 3, 1), new DateTime(2015, 3, 5)).OrderBy(o => o);

            Assert.AreEqual(1, occurences.Count());

            IEnumerable<DateTime> expectedOccurences = new List<DateTime>
            {
                new DateTime(2015, 3, 1)
            };

            CollectionAssert.AreEqual(expectedOccurences, occurences);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetOccurenceDateTime_WhenOccurencesCountIsSmallerThanOne_ThrowsArgumentOutOfRangeException()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            DateTime occurenceDateTime = scheduler.GetOccurenceDateTime(new DateTime(2015, 3, 13), 0);
        }

        [Test]
        public void GetOccurenceDateTime_FrequencySubtypeIsNone_ReturnsProperDateTime()
        {
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.None, 1);

            DateTime occurenceDateTime = scheduler.GetOccurenceDateTime(new DateTime(2015, 3, 1), 3);

            Assert.AreEqual(new DateTime(2015, 3, 15), occurenceDateTime);
        }

        [Test]
        public void GetOccurenceDateTime_FrequencySubtypeIsDefined_ReturnsProperDateTime()
        {
            // Check for different variants.
            WeeklyScheduler scheduler = new WeeklyScheduler(FrequencySubtype.Monday | FrequencySubtype.Saturday | FrequencySubtype.Sunday, 1);
            DateTime occurenceDateTime = scheduler.GetOccurenceDateTime(new DateTime(2015, 3, 1), 5);
            
            Assert.AreEqual(new DateTime(2015, 3, 9), occurenceDateTime);

            scheduler = new WeeklyScheduler(FrequencySubtype.Monday | FrequencySubtype.Wednesday, 2);
            occurenceDateTime = scheduler.GetOccurenceDateTime(new DateTime(2015, 3, 3), 3);

            Assert.AreEqual(new DateTime(2015, 3, 18), occurenceDateTime);
        }
    }
}
