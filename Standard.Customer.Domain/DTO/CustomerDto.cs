using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Customer.Domain.DTO
{
    public class CustomerDto {
        public int UniqueId { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }
    }
    public class CreateCustomerDto : CustomerDto
    {
        public CreateCustomerDto()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }

    public class UpdateCustomerDto : CustomerDto
    {
    }
}
