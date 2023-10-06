namespace Base.API.Models
{
    public class OkResult
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
