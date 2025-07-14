using System;
using System.Drawing;
using System.Windows.Forms;

namespace EBOS
{
    public class CustomColorRenderer : ToolStripProfessionalRenderer
    {
        private Color hoverColor = Color.FromArgb(90, 130, 45);

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
                using (SolidBrush brush = new SolidBrush(hoverColor))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }
}
