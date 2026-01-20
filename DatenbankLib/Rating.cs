using System.Data;

namespace DatenbankLib
{
    public class Rating
    {
        // Merkmale 
        private static string table_friendsOfAward_Ranking = "friendsOfAward_Ranking";

        // Properties
        private int id { get; }
        public int TopFavorit { get; }
		public int Favorit1 { get; }
		public int Favorit2 { get; }
		public int Favorit3 { get; }
		public int Favorit4 { get; }
		public int Favorit5 { get; }
        // Konstruktor
        public Rating(int id, int topFavorit, int favorit1, int favorit2, int favorit3, int favorit4, int favorit5)
        {
            this.id = id;
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

        public static int DeleteAllRatings()
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql = "DELETE FROM friendsOfAward_ranking;";
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
            string errorMessage = $"Erfolgreich eingefügt: {rating}";
            string sql;
            int errorCount = 0;
            sql = $"INSERT INTO friendsOfAward_Ranking(TopFavorit, Favorit1, Favorit2, Favorit3, Favorit4, Favorit5) VALUES ('{rating.TopFavorit}','{rating.Favorit1}','{rating.Favorit2}','{rating.Favorit3}','{rating.Favorit4}','{rating.Favorit5}');";
            try
            {
                wrappr.RunNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorCount++;
            }
            if (errorCount > 0)
            {
                errorMessage = $"Fehler beim Einfügen von {errorCount} Einträgen.";
            }
            return errorMessage;
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
                Rating rating = new Rating(Convert.ToInt32(row[0]), Convert.ToInt32(row[1]), Convert.ToInt32(row[2]), Convert.ToInt32(row[3]), Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), Convert.ToInt32(row[6]));
                ratings.Add(rating);
            }
            return ratings;
        }
    }
}
