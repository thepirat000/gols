// Thepirat 2011
// thepirat000@hotmail.com

using System;
using System.Windows.Forms;

namespace JVida_Fast_CSharp
{
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
            value = null;
            return DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
