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
    public partial class FormEditTest : Form
    {
        private Test currentTest;

        public FormEditTest(int testId)
        {
            InitializeComponent();
            LoadTest(testId);
        }

        private void LoadTest(int testId)
        {
            currentTest = DatabaseHelper.GetTestById(testId);

            if (currentTest == null)
            {
                MessageBox.Show("Тест не знайдено!");
                this.Close();
                return;
            }

            txtTitle.Text = currentTest.Title;
            txtDescription.Text = currentTest.Description;

            RefreshQuestionsList();
        }

        private void FormEditTest_Load(object sender, EventArgs e)
        {
            txtTitle.Text = currentTest.Title;
            txtDescription.Text = currentTest.Description;

            RefreshQuestionsList();
        }

        private void RefreshQuestionsList()
        {
            listQuestions.Items.Clear();
            for (int i = 0; i < currentTest.Questions.Count; i++)
            {
                var q = currentTest.Questions[i];
                listQuestions.Items.Add($"Питання №{i + 1}: {q.Text}");
            }
        }

        private void btnAddQuestion_Click(object sender, EventArgs e)
        {
            FormCreateQuestion form = new FormCreateQuestion();
            if (form.ShowDialog() == DialogResult.OK)
            {
                currentTest.Questions.Add(form.CreatedQuestion);
                RefreshQuestionsList();
            }
        }

        private void btnEditQuestion_Click(object sender, EventArgs e)
        {
            if (listQuestions.SelectedIndex == -1) return;

            int index = listQuestions.SelectedIndex;
            Question q = currentTest.Questions[index];

            FormCreateQuestion form = new FormCreateQuestion(q);
            if (form.ShowDialog() == DialogResult.OK)
            {
                currentTest.Questions[index] = form.CreatedQuestion;
                RefreshQuestionsList();
            }
        }

        private void btnDeleteQuestion_Click(object sender, EventArgs e)
        {
            if (listQuestions.SelectedIndex == -1) return;

            int index = listQuestions.SelectedIndex;
            Question q = currentTest.Questions[index];

            if (MessageBox.Show("Видалити обране питання?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (q.Id != 0) { DatabaseHelper.DeleteQuestion(q.Id); }

                currentTest.Questions.RemoveAt(index);
                RefreshQuestionsList();
            }
        }

        private void btnSaveTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введіть назву тесту!");
                return;
            }

            currentTest.Title = txtTitle.Text.Trim();
            currentTest.Description = txtDescription.Text.Trim();

            try
            {
                DatabaseHelper.UpdateTest(currentTest);
                MessageBox.Show("Тест успішно оновлено!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Помилка при збереженні тесту: " + ex.Message); }
        }
    }
}
