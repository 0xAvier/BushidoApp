using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BushidoApp
{

    public static class Image
    {
        public static BitmapImage Get(string Path)
        {
            if (Path == null) return null;
            BitmapImage image = new BitmapImage(new Uri(Path, UriKind.Relative));
            return image;
        }
    }
    
    public class Character
    {
        /*
         * Define a character (name / picture / profile)
         */

        public string Name { get; set; }
        // text infos
        public string MinisPath { get; set; }
        public List<string> ProfilePaths { get; set; }
        // image objects
        public BitmapImage MinisImage { get; set; }
        public List<BitmapImage> ProfileImage { get; set; }

        public Character(string pName, string pMinisPath, List<string> pProfilePaths)
        {
            // init name
            Name = pName;
            // init path for profile
            MinisPath = pMinisPath;
            MinisImage = Image.Get(MinisPath);
            // init path for cards
            ProfilePaths = pProfilePaths;
            ProfileImage = new List<BitmapImage>();
            // a profile will have several cards
            foreach (string path in ProfilePaths)
            { 
                ProfileImage.Add(Image.Get(path));
            }
        }
    }   

    public static class CharacterList
    {
        /*
         * Define a character list that gives an initialized list of character
         */

        private static Character initializeCharacter(XElement character, string imagePath)
        {
            // extract the name
            string name = character.Element("name").Value;
            // extract the minis image
            string miniPath;
            try {
                miniPath = imagePath + character.Element("mini").Value;
            } catch (Exception e) {
                Exception tmp;
                tmp = e;
                e = tmp;
                miniPath = null;
            }
            // extract the profiles images
            List<string> cardsPath = new List<string>();
            foreach (XElement card in character.Descendants("card")) {
                cardsPath.Add(imagePath + card.Value);
            }
            // return the character
            return new Character(name, miniPath, cardsPath);
        }
        
        // return a list of character, reading into 
        public static List<Character> GetCharacters(string listPath, string imagePath)
        {
            List<Character> characters = new List<Character>();
            foreach (XElement character in Parsing.ParseFaction(listPath))
            {
                // for each xml element, get a character out of it.
                characters.Add(initializeCharacter(character, imagePath));
            }
            
            // sort the list
            characters.Sort((x,y) => string.Compare(x.Name, y.Name));

            return characters;
        }
    }

    public class Faction
    {
        /*
         * Define a faction (name / logo / character list / theme color)
         */

        public string Name { get; set; }
        public string Path { get; set; }
        public string LogoPath { get; set; }
        public BitmapImage Logo { get; set; }
        public List<Character> Characters { get; set; }

        public Faction(string pName, string pPath)
        {
            Name = pName;
            Path = pPath;
            Logo = Image.Get(Path + "logo.png");
            LogoPath = Path + "logo.png";

            Characters = CharacterList.GetCharacters(Path + "list.xml", Path);
        }

        // faction color
        public static System.Windows.Media.SolidColorBrush GetColor(string factionName)
        {
            //FF 00 00 8B
            if (factionName == "Prefecture of Ryu") return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 205));
            //FF 8B 00 8B
            if (factionName == "Cult of Yurei") return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 139, 0, 139));
            //FF 00 80 00
            if (factionName == "The Ito Clan") return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 168, 0));
            //FF DC 14 3C
            if (factionName == "Sylvermoon Syndicat") return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 220, 20, 60));
            //FF 32 CD 32
            if (factionName == "Temple of Ro-Kan") return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 50, 205, 50));
            //FF B2 22 22
            if (factionName == "Savage Wave") return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 178, 34, 34));

            return new SolidColorBrush(Colors.Blue);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public static class FactionList
    {
        /*
         * Define the faction list by reading the resources file 
         */

        public static List<Faction> FactionsList { get; set; }

        private static List<Faction> setFactions()
        {
            FactionsList = new List<Faction>();
            // read the faction from the xml file
            foreach (XElement faction in Parsing.ParseCatalog())
            {
                // initialize them
                FactionsList.Add(new Faction(
                                faction.Element("name").Value,
                                faction.Element("path").Value));
            }

            return FactionsList;
        }
        
        public static List<Faction> Factions() 
        {
            if (FactionsList == null)
            {
                setFactions();
            }
            return FactionsList;
        }
    }

}
