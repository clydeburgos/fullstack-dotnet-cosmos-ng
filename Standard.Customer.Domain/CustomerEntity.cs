namespace Standard.Customer.Domain
{
    public class CustomerEntity
    {
        public string id { get; set; }
        public string uniqueid { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }   
        public string gender { get; set; }
        public bool status { get; set; }
    }
}
