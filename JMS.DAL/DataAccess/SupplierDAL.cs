using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class SupplierDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SupplierDAL));
        public List<Supplier> GetAllSuppliers()
        {
            try
            {
                List<Supplier> _supplier = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _supplier = context.Suppliers.ToList<Supplier>();

                }
                return _supplier;
            }
            catch (Exception ex)
            {
                Log.Error("SupplierDAL-GetAllSuppliers-Exception", ex);
                return null;
            }
        }
        public void AddSupplier(Supplier supplier)
        {
            try
            {
                Supplier newSupplier = new Supplier
                {

                    SupplierName = supplier.SupplierName,
                    SupplierCode = supplier.SupplierCode,
                    Address = supplier.Address,
                    Phone = supplier.Phone,
                    Status = supplier.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.Suppliers.Add(newSupplier);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("SupplierDAL-AddSupplier-Exception", ex);
                throw;
            }
        }
        public Supplier GetSupplierById(int? id)
        {
            try
            {
                Supplier supplier = new Supplier();
                using (JMSEntities context = new JMSEntities())
                {
                    supplier = context.Suppliers.FirstOrDefault((suppliers) => suppliers.Id == id);
                }
                return supplier;
            }
            catch (Exception ex)
            {
                Log.Error("SupplierDAL-GetSupplierById-Exception", ex);
                return null;
            }
        }

        public void UpdateSupplier(Supplier supplier)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Suppliers.FirstOrDefault((suppliers) => suppliers.Id == supplier.Id);
                    selected.SupplierName = supplier.SupplierName;
                    selected.SupplierCode = supplier.SupplierCode;
                    selected.Address = supplier.Address;
                    selected.Phone = supplier.Phone;
                    selected.Status = supplier.Status;
                    selected.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("SupplierDAL-UpdateSupplier-Exception", ex);
                new Exception(ex.Message);
            }
        }
        public void DeleteSupplier(int id)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Suppliers.FirstOrDefault((suppliers) => suppliers.Id == id);
                    selected.Status = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("SupplierDAL-DeleteSupplier-Exception", ex);
                throw;
            }
        }

    }
}
