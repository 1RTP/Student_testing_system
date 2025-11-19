using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_testing_system.Forms
{
    public partial class TeacherDashboard : Form
    {
        private User currentUser;

        public TeacherDashboard(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Text = $"Teacher - {user.Fullname}";
        }

        private void TeacherDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
