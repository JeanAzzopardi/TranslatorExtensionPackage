using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;

namespace JeanAzzopardi.TranslatorExtensionPackage
{
    public partial class FormOptions : Form
    {
        public static StringDictionary LanguageNamesToShortCodes = new StringDictionary()
        {
            {"Afrikaans","af"},
            {"Albanian","sq"},
            {"Arabic","ar"},
            {"Belarusian","be"},
            {"Bulgarian","bg"},
            {"Catalan","ca"},
            {"Chinese","zh-CN"},
            {"Chinese (Simplified)","zh-CN"},
            {"Simplified Chinese","zh-CN"},
            {"Chinese (Traditional)","zh-TW"},
            {"Traditional Chinese","zh-TW"},
            {"Croatian","hr"},
            {"Czech","cs"},
            {"Danish","da"},
            {"Dutch","nl"},
            {"English","en"},
            {"Estonian","et"},
            {"Filipino","tl"},
            {"Finnish","fi"},
            {"French","fr"},
            {"Galician","gl"},
            {"German","de"},           
            {"Greek","el"},
            {"Hebrew","iw"},
            {"Hindi","hi"},
            {"Hungarian","hu"},
            {"Icelandic","is"},
            {"Indonesian","id"},
            {"Irish","ga"},
            {"Italian","it"},
            {"Japanese","ja"},
            {"Korean","ko"},
            {"Latvian","lv"},
            {"Lithuanian","lt"},
            {"Macedonian","mk"},
            {"Malay","ms"},
            {"Maltese","mt"},
            {"Persian","fa"},
            {"Polish","pl"},
            {"Portugese","pt"},
            {"Romanian","ro"},
            {"Russian","ru"},
            {"Serbian","sr"},
            {"Slovak","sk"},
            {"Slovenian","sl"},
            {"Spanish","es"},
            {"Swahili","sw"},
            {"Swedish","sv"},
            {"Thai","th"},
            {"Turkish","tr"},
            {"Ukranian","uk"},
            {"Vietnamese","vi"},
            {"Welsh","cy"},
            {"Yiddish","yi"}
        };

        public FormOptions()
        {
            InitializeComponent();
        


            List<String> LanguageNames = new List<string>();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;


            foreach (String LanguageName in LanguageNamesToShortCodes.Keys)
            {
                LanguageNames.Add(textInfo.ToTitleCase(LanguageName));
            }
            LanguageNames.Sort();
            foreach (String LanguageName in LanguageNames)
            {
                CmbFrom.Items.Add(LanguageName);
                CmbTo.Items.Add(LanguageName);
            }

            CmbFrom.SelectedItem = LanguageSettings.Default.LangFrom;
            CmbTo.SelectedItem = LanguageSettings.Default.LangTo;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            LanguageSettings.Default.LangFrom = CmbFrom.SelectedItem.ToString();
            LanguageSettings.Default.LangTo = CmbTo.SelectedItem.ToString();
            LanguageSettings.Default.Save();
            this.Close();
        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {            
            String tempLang = CmbTo.SelectedItem.ToString();
            CmbTo.SelectedItem = CmbFrom.SelectedItem.ToString();
            CmbFrom.SelectedItem = tempLang;
        }

     

      

       
    }
}
