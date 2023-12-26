﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moria
{
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; bunifuLabel1.Text = value; AddHeighttext(); }
        }
        void AddHeighttext()
        {  
            UserControl2 user = new UserControl2();
            user.BringToFront();
            bunifuLabel1.Height = Uilist.GeTTextHeight(bunifuLabel1) + 10;
            user.Height = bunifuLabel1.Top + bunifuLabel1.Right;
            this.Height = user.Bottom + 10;
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            AddHeighttext();
        }
    }
}