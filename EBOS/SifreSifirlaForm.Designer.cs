namespace EBOS
{
    partial class SifreSifirlamaForm
    {
        /// <summary>
        /// Gereken tasarım bileşenlerini temizler.
        /// </summary>
        /// <param name="disposing">yönetilen kaynaklar atılsın mı?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Visual Studio Designer tarafından kullanılır. Şu an boş.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // SifreSifirlamaForm
            // 
            ClientSize = new Size(282, 253);
            Name = "SifreSifirlamaForm";
            Load += SifreSifirlamaForm_Load;
            ResumeLayout(false);
            // Artık tamamen boş - elle tasarım kullanılmadığı için.
        }

        private System.ComponentModel.IContainer components = null;
    }
}
