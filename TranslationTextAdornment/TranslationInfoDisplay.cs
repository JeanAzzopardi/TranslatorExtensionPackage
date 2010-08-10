using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
namespace TranslationTextAdornment
{
    internal class TranslationInfoDisplay : Canvas
    {
        private Geometry textGeometry;
        private Grid postGrid;
        private static Brush brush;
        private static Pen solidPen;
        private static Pen dashPen;

        public EventHandler<System.Windows.Input.MouseButtonEventArgs> onMouseDown;

        public TranslationInfoDisplay(double textRightEdge,
            double viewRightEdge,
            Geometry newTextGeometry,
            string body)
        {
            if (brush == null)
            {
                brush = new SolidColorBrush(Color.FromArgb(0x20, 0x48, 0x3d, 0x8b));
                brush.Freeze();
                Brush penBrush = new SolidColorBrush(Colors.DarkSlateBlue);
                penBrush.Freeze();
                solidPen = new Pen(penBrush, 0.5);
                solidPen.Freeze();
                dashPen = new Pen(penBrush, 0.5);
                dashPen.DashStyle = DashStyles.Dash;
                dashPen.Freeze();
            }

            this.textGeometry = newTextGeometry;

            TextBlock tb = new TextBlock();
            tb.Text = body;

            const int MarginWidth = 8;
            this.postGrid = new Grid();
            this.postGrid.RowDefinitions.Add(new RowDefinition());
            this.postGrid.RowDefinitions.Add(new RowDefinition());
            ColumnDefinition cEdge = new ColumnDefinition();
            cEdge.Width = new GridLength(MarginWidth);
            ColumnDefinition cEdge2 = new ColumnDefinition();
            cEdge2.Width = new GridLength(MarginWidth);
            this.postGrid.ColumnDefinitions.Add(cEdge);
            this.postGrid.ColumnDefinitions.Add(new ColumnDefinition());
            this.postGrid.ColumnDefinitions.Add(cEdge2);

            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
            rect.RadiusX = 6;
            rect.RadiusY = 3;
            rect.Fill = brush;
            rect.Stroke = Brushes.DarkSlateBlue;

            Size inf = new Size(double.PositiveInfinity, double.PositiveInfinity);
            tb.Measure(inf);
            this.postGrid.Width = tb.DesiredSize.Width + 2 * MarginWidth;

            Grid.SetColumn(rect, 0);
            Grid.SetRow(rect, 0);
            Grid.SetRowSpan(rect, 1);
            Grid.SetColumnSpan(rect, 3);
            Grid.SetRow(tb, 0);
            Grid.SetColumn(tb, 1);
            this.postGrid.Children.Add(rect);
            this.postGrid.Children.Add(tb);

            Canvas.SetLeft(this.postGrid, Math.Max(viewRightEdge - this.postGrid.Width - 20.0, textRightEdge + 20.0));
            Canvas.SetTop(this.postGrid, textGeometry.GetRenderBounds(solidPen).Top);

            this.Children.Add(this.postGrid);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (this.textGeometry != null)
            {
                dc.DrawGeometry(brush, solidPen, this.textGeometry);
                Rect textBounds = this.textGeometry.GetRenderBounds(solidPen);
                Point p1 = new Point(textBounds.Right, textBounds.Bottom);
                Point p2 = new Point(Math.Max(Canvas.GetLeft(this.postGrid) - 20.0, p1.X), p1.Y);
                Point p3 = new Point(Math.Max(Canvas.GetLeft(this.postGrid), p1.X), (Canvas.GetTop(this.postGrid) + p1.Y) * 0.5);
                dc.DrawLine(dashPen, p1, p2);
                dc.DrawLine(dashPen, p2, p3);
            }
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            onMouseDown.Invoke(this, e);

        }
        


    }
}
