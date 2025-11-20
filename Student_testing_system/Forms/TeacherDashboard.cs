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

        }
    }
}
