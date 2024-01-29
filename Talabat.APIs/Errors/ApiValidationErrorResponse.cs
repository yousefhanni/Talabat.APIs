namespace Talabat.APIs.Errors
{
    //this Class To create Object that will appear To developer   
    public class ApiValidationErrorResponse:ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }


        public ApiValidationErrorResponse():base(400)
        {
            Errors =new List<string>();
        }
    }
}
