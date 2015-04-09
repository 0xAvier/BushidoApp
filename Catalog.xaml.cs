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

        public Catalog()
        {
            InitializeComponent();
            factionList.ItemsSource = FactionList.Factions();
        }

        private void factionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = factionList.SelectedIndex;
            
            if (0 <= index && index < FactionList.Factions().Count)
            {
                string Url = "/FactionPage.xaml?index=" + index;
                NavigationService.Navigate(new Uri(Url, UriKind.Relative));
                factionList.SelectedIndex = -1;
            }
            
        }
    }
}