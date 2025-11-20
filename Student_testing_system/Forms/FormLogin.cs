using Student_testing_system.dbClass;
using Student_testing_system.Forms;
using System;
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
            DatabaseHelper.InitializeDatabase();

            txtEmail.Text = "";
            txtPassword.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заповніть усі поля!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            User user = DatabaseHelper.LoginUser(email, password);

            if (user != null)
            {
                Session.CurrentUser = user;
                Form nextForm;

                if (user.IsTeacher)
                {
                    nextForm = new TeacherDashboard(user);
                }
                else
                {
                    nextForm = new StudentDashboard(user);
                }

                this.Hide();
                nextForm.Show();
                nextForm.FormClosed += (s, args) => Application.Exit();
            }
            else
            {
                MessageBox.Show("Невірний email або пароль!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
    }
}