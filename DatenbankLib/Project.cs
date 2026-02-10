using System.Data;
using System.Diagnostics;
using System.Security.Policy;

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
        public int Schulvoting { get; }
        public int Publikumsvoting { get; }
        public int Juryvoting { get; }

        public Project(string id, string title, string author, int schulvoting, int publikumsvoting, int juryvoting)
        {
            Id = id;
            Title = title;
            Author = author;
            Schulvoting = schulvoting;
            Publikumsvoting = publikumsvoting;
            Juryvoting = juryvoting;

        }

        public static int DeleteAllProjects()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql = $"DELETE FROM {table_friendsOfAward_Projects};";
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
                sql = $"INSERT INTO {table_friendsOfAward_Projects}(Title, Author) VALUES ('{project.Title}','{project.Author}');";
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

        public static int GetProjectCount()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            DataTable dt = new();
            string sql = $"SELECT Count(*) FROM {table_friendsOfAward_Projects};";
            int count = 0;

            try
            {
                dt = wrappr.RunQuery(sql);

                count = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                count = -1;
            }

            return count;
        }

        public static string CalculatePublikumsvoting()
        {
            string errorMessage = "";

            List<Rating> ratings = DatenbankLib.Rating.GetAllRatings();

            int count = GetProjectCount();
            if (count == -1) return "Keine Projekte gefunden!";

            int[] votePoints = new int[count];

            foreach (Rating rating in ratings)
            {
                votePoints[(rating.TopFavorit) - 1] += 2;
                votePoints[(rating.Favorit1) - 1] += 1;
                votePoints[(rating.Favorit2) - 1] += 1;
                votePoints[(rating.Favorit3) - 1] += 1;
                votePoints[(rating.Favorit4) - 1] += 1;
                votePoints[(rating.Favorit5) - 1] += 1;
            }

            Console.WriteLine("\nVotes:");
            for (int i = 1; i < count; i++)
            {
                Console.WriteLine($"{i + 1}: {votePoints[i]} Votes");
            }

            int maxPoints = votePoints[0];

            for (int i = 0; i < count; i++)
            {
                if (votePoints[i] > maxPoints) maxPoints = votePoints[i];
            }

            Console.WriteLine($"\nMaximalpunkte: {maxPoints} Punkte");

            double[] totalPoints = new double[count];
            Console.WriteLine("\nPunkte:");

            for (int i = 0; i < count; i++)
            {
                totalPoints[i] = (votePoints[i] * 30.0) / maxPoints;
                Console.WriteLine($"{i + 1}: {totalPoints[i]} Punkte");
            }

            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql;
            int errorCount = 0;

            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine(i);
                sql = $"UPDATE {table_friendsOfAward_Projects} SET Publikumsvoting = '{totalPoints[i - 1]}' WHERE id = '{i}'";
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
                errorMessage = $"Erfolgreich eingefügt: {count - errorCount} / Fehler: {errorCount}";
            }

            return errorMessage;
        }

        public static string CalculateJuryvoting(List<string> projectNames)
        {
            string errorMessage = "";

            List<JuryRating> ratings = DatenbankLib.JuryRating.GetAllJuryRatings();

            int count = projectNames.Count();
            if (count <= 0) return "Keine Projekte gefunden!";

            int[] votePoints = new int[count];

            foreach (JuryRating rating in ratings)
            {
                votePoints[(rating.Projekt1) - 1] += 2;
                votePoints[(rating.Projekt2) - 1] += 1;
                votePoints[(rating.Projekt3) - 1] += 1;
                votePoints[(rating.Projekt4) - 1] += 1;
                votePoints[(rating.Projekt5) - 1] += 1;
                votePoints[(rating.Projekt6) - 1] += 1;
            }

            Console.WriteLine("\nVotes:");
            for (int i = 1; i < count; i++)
            {
                Console.WriteLine($"{i + 1}: {votePoints[i]} Votes");
            }

            int maxPoints = votePoints[0];

            for (int i = 0; i < count; i++)
            {
                if (votePoints[i] > maxPoints) maxPoints = votePoints[i];
            }

            Console.WriteLine($"\nMaximalpunkte: {maxPoints} Punkte");

            double[] totalPoints = new double[count];
            Console.WriteLine("\nPunkte:");

            for (int i = 0; i < count; i++)
            {
                totalPoints[i] = (votePoints[i] * 30.0) / maxPoints;
                Console.WriteLine($"{i + 1}: {totalPoints[i]} Punkte");
            }

            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql;
            int errorCount = 0;

            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine(i);
                sql = $"UPDATE {table_friendsOfAward_Projects} SET Juryvoting = '{totalPoints[i - 1]}' WHERE Title = '{projectNames[i]}'";
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
                errorMessage = $"Erfolgreich eingefügt: {count - errorCount} / Fehler: {errorCount}";
            }

            return errorMessage;
        }
    }
}
