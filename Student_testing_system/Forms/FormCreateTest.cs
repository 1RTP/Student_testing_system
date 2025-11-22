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
    public partial class FormCreateTest : Form
    {
        private List<Question> questions = new List<Question>();

        public FormCreateTest()
        {
            InitializeComponent();
        }

        private void FormCreateTest_Load(object sender, EventArgs e)
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
        }

        private void btnAddQuestion_Click(object sender, EventArgs e)
        {
            FormCreateQuestion form = new FormCreateQuestion();
            if (form.ShowDialog() == DialogResult.OK)
            {
                questions.Add(form.CreatedQuestion);
                listQuestions.Items.Add($"Питання №{questions.Count}: {form.CreatedQuestion.Text}");
            }
        }

        private void btnSaveTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введіть назву тесту!");
                return;
            }

            if (questions.Count == 0)
            {
                MessageBox.Show("Додайте хоча б одне питання!");
                return;
            }

            Test newTest = new Test
            {
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Questions = questions
            };

            try
            {
                DatabaseHelper.SaveTest(newTest); 
                MessageBox.Show("Тест успішно створено!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженні тесту: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }  
}
