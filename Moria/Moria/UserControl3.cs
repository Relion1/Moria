using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms;

namespace Moria
{
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; bunifuLabel1.Text = value; }
        }

        private Image _icon;
        public Image Icon
        {
            get { return _icon; }
            set { _icon = value; bunifuPictureBox1.Image = value; AddHeighttext(); }
        }
        void AddHeighttext()
        {
            bunifuLabel1.Height = Uilist.GeTTextHeight(bunifuLabel1) + 10;
            this.Height = bunifuLabel1.Bottom + 10;
        }

        private void UserControl3_Load(object sender, EventArgs e)
        {
            AddHeighttext();
        }
    }
}