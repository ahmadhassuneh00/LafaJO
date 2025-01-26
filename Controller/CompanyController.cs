using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjAPI.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly DataContextEF _entityFramWork;
        private readonly ICompanyInterFaceRepositry _CompanyRepositry;
        private readonly DataContextDapper _dapper;
        private readonly IMapper _mapper;

        public CompanyController(IConfiguration confg, ICompanyInterFaceRepositry CompanyRepositry, DbContextOptions<DataContextEF> options)
        {
            _dapper = new DataContextDapper(confg);
            _entityFramWork = new DataContextEF(options, confg);
            _CompanyRepositry = CompanyRepositry;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsersDto, Users>();
            }));
        }

        [HttpGet("GetTypeID/{ContactEmail}")]
        public ActionResult<int> GetTypeID(string ContactEmail)
        {
            var typeId = _CompanyRepositry.GetTypeID(ContactEmail);
            return Ok(typeId);
        }

        [HttpGet("GetCompanies")]
        public ActionResult<IEnumerable<Company>> GetCompanies()
        {
            var companies = _CompanyRepositry.GetCompanies();
            return Ok(companies);
        }

        [HttpGet("GetCompany/{RegistrationID}")]
        public ActionResult<Company> GetCompany(int RegistrationID)
        {
            var company = _CompanyRepositry.GetCompany(RegistrationID);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpGet("GetNameCompany/{RegistrationID}")]
        public ActionResult<string> GetNameCompany(int RegistrationID)
        {
            var name = _CompanyRepositry.GetNameCompany(RegistrationID);
            return Ok(name);
        }
        [HttpGet("GetPhoneCompany/{RegistrationID}")]
        public ActionResult<string> GetPhoneCompany(int RegistrationID)
        {
            var name = _CompanyRepositry.GetPhoneCompany(RegistrationID);
            return Ok(name);
        }

        [HttpPut("EditCompany")]
        public IActionResult EditCompany([FromBody] Company company)
        {
            if (company == null || company.RegistrationID <= 0)
            {
                return BadRequest("Invalid company data.");
            }

            var companyDb = _CompanyRepositry.GetCompany(company.RegistrationID);
            if (companyDb == null)
            {
                return NotFound("Company not found.");
            }

            // Update properties
            companyDb.CompanyName = company.CompanyName;
            companyDb.Email = company.Email;
            companyDb.ContactEmail = company.ContactEmail;

            if (_CompanyRepositry.SaveChanges())
            {
                return NoContent(); // 204 No Content response
            }
            return StatusCode(500, "Failed to edit company.");
        }

        [HttpDelete("RemoveCompany/{RegistrationID}")]
        public IActionResult RemoveCompany(int RegistrationID)
        {
            var companyDb = _entityFramWork.Companies.FirstOrDefault(c => c.RegistrationID == RegistrationID);
            if (companyDb == null)
            {
                return NotFound("Company not found.");
            }

            _CompanyRepositry.RemoveEntity(companyDb);

            if (_CompanyRepositry.SaveChanges())
            {
                return NoContent(); // 204 No Content response
            }
            return StatusCode(500, "Failed to delete company.");
        }
    }
}
