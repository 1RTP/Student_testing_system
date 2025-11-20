using Student_testing_system.dbClass;

namespace Student_testing_system
{
    public static class Session
    {
        public static User CurrentUser { get; set; } = null;
    }
}