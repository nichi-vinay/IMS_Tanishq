using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
 public class CustomerDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomerDAL));
        public List<Customer> GetAllCustomers()
        {
            try
            {
                List<Customer> _customer = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _customer = context.Customers.ToList<Customer>();

                }
                return _customer;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-GetAllCustomers", ex);
                return null;
            }
        }

        public int AddCustomer(Customer customer)
        {
            try
            {
                Customer newcustomer = new Customer
                {
                    CustomerName=customer.CustomerName,
                    CustomerPhone=customer.CustomerPhone,
                    Address=customer.Address,
                    //DLNumber=customer.DLNumber,
                    DLNumber= customer.DLNumber,
                    ExpDate =customer.ExpDate,
                    DOB=customer.DOB,
                    Email=customer.Email,
                    City=customer.City,
                    State=customer.State,
                    Zip=customer.Zip,

                    Status=customer.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.Customers.Add(newcustomer);
                    context.SaveChanges();
                    return newcustomer.Id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-AddCustomer", ex);
                return 0;
            }
        }

        public Customer GetCustomerById(int? id)
        {
            try
            {
                Customer customer = new Customer();
                using (JMSEntities context = new JMSEntities())
                {
                    customer = context.Customers.FirstOrDefault((customers) => customers.Id == id);
                }
                return customer;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-GetCustomerById", ex);
                return null;
            }
        }
        public void UpdateCustomer(Customer customer)
        {

            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Customers.FirstOrDefault((customers) => customers.Id == customer.Id);
                    selected.CustomerName = customer.CustomerName;
                    selected.CustomerPhone = customer.CustomerPhone;
                    selected.Address = customer.Address;
                    selected.DLNumber = customer.DLNumber;
                    selected.ExpDate = customer.ExpDate;
                    selected.DOB = customer.DOB;
                    selected.Email = customer.Email;
                    selected.City = customer.City;
                    selected.State = customer.State;
                    selected.Zip = customer.Zip;
                    selected.Status = customer.Status;
                    selected.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-UpdateCustomer", ex);
                new Exception(ex.Message);
            }
        }
        public void DeleteCustomer(int id)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Customers.FirstOrDefault((customers) => customers.Id == id);
                    selected.Status = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-DeleteCustomer", ex);
                throw;
            }
        }
        public Customer GetCustomersByPhoneNumber(string phoneNumber)
        {
            try
            {
                Customer customer = new Customer();
                using (JMSEntities context = new JMSEntities())
                {
                    customer = context.Customers.FirstOrDefault((customers) => customers.CustomerPhone == phoneNumber);
                }
                return customer;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-GetCustomersByPhoneNumber", ex);
                return new Customer();
            }
        }
        public Customer GetCustomersByDLNumber(string dlNumber)
        {
            try
            {
                Customer customer = new Customer();
                using (JMSEntities context = new JMSEntities())
                {
                    customer = context.Customers.FirstOrDefault((customers) => customers.DLNumber == dlNumber);
                }
                return customer;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-GetCustomersByDLNumber", ex);
                return new Customer();
            }
        }
        public List<Customer> GetCustomersByName(string customerName)
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                using (JMSEntities context = new JMSEntities())
                {
                    customers = context.Customers.Where((customer) => customer.CustomerName.ToLower().Contains(customerName.ToLower())).ToList();
                }
                return customers;
            }
            catch (Exception ex)
            {
                Log.Error("CustomerDAL-Exception-GetCustomersByDLNumber", ex);
                return new List<Customer>();
            }
        }
    }
}
