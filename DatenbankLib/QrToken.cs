using System.Data;

namespace DatenbankLib
{
    public class QrToken
    {
        // MERKMALE
        private static string table_friendsOfAward_User = "friendsOfAward_User";

        // PROPERTIES
        public string Token { get; set; } = string.Empty;
        public bool Used { get; set; } = false;
        public string TokenType { get; set; } = string.Empty;

        public QrToken(string token, bool used, string tokenType)
        {
            Token = token;
            Used = used;
            TokenType = tokenType;
        }

        public static bool AddQrToken(QrToken token)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql = $"INSERT INTO {table_friendsOfAward_User}(token, TokenUsed, tokentype) VALUES ('{token.Token}', {(token.Used ? 1 : 0)}, '{token.TokenType}');";

            try
            {
                wrappr.RunNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public static bool CheckTokenExits(string token)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            DataTable dt = new();

            string sql = $"SELECT token FROM friendsOfAward_User WHERE token = '{token}' LIMIT 1;";

            try
            {
                dt = wrappr.RunQuery(sql);

                if (dt.Rows.Count == 0) return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public static bool CheckTokenUsed(string token)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            DataTable dt = new();

            string sql = $"SELECT TokenUsed FROM friendsOfAward_User WHERE token = '{token}' LIMIT 1;";
            
            try
            {
                dt = wrappr.RunQuery(sql);
                return (bool)dt.Rows[0][0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static string GetTokenType(string token)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;
            DataTable dt = new();

            string sql = $"SELECT tokentype FROM friendsOfAward_User WHERE token = '{token}' LIMIT 1;";

            try
            {
                dt = wrappr.RunQuery(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public static bool UpdateToken(string token)
        {
            DbWrapperMySql wrappr = DbWrapperMySql.Wrapper;

            string sql = $"UPDATE friendsOfAward_User SET TokenUsed = 1 WHERE token = '{token}'";

            try
            {
                wrappr.RunNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
