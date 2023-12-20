using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Moria
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();


        }

        Color btn = Color.SpringGreen;
        Color btr = Color.FromArgb(137, 140, 142);
        Color bb = Color.DarkSlateGray;
        string constring = "Data Source=KAPOS\\SQLEXPRESS;Initial Catalog=moria_database;Integrated Security=True";
        private void Form1_Load(object sender, EventArgs e)
        {
            BtnLogin.PerformClick();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();

        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            
           
            if (bunifuPictureBox1.Image == null)
            {
                MessageBox.Show("Select a photo");
               
            }
            else 
            {
                if (string.IsNullOrEmpty(bunifuTextBox4.Text.Trim()))
                {
                    errorProvider1.SetError(bunifuTextBox4, "Bos gecemessin");
                    return;
                }
                else
                {
                    errorProvider1.SetError(bunifuTextBox4, string.Empty);
                }

                if (string.IsNullOrEmpty(bunifuTextBox3.Text.Trim()))
                {
                    errorProvider1.SetError(bunifuTextBox3, "Bos gecemessin");
                    return;
                }
                else
                {
                    errorProvider1.SetError(bunifuTextBox3, string.Empty);
                }

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
                if(bunifuTextBox6.Text != bunifuTextBox7.Text)
                {
                    MessageBox.Show("Password not equal");
                }
                else
                {
                    SqlConnection con = new SqlConnection(constring);
                    string query = "insert into Login(firstname,lastname,email,password,confirmpassword,image)values(@firstname,@lastname,@email,@password,@confirmpassword,@image)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    MemoryStream me = new MemoryStream();
                    bunifuPictureBox1.Image.Save(me, bunifuPictureBox1.Image.RawFormat);
                    cmd.Parameters.AddWithValue("firstname", bunifuTextBox4.Text);
                    cmd.Parameters.AddWithValue("lastname", bunifuTextBox3.Text);
                    cmd.Parameters.AddWithValue("email", bunifuTextBox5.Text);
                    cmd.Parameters.AddWithValue("password", bunifuTextBox6.Text);
                    cmd.Parameters.AddWithValue("confirmpassword", bunifuTextBox7.Text);
                    cmd.Parameters.AddWithValue("image", me.ToArray());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Registeration successfully complated");
                    bunifuTextBox4.Clear();
                    bunifuTextBox3.Clear();
                    bunifuTextBox5.Clear();
                    bunifuTextBox6.Clear();
                    bunifuTextBox7.Clear();
                    bunifuPictureBox1.Image = null;
                }
            }
        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "SELECT image(*Jpg; *.png; *Gif|*.Jpg; *.png; *Gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bunifuPictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bunifuTextBox1.Text.Trim()))
            {
                errorProvider1.SetError(bunifuTextBox1, "email giriniz");
                return;
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox1, string.Empty);
            }

            if (string.IsNullOrEmpty(bunifuTextBox2.Text.Trim()))
            {
                errorProvider1.SetError(bunifuTextBox2, "sifre giriniz");
                return;
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox2, string.Empty);
            }
            SqlConnection con= new SqlConnection(constring);
            con.Open();
            string q = "SELECT * FROM Login WHERE email = '" + bunifuTextBox1.Text + "'AND password='" + bunifuTextBox2.Text + "'";
            SqlCommand cmd = new SqlCommand(q,con);
            SqlDataReader dataReader;
            dataReader = cmd.ExecuteReader();
            if (dataReader.HasRows == true)
            {
                panel3.BringToFront();
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Email ve şifrenizi kontrol ediniz");
            }
            con.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(bunifuCircleProgress1.Value < 100)
            {
                bunifuCircleProgress1.Value += 2;
            }
            else
            {
                timer1.Stop();
                Form2 f2 = new Form2();
                f2.emailname = bunifuTextBox1.Text;
                this.Hide();
                f2.Show();
            }
        }
    }
}
