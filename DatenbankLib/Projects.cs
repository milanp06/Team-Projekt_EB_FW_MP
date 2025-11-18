using System.Data;

namespace DatenbankLib
{
    internal class Projects
    {
        // MERKMALE
        private static string table_friendsOfAward_Projects = "friendsOfAward_Projects";

        // PROPERTIES
        public string Id { get; }
        public string Title { get; }
        public string Author { get; }

        public Projects(string id, string title, string author)
        {
            Id = id;
            Title = title;
            Author = author;
        }

        public static string ReloadProjects(Projects[] projects)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string errorMessage = "Erfolgreich aktualisiert";

            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);   // ein bisschen Debugging schadet nicht.
                errorMessage = "UPDATE - EXCEPTION";
            }

            return errorMessage;
        }
    }
}
