using Student_testing_system.dbClass;
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
    public partial class FormTakeTest : Form
    {
        private Test currentTest;
        private User currentUser;

        private int currentIndex = 0;
        private List<int> selectedAnswers;

        public FormTakeTest(int testId, User user)
        {
            InitializeComponent();

            currentTest = DatabaseHelper.GetTestById(testId);
            currentUser = user;

            if (currentTest == null || currentTest.Questions.Count == 0)
            {
                MessageBox.Show("Помилка: тест не знайдено або він порожній!");
                this.Close();
                return;
            }

            selectedAnswers = new List<int>();
            foreach (var q in currentTest.Questions) selectedAnswers.Add(-1);

            this.Text = $"Проходження тесту: {currentTest.Title}";
        }

        private void FormTakeTest_Load(object sender, EventArgs e)
        {
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            var q = currentTest.Questions[currentIndex];

            lblQuestionNumber.Text = $"Питання {currentIndex + 1} із {currentTest.Questions.Count}";
            lblQuestionText.Text = q.Text;

            panelAnswers.Controls.Clear();

            int top = 5;

            foreach (var a in q.Answers)
            {
                RadioButton rb = new RadioButton();
                rb.Text = a.Text;
                rb.Tag = a.Id;
                rb.Left = 5;
                rb.Top = top;
                rb.Width = 500;
                rb.AutoSize = true;

                if (selectedAnswers[currentIndex] == a.Id) rb.Checked = true;

                panelAnswers.Controls.Add(rb);
                top += 30;
            }
            btnPrev.Enabled = currentIndex > 0;
            btnNext.Enabled = currentIndex < currentTest.Questions.Count - 1;

            btnFinish.Enabled = currentIndex == currentTest.Questions.Count - 1;
        }

        private void SaveCurrentAnswer()
        {
            int selectedId = -1;

            foreach (RadioButton rb in panelAnswers.Controls)
            {
                if (rb.Checked)
                {
                    selectedId = (int)rb.Tag;
                    break;
                }
            }

            selectedAnswers[currentIndex] = selectedId;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();

            if (selectedAnswers[currentIndex] == -1)
            {
                MessageBox.Show("Оберіть відповідь!");
                return;
            }

            currentIndex++;
            ShowQuestion();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();
            currentIndex--;
            ShowQuestion();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();

            if (selectedAnswers.Any(a => a == -1))
            {
                MessageBox.Show("Потрібно відповісти на всі питання!");
                return;
            }

            FinishTest();
        }

        private void FinishTest()
        {
            int correct = 0;

            for (int i = 0; i < currentTest.Questions.Count; i++)
            {
                var q = currentTest.Questions[i];

                var answer = q.Answers.FirstOrDefault(a => a.Id == selectedAnswers[i]);
                if (answer != null && answer.IsCorrect) correct++;
            }

            DatabaseHelper.SaveResult(
                currentUser.UserId,
                currentTest.Id,
                correct,
                currentTest.Questions.Count
            );

            MessageBox.Show(
                $"Результат: {correct} із {currentTest.Questions.Count}",
                "Тест завершено",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            this.Close();
        }
    }
}
