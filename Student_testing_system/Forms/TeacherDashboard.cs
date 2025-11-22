using System;
using System.Windows.Forms;
using Student_testing_system.dbClass;

namespace Student_testing_system.Forms
{
    public partial class TeacherDashboard : Form
    {
        private User currentUser;

        public TeacherDashboard(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Text = $"Teacher - {user.FullName}";
        }

        private void TeacherDashboard_Load(object sender, EventArgs e)
        {
            LoadTests();
        }

        private void LoadTests()
        {
            dgvTests.Rows.Clear();
            dgvTests.Columns.Clear();

            dgvTests.Columns.Add("Id", "№");
            dgvTests.Columns.Add("Title", "Назва тесту");
            dgvTests.Columns.Add("Desc", "Опис");

            dgvTests.Columns["Id"].Visible = false;

            var tests = DatabaseHelper.GetAllTests();

            foreach (var t in tests)
            {
                dgvTests.Rows.Add(t.Id, t.Title, t.Description);
            }
        }

        private void btnAddTest_Click(object sender, EventArgs e)
        {
            FormCreateTest f = new FormCreateTest();
            f.ShowDialog();
            LoadTests();
        }

        private void btnEditTest_Click(object sender, EventArgs e)
        {
            if (dgvTests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть тест для редагування.");
                return;
            }

            int id = Convert.ToInt32(dgvTests.SelectedRows[0].Cells["Id"].Value);
            FormEditTest f = new FormEditTest(id);
            f.ShowDialog();
            LoadTests();
        }

        private void btnDeleteTest_Click(object sender, EventArgs e)
        {
            if (dgvTests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть тест для видалення.");
                return;
            }

            int id = Convert.ToInt32(dgvTests.SelectedRows[0].Cells["Id"].Value);

            if (MessageBox.Show("Видалити тест?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseHelper.DeleteTest(id);
                LoadTests();
            }
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
