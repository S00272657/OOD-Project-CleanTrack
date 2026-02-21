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


    }
}
