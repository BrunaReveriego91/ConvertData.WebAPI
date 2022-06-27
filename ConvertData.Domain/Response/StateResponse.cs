using System.Net;

namespace ConvertData.Domain.Response
{
    public class StateResponse
    {
        public int Status { get; set; }
        public object Response { get; set; }
        public bool IsSuccessStatusCode
        {
            get { return ((int)Status >= 200) && ((int)Status <= 299); }
        }


        public StateResponse(HttpStatusCode _status, Object Respose)
        {
            Status = (int)_status;
            Response = Respose;
        }
    }
}
