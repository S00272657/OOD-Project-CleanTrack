using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanTrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lbxJobs.ItemsSource = cleaningJobs;
            lbxClients.ItemsSource = customers;
        }

        public List<string> cleaningJobs = new List<string>
        {
            "Office Deep Cleaning - 2026-02-10",
            "Carpet Steam Cleaning - 2026-02-12",
            "Window Cleaning (Exterior) - 2026-02-14",
            "Post-Construction Cleanup - 2026-02-15",
            "Residential Kitchen Deep Clean - 2026-02-18",
            "Warehouse Floor Scrubbing - 2026-02-20",
            "Office Deep Cleaning - 2026-02-10",
            "Carpet Steam Cleaning - 2026-02-12",
            "Window Cleaning (Exterior) - 2026-02-14",
            "Post-Construction Cleanup - 2026-02-15",
            "Residential Kitchen Deep Clean - 2026-02-18",
            "Warehouse Floor Scrubbing - 2026-02-20"
        };

        List<string> customers = new List<string>
{
    "John Smith | 555-123-4567 | john.smith@email.com | 12 Oak Street, Boston, MA | 5 |  $1,250 |  2026-01-15",
    "Sarah Johnson | 555-987-6543 | sarah.j@email.com | 44 Pine Ave, Chicago, IL |  3 |  $780 |  2026-02-02",
    "Michael Brown | 555-222-3344 | mbrown@email.com | 89 Elm Road, Dallas, TX |  8 |  $2,340 | Last Job: 2026-02-10",
    "Emily Davis | 555-111-9988 | emily.d@email.com | 15 Sunset Blvd, Miami, FL |  2 |  $420 | Last Job: 2025-12-18",
    "David Wilson | 555-444-7788 | dwilson@email.com | 230 Lake View Dr, Denver, CO |  6 | Total Spent: $1,560 | Last Job: 2026-01-28",
    "Jessica Taylor | 555-666-1212 | jess.t@email.com | 9 Maple Street, Seattle, WA |  4 | Total Spent: $980 | Last Job: 2026-02-05",
    "Chris Anderson | 555-333-4455 | canderson@email.com | 72 River Rd, Atlanta, GA |  7 | Total Spent: $1,890 | Last Job: 2026-02-12",
    "Amanda Thomas | 555-777-8899 | amanda.t@email.com | 5 Hillcrest Ave, Phoenix, AZ |  1 | Total Spent: $210 | Last Job: 2025-11-30",
    "Daniel Martinez | 555-888-2323 | dmartinez@email.com | 310 Forest Lane, Austin, TX |  9 | Total Spent: $2,760 | Last Job: 2026-02-14",
    "Olivia Hernandez | 555-999-4545 | olivia.h@email.com | 18 Bay Street, San Diego, CA |  3 | Total Spent: $840 | Last Job: 2026-01-22",
    "Matthew Lee | 555-101-2020 | mlee@email.com | 66 Cedar Park, Portland, OR | Total Jobs: 5 |  $1,375 | Last Job: 2026-02-01",
    "Sophia Walker | 555-303-4040 | sophia.w@email.com | 27 Greenway Blvd, Charlotte, NC | Total Jobs: 4 | Total Spent: $1,020 | Last Job: 2026-02-16"
};


    }
}
