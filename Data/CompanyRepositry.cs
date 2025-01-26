using System.Security.Cryptography;
using FinalProjAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjAPI.Data;
public class CompanyRepositry : ICompanyInterFaceRepositry
{
    private DataContextEF _entityFrameWork;

    public CompanyRepositry(DbContextOptions<DataContextEF> options, IConfiguration configuration)
    {
        _entityFrameWork = new DataContextEF(options, configuration);
    }
    public bool SaveChanges()
    {
        return _entityFrameWork.SaveChanges() > 0;
    }
    public bool RemoveEntity<T>(T entityToRemove)
    {
        if (entityToRemove != null)
        {
            _entityFrameWork.Remove(entityToRemove);
            return true;
        }
        return false;
    }
    public IEnumerable<Company> GetCompanies()
    {
        IEnumerable<Company> companies = _entityFrameWork.Companies.ToList<Company>();
        return companies;
    }
    public Company GetCompany(int RegistrationID)
    {
        Company? company = _entityFrameWork.Companies.Where(u => u.RegistrationID == RegistrationID).FirstOrDefault<Company>();
        if (company != null)
        {
            return company;
        }
        else
        {
            throw new Exception("Failed to get company");
        }
    }
    public string GetNameCompany(int RegistrationID)
    {
        Company? company = _entityFrameWork.Companies.Where(u => u.RegistrationID == RegistrationID).FirstOrDefault<Company>();
        if (company != null)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return company.CompanyName;
#pragma warning restore CS8603 // Possible null reference return.
        }
        else
        {
            throw new Exception("Failed to get name");
        }
    }
    public string GetPhoneCompany(int RegistrationID)
    {
        Company? company = _entityFrameWork.Companies.Where(u => u.RegistrationID == RegistrationID).FirstOrDefault<Company>();
        if (company != null)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return company.ContactPhone;
#pragma warning restore CS8603 // Possible null reference return.
        }
        else
        {
            throw new Exception("Failed to get name");
        }
    }
    public int GetTypeID(string ContactEmail)
    {
        Company? company = _entityFrameWork.Companies.Where(u => u.ContactEmail == ContactEmail).FirstOrDefault<Company>();
        if (company != null)
        {
            return company.TypeID;
        }
        else
        {
            throw new Exception("Failed to get company");
        }
    }


}