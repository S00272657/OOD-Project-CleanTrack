using System.Windows;

namespace CleanTrack
{
    public partial class ClientDetailsWindow : Window
    {
        public ClientDetailsWindow(Client client)
        {
            InitializeComponent();
            LoadClientDetails(client);
        }

        private void LoadClientDetails(Client client)
        {
            txtName.Text    = client.FullName;
            txtPhone.Text   = client.PhoneNumber;
            txtEmail.Text   = client.Email;
            txtAddress.Text = client.Address;
            txtCity.Text    = client.City;
            txtEircode.Text = client.Eircode;
            txtStatus.Text  = client.IsActive ? "Active" : "Inactive";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
