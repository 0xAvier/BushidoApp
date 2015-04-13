using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BushidoApp
{
    public partial class FactionPage : PhoneApplicationPage
    {
        Faction currentFaction { get; set; }
        int currentFactionIndex { get; set; }

        public FactionPage()
        {
            InitializeComponent();
        }

        // on page load
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string indexStr;
            if (NavigationContext.QueryString.TryGetValue("index", out indexStr))
            {
                // get the faction index
                currentFactionIndex = int.Parse(indexStr);
                // get the faction corresponding to the index
                currentFaction = FactionList.Factions()[currentFactionIndex];
                
                // initialize the title
                PageTitle.Text = currentFaction.Name;
                // fill the list
                characterList.ItemsSource = currentFaction.Characters;
                
                // set the background image
                BackgroundImage.ImageSource = Image.Get(currentFaction.LogoPath);
                // set the text color 
                ChangeColor(currentFaction.Name);
            }
            base.OnNavigatedTo(e);
        }

        // change the color theme
        private void ChangeColor(string factionName)
        {
            characterList.Foreground = Faction.GetColor(currentFaction.Name);
            PageTitle.Foreground = Faction.GetColor(currentFaction.Name);
            ApplicationTitle.Foreground = Faction.GetColor(currentFaction.Name);
        }

        // on click on a character name: go to the corresponding page
        private void characterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CharacterPage.xaml?factIndex=" + currentFactionIndex + "&charIndex=" +
                                                characterList.SelectedIndex, UriKind.Relative));
        }
    }
}