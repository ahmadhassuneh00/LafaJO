using FinalProjAPI.Models;

namespace FinalProjAPI.Data;
public interface ICompanyInterFaceRepositry
{
    public bool SaveChanges();
    public bool RemoveEntity<T>(T entityToAdd);
    public IEnumerable<Company> GetCompanies();
    public Company GetCompany(int RegistrationID);
    public string GetNameCompany(int RegistrationID);
    public string GetPhoneCompany(int RegistrationID);
    public int GetTypeID(string ContactEmail);
}
