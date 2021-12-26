namespace AngularAspCore.Extensions
{
    public class JsonResponse
    {
        public Boolean success { get; set; }
        public string? message { get; set; }
        public JsonResponse()
        { 

        }

        public JsonResponse(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }
    }
}
