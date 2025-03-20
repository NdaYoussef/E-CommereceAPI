namespace TestToken.Helpers
{
    public static class GenerateOTP
    {
         static public string GenerateeOTP()
        {
            var num = new Random();
            return num.Next(100000,999999).ToString();
        }
    }
}
