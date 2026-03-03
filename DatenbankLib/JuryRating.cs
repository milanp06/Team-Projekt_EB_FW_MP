using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenbankLib
{
    public class JuryRating
    {
        // Merkmale 
        private static string table_friendsOfAward_JuryRating = "friendsOfAward_JuryRating";

        // Properties
        public string Token { get; }
        public double Projekt1 { get; }
        public double Projekt2 { get; }
        public double Projekt3 { get; }
        public double Projekt4 { get; }
        public double Projekt5 { get; }
        public double Projekt6 { get; }
        // Konstruktor
        public JuryRating(string token, double projekt1, double projekt2, double projekt3, double projekt4, double projekt5, double projekt6)
        {
            Token = token;
            Projekt1 = projekt1;
            Projekt2 = projekt2;
            Projekt3 = projekt3;
            Projekt4 = projekt4;
            Projekt5 = projekt5;
            Projekt6 = projekt6;
        }
        private static string SqlEscape(string? s) => (s ?? "").Replace("'", "''");

        public static void CreateJuryRankingTable()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            List<string> projects = Project.GetJuryProjects();

            if (projects == null || projects.Count == 0)
                throw new Exception("Keine Projekte gefunden.");

            string sql = "DROP TABLE IF EXISTS friendsofaward_juryrating;";
            sql += "CREATE TABLE friendsofaward_juryrating (";
            sql += "token VARCHAR(255) NOT NULL,";

            foreach (var project in projects)
            {
                string safeProject = project.Replace("`", "").Replace("'", "");
                sql += $"`{safeProject}` DOUBLE NOT NULL DEFAULT 0,";
            }

            sql += "PRIMARY KEY (token),";
            sql += "CONSTRAINT FK_jury_token ";
            sql += "FOREIGN KEY (token) ";
            sql += "REFERENCES friendsofaward_user (token)";
            sql += ") ENGINE=InnoDB;";

            try
            {
                wrappr.RunNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Fehler beim Erstellen der Tabelle: {ex.Message}");
            }
        }

        public static string AddJuryRanking(JuryRating rating)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            List<string> projects = Project.GetJuryProjects();
            string successMessage = $"Erfolgreich eingefügt: {rating}";
            string errorMessage = successMessage;
            int errorCount = 0;

            string sql = $@"
                INSERT INTO {table_friendsOfAward_JuryRating}
                (token, `{projects[0]}`, `{projects[1]}`, `{projects[2]}`,
                 `{projects[3]}`, `{projects[4]}`, `{projects[5]}`)
                SELECT
                u.token,
                {rating.Projekt1},
                {rating.Projekt2},
                {rating.Projekt3},
                {rating.Projekt4},
                {rating.Projekt5},
                {rating.Projekt6}
                FROM friendsofaward_user u
                WHERE u.token = '{SqlEscape(rating.Token)}'
                LIMIT 1;";

            try
            {
                int affected = wrappr.RunNonQuery(sql);
                if (affected == 0)
                {
                    // No row inserted — likely missing user token or duplicate PK in ranking table.
                    errorCount++;
                    errorMessage = "Ranking wurde nicht eingefügt: zugehöriger Benutzer nicht vorhanden oder bereits ein Ranking für diesen Token.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorCount++;
                errorMessage = $"Fehler beim Einfügen: {ex.Message}";
            }

            if (errorCount > 0)
            {
                return errorMessage;
            }

            return successMessage;
        }

        public static List<JuryRating> GetAllJuryRatings()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            string sql = $"SELECT * FROM {table_friendsOfAward_JuryRating};";
            DataTable eventTable = new DataTable();

            List<JuryRating> ratings = new List<JuryRating>();

            try
            {
                eventTable = wrappr.RunQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<JuryRating>();
            }

            foreach (DataRow row in eventTable.Rows)
            {
                JuryRating rating = new JuryRating(row[0].ToString(), Convert.ToInt32(row[1]), Convert.ToInt32(row[2]), Convert.ToInt32(row[3]), Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), Convert.ToInt32(row[6]));
                ratings.Add(rating);
            }
            return ratings;
        }
    }
}
