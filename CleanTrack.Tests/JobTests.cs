using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CleanTrack;

namespace CleanTrack.Tests
{
    [TestClass]
    public class JobTests
    {
        [TestMethod]
        public void CompareTo_EarlierDate_ReturnsNegative()
        {
            // Arrange
            var job1 = new Job { Date = new DateTime(2026, 4, 1) };
            var job2 = new Job { Date = new DateTime(2026, 4, 10) };

            // Act
            int result = job1.CompareTo(job2);

            // Assert
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareTo_LaterDate_ReturnsPositive()
        {
            // Arrange
            var job1 = new Job { Date = new DateTime(2026, 4, 10) };
            var job2 = new Job { Date = new DateTime(2026, 4, 1) };

            // Act
            int result = job1.CompareTo(job2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareTo_SameDate_ReturnsZero()
        {
            // Arrange
            var job1 = new Job { Date = new DateTime(2026, 4, 5) };
            var job2 = new Job { Date = new DateTime(2026, 4, 5) };

            // Act
            int result = job1.CompareTo(job2);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_NullJob_ReturnsPositive()
        {
            // Arrange
            var job1 = new Job { Date = new DateTime(2026, 4, 1) };

            // Act
            int result = job1.CompareTo(null);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void SortList_SortsByDateAscending()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job { Date = new DateTime(2026, 4, 15) },
                new Job { Date = new DateTime(2026, 4, 3) },
                new Job { Date = new DateTime(2026, 4, 10) },
            };

            // Act
            jobs.Sort();

            // Assert
            Assert.AreEqual(new DateTime(2026, 4, 3),  jobs[0].Date);
            Assert.AreEqual(new DateTime(2026, 4, 10), jobs[1].Date);
            Assert.AreEqual(new DateTime(2026, 4, 15), jobs[2].Date);
        }

        [TestMethod]
        public void DefaultConstructor_StatusIsBooked()
        {
            // Arrange & Act
            var job = new Job();

            // Assert
            Assert.AreEqual("Booked", job.Status);
        }

        [TestMethod]
        public void DefaultConstructor_EmployeesListNotNull()
        {
            // Arrange & Act
            var job = new Job();

            // Assert
            Assert.IsNotNull(job.Employees);
        }
    }
}
