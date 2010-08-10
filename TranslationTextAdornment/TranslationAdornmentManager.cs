using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace TranslationTextAdornment
{
    internal class TranslationAdornmentManager
    {
        private readonly IWpfTextView view;
        private readonly IAdornmentLayer layer;
        private readonly TranslationAdornmentProvider provider;

        private TranslationAdornmentManager(IWpfTextView view)
        {
            this.view = view;
            this.view.LayoutChanged += OnLayoutChanged;
            this.view.Closed += OnClosed;
            

            this.layer = view.GetAdornmentLayer("TranslationAdornmentLayer");

            this.provider = TranslationAdornmentProvider.Create(view);
            this.provider.TranslationsChanged += OnPostsChanged;
        }
        public static TranslationAdornmentManager Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty<TranslationAdornmentManager>(delegate { return new TranslationAdornmentManager(view); });
        }
        private void OnPostsChanged(object sender, TranslationsChangedEventArgs e)
        {
            //Remove the post (when the adornment was added, the post adornment was used as the tag).
            if (e.TranslationRemoved != null)
                this.layer.RemoveAdornmentsByTag(e.TranslationRemoved);

            //Draw the newly added post (this will appear immediately: the view does not need to do a layout).
            if (e.TranslationAdded != null)
                this.DrawPost(e.TranslationAdded);
        }
        private void OnClosed(object sender, EventArgs e)
        {
            this.provider.Detach();
            this.view.LayoutChanged -= OnLayoutChanged;
            this.view.Closed -= OnClosed;
        }
        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            //Get all of the posts that intersect any of the new or reformatted lines of text.
            List<TranslationAdornment> newPosts = new List<TranslationAdornment>();

            ////The event args contain a list of modified lines and a NormalizedSpanCollection of the spans of the modified lines. 
            ////Use the latter to find the posts that intersect the new or reformatted lines of text.
            foreach (Span span in e.NewOrReformattedSpans)
            {
                newPosts.AddRange(this.provider.GetPosts(new SnapshotSpan(this.view.TextSnapshot, span)));
            }

            ////It is possible to get duplicates in this list if a post spanned 3 lines, and the first and last lines were modified but the middle line was not.
            ////Sort the list and skip duplicates.
            newPosts.Sort(delegate(TranslationAdornment a, TranslationAdornment b) { return a.GetHashCode().CompareTo(b.GetHashCode()); });

            TranslationAdornment lastPost = null;
            foreach (TranslationAdornment post in newPosts)
            {
                if (post != lastPost)
                {
                    lastPost = post;
                    this.DrawPost(post);
                }
            }
            
            //foreach (TranslationAdornment post in this.provider.translations)
            //{               
            //        DrawPost(post);               
            //}


        }
        private void DrawPost(TranslationAdornment post)
        {
          
            SnapshotSpan span = post.Span.GetSpan(this.view.TextSnapshot);
            Geometry g = this.view.TextViewLines.GetMarkerGeometry(span);

            if (g != null)
            {
                //Find the rightmost coordinate of all the lines that intersect the adornment.
                double maxRight = 0.0;
                foreach (ITextViewLine line in this.view.TextViewLines.GetTextViewLinesIntersectingSpan(span))
                    maxRight = Math.Max(maxRight, line.Right);

                //Create the visualization.
                TranslationInfoDisplay block = new TranslationInfoDisplay(maxRight, this.view.ViewportRight, g, post.Text);
                block.onMouseDown += RemoveAdornment;
                block.onMouseDown += (o, e) => { this.layer.RemoveAdornment(o as System.Windows.UIElement);
                this.provider.translations.Remove(post);
                };
                //Add it to the layer.
                this.layer.AddAdornment(span, post, block);
                
               
            }
            
        }

        private void RemoveAdornment(object o, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.layer.RemoveAdornment(o as System.Windows.UIElement);
            
        }


    }
}

