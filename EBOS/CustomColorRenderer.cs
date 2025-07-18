using System;
using System.Drawing;
using System.Windows.Forms;

namespace EBOS
{
    public class CustomColorRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Color hoverColor = TemaYonetici.ContextMenuHoverRenk(); // Tema'ya göre hover rengi alınıyor

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
