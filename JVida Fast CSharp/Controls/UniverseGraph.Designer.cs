using System.ComponentModel;

namespace JVida_Fast_CSharp
{
    public partial class UniverseGraph
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (null != this._bmp)
            {
                this._bmp.Dispose();
            }
            if (null != this._font)
            {
                this._font.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UniverseGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "UniverseGraph";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.UniverseGraph_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.UniverseGraph_DragOver);
            this.DragLeave += new System.EventHandler(this.UniverseGraph_DragLeave);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UniverseGraph_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UniverseGraph_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
