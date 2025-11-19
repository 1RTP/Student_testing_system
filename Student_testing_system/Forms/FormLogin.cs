using Student_testing_system.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_testing_system
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            DataService.Initialize();

            txtEmail.Text = "";
            txtPassword.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) { MessageBox.Show("Заповніть усі поля!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var users = DataService.LoadUsers();

            var foundUser = users.FirstOrDefault(u => string.Equals(u.Email?.Trim(), email, StringComparison.OrdinalIgnoreCase) && (u.PasswordHash == password) );
            if (foundUser == null) { MessageBox.Show("Невірна пошта або пароль.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            MessageBox.Show($"Вхід успішний. Роль: {foundUser.Role}", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (foundUser.Role == "teacher")
            {
                var td = new TeacherDashboard(foundUser);
                td.FormClosed += (s, args) => Application.Exit();
                td.Show();
                this.Hide();
            }
            else
            {
                var sd = new StudentDashboard(foundUser);
                sd.FormClosed += (s, args) => Application.Exit();
                sd.Show();
                this.Hide();
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
