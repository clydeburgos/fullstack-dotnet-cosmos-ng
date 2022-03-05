using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Standard.Customer.Application
{
    public interface IMigrationService
    {
        Task<bool> Migrate();
    }
}
