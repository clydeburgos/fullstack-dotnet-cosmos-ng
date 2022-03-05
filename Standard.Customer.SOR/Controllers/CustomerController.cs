using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Standard.Customer.Application;
using Standard.Customer.Domain;
using Standard.Customer.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standard.Customer.SOR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CustomerEntity> _dataRepository;
        private readonly IMigrationService _migrationService;
        public CustomerController(IMapper mapper, IRepository<CustomerEntity> dataRepository, IMigrationService migrationService)
        {
            _mapper = mapper;
            _dataRepository = dataRepository;
            _migrationService = migrationService;
        }

        [HttpGet("migrate")]
        public async Task<IActionResult> Migrate() {
            await _migrationService.Migrate();
            return Ok();
        }

        [HttpGet()]
        public async Task<IActionResult> GetMany(string searchKeyword, int? page, int pageSize = 20)
        {
            var data = await _dataRepository.GetMany(searchKeyword, page, pageSize);
            var customersResponse = _mapper.Map<List<CustomerDto>>(data);
            return Ok(customersResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _dataRepository.Get(id);
            var customerResponse = _mapper.Map<CustomerDto>(data);
            return Ok(customerResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await _dataRepository.Delete(id);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerDto customerDto)
        {
            var customerPayload = _mapper.Map<CustomerEntity>(customerDto);
            var data = await _dataRepository.Update(customerPayload);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto customerDto)
        {
            var customerPayload = _mapper.Map<CustomerEntity>(customerDto);
            var response = await _dataRepository.Add(customerPayload, CreateType.Create);

            return Ok(response);
        }
    }
}
