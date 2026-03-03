using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenbankLib
{
    internal class JuryRating
    {
        // Merkmale 
        private static string table_friendsOfAward_JuryRating = "friendsOfAward_JuryRating";

        // Properties
        public string Token { get; }
        public int Projekt1 { get; }
        public int Projekt2 { get; }
        public int Projekt3 { get; }
        public int Projekt4 { get; }
        public int Projekt5 { get; }
        public int Projekt6 { get; }
        // Konstruktor
        public JuryRating(string token, int projekt1, int projekt2, int projekt3, int projekt4, int projekt5, int projekt6)
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
        public static string AddJuryRanking(JuryRating rating)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            string successMessage = $"Erfolgreich eingefügt: {rating}";
            string errorMessage = successMessage;
            int errorCount = 0;

            string sql = $"INSERT INTO {table_friendsOfAward_JuryRating} " +
             "(Token, Projekt1, Projekt2, Projekt3, Projekt4, Projekt5, Projekt6) " +
             $"SELECT u.token, {rating.Projekt1}, {rating.Projekt2}, {rating.Projekt3}, {rating.Projekt4}, {rating.Projekt5}, {rating.Projekt6} " +
             $"FROM friendsofaward_user u WHERE u.token = '{SqlEscape(rating.Token)}' LIMIT 1;";

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
