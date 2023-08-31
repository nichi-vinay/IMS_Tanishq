using JMS.BAL.ViewModel;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.BussinesLogic
{
    public class CompanyBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CompanyBAL));
        public CompanyViewModel GetAllCompany()
        {
            try
            {
                CompanyViewModel companyViewModels = new CompanyViewModel();
                var companyDALObj = new CompanyDAL();
                List<CompanyModel> companyModels = new List<CompanyModel>();
                var lstOfCompany = companyDALObj.GetAllCompany();
                foreach (var item in lstOfCompany)
                {
                    var obj = new CompanyModel() {
                        Id = item.Id,
                        CompanyName = item.CompanyName,
                        Status = item.Status,
                        StatusName = item.Status == true ? "Active" : "In-Active"
                    };
                    companyModels.Add(obj);
                }
                companyViewModels.Items = companyModels;
                return companyViewModels;
            }
            catch (Exception ex)
            {
                Log.Error("CompanyBAL-GetAllCompany-Exception", ex);
                return new CompanyViewModel();
            }
        }

        public void UpdateCompany(CompanyModel newCompany)
        {
            try
            {
                var companyDALObj = new CompanyDAL();
                companyDALObj.UpdateCompany(new Company() { 
                    Id=newCompany.Id,
                    CompanyName=newCompany.CompanyName,
                    Status=newCompany.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("CompanyBAL-UpdateCompany-Exception", ex);
                throw;
            }
        }
        public void AddCompany(CompanyModel newCompany)
        {
            try
            {
                var companyDALObj = new CompanyDAL();
                companyDALObj.AddCompany(new Company() { 
                    CompanyName=newCompany.CompanyName,
                    Status=newCompany.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("CompanyBAL-AddCompany-Exception", ex);
                throw;
            }
        }
        public void DeleteCompany(int companyId)
        {
            try
            {
                var companyDALObj = new CompanyDAL();
                companyDALObj.DeleteCompany(companyId);
            }
            catch (Exception ex)
            {
                Log.Error("CompanyBAL-DeleteCompany-Exception", ex);
                throw;
            }
        }
        public CompanyModel GetCompanyById(int companyId)
        {
            try
            {
                var companyDALObj = new CompanyDAL();
                var dalCompany = companyDALObj.GetCompanyById(companyId);
                if (dalCompany!=null)
                {
                    return new CompanyModel()
                    {
                        Id = dalCompany.Id,
                        CompanyName = dalCompany.CompanyName,
                        Status = dalCompany.Status
                    }; 
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("CompanyBAL-GetCompanyById-Exception", ex);
                return null;
            }
        }
    }
}
