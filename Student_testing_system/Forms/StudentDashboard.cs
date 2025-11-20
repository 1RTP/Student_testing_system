using System;
using System.Windows.Forms;
using Student_testing_system.dbClass;

namespace Student_testing_system.Forms
{
    public partial class StudentDashboard : Form
    {
        private User currentUser;

        public StudentDashboard(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Text = $"Student - {user.FullName}";
        }

        private void StudentDashboard_Load(object sender, EventArgs e)
        {

        }

    }
}
