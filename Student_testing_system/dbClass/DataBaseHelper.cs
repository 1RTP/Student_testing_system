using Student_testing_system.dbClass;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public class DatabaseHelper
{
    private const string DbName = "database.db";
    private const string ConnectionString = "Data Source=" + DbName + ";Version=3;";

    public static void InitializeDatabase()
    {
        if (!File.Exists(DbName))
        {
            SQLiteConnection.CreateFile(DbName);
        }

        using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            string sql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    fullname TEXT,
                    email TEXT UNIQUE,
                    password TEXT,
                    isTeacher INTEGER
                );
                
                CREATE TABLE IF NOT EXISTS Tests (
                    test_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT,
                    description TEXT
                );

                CREATE TABLE IF NOT EXISTS Results (
                    result_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER,
                    test_id INTEGER,
                    score INTEGER,
                    max_score INTEGER,
                    FOREIGN KEY(user_id) REFERENCES Users(user_id),
                    FOREIGN KEY(test_id) REFERENCES Tests(test_id)
                );
                
                CREATE TABLE IF NOT EXISTS Questions (
                    question_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    test_id INTEGER,
                    question_text TEXT,
                    FOREIGN KEY(test_id) REFERENCES Tests(test_id)
                );
                
                CREATE TABLE IF NOT EXISTS Answers (
                    answer_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    question_id INTEGER,
                    answer_text TEXT,
                    is_correct INTEGER,
                    FOREIGN KEY(question_id) REFERENCES Questions(question_id)
                );
            ";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void RegisterUser(User user)
    {
        using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            string sql = "INSERT INTO Users (fullname, email, password, isTeacher) VALUES (@name, @email, @pass, @teacher)";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@name", user.FullName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@pass", PasswordHelper.HashPassword(user.Password));
                cmd.Parameters.AddWithValue("@teacher", user.IsTeacher ? 1 : 0);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public static User LoginUser(string email, string password)
    {
        using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            string sql = "SELECT * FROM Users WHERE email = @email AND password = @pass";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", PasswordHelper.HashPassword(password));

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            FullName = reader["fullname"].ToString(),
                            Email = reader["email"].ToString(),
                            IsTeacher = Convert.ToInt32(reader["isTeacher"]) == 1
                        };
                    }
                }
            }
        }
        return null;
    }

    public static List<Test> GetAllTests()
    {
        List<Test> tests = new List<Test>();

        using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            string sql = "SELECT * FROM Tests";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tests.Add(new Test
                        {
                            Id = Convert.ToInt32(reader["test_id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString()
                        });
                    }
                }
            }
        }
        return tests;
    }

    public static void SaveTest(Test test)
    {
        using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();

            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    string sqlTest = "INSERT INTO Tests (title, description) VALUES (@title, @desc); SELECT last_insert_rowid();";
                    long testId;

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlTest, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", test.Title);
                        cmd.Parameters.AddWithValue("@desc", test.Description);
                        testId = (long)cmd.ExecuteScalar();
                    }

                    foreach (var question in test.Questions) //look all questions
                    {
                        string sqlQuest = "INSERT INTO Questions (test_id, question_text) VALUES (@tid, @qtext); SELECT last_insert_rowid();";
                        long questionId;

                        using (SQLiteCommand cmd = new SQLiteCommand(sqlQuest, conn))
                        {
                            cmd.Parameters.AddWithValue("@tid", testId);
                            cmd.Parameters.AddWithValue("@qtext", question.Text);
                            questionId = (long)cmd.ExecuteScalar();
                        }

                        foreach (var answer in question.Answers) // look all answers
                        {
                            string sqlAnsw = "INSERT INTO Answers (question_id, answer_text, is_correct) VALUES (@qid, @atext, @correct)";
                            using (SQLiteCommand cmd = new SQLiteCommand(sqlAnsw, conn))
                            {
                                cmd.Parameters.AddWithValue("@qid", questionId);
                                cmd.Parameters.AddWithValue("@atext", answer.Text);
                                cmd.Parameters.AddWithValue("@correct", answer.IsCorrect ? 1 : 0);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit(); //when we don`t have errors, we save all changes
                }
                catch (Exception)
                {

                    transaction.Rollback(); //when we have error, we cancel all changes
                    throw;
                }
            }
        }
    }
}