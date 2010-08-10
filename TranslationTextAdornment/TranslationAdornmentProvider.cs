using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace TranslationTextAdornment
{
    internal class TranslationsChangedEventArgs : EventArgs
    {
        public readonly TranslationAdornment TranslationAdded;

        public readonly TranslationAdornment TranslationRemoved;

        public TranslationsChangedEventArgs(TranslationAdornment added, TranslationAdornment removed)
        {
            this.TranslationAdded = added;
            this.TranslationRemoved = removed;
        }
    }

    internal class TranslationAdornmentProvider
    {
        private ITextBuffer buffer;
        TranslationAdornment adornment;
        public IList<TranslationAdornment> translations = new List<TranslationAdornment>();

        private TranslationAdornmentProvider(ITextBuffer buffer)
        {
            this.buffer = buffer;
            //listen to the Changed event so we can react to deletions.
            this.buffer.Changed += OnBufferChanged;
            
        }
        public static TranslationAdornmentProvider Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty<TranslationAdornmentProvider>(delegate { return new TranslationAdornmentProvider(view.TextBuffer); });
        }
        public void Detach()
        {
            if (this.buffer != null)
            {
                //remove the Changed listener
                this.buffer.Changed -= OnBufferChanged;
                this.buffer = null;
            }
        }
        private void OnBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            //Make a list of all posts that have a span of at least one character after applying the change. There is no need to raise a changed event for the deleted adornments. The adornments are deleted only if a text change would cause the view to reformat the line and discard the adornments.
           // IList<TranslationAdornment> keptPosts = new List<TranslationAdornment>(this.posts.Count);

            foreach (TranslationAdornment translation in this.translations)
            {
                
                //Span span = post.Span.GetSpan(e.After);
                ////if a post does not span at least one character, its text was deleted.
                //if (span.Length != 0)
                //{
                //    keptPosts.Add(post);
                //}

                EventHandler<TranslationsChangedEventArgs> translationChanged = this.TranslationsChanged;
                if (translationChanged != null)
                    translationChanged(this, new TranslationsChangedEventArgs(null, translation));
                
            }

            this.translations.Clear();
           // this.posts = keptPosts;
        }
        public event EventHandler<TranslationsChangedEventArgs> TranslationsChanged;
        public void Add(SnapshotSpan span, string text)
        {
            if (text.Equals(""))
                return;
            if (span.Length == 0)
                throw new ArgumentOutOfRangeException("span");
            if (text == null)
                throw new ArgumentNullException("text");

            //Create a post adornment given the span, and text.
            adornment = new TranslationAdornment(span, text);

            //Add it to the list of posts.
            translations.Add(adornment);
            

            //Raise the changed event.
            EventHandler<TranslationsChangedEventArgs> postsChanged = this.TranslationsChanged;
            if (postsChanged != null)
                postsChanged(this, new TranslationsChangedEventArgs(adornment, null));
        }
        public void RemovePosts(SnapshotSpan span)
        {
            EventHandler<TranslationsChangedEventArgs> postsChanged = this.TranslationsChanged;

            //Get a list of all the posts that are being kept 
            IList<TranslationAdornment> keptPosts = new List<TranslationAdornment>(this.translations.Count);

            foreach (TranslationAdornment post in this.translations)
            {
                //find out if the given span overlaps with the post text span. If two spans are adjacent,
                //they do not overlap. To consider adjacent spans, use IntersectsWith.
                if (post.Span.GetSpan(span.Snapshot).OverlapsWith(span))
                {
                    //Raise the change event to delete this post.
                    if (postsChanged != null)
                        postsChanged(this, new TranslationsChangedEventArgs(null, post));
                }
                else
                    keptPosts.Add(post);
            }

            this.translations = keptPosts;
        }
        public Collection<TranslationAdornment> GetPosts(SnapshotSpan span)
        {
            IList<TranslationAdornment> overlappingPosts = new List<TranslationAdornment>();
            foreach (TranslationAdornment post in this.translations)
            {
                if (post.Span.GetSpan(span.Snapshot).OverlapsWith(span))
                    overlappingPosts.Add(post);
            }

            return new Collection<TranslationAdornment>(overlappingPosts);
        }
    }


}
