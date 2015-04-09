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

        private void characterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CharacterPage.xaml?factIndex=" + currentFactionIndex + "&charIndex=" + 
                                                characterList.SelectedIndex, UriKind.Relative));
            characterList.SelectedItem = null;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string indexStr;
            if (NavigationContext.QueryString.TryGetValue("index", out indexStr))
            {
                currentFactionIndex = int.Parse(indexStr);
                currentFaction = FactionList.Factions()[int.Parse(indexStr)];
                PageTitle.Text = currentFaction.Name;
                characterList.ItemsSource = currentFaction.Characters;
                
                ///BackgroundImage.ImageSource.
                BackgroundImage.ImageSource = Image.Get(currentFaction.LogoPath);
                
                ChangeColor(currentFaction.Name);
            }
            base.OnNavigatedTo(e);
        }

        private void ChangeColor(string factionName)
        {
            characterList.Foreground = Faction.GetColor(currentFaction.Name);
            PageTitle.Foreground = Faction.GetColor(currentFaction.Name);
            ApplicationTitle.Foreground = Faction.GetColor(currentFaction.Name);
        }
    }
}