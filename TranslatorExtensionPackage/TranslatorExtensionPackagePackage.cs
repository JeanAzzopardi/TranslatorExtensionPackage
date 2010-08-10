using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Editor;
using TranslationTextAdornment;
using System.Collections.Specialized;
using System.Collections.Generic;
using EnvDTE;
using System.Text;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace JeanAzzopardi.TranslatorExtensionPackage
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidTranslatorExtensionPackagePkgString)]
    public sealed class TranslatorExtensionPackagePackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public TranslatorExtensionPackagePackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                CommandID translateCommandID = new CommandID(GuidList.guidTranslatorExtensionPackageCmdSet, (int)PkgCmdIDList.cmdIDTranslate);
                MenuCommand menuItemTranslate = new MenuCommand(MenuItemCallback, translateCommandID );
                mcs.AddCommand( menuItemTranslate );

                CommandID translateOptionsCommandID = new CommandID(GuidList.guidTranslatorExtensionPackageCmdSet, (int)PkgCmdIDList.cmdIDTranslateOptions);
                MenuCommand menuItemTranslateOptions = new MenuCommand(MenuItemOptions, translateOptionsCommandID );
                mcs.AddCommand(menuItemTranslateOptions);

            }

            

         
        }
        #endregion

        static string GetTranslation(String input)
        {
            String baseUrl = "http://ajax.googleapis.com/ajax/services/language/translate?";
            String dataToBeTranslated = "";
            String sourceLanguage = "";
            String destinationLanguage = "";

            
            if (input.Contains("|"))
            {
                String[] splitInput = input.Split('|');
                dataToBeTranslated = splitInput[0].Trim();
                destinationLanguage = FormOptions.LanguageNamesToShortCodes[splitInput[1].Trim()];
                sourceLanguage = splitInput.Length == 3 ? splitInput[2].Trim() : "";
                if (sourceLanguage != "") sourceLanguage = FormOptions.LanguageNamesToShortCodes[sourceLanguage];

            }



            String langpair = String.Format("{0}|{1}", sourceLanguage, destinationLanguage);
            UTF8Encoding utf8encoding = new UTF8Encoding();
            UnicodeEncoding utf32encoding = new UnicodeEncoding();
            String data = MakeQueryString(new Dictionary<string, string>()
            {
                {"v","1.0"},
                {"q",dataToBeTranslated},
                {"ie","UTF8"},
                {"langpair",langpair}
            });
            String url = baseUrl + data;
            String json = GetWebContents(url);


            JObject o = JObject.Parse(json);
            String s = o["responseData"]["translatedText"].ToString();

            String utf8DecodedString = Encoding.UTF8.GetString(Encoding.Default.GetBytes(s));


            return utf8DecodedString;
        }

        static String GetWebContents(String url)
        {
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            return client.DownloadString(url);
        }

        //Create a query string from a dictionary
        static public string MakeQueryString(Dictionary<string, string> args)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string name in args.Keys)
            {
                sb.Append(HttpUtility.UrlEncode(name));
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(args[name]));
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
     
        // get the active WpfTextView, if there is one.
        private IWpfTextView GetActiveTextView()
        {
            IWpfTextView view = null;
            IVsTextView vTextView = null;

            IVsTextManager txtMgr =
                (IVsTextManager)GetService(typeof(SVsTextManager));
            int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (null != userData)
            {
                IWpfTextViewHost viewHost;
                object holder;
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out holder);
                viewHost = (IWpfTextViewHost)holder;
                view = viewHost.TextView;
            }

            return view;
        }


        private void MenuItemOptions(object sender, EventArgs e)
        {
            var frmOption = new FormOptions();
            frmOption.ShowDialog();
            return;
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            IWpfTextView view = GetActiveTextView();
            String selectedText = view.Selection.SelectedSpans[0].GetText();

            selectedText = SentenceExtractor(selectedText);

            String translationString = String.Format("{0}|{1}|{2}", selectedText, LanguageSettings.Default.LangTo, LanguageSettings.Default.LangFrom);
            String translation = GetTranslation(translationString);
            translation = translation.Replace("\"", "");
            translation = HttpUtility.HtmlDecode(translation);

            Guid clsid = Guid.Empty;
            Connector.Execute(view, String.Format("{0}->{1}: {2}",FormOptions.LanguageNamesToShortCodes[LanguageSettings.Default.LangFrom],
                FormOptions.LanguageNamesToShortCodes[LanguageSettings.Default.LangTo],translation.Trim()));
            int result;
            
        }

        static public string SentenceExtractor(String input)
        {
            if (input.Length < 3) return input;
            Stack<int> spacePositions = new Stack<int>();
            Char c = input[0];

            input = input.Replace('_', ' ');
            String output = input;

            for (int i = 0; i < input.Length - 1; i++)
            {
                c = input[i];
                if (c.IsLowerCase())
                {
                    if (input[i + 1].IsUpperCase())
                    {
                        spacePositions.Push(i);
                    }
                }

                if (c.IsUpperCase())
                {
                    if (i > 0 && i < input.Length - 1)
                        if (input[i - 1].IsUpperCase() && input[i + 1].IsLowerCase())
                        {
                            spacePositions.Push(i - 1);
                        }
                }


            }

            foreach (int pos in spacePositions)
            {
                output = output.Insert(pos + 1, " ");
            }

            return output;
        }

    }
}
