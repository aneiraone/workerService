using System;

namespace Common
{
    public class Token
    {
        public Token()
        {
        }

        public Token(string tokenValue, double expiredTime = 30)
        {
            token = tokenValue;
            expired = DateTime.Now.AddMinutes(expiredTime);
        }
        public string token { get; set; }
        public DateTime expired { get; set; }

        public bool ValidateToken(Token token)
        {
            if (token.token == null)
                return false;

            if (DateTime.Now.Subtract(token.expired).TotalSeconds > 0)
                return false;

            return true;
        }
    }
}