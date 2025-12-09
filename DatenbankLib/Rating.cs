using System.Data;

namespace DatenbankLib
{
    public class Rating
    {
        // Merkmale 
        private static string table_friendsOfAward_Ranking = "friendsOfAward_Ranking";

        // Properties
        private int id { get; }
        private string TopFavorit { get; }
        private string Favorit1 { get; }
        private string Favorit2 { get; }
        private string Favorit3 { get; }
        private string Favorit4 { get; }
        private string Favorit5 { get; }
        // Konstruktor
        public Rating(int id, string topFavorit, string favorit1, string favorit2, string favorit3, string favorit4, string favorit5)
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

            Rating rating = new Rating(0, "", "", "", "", "", "");
            List<Rating> ratings = new List<Rating>();
            eventTable = wrappr.RunQuery(sql);

            foreach (DataRow row in eventTable.Rows)
            {

                rating = new Rating((int)row[0], (string)row[1], (string)row[2], (string)row[3], (string)row[4], (string)row[5], (string)row[6]);
                ratings.Add(rating);
            }
            return ratings;
        }
        public static List<Rating> Evaluation()
        {
            List<Rating> ratings = GetAllRatings();
            foreach (Rating rating in ratings)
            {

                var punkte = new Dictionary<string, int>();

                for (int i = 0; i < ratings.Count; i++)
                {
                    punkte[ratings[i].ToString()] = (i == 0) ? 2 : 1;
                }


            }
            return ratings;
        }
    }
}

