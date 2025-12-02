namespace DatenbankLib
{
    internal class Rating
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

    }
}
