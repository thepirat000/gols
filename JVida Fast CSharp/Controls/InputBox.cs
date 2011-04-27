// Thepirat 2011
// thepirat000@hotmail.com
namespace JVida_Fast_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public partial class InputBox : Form
    {
        private InputBox()
        {
            InitializeComponent();
        }

        public static DialogResult Show(string title, string question, string defaultValue, out string value)
        {
            InputBox ib = new InputBox();
            ib.Text = title;
            ib.lblInfo.Text = question;
            ib.txtValue.Text = defaultValue;
            if (ib.ShowDialog() == DialogResult.OK)
            {
                value = ib.txtValue.Text;
                return DialogResult.OK;
            }
            else
            {
                value = null;
                return DialogResult.Cancel;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
