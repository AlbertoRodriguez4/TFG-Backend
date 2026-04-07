namespace AA2_CS.Model
{
    public class EmailVerification
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string code { get; set; } = string.Empty;
        public DateTime createdat { get; set; } = DateTime.UtcNow;
        public DateTime expiresat { get; set; }
        public bool isused { get; set; } = false;

        public EmailVerification() { }

        public EmailVerification(int userid, string code, int expirationMinutes = 15)
        {
            this.userid = userid;
            this.code = code;
            this.createdat = DateTime.UtcNow;
            this.expiresat = DateTime.UtcNow.AddMinutes(expirationMinutes);
        }
    }
}
