﻿namespace TestToken.DTO.UserDtos
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string OTP { get; set; }
    }
}
