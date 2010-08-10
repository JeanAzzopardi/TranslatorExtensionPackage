using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;


namespace TranslationTextAdornment
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public sealed class Connector : IWpfTextViewCreationListener
    {
        public void TextViewCreated(IWpfTextView textView)
        {
            TranslationAdornmentManager.Create(textView);
        }

        static public void Execute(IWpfTextView view, string translation)
        {
            //Add a post on the selected text.
            //Get the provider for the post adornments in the property bag of the view.
            TranslationAdornmentProvider provider = view.Properties.GetProperty<TranslationAdornmentProvider>(typeof(TranslationAdornmentProvider));

            //Add the post adornment using the provider.
            provider.Add(view.Selection.SelectedSpans[0], translation);
        }

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("TranslationAdornmentLayer")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        public AdornmentLayerDefinition postLayerDefinition;
    }
}
