using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdmondsKarp
{
    public partial class GetElementTransition : Form
    {
        public int Element;
        public bool isCanceled = false;

        //Properties para impedir o close da janela
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }


        public GetElementTransition()
        {
            InitializeComponent();
            
            this.cbElementos.DropDownStyle = ComboBoxStyle.DropDownList;
            
            for (int i = 0; i < 100; i++)
                this.cbElementos.Items.Add(i);

        }

        private void btnAdicionarElemento_Click(object sender, EventArgs e)
        {
            this.Element = int.Parse(cbElementos.SelectedItem.ToString());
            this.Hide();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.isCanceled = true;
            this.Hide();
        }

        private void cbElementos_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Element = Convert.ToInt32(cbElementos.SelectedValue);
        }
    }
}
