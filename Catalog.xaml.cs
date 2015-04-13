using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

//using BushidoApp.FactionList;

namespace BushidoApp
{
    public partial class Catalog : PhoneApplicationPage
    {
        /*
         * This class correspond to the catalog entry.
         * It will be automatically filled using the FactionList class.
         */ 
        public Catalog()
        {
            InitializeComponent();
            // initialize the faction list 
            factionList.ItemsSource = FactionList.Factions();
        }

        // on selection changed
        private void factionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = factionList.SelectedIndex;
            // fill the page with faction logo
            if (0 <= index && index < FactionList.Factions().Count)
            {
                // fill the url
                string Url = "/FactionPage.xaml?index=" + index;
                NavigationService.Navigate(new Uri(Url, UriKind.Relative));
            }
        }
    }
}