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
    public partial class FormCreateQuestion : Form
    {
        public Question CreatedQuestion { get; private set; }
        private List<Answer> answers = new List<Answer>();

        public FormCreateQuestion()
        {
            InitializeComponent();
        }

        public FormCreateQuestion(Question question) : this()
        {
            CreatedQuestion = question;
            answers = question.Answers.Select(a => new Answer
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList();
        }

        private void FormCreateQuestion_Load(object sender, EventArgs e)
        {
            if (CreatedQuestion != null)
            {
                txtQuestion.Text = CreatedQuestion.Text;
                RefreshAnswersList();
            }
            else { txtQuestion.Text = ""; }
        }

        private void btnAddAnswer_Click(object sender, EventArgs e)
        {
            string text = txtAnswer.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Введіть текст відповіді!");
                return;
            }

            if (answers.Any(a => a.Text.Equals(text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Така відповідь уже існує.");
                return;
            }

            Answer newAnswer = new Answer
            {
                Id = answers.Count + 1,
                Text = text,
                IsCorrect = false
            };

            answers.Add(newAnswer);
            RefreshAnswersList();
            txtAnswer.Text = "";
        }

        private void btnSetCorrect_Click(object sender, EventArgs e)
        {
            if (listAnswers.SelectedIndex == -1)
            {
                MessageBox.Show("Виберіть відповідь!");
                return;
            }

            foreach (var a in answers) a.IsCorrect = false;
            answers[listAnswers.SelectedIndex].IsCorrect = true;

            RefreshAnswersList();
        }

        private void RefreshAnswersList()
        {
            listAnswers.Items.Clear();
            foreach (var a in answers)
            {
                string label = a.IsCorrect ? $"{a.Text}   (Правильна)" : a.Text;
                listAnswers.Items.Add(label);
            }
        }

        private void btnSaveQuestion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                MessageBox.Show("Введіть текст питання!");
                return;
            }

            if (answers.Count < 2)
            {
                MessageBox.Show("Додайте мінімум дві відповіді!");
                return;
            }

            if (!answers.Any(a => a.IsCorrect))
            {
                MessageBox.Show("Позначте правильну відповідь!");
                return;
            }

            CreatedQuestion = new Question
            {
                Id = CreatedQuestion?.Id ?? 0,
                Text = txtQuestion.Text.Trim(),
                Answers = new List<Answer>(answers)
            };

            DialogResult = DialogResult.OK;
            Close();
        }

    }


}