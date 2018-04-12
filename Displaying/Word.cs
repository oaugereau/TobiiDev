using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Displaying
{
    public class Word
    {
        public string Text { get; set; }
        public Rectangle Rectangle { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }

        public Word(string text, Rectangle rect)
        {
            Text = text;
            Rectangle = rect;
            CenterX = Rectangle.Left + Rectangle.Width / 2;
            CenterY = Rectangle.Bottom + Rectangle.Height / 2;
        }
    }
}
