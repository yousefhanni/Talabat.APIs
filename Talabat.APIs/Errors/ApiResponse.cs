namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public ApiResponse(int statusCode ,string? message=null)
        {
            StatusCode = statusCode;

            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            //Switch Expression, feature appeared at C# 8 
            //switch on statusCode, if 400 return Bad Request ,else if.... 

            return statusCode switch
            {
                400 => "A Bad Request,you have made",
                401 => "You are not Authorized",
                404=>"Resource was not found",
                500=>"Errors are the path the dark side. Errors lead to anger. Anger leads to hate.Hate leads to career change",
                _ => null,
            };
        }
    }
}
