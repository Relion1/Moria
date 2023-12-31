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
using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
using System.Net;

namespace Moria
{
    public partial class Form2 : Form
    {
        public string emailname {get;set;} 
        public Form2()
        {
            InitializeComponent();
        }

        String randomCode;
        public static String to;
        private bool emailDogrulandimi = false;

        string constring = "Data Source=KAPOS\\SQLEXPRESS;Initial Catalog=moria_database;Integrated Security=True";
        private void Form2_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = (10 * 1000);
            timer.Tick += new EventHandler(timer3_Tick);
            timer.Start();
            LoadFormData();
            MessageChat();
            bunifuButton21.Enabled = false;
            label13.Text = bunifuTextBox5.Text;
        }

        private void LoadFormData()
        {

            label2.Text = emailname;
            byte[] getimage = new byte[0];
            SqlConnection con = new SqlConnection(constring);
            string q = "SELECT * FROM Login WHERE email='" + emailname + "'"; //label2.text sol bardaki email yazısı bu yazıya göre kullanıcı seçiliyor veritabanında
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

        private bool IsEmailExists(string email)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Login WHERE email = @email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void changeUserCredentials()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string updateChatQuery = $"UPDATE Chat SET userone = @newUsername WHERE userone = @oldUsername; " +
                                     $"UPDATE Chat SET usertwo = @newUsername WHERE usertwo = @oldUsername";
            SqlCommand updateChatCmd = new SqlCommand(updateChatQuery, con);
            updateChatCmd.Parameters.AddWithValue("@newUsername", bunifuTextBox5.Text);
            updateChatCmd.Parameters.AddWithValue("@oldUsername", label13.Text);
            updateChatCmd.ExecuteNonQuery();

            string updateLoginQuery = $"UPDATE login SET firstname=@fname, lastname=@lname, email=@email, image=@image WHERE email = '{emailname}'";
            emailname = bunifuTextBox7.Text;

            
            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                bunifuPictureBox5.Image.Save(ms, bunifuPictureBox1.Image.RawFormat);
                imageBytes = ms.ToArray();//resmi sorguya eklememiş ondan olmuyormuş sorguya eklemek içinde byte dönüştürmek gerekiyordu onu yaptım
                //register sayfasında var zaten aynı kod direk ona bakarak yaptım.
            }
            SqlCommand cmd = new SqlCommand(updateLoginQuery, con);
            cmd.Parameters.AddWithValue("@fname", bunifuTextBox5.Text);
            cmd.Parameters.AddWithValue("@lname", bunifuTextBox6.Text);
            cmd.Parameters.AddWithValue("@email", bunifuTextBox7.Text);
            cmd.Parameters.AddWithValue("@image", imageBytes);
            cmd.ExecuteNonQuery();

            con.Close();
            MessageBox.Show("Profile is updated, please restart the app for the change");
            label13.Text = bunifuTextBox5.Text; //yeni kullanıcı ismini ekrana yazdırsın diye 
            LoadFormData();
        }

        private void bunifuButton21_Click(object sender, EventArgs e) // SAVE BUTTON
        {
            if (!emailDogrulandimi)
            {
                MessageBox.Show("Önce e postanı doğrulaman gerekir");
                return;
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

            if(emailname != bunifuTextBox7.Text)
            {
                if (!IsEmailExists(bunifuTextBox7.Text))//eğer uygulama kapatıpı açılmaz ise login page den giriş yapılamıyor restart gerekiyor
                {
                    //eğer adam emaili dahil her şeyi değiştirmek istiyorsa bu kod çalışıyor eğer sadece isim değişecekse mailler de çakışma yapar ondan 
                    changeUserCredentials();
                }
                else
                {
                    MessageBox.Show("Bu email zaten kullanılıyor lütfen farklı bir mail deneyiniz");
                }
            }
            else
            {
                //IsEmailExists fonksiyonunu çalıştırmadan değiştirme yapıyoruz bu saye de çakışma olmuyor 
                //her iki ihtimaldede email değişiyor ama birinde aynı email üstüne yazılıyor ve zaten o mail doğrulanmış oldundan sıkıntı çıkmıcak
                changeUserCredentials();
            }

            bunifuButton21.Enabled = false;
            emailDogrulandimi = false;
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
            if(bunifuPictureBox3.Visible == true)
            {
                bunifuPictureBox3.Visible = false;
            }
            
            panel2.BringToFront();
            if (panel2.Visible == false)
            {
                panel2.Visible = true;
            }
            if (panel4.Visible==true)
            { 
                panel4.Visible = false;
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

        private void updatepassword_Click(object sender, EventArgs e)
        {

            if (bunifuPictureBox3.Visible == true)
            {
                bunifuPictureBox3.Visible = false;
            }


            panel4.BringToFront();
            if (panel4.Visible == false)
            {
                panel4.Visible = true;
            }
            if (panel2.Visible==true)
            {
                panel2.Visible = false;
            }
        }

        private void bunifuPictureBox7_Click(object sender, EventArgs e)
        {
            if (panel4.Visible == true)
            {
                panel4.Visible = false;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuButton23_Click(object sender, EventArgs e) // SAVE2 BUTTON (şifredeğiştirmeiçin)
        {

            if (string.IsNullOrEmpty(bunifuTextBox8.Text.Trim())) //boş geçemessin uyarısı şunun için bunifutextbox8=oldpassword
            {
                errorProvider1.SetError(bunifuTextBox8, "Bos gecemessin");
                return;
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox8, string.Empty);
            }

            if (string.IsNullOrEmpty(bunifuTextBox10.Text.Trim())) //boş geçemessin uyarısı şunun için bunifutextbox10=confirmpassword
            {
                errorProvider1.SetError(bunifuTextBox10, "Bos gecemessin");
                return;
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox10, string.Empty);
            }


            if (bunifuTextBox9.Text == bunifuTextBox10.Text && bunifuTextBox8.Text == bunifuTextBox4.Text)
            {
                validatepassword();
            }
            else if (bunifuTextBox8.Text != bunifuTextBox4.Text)
            {
                MessageBox.Show("Eski sifre yanlis!");
                return;
            }
            else if (bunifuTextBox9.Text != bunifuTextBox10.Text)
            {
                MessageBox.Show("Sifre ve onay sifresi ayni olmak zorunda!");
                return;
            }
            

        }


        public void validatepassword() // sifre değiştirme kodu
        {
            var input = bunifuTextBox9.Text;

            if (string.IsNullOrWhiteSpace(input)) //boş geçemessin uyarısı şunun için bunifutextbox9=confirmpassword
            {
                errorProvider1.SetError(bunifuTextBox9, "Sifre bos olmamalidir");
                return;
            }

            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]}; :<>|./?,-]");
            var hasNumber = new Regex(@"[0-9]+");
            var hasMiniMaxChars = new Regex(@".{8,8}");


            if (!hasLowerChar.IsMatch(input))
            {
                MessageBox.Show("Sifre En az bir kucuk harf icermelidir");
                return;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                MessageBox.Show("Sifre En az bir buyuk harf icermelidir");
                return;
            }
            else if (!hasSymbols.IsMatch(input))
            {
                MessageBox.Show("Sifre En az bir ozel karakter icermelidir");
                return;
            }
            else if (!hasNumber.IsMatch(input))
            {
                MessageBox.Show("Sifre En az bir numara icermelidir");
                return;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                MessageBox.Show("Sifre 8 karakterden az olmamalidir");
                return;
            }
            else
            {
                SqlConnection con = new SqlConnection(constring);
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Login SET password ='" + bunifuTextBox9.Text + "',confirmpassword = '" + bunifuTextBox10.Text + "'where email = '" + label2.Text + "'and password = '" + bunifuTextBox8.Text + "'", con);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Sifreniz Degistirildi");
                bunifuTextBox4.Text = bunifuTextBox9.Text;
                bunifuTextBox9.Text = string.Empty;
            }
        }

        private void passChangeShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if(passChangeShowPass.Checked)
            {
                bunifuTextBox8.PasswordChar = '\0';
                bunifuTextBox9.PasswordChar = '\0';
                bunifuTextBox10.PasswordChar = '\0';
            }
            else
            {
                bunifuTextBox8.PasswordChar = '*';
                bunifuTextBox9.PasswordChar = '*';
                bunifuTextBox10.PasswordChar = '*';
            }
        }

        private void bunifuButton5_Click(object sender, EventArgs e) // CHAT BUTTON 
        {

            if (bunifuPictureBox3.Visible == true)
            {
                bunifuPictureBox3.Visible = false;
            }


            UserItem();
            panel5.BringToFront();
            if (panel2.Visible == true || panel4.Visible==true)
            {
                panel2.Visible = false;
                panel4.Visible = false;
            }

                panel5.Visible = true;
            
        }

        private void bunifuButton2_Click(object sender, EventArgs e) // HOME BUTTON
        {


            if (bunifuPictureBox3.Visible == false)
            {
                bunifuPictureBox3.Visible = true;
            }


            if (panel2.Visible == true || panel4.Visible == true || panel5.Visible == true)
            {
                panel2.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
            }
        }


        private void UserItem()
        {
            flowLayoutPanel1.Controls.Clear();
            SqlDataAdapter adapter;
            adapter = new SqlDataAdapter("select * from Login", constring);
            DataTable table = new DataTable();
            adapter.Fill(table);

            if(table !=null)
            {
                if(table.Rows.Count > 0)
                {
                    UserControl1[] userControls = new UserControl1[table.Rows.Count];

                    for(int i = 0; i < 1; i++)
                    {
                        foreach(DataRow row in table.Rows)
                        {

                        userControls[i] = new UserControl1();
                        MemoryStream stream = new MemoryStream((byte[])row["image"]);
                        userControls[i].Icon = new Bitmap(stream);
                        userControls[i].Title = row["firstname"].ToString();

                        if (userControls[i].Title == bunifuTextBox1.Text)
                            {
                                flowLayoutPanel1.Controls.Remove(userControls[i]);
                            }
                        else
                            {
                                flowLayoutPanel1.Controls.Add(userControls[i]);
                            }
                            userControls[i].Click += new System.EventHandler(this.userControl11_Load);
                        }
                    }
                }
            }
        }

        private void bunifuButton7_Click(object sender, EventArgs e) // mesaj gönderme buttonu
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string q = "insert into Chat(userone,usertwo,message)values(@userone,@usertwo,@message)";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.Parameters.AddWithValue("@userone",bunifuTextBox1.Text);
            cmd.Parameters.AddWithValue("@usertwo",bunifuLabel1.Text);
            cmd.Parameters.AddWithValue("@message",bunifuTextBox11.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageChat();
            bunifuTextBox11.Clear();
        }

        private void MessageChat()
        {
            SqlDataAdapter adapter;
            adapter = new SqlDataAdapter("SELECT * FROM Chat WHERE (userone = @user1 AND usertwo = @user2) OR (userone = @user2 AND usertwo = @user1)", constring);
            adapter.SelectCommand.Parameters.AddWithValue("@user1", bunifuLabel1.Text);
            adapter.SelectCommand.Parameters.AddWithValue("@user2", bunifuTextBox1.Text);
            DataTable table = new DataTable();
            adapter.Fill(table);
            flowLayoutPanel2.Controls.Clear();

            if (table != null && table.Rows.Count > 0)
            {
                UserControl2[] userControls2s = new UserControl2[table.Rows.Count];
                UserControl3[] userControls3s = new UserControl3[table.Rows.Count];
                int userControl2Index = 0;
                int userControl3Index = 0;

                foreach (DataRow row in table.Rows)
                {
                    if (bunifuTextBox1.Text == row["userone"].ToString() && bunifuLabel1.Text == row["usertwo"].ToString())
                    {
                        userControls2s[userControl2Index] = new UserControl2();
                        //userControls2s[userControl2Index].Dock = DockStyle.Top; tasarımda var
                        //userControls2s[userControl2Index].BringToFront();       tasarımda var
                        userControls2s[userControl2Index].Title = row["message"].ToString();

                        flowLayoutPanel2.Controls.Add(userControls2s[userControl2Index]);
                        flowLayoutPanel2.ScrollControlIntoView(userControls2s[userControl2Index]);

                        userControl2Index++;
                    }
                    else if (bunifuLabel1.Text == row["userone"].ToString() && bunifuTextBox1.Text == row["usertwo"].ToString())
                    {
                        userControls3s[userControl3Index] = new UserControl3();
                        //userControls3s[userControl3Index].Dock = DockStyle.Top; tasarımda var
                        //userControls3s[userControl3Index].BringToFront();       tasarımda var
                        userControls3s[userControl3Index].Title = row["message"].ToString();
                        userControls3s[userControl3Index].Icon = bunifuPictureBox8.Image;

                        flowLayoutPanel2.Controls.Add(userControls3s[userControl3Index]);
                        flowLayoutPanel2.ScrollControlIntoView(userControls3s[userControl3Index]);

                        userControl3Index++;
                    }
                }
            }
        }


        private void userControl11_Load(object sender, EventArgs e)
        {
            if(panel6.Visible == false && panel7.Visible == false && flowLayoutPanel2.Visible == false)
            {
                panel6.Visible = true;
                panel7.Visible = true;
                flowLayoutPanel2.Visible = true;
            }
            UserControl1 control = (UserControl1)sender;
            bunifuLabel1.Text = control.Title;
            bunifuPictureBox8.Image = control.Icon;
            MessageChat();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            MessageChat();
        }

        private void bunifuPictureBox10_Click(object sender, EventArgs e)
        {
            if (panel6.Visible == true && panel7.Visible == true && flowLayoutPanel2.Visible == true)
            {
                panel6.Visible = false;
                panel7.Visible = false;
                flowLayoutPanel2.Visible = false;
            }
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            if(label2.Text != bunifuTextBox7.Text)
            {
                if (IsEmailExists(bunifuTextBox7.Text))
                {
                    MessageBox.Show("Bu mail zaten kullanılıyor lütfen başka bir mail giriniz");
                    return;
                }//eğer mail farklı ise kod aşağı doğru çalışmaya devam eder yani adam maili değişmicekse herşey olduğu gibi çalışmaya devam eder
            }
            String from, pass, messageBody;
            Random rand = new Random();
            randomCode = (rand.Next(999999)).ToString();
            MailMessage message = new MailMessage();
            to = (bunifuTextBox7.Text).ToString();
            from = "nova.turhan@yandex.com";
            pass = "kslhgcuifgohlzgp";
            messageBody = "Doğrulama kodunuz: " + randomCode;
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Password Reseting Code";
            SmtpClient smtp = new SmtpClient("smtp.yandex.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);

            try
            {
                smtp.Send(message);
                MessageBox.Show("Code Send Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {
            if (randomCode == (bunifuTextBox12.Text).ToString())
            {
                to = bunifuTextBox12.Text;
                emailDogrulandimi = true;
                bunifuButton21.Enabled = true;
                MessageBox.Show("Code İs Correct");
            }
            else
            {
                MessageBox.Show("Wrong Code");
            }
        }
    }
}