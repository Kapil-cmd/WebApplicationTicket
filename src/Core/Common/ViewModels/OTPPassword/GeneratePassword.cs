namespace Common.ViewModels.OTPPassword
{
    public string GeneratePassword()
    {
        string OTPLength = "4";
        string OTP = string.Empty;

        string Chars = string.Empty;
        Chars = "1,2,3,4,5,6,7,8,9,0";

        char[] splitChar = { ',' };
        string[] arr = Chars.Split(splitChar);
        string NewOTP = "";
        string temp = "";
        Random random = new Random();
        for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
        {
            temp = arr[random.Next(0, arr.Length)];
            NewOTP += temp;
            OTP = NewOTP;
        }
        return OTP;
    }
}
