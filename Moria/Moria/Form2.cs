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
using Moria.Properties;
using System.Text.RegularExpressions;

namespace Moria
{
    public partial class Form2 : Form
    {
        public string emailname {get;set;} 
        public Form2()
        {
            InitializeComponent();
        }
        string constring = "Data Source=DESKTOP-EHBA0PG\\SQLEXPRESS;Initial Catalog=moria_database;Integrated Security=True";
        private void Form2_Load(object sender, EventArgs e)
        {
            LoadFormData();
        }

        private void LoadFormData()
        {

            label2.Text = emailname;
            byte[] getimage = new byte[0];
            SqlConnection con = new SqlConnection(constring);
            string q = "SELECT * FROM Login WHERE email='" + label2.Text + "'"; //label2.text sol bardaki email yazısı bu yazıya göre kullanıcı seçiliyor veritabanında
            SqlCommand cmd = new SqlCommand(q, con);
            con.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            if (dataReader.HasRows)
            {

                label2.Text = dataReader["email"].ToString();
                bunifuTextBox1.Text = dataReader["firstname"].ToString();
                bunifuTextBox5.Text = dataReader["firstname"].ToString();


                bunifuTextBox2.Text = dataReader["lastname"].ToString();
                bunifuTextBox6.Text = dataReader["lastname"].ToString();


                bunifuTextBox3.Text = dataReader["email"].ToString();
                bunifuTextBox7.Text = dataReader["email"].ToString();


                bunifuTextBox4.Text = dataReader["password"].ToString();
                byte[] images = (byte[])dataReader["image"];
                if (images == null)
                {
                    //bunifuButton1.Image = null;
                }
                else
                {
                    MemoryStream me = new MemoryStream(images);
                    bunifuPictureBox1.Image = Image.FromStream(me);
                    bunifuPictureBox4.Image = Image.FromStream(me);
                    bunifuPictureBox5.Image = Image.FromStream(me);
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
        private bool check;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (check)
            {
                bunifuPanel1.Width += 10;
                if(bunifuPanel1.Size == bunifuPanel1.MaximumSize) {
                    bunifuPictureBox2.Left = +230;
                    timer1.Stop();
                    check = false;
                    bunifuPictureBox2.Image = Resources.leftArrow;
                }
            }
            else
            {
                bunifuPanel1.Width -= 10;
                if(bunifuPanel1.Size==bunifuPanel1.MinimumSize)
                {
                    bunifuPictureBox2.Left = 2;
                    timer1.Stop();
                    check = true;
                    bunifuPictureBox2.Image = Resources.menu;
                }
            }
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void bunifuPictureBox3_Click(object sender, EventArgs e)
        {
            if(bunifuPanel6.Visible == false)
            {
                bunifuPanel6.Visible = true;
            }
            else
            {
                bunifuPanel6.Visible = false;
            }
        }

        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuPictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == false)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }
        }

        private void bunifuPictureBox5_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "SELECT image(*Jpg; *.png; *Gif|*.Jpg; *.png; *Gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bunifuPictureBox5.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void bunifuButton21_Click(object sender, EventArgs e) //bu save button
        {


            if (string.IsNullOrEmpty(bunifuTextBox5.Text.Trim()))
            {
                errorProvider1.SetError(bunifuTextBox5, "Bos gecemessin");
                return;
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox5, string.Empty);
            }



            if (string.IsNullOrEmpty(bunifuTextBox6.Text.Trim()))
            {
                errorProvider1.SetError(bunifuTextBox6, "Bos gecemessin");
                return;
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox6, string.Empty);
            }



            if (string.IsNullOrEmpty(bunifuTextBox7.Text.Trim()))
            {
                errorProvider1.SetError(bunifuTextBox7, "Bos gecemessin");
                return;
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox7, string.Empty);
            }



            string validEmail = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            if (Regex.IsMatch(bunifuTextBox7.Text, validEmail))
            {
                errorProvider1.Clear();
            }
            else
            {
                errorProvider1.SetError(this.bunifuTextBox7, "Please provide vaild Mail address");
                return;
            }



            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string q = "UPDATE login SET password ='"+bunifuTextBox4.Text+"',firstname=@fname,lastname=@lname,email=@email,image=@image";
            MemoryStream me = new MemoryStream();
            bunifuPictureBox5.Image.Save(me, bunifuPictureBox1.Image.RawFormat);
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.Parameters.AddWithValue("@fname", bunifuTextBox5.Text);
            cmd.Parameters.AddWithValue("@lname", bunifuTextBox6.Text);
            cmd.Parameters.AddWithValue("@email", bunifuTextBox7.Text);
            cmd.Parameters.AddWithValue("@image", me.ToArray());
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Profile is updated");
            LoadFormData();


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (check)
            {
                panel3.Height += 10;
                if (panel3.Size == panel3.MaximumSize)
                {
                    
                    timer2.Stop();
                    check = false;
                    //alt satır olmuyor anlamadım butonun image özelliği yok diye herhalde çok önemli değil bence
                    //  bunifuButton22.Image = Resources.Collapce;
                }
            }
            else
            {
                panel3.Height -= 10;
                if (panel3.Size == panel3.MinimumSize)
                {
                    
                    timer2.Stop();
                    check = true;
                    //alt satır olmuyor anlamadım butonun image özelliği yok diye herhalde çok önemli değil bence
                    // bunifuButton22.Image = Expand_Arrow;
                }
            }
        }

        private void updateprofile_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {
                panel2.Visible = true;
            }
        }

        private void bunifuPictureBox6_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }
    }
}
