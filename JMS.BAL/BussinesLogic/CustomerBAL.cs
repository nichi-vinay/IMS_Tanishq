using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.BussinesLogic
{
    public class CustomerBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomerBAL));
        public CustomerViewModel GetAllCustomers()
        {
            try
            {
                CustomerViewModel customerViewModel = new CustomerViewModel();
                List<CustomerModel> customerModels = new List<CustomerModel>();
                var customerDALObj = new CustomerDAL();
                var lstCustomers = customerDALObj.GetAllCustomers();
                foreach (var item in lstCustomers)
                {
                    var obj = new CustomerModel()
                    {
                        Id = item.Id,
                        CustomerName=item.CustomerName,
                        CustomerPhone=item.CustomerPhone,
                        Address=item.Address,
                        DLNumber=item.DLNumber,
                        ExpDate= item.ExpDate,
                        DOB = item.DOB,
                        Email=item.Email,
                        City=item.City,
                        State=item.State,
                        Zip=item.Zip,
                        StatusName = item.Status == true ? "Active" : "In-Active"
                    };
                    customerModels.Add(obj);
                }
                customerViewModel.Items = customerModels;
                return customerViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-GetAllCustomers-Exception",ex);
                return null;
            }
        }
        public int AddCustomer(CustomerModel customerModel)
        {
            try
            {
                var customerDALObjObj = new CustomerDAL();
                return customerDALObjObj.AddCustomer(new Customer()
                {
                    CustomerName = customerModel.CustomerName,
                    CustomerPhone = customerModel.CustomerPhone,
                    Address = Helper.encrypt(customerModel.Address),
                    DLNumber = Helper.encrypt(customerModel.DLNumber),
                    ExpDate = Helper.encrypt(customerModel.ExpDate),
                    DOB = Helper.encrypt(customerModel.DOB),
                    Email = customerModel.Email,
                    City=customerModel.City,
                    State=customerModel.State,
                    Zip=customerModel.Zip,
                    Status = customerModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-AddCustomer-Exception", ex);
                return 0;
            }
        }
        public CustomerModel GetCustomerById(int? customerId)
        {

            try
            {
                var customerDALObj = new CustomerDAL();
                var customerDetails = customerDALObj.GetCustomerById(customerId);
                if (customerDetails != null)
                {
                    CustomerModel customerModel = new CustomerModel()
                    {
                        Id = customerDetails.Id,
                        CustomerName = customerDetails.CustomerName,
                        CustomerPhone = customerDetails.CustomerPhone,
                        Address = customerDetails.Address,
                        DLNumber = customerDetails.DLNumber,
                        ExpDate = customerDetails.ExpDate,
                        DOB =customerDetails.DOB, 
                        Email = customerDetails.Email,
                        City= customerDetails.City,
                        State=customerDetails.State,
                        Zip=customerDetails.Zip,
                        Status = customerDetails.Status
                    };

                    return customerModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-GetCustomerById-Exception", ex);
                return null;
            }
        }

        public void UpdateCustomer(CustomerModel customerModel)
        {
            try
            {
                var customerDALObj = new CustomerDAL();
                customerDALObj.UpdateCustomer(new Customer()
                {
                    Id = customerModel.Id,
                    CustomerName = customerModel.CustomerName,
                    CustomerPhone = customerModel.CustomerPhone,
                    Address = Helper.encrypt(customerModel.Address),
                    DLNumber =Helper.encrypt(customerModel.DLNumber),
                    ExpDate = Helper.encrypt(customerModel.ExpDate),
                    DOB = Helper.encrypt(customerModel.DOB),
                    Email = customerModel.Email,
                    City=customerModel.City,
                    State=customerModel.State,
                    Zip=customerModel.Zip,
                    Status = customerModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-UpdateCustomer-Exception", ex);
                throw;
            }
        }
        public void DeleteCustomer(int id)
        {
            try
            {
                var customerDALObj = new CustomerDAL();
                customerDALObj.DeleteCustomer(id);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-DeleteCustomer-Exception", ex);
                throw;
            }
        }

        public CustomerModel GetCustomersByPhoneNumber(string phoneNumber)
        {
            try
            {
                var customer = new CustomerDAL().GetCustomersByPhoneNumber(phoneNumber);
                if (customer != null)
                {
                    CustomerModel customerModel = new CustomerModel()
                    {
                        Id = customer.Id,
                        CustomerName = customer.CustomerName,
                        CustomerPhone = customer.CustomerPhone,
                        Address = customer.Address,
                        DLNumber = customer.DLNumber,
                        ExpDate = customer.ExpDate,
                        DOB = customer.DOB,
                        Email = customer.Email,
                        State = customer.State,
                        City = customer.City,
                        Zip = customer.Zip,
                        Status = customer.Status
                    };

                    return customerModel;
                }
                else
                {
                    return new CustomerModel();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-GetCustomersByPhoneNumber-Exception", ex);
                return new CustomerModel();
            }
        }
        public CustomerModel GetCustomersByDLNumber(string dlNumber)
        {
            try
            {
                var customer = new CustomerDAL().GetCustomersByDLNumber(dlNumber);
                if (customer != null)
                {
                    CustomerModel customerModel = new CustomerModel()
                    {
                        Id = customer.Id,
                        CustomerName = customer.CustomerName,
                        CustomerPhone = customer.CustomerPhone,
                        Address = customer.Address,
                        DLNumber = customer.DLNumber,
                        ExpDate = customer.ExpDate,
                        DOB = customer.DOB,
                        Email = customer.Email,
                        State=customer.State,
                        City=customer.City,
                        Zip=customer.Zip,
                        Status = customer.Status
                    };

                    return customerModel;
                }
                else
                {
                    return new CustomerModel();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-GetCustomersByDLNumber-Exception", ex);
                return new CustomerModel();
            }
        }
        public List<CustomerModel> GetCustomersByName(string customerName)
        {
            try
            {
                var customers = new CustomerDAL().GetCustomersByName(customerName);
                var filteredCustomers = new List<CustomerModel>();
                foreach (var item in customers)
                {
                    var obj = new CustomerModel()
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerPhone = item.CustomerPhone,
                        Address = item.Address,
                        DLNumber = item.DLNumber,
                        ExpDate = item.ExpDate,
                        DOB = item.DOB,
                        Email = item.Email,
                        City = item.City,
                        State = item.State,
                        Zip = item.Zip,
                        StatusName = item.Status == true ? "Active" : "In-Active"
                    };
                    filteredCustomers.Add(obj);
                }
                return filteredCustomers;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerBAL-GetCustomersByName-Exception", ex);
                return new List<CustomerModel>();
            }
        }
        //public int GetCustomerIdByCustomerPhone(string phone)
        //{
        //    CustomerDAL customerDALObj = new CustomerDAL();
        //  var customerDetails= customerDALObj.GetAllCustomers();
        //  var customerId=  customerDetails.FirstOrDefault(x => x.CustomerPhone == phone).Id;
        //    return customerId;
        //}
    }
}
