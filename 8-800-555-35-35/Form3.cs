using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SqlClient;
using System.Windows;

namespace _8_800_555_35_35
{
    public partial class Register : MaterialForm
    {
        DataBase dataBase = new DataBase();
        public Register()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Vxod form1 = new Vxod();
            this.Hide();
            form1.ShowDialog();
        }

        private void Reg_btn_Click(object sender, EventArgs e)
        {
            var login = Login.Text;
            var pass = Password.Text;

            if (checkuser())
            {
                return;
            }

            string querystring = $"insert into Register(login_user, password_user, is_admin) values ('{login}', '{pass}', 0)";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());

            dataBase.OpenConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                System.Windows.MessageBox.Show("Аккаунт создан!", "Успешно!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Information);
                Vxod vxod = new Vxod();
                this.Hide();
                vxod.ShowDialog();
            }
            else System.Windows.MessageBox.Show("Аккаунт не создан!");

            dataBase.CloseConnection();
        }

        private Boolean checkuser()
        {
            var login = Login.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id_user, login_user, is_admin from Register where login_user = '{login}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                System.Windows.MessageBox.Show("Пользователь уже существует!");
                return true;
            }
            else { return false; }
        }

        private void Register_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
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
