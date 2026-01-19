using System.Data;

namespace DatenbankLib
{
    public class Rating
    {
        // Merkmale 
        private static string table_friendsOfAward_Ranking = "friendsofaward_ranking";

        // Properties
        private string Token { get; }
        private string TopFavorit { get; }
        private string Favorit1 { get; }
        private string Favorit2 { get; }
        private string Favorit3 { get; }
        private string Favorit4 { get; }
        private string Favorit5 { get; }

        // Konstruktor
        public Rating(string token, string topFavorit, string favorit1, string favorit2, string favorit3, string favorit4, string favorit5)
        {
            Token = token;
            TopFavorit = topFavorit;
            Favorit1 = favorit1;
            Favorit2 = favorit2;
            Favorit3 = favorit3;
            Favorit4 = favorit4;
            Favorit5 = favorit5;
        }

        public override string? ToString()
        {
            return TopFavorit + " " + Favorit1 + " " + Favorit2 + " " + Favorit3 + " " + Favorit4 + " " + Favorit5;
        }

        private static string SqlEscape(string? s) => (s ?? "").Replace("'", "''");

        public static int DeleteAllRatings()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql = $"DELETE FROM {table_friendsOfAward_Ranking};";
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

        public static string AddRanking(Rating rating)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            string successMessage = $"Erfolgreich eingefügt: {rating}";
            string errorMessage = successMessage;
            int errorCount = 0;

            string sql = $"INSERT INTO {table_friendsOfAward_Ranking} (Token, TopFavorit, Favorit1, Favorit2, Favorit3, Favorit4, Favorit5) " +
                         $"SELECT u.token, '{SqlEscape(rating.TopFavorit)}', '{SqlEscape(rating.Favorit1)}', '{SqlEscape(rating.Favorit2)}', '{SqlEscape(rating.Favorit3)}', '{SqlEscape(rating.Favorit4)}', '{SqlEscape(rating.Favorit5)}' " +
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

        public static List<Rating> GetAllRatings()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            string sql = $"SELECT Token, TopFavorit, Favorit1, Favorit2, Favorit3, Favorit4, Favorit5 FROM {table_friendsOfAward_Ranking};";
            DataTable eventTable = new DataTable();

            List<Rating> ratings = new List<Rating>();
            eventTable = wrappr.RunQuery(sql);

            foreach (DataRow row in eventTable.Rows)
            {
                // map columns explicitly to constructor order
                var token = row[0]?.ToString() ?? "";
                var top = row[1]?.ToString() ?? "";
                var f1 = row[2]?.ToString() ?? "";
                var f2 = row[3]?.ToString() ?? "";
                var f3 = row[4]?.ToString() ?? "";
                var f4 = row[5]?.ToString() ?? "";
                var f5 = row[6]?.ToString() ?? "";

                var rating = new Rating(token, top, f1, f2, f3, f4, f5);
                ratings.Add(rating);
            }
            return ratings;
        }
    }
}
