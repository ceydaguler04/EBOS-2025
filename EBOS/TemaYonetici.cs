using EBOS;
using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

public static class TemaYonetici
{
    public static string AktifTema { get; set; } = "Yesil";

    public static Color TemaButonRenk()
    {
        return AktifTema switch
        {
            "Lacivert" => Color.FromArgb(30, 75, 200),
            "Koyu" => Color.FromArgb(80, 80, 80),
            _ => Color.FromArgb(90, 115, 47) // Yeşil
        };
    }

    public static Color TemaButonYaziRenk()
    {
        return Color.White;
    }

    public static Color SeciliButonRengi()
    {
        return AktifTema switch
        {
            "Lacivert" => Color.FromArgb(30, 75, 200),
            "Koyu" => Color.FromArgb(80, 80, 80),
            _ => Color.FromArgb(120, 160, 60) // Yeşil tema için
        };
    }

    public static Color HoverRenk()
    {
        switch (AktifTema)
        {
            case "Lacivert": return Color.FromArgb(60, 70, 140);
            case "Koyu": return Color.FromArgb(100, 100, 100);
            default: return Color.FromArgb(120, 150, 60); // Yeşil hover
        }
    }

    public static void Uygula(Form form)
    {
        if (form is null) return;

        Color topColor, leftColor, backColor, butonRenk, butonYaziRenk;

        switch (AktifTema)
        {
            case "Lacivert":
                backColor = Color.White;
                topColor = leftColor = Color.FromArgb(40, 55, 120);
                break;
            case "Koyu":
                backColor = Color.FromArgb(120, 120, 120);
                topColor = leftColor = Color.FromArgb(50, 50, 50);
                break;
            default:
                backColor = Color.White;
                topColor = leftColor = Color.FromArgb(90, 115, 47);
                break;
        }

        butonRenk = TemaButonRenk();
        butonYaziRenk = TemaButonYaziRenk();

        form.BackColor = backColor;

        foreach (Control control in form.Controls)
        {
            if (control is Guna2Panel panel)
            {
                if (panel.Dock == DockStyle.Top)
                    panel.FillColor = topColor;
                else if (panel.Location.X == 0 && panel.Width < 300)
                    panel.FillColor = leftColor;
            }

            if (control is Guna2Button button)
            {
                button.FillColor = butonRenk;
                button.ForeColor = butonYaziRenk;
            }

            foreach (Control inner in control.Controls)
            {
                if (inner is Guna2Button innerBtn)
                {
                    innerBtn.FillColor = butonRenk;
                    innerBtn.ForeColor = butonYaziRenk;
                }
            }
        }
    }

    public static void ContextMenuRenkleriUygula(ContextMenuStrip menu)
    {
        if (menu == null) return;

        menu.BackColor = AktifTema switch
        {
            "Lacivert" => Color.FromArgb(40, 55, 120),
            "Koyu" => Color.FromArgb(50, 50, 50),
            _ => Color.FromArgb(90, 115, 70) // Yeşil tema
        };

        menu.ForeColor = Color.White;
        menu.Renderer = new CustomColorRenderer(); // özel renderer uygulanıyor
    }

    public static Color ContextMenuHoverRenk()
    {
        return AktifTema switch
        {
            "Lacivert" => Color.FromArgb(60, 70, 140),
            "Koyu" => Color.FromArgb(80, 80, 80),
            _ => Color.FromArgb(110, 135, 55) // Yeşil hover
        };
    }
}

// ✅ EKLENMESİ GEREKEN SINIF
public class CustomColorRenderer : ToolStripProfessionalRenderer
{
    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        Color hoverColor = TemaYonetici.ContextMenuHoverRenk();

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
