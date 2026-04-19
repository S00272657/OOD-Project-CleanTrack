 using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CleanTrack
{
    public partial class MainWindow
    {
        #region ReportsTab

        private void tcMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ignore SelectionChanged events that bubble up from DataGrids/ComboBoxes inside the tabs
            if (!ReferenceEquals(e.OriginalSource, tcMain)) return;
            if (tcMain.SelectedItem is TabItem selected && selected.Name == "Reports")
                LoadReports();
        }

        private void LoadReports()
        {   
            try
            {
                var jobs = db.Jobs.Include("Client").ToList();

                // --- Summary stats ---
                //get sum of completed jobs
                decimal totalRevenue = jobs
                                        .Where(j => j.Status == "Completed")
                                        .Sum(j => (decimal?)j.Price) ?? 0;

                //get count of jobs in current month
                int jobsThisMonth = jobs.Count(j => j.Date.Month == DateTime.Now.Month && j.Date.Year == DateTime.Now.Year);

                //get count of active clients
                int activeClients = db.Clients.Count(c => c.IsActive);

                //get most common job type
                string topJobType = jobs.GroupBy(j => j.JobType)
                                          .OrderByDescending(g => g.Count())
                                          .Select(g => g.Key)
                                          .FirstOrDefault() ?? "N/A";

                txtTotalRevenue.Text = $"€{totalRevenue:F2}";
                txtJobsThisMonth.Text = jobsThisMonth.ToString();
                txtActiveClients.Text = activeClients.ToString();
                txtTopJobType.Text = topJobType;

                // --- Monthly breakdown ---
                var monthly = jobs
                    .GroupBy(j => new { j.Date.Year, j.Date.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new MonthlyRevenue //put into MonthlyRevenue objects to bind to datagrid
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"),
                        JobCount = g.Count(),
                        TotalRevenue = g.Sum(j => j.Price)
                    })
                    .ToList();

                dgMonthlyRevenue.ItemsSource = monthly;
                
                // --- Weekly summary (initialise to current week on first load) ---
                if (_weekStart == DateTime.MinValue)
                {
                    //days since monday = (today as num - monday as num + 7) mod 7
                    // + 7 and % 7 prevents crash if today is sunday:  0 - 1 = -1
                    int daysSinceMonday = ((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                    _weekStart = DateTime.Today.AddDays(-daysSinceMonday);
                }
                LoadWeeklySummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadWeeklySummary()
        {
            try
            {
                // Use exclusive upper bound (< AddDays(7)) so any time component on the last day is included
                DateTime weekEnd = _weekStart.AddDays(7);

                var weekJobs = db.Jobs
                    .Include("Client")
                    .Where(j => j.Date >= _weekStart && j.Date < weekEnd)
                    .ToList();

                weekJobs.Sort(); // uses Job.CompareTo — sorts by date ascending

                txtWeekRange.Text = $"{_weekStart:dd MMM} – {weekEnd.AddDays(-1):dd MMM yyyy}";
                txtWeekStats.Text = $"{weekJobs.Count} job{(weekJobs.Count != 1 ? "s" : "")}   |   €{weekJobs.Sum(j => j.Price):F2} revenue";
                dgWeeklyJobs.ItemsSource = weekJobs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load weekly summary: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrevWeek_Click(object sender, RoutedEventArgs e)
        {
            _weekStart = _weekStart.AddDays(-7);
            LoadWeeklySummary();
        }

        private void btnNextWeek_Click(object sender, RoutedEventArgs e)
        {
            _weekStart = _weekStart.AddDays(7);
            LoadWeeklySummary();
        }

        private void btnExportWeeklyJSON_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime weekEnd = _weekStart.AddDays(7);

                var weekJobs = db.Jobs
                    .Include("Client")
                    .Where(j => j.Date >= _weekStart && j.Date < weekEnd)
                    .ToList();

                weekJobs.Sort();

                //----create json content---
                var sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine($"  \"exported\": \"{DateTime.Now:yyyy-MM-dd HH:mm}\",");
                sb.AppendLine($"  \"week\": \"{_weekStart:dd MMM} - {weekEnd.AddDays(-1):dd MMM yyyy}\",");
                sb.AppendLine($"  \"total_jobs\": {weekJobs.Count},");
                sb.AppendLine($"  \"total_revenue\": {weekJobs.Sum(j => j.Price):F2},");
                sb.AppendLine("  \"jobs\": [");

                for (int i = 0; i < weekJobs.Count; i++)
                {
                    var j = weekJobs[i];
                    string comma = i < weekJobs.Count - 1 ? "," : "";
                    sb.AppendLine($"    {{ \"date\": \"{j.Date:dd/MM/yyyy}\", \"client\": \"{j.Client?.FullName}\", \"type\": \"{j.JobType}\", \"price\": {j.Price:F2}, \"status\": \"{j.Status}\" }}{comma}");
                }

                sb.AppendLine("  ]");
                sb.AppendLine("}");

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    FileName = $"CleanTrack_Week_{_weekStart:yyyy-MM-dd}"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveDialog.FileName, sb.ToString());
                    MessageBox.Show("Weekly summary exported successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export weekly summary: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExportJSON_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var jobs = db.Jobs.ToList();

                decimal totalRevenue = jobs.Where(j => j.Status == "Completed").Sum(j => (decimal?)j.Price) ?? 0;

                var monthly = jobs
                    .GroupBy(j => new { j.Date.Year, j.Date.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new MonthlyRevenue
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"),
                        JobCount = g.Count(),
                        TotalRevenue = g.Sum(j => j.Price)
                    })
                    .ToList();

                var sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine($"  \"exported\": \"{DateTime.Now:yyyy-MM-dd HH:mm}\",");
                sb.AppendLine($"  \"total_revenue_completed\": {totalRevenue:F2},");
                sb.AppendLine("  \"monthly_breakdown\": [");

                for (int i = 0; i < monthly.Count; i++)
                {
                    var m = monthly[i];
                    string comma = i < monthly.Count - 1 ? "," : "";
                    sb.AppendLine($"    {{ \"month\": \"{m.Month}\", \"jobs\": {m.JobCount}, \"revenue\": {m.TotalRevenue:F2} }}{comma}");
                }

                sb.AppendLine("  ]");
                sb.AppendLine("}");

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    FileName = $"CleanTrack_Report_{DateTime.Now:yyyy-MM-dd}"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveDialog.FileName, sb.ToString());
                    MessageBox.Show("Report exported successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export report: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
