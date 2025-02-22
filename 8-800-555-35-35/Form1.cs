using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SqlClient;

namespace _8_800_555_35_35
{
    public partial class Vxod : MaterialForm
    {
        DataBase dataBase = new DataBase();
        public Vxod()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            var loguser = Login.Text;
            var passuser = Password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id_user, login_user, password_user, is_admin from Register where login_user = '{loguser}' and password_user = '{passuser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                var user = new CheckUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));

                System.Windows.MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Information);
                Form2 form2 = new Form2(user);
                this.Hide();
                form2.ShowDialog();
                this.Show();
            }
            else 
                System.Windows.MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Information);
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            Register form3 = new Register();
            form3.Show();
            this.Hide();
        }

        private void Vxod_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            Login.SelectionLength = 50;
            Password.SelectionLength = 100;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Password.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Password.UseSystemPasswordChar = false;
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }
    }
}
