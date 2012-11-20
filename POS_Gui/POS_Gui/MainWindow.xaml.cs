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
using System.Web;

namespace POS_Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //other initializations can go here
        }

        private void search_bar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) 
            {
                //This is unsafe code need URI coding to make searches safe
                search_results.Source = new Uri("http://www.google.com/search?q=" + search_bar.Text);
            }
        }

        private void search_bar_LostFocus(object sender, RoutedEventArgs e)
        {
            search_results.Source = new Uri("http://www.google.com/search?q=" + search_bar.Text);
        }
    }
}
