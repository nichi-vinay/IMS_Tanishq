using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class CompanyDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CompanyDAL));
        public List<Company> GetAllCompany()
        {
            try
            {
                List<Company> _company = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _company = context.Companies
                    .ToList<Company>();
                }
                return _company;
            }
            catch (Exception ex)
            {
                Log.Error("CompanyDAL-GetAllCompany-Exception", ex);
                return null;
            }
        }
        public void AddCompany(Company company)
        {
            try
            {
                Company newCompany = new Company
                {

                    CompanyName = company.CompanyName,
                    Status = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    MISC1 = null,
                    MISC2 = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.Companies.Add(newCompany);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CompanyDAL-AddCompany-Exception", ex);
                throw;
            }
        }

        public Company GetCompanyById(int companyId)
        {
            try
            {
                Company company = new Company();
                using (JMSEntities content = new JMSEntities())
                {
                    company = content.Companies.FirstOrDefault((comp) => comp.Id == companyId);
                }

                return company;
            }
            catch (Exception ex)
            {
                Log.Error("CompanyDAL-GetCompanyById-Exception", ex);
                throw;
            }
        }

        public void UpdateCompany(Company newCompany)
        {
            try
            {
                Company company = new Company();
                using (JMSEntities content = new JMSEntities())
                {
                    company = content.Companies.FirstOrDefault((comp) => comp.Id == newCompany.Id);
                    company.CompanyName = newCompany.CompanyName;
                    company.Status = newCompany.Status;
                    company.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CompanyDAL-UpdateCompany-Exception", ex);
                throw;
            }
        }
        public void DeleteCompany(int id)
        {
            try
            {
                Company company = new Company();
                using (JMSEntities content = new JMSEntities())
                {
                    company = content.Companies.FirstOrDefault((comp) => comp.Id == id);
                    company.Status = false;
                    company.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CompanyDAL-DeleteCompany-Exception", ex);
                throw;
            }
        }
    }
}
