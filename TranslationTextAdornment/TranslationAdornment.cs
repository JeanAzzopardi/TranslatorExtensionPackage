using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace TranslationTextAdornment
{
    internal class TranslationAdornment
    {
        public readonly ITrackingSpan Span;
        public readonly string Text;
        public bool drawn;

        public TranslationAdornment(SnapshotSpan span, string text)
        {
            this.Span = span.Snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeExclusive);
            this.Text = text;
        }

    }
}
