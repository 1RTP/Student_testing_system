using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Student_testing_system
{
    public static class DataService
    {
        public static string ProjectRoot { get; }
        public static string DataFolder { get; }
        public static string UsersFile { get; }

        static DataService()
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            ProjectRoot = Path.GetFullPath(Path.Combine(exeDir, "..", ".."));
            DataFolder = Path.Combine(ProjectRoot, "Data");
            UsersFile = Path.Combine(DataFolder, "users.json");

            EnsureDataFolderExists();
            EnsureUsersFileExists();
        }

        public static void Initialize() { var _ = DataFolder; }

        private static void EnsureDataFolderExists()
        {
            if (!Directory.Exists(DataFolder)) Directory.CreateDirectory(DataFolder);
        }

        private static void EnsureUsersFileExists()
        {
            if (!File.Exists(UsersFile)) File.WriteAllText(UsersFile, "[]");
        }

        public static List<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(UsersFile)) return new List<User>();
                string json = File.ReadAllText(UsersFile);
                var list = JsonConvert.DeserializeObject<List<User>>(json);
                return list ?? new List<User>();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Помилка читання users.json: {ex.Message}", "Error");
                return new List<User>();
            }
        }

        public static void SaveUsers(List<User> users)
        {
            try
            {
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(UsersFile, json);
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"Помилка збереження users.json: {ex.Message}", "Error"); }
        }
    }
}
