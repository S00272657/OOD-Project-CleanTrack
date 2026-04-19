using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CleanTrack
{
    public partial class MainWindow
    {
        #region CalendarTab

        private void calMain_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calMain.SelectedDate == null) return;

            try
            {
                DateTime selected = calMain.SelectedDate.Value.Date;

                using (var freshDb = new AppDbContext())
                {
                    var jobsOnDay = freshDb.Jobs
                        .Include("Client")
                        .Include("Employees")
                        .Where(j => System.Data.Entity.DbFunctions.TruncateTime(j.Date) == selected)
                        .ToList();

                    tbxCalendarDate.Text = selected.ToString("dddd, dd MMMM yyyy");
                    tbxCalendarJobCount.Text = jobsOnDay.Count == 0
                        ? "No jobs this day"
                        : $"{jobsOnDay.Count} job{(jobsOnDay.Count > 1 ? "s" : "")} scheduled";

                    dgCalendarJobs.ItemsSource = jobsOnDay;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load calendar jobs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
