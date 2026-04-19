using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CleanTrack
{
    public partial class MainWindow
    {
        //---filtering method---
        private void FilterClientResults()
        {
            if (tbxSearchClients == null || cbxClientActiveFilter == null || dgClients == null) return;

            try
            {
                var filtered = db.Clients.AsQueryable();

                string search = tbxSearchClients.Text.ToLower();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    filtered = filtered.Where(c =>
                        c.FirstName.ToLower().Contains(search) ||
                        c.LastName.ToLower().Contains(search) ||
                        (c.FirstName + " " + c.LastName).ToLower().Contains(search) ||
                        c.Email.ToLower().Contains(search) ||
                        c.PhoneNumber.Contains(search) ||
                        c.Address.ToLower().Contains(search));
                }

                if (cbxClientActiveFilter.SelectedItem is ComboBoxItem activeItem && activeItem.Content.ToString() != "All")
                {
                    bool isActive = activeItem.Content.ToString() == "Active";
                    filtered = filtered.Where(c => c.IsActive == isActive);
                }

                dgClients.ItemsSource = null;
                dgClients.ItemsSource = filtered.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to filter clients: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- event listeners ---
        private void tbxSearchClients_TextChanged(object sender, TextChangedEventArgs e) => FilterClientResults();
        private void cbxClientActiveFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => FilterClientResults();

        //--- action buttons
        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddClientWindow addClientWindow = new AddClientWindow();
                addClientWindow.Owner = this;

                if (addClientWindow.ShowDialog() == true)
                {
                    db.Clients.Add(addClientWindow.NewClient);
                    db.SaveChanges();
                    FilterClientResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add client: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditClient_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Please select a client to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Client selectedClient = dgClients.SelectedItem as Client;
                EditClientWindow editClientWindow = new EditClientWindow(selectedClient);
                editClientWindow.Owner = this;

                if (editClientWindow.ShowDialog() == true)
                {
                    db.SaveChanges();
                    FilterClientResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save client changes: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Please select a client to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Client selectedClient = dgClients.SelectedItem as Client;

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete {selectedClient.FullName}?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.Clients.Remove(selectedClient);
                    db.SaveChanges();
                    FilterClientResults();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete client: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnViewClientDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Please select a client to view.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Client selectedClient = dgClients.SelectedItem as Client;
            ClientDetailsWindow detailsWindow = new ClientDetailsWindow(selectedClient);
            detailsWindow.Owner = this;
            detailsWindow.ShowDialog();
        }

    }
}
