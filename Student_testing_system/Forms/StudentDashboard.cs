using Student_testing_system.dbClass;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Student_testing_system.Forms
{
    public partial class StudentDashboard : Form
    {
        private User currentUser;
        private List<Test> testsList = new List<Test>();

        public StudentDashboard(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Text = $"Student - {user.FullName}";
        }

        private void StudentDashboard_Load(object sender, EventArgs e)
        {
            LoadTests();
        }

        private void LoadTests()
        {
            testsList = DatabaseHelper.GetAllTests();

            dgvTests.Rows.Clear();
            dgvTests.Columns.Clear();

            dgvTests.Columns.Add("Id", "№");
            dgvTests.Columns.Add("Title", "Назва тесту");
            dgvTests.Columns.Add("Description", "Опис");
            dgvTests.Columns.Add("Questions", "Кількість питань");

            dgvTests.Columns["Id"].Width = 30;
            dgvTests.Columns["Questions"].Width = 120;

            foreach (var t in testsList)
            {
                int questionCount = DatabaseHelper.GetQuestionCount(t.Id);
                dgvTests.Rows.Add(t.Id, t.Title, t.Description, questionCount);
            }
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            if (dgvTests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть тест!");
                return;
            }

            int testId = Convert.ToInt32(dgvTests.SelectedRows[0].Cells[0].Value);

            Test selectedTest = testsList.Find(t => t.Id == testId);
            if (selectedTest == null)
            {
                MessageBox.Show("Тест не знайдено!");
                return;
            }

            FormTakeTest form = new FormTakeTest(selectedTest.Id, currentUser);
            form.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTests();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Session.CurrentUser = null;
            Application.Restart();
        }
    }
}
