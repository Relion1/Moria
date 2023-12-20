using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Moria
{
    public partial class Form2 : Form
    {
        public string emailname {get;set;} 
        public Form2()
        {
            InitializeComponent();
        }
        string constring = "Data Source=KAPOS\\SQLEXPRESS;Initial Catalog=moria_database;Integrated Security=True";
        private void Form2_Load(object sender, EventArgs e)
        {
            label2.Text = emailname;
            byte[] getimage=new byte[0];
            SqlConnection con= new SqlConnection(constring);
            string q = "SELECT * FROM Login WHERE email='" + label2.Text + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            con.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            if (dataReader.HasRows)
            {
                label2.Text = dataReader[0].ToString();
                byte[] images = (byte[])dataReader["image"];
                if (images == null)
                {
                    button1.Image = null;
                    //bunifuButton1.Image = null;
                }
                else
                {
                    MemoryStream me = new MemoryStream(images);
                    bunifuPictureBox1.Image=Image.FromStream(me);
                }
            }
            con.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.Show();
        }
    }
}
