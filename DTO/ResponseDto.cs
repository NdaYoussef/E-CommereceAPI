namespace TestToken.DTO
{
    public class ResponseDto
    {
        public bool IsSucceeded {  get; set; }
        public bool IsConfirmed { get; set; }
        public string  Message { get; set; }

        public object? model {  get; set; }
        public int StatusCode { get; set; }

    }
}
