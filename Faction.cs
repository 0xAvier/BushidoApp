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
        public Character(string pName, string pMinisPath, List<string> pProfilePaths)
        {
            Name = pName;
            MinisPath = pMinisPath;
            ProfilePaths = pProfilePaths;
            //*
            MinisImage = Image.Get(MinisPath);
            ProfileImage = new List<BitmapImage>();
            foreach (string path in ProfilePaths)
            { 
                ProfileImage.Add(Image.Get(path));
            }
            //*/
        }
        
        public string Name { get; set; }
        public string MinisPath { get; set; }
        public List<string> ProfilePaths { get; set; }
        
        public BitmapImage MinisImage { get; set; }
        public List<BitmapImage> ProfileImage { get; set; }
    }   

    public static class CharacterList
    {
        public static List<Character> GetCharacters(string listPath, string imagePath)
        {
            List<Character> characters = new List<Character>();
            foreach (XElement character in Parsing.ParseFaction(listPath))
            {
                string name = character.Element("name").Value;
                string miniPath;
                try
                {
                    miniPath = imagePath + character.Element("mini").Value;
                }
                catch (Exception e)
                {
                    Exception tmp;
                    tmp = e;
                    e = tmp;
                    miniPath = null;
                }
                List<string> cardsPath = new List<string>();
                foreach (XElement card in character.Descendants("card"))
                {
                    cardsPath.Add(imagePath + card.Value);
                }
                
                characters.Add(new Character(name, miniPath, cardsPath));
            }
            
            characters.Sort((x,y) => string.Compare(x.Name, y.Name));

            return characters;
        }
        public static List<Character> TestChar(List<Character> entry)
        {
            List<string> list = new List<string>();
            list.Add("\\image\\kenzondprofilecard.jpg");
            for (int i = 0; i < 15; i++)
            {
                entry.Add(new Character("Number " + i.ToString(),
                    "\\image\\kenzo.jpg", list));
            }

            return entry;
        }
    }

    public class Faction
    {
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

        public Faction(string pName, string pPath)
        {
            Name = pName;
            Path = pPath;
            Logo = Image.Get(Path + "logo.png");
            LogoPath = Path + "logo.png";
            /*
            if (Name == "Cult of Yurei") Logo = Image.Get(Path + "logo.png");
            else Logo = Image.Get(Path + "logo.jpg");
             */
            
            Characters = CharacterList.GetCharacters(Path + "list.xml", Path);
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string LogoPath { get; set; }
        public BitmapImage Logo { get; set; }
        public List<Character> Characters { get; set; }
    }

    public static class FactionList
    {
        public static List<Faction> FactionsList { get; set; }

        private static List<Faction> setFactions()
        {
            FactionsList = new List<Faction>();
            foreach (XElement faction in Parsing.ParseCatalog())
            {
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
