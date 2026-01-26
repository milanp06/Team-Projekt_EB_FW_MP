using System.Data;

namespace DatenbankLib
{
    public class Rating
    {
        // Merkmale 
        private static string table_friendsOfAward_Ranking = "friendsOfAward_Ranking";

        // Properties
        public string Token { get; }
        public int TopFavorit { get; }
	    public int Favorit1 { get; }
	    public int Favorit2 { get; }
	    public int Favorit3 { get; }
	    public int Favorit4 { get; }
	    public int Favorit5 { get; }
        // Konstruktor
        public Rating(string token, int topFavorit, int favorit1, int favorit2, int favorit3, int favorit4, int favorit5)
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
            string sql = $"INSERT INTO {table_friendsOfAward_Ranking} " +
             "(Token, TopFavorit, Favorit1, Favorit2, Favorit3, Favorit4, Favorit5) " +
             $"SELECT u.token, {rating.TopFavorit}, {rating.Favorit1}, {rating.Favorit2}, {rating.Favorit3}, {rating.Favorit4}, {rating.Favorit5} " +
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
            string sql = "SELECT * FROM friendsOfAward_Ranking;";
            DataTable eventTable = new DataTable();

            List<Rating> ratings = new List<Rating>();

            try
            {
				eventTable = wrappr.RunQuery(sql);
			}
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                return new List<Rating>();
			}

            foreach (DataRow row in eventTable.Rows)
            {
                Rating rating = new Rating(row[0].ToString(), Convert.ToInt32(row[1]), Convert.ToInt32(row[2]), Convert.ToInt32(row[3]), Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), Convert.ToInt32(row[6]));
                ratings.Add(rating);
            }
            return ratings;
        }
    }
    public class Results
    {
        public string Arbeit { get; set; }
        public double Schulvoting { get; set; }
        public double Publikumsvoting { get; set; }
        public double Juryvoting { get; set; }

        public Results(string arbeit, double schulvoting, double publikumsvoting, double juryvoting)
        {
            this.Arbeit = arbeit;
            this.Schulvoting = schulvoting;
            this.Publikumsvoting = publikumsvoting;
            this.Juryvoting = juryvoting;
        }

        public static List<Results> GetResults()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            string sql = "SELECT Title, Schulvoting, Publikumsvoting, Juryvoting FROM friendsOfAward_projects;";
            DataTable eventTable = new DataTable();
            List<Results> res = new();

            try
            {
                eventTable = wrappr.RunQuery(sql);
                foreach (DataRow row in eventTable.Rows)
                {
                    res.Add(new(row[0].ToString(), double.Parse(row[1].ToString()), double.Parse(row[2].ToString()), double.Parse(row[3].ToString())));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return res;
        }
    }
}
