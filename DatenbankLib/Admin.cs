using System.Data;

namespace DatenbankLib
{
    public class Admin
    {
        private static string table_friendsOfAward_Admin = "friendsOfAward_Admin";

        // PROPERTIES
        public string Email { get; }
        public string Password { get; }

        public Admin(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public static void CreateAdmin(string email, string password)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string hash = PasswordHasher.HashPassword(password);

            string sql = $"INSERT INTO {table_friendsOfAward_Admin} (USER_EMAIL, PASSWORD_ADMIN) VALUES ('{email}', '{hash}');";

            try
            {
                wrappr.RunNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool CheckAdminLogin(string email, string password)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            DataTable dt = new();

            string sql = $"SELECT PASSWORD_ADMIN FROM {table_friendsOfAward_Admin} WHERE USER_EMAIL = '{email}' LIMIT 1;";

            try
            {
                dt = wrappr.RunQuery(sql);

                if (dt.Rows.Count == 0) return false;

                string hashFromDb = dt.Rows[0][0].ToString();

                return PasswordHasher.VerifyPassword(password, hashFromDb);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static string AddProjects(Project[] projects)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string errorMessage = $"Erfolgreich eingefügt: {projects.Count()}";
            string sql;
            int errorCount = 0;

            foreach (Project project in projects)
            {
                sql = $"INSERT INTO friendsOfAward_Projects(Title, Author) VALUES ('{project.Title}','{project.Author}');";
                try
                {
                    wrappr.RunNonQuery(sql);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    errorCount++;
                }
            }

            if (errorCount > 0)
            {
                errorMessage = $"Erfolgreich eingefügt: {projects.Count() - errorCount} / Fehler: {errorCount}";
            }

            return errorMessage;
        }

    }
}
