using System.Collections.Generic;

namespace Standard.Customer.SOR.Models.Responses
{
    public class DataResponse<T>
    {
        public IEnumerable<T> Result { get; set; }
        public int Count { get; set; }
    }
}
