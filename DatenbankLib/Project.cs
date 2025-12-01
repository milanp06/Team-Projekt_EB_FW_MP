using System.Data;
using System.Diagnostics;

namespace DatenbankLib
{
    public class Project
    {
        // MERKMALE
        private static string table_friendsOfAward_Projects = "friendsOfAward_Projects";

        // PROPERTIES
        public string Id { get; }
        public string Title { get; }
        public string Author { get; }

        public Project(string id, string title, string author)
        {
            Id = id;
            Title = title;
            Author = author;
        }

		public static int DeleteAllProjects()
		{
			DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql = "DELETE FROM friendsOfAward_Projects;";
            int count = 0;

			try
			{
				count = wrappr.RunNonQuery(sql);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				count = -1;
			}

			return count;
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
