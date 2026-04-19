using System.Windows;

namespace CleanTrack
{
    public partial class EditClientWindow : Window
    {
        private Client _existingClient;

        public EditClientWindow(Client client)
        {
            InitializeComponent();
            _existingClient = client;
            PreFillFields();
        }

        private void PreFillFields()
        {
            tbxFirstName.Text     = _existingClient.FirstName;
            tbxLastName.Text      = _existingClient.LastName;
            tbxPhone.Text         = _existingClient.PhoneNumber;
            tbxEmail.Text         = _existingClient.Email;
            tbxAddress.Text       = _existingClient.Address;
            tbxCity.Text          = _existingClient.City;
            tbxEircode.Text       = _existingClient.Eircode;
            chkIsActive.IsChecked = _existingClient.IsActive;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxFirstName.Text))
            {
                MessageBox.Show("Please enter a first name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxLastName.Text))
            {
                MessageBox.Show("Please enter a last name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxPhone.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxEmail.Text))
            {
                MessageBox.Show("Please enter an email.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxAddress.Text))
            {
                MessageBox.Show("Please enter an address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _existingClient.FirstName   = tbxFirstName.Text;
            _existingClient.LastName    = tbxLastName.Text;
            _existingClient.PhoneNumber = tbxPhone.Text;
            _existingClient.Email       = tbxEmail.Text;
            _existingClient.Address     = tbxAddress.Text;
            _existingClient.City        = tbxCity.Text;
            _existingClient.Eircode     = tbxEircode.Text;
            _existingClient.IsActive    = chkIsActive.IsChecked == true;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
