
using JMS.BAL.ViewModel;
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
    public class SupplierBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SupplierBAL));
        public SupplierViewModel GetAllSuppliers()
        {
            try
            {
                SupplierViewModel supplierViewModel = new SupplierViewModel();
                List<SupplierModel> supplierModels = new List<SupplierModel>();
                var supplierDALObj = new SupplierDAL();
                var lstRoles = supplierDALObj.GetAllSuppliers();
                foreach (var item in lstRoles)
                {
                    var obj = new SupplierModel()
                    {
                        Id = item.Id,
                        SupplierName=item.SupplierName,
                        SupplierCode = item.SupplierCode,
                        Address=item.Address,
                        Phone=item.Phone,                        
                        StatusName = item.Status == true ? "Active" : "In-Active",
                    };
                    supplierModels.Add(obj);
                }
                supplierViewModel.Items = supplierModels;
                return supplierViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("SupplierBAL-GetAllSuppliers-Exception", ex);
                return null;
            }
        }
        public void AddSupplier(SupplierModel supplierModel)
        {
            try
            {
                var supplierDALObj = new SupplierDAL();
                supplierDALObj.AddSupplier(new Supplier()
                {
                  SupplierName=supplierModel.SupplierName,
                  SupplierCode = supplierModel.SupplierCode,
                  Address=supplierModel.Address,
                  Phone=supplierModel.Phone,
                  Status = supplierModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("SupplierBAL-AddSupplier-Exception", ex);
                throw;
            }
        }
        public SupplierModel GetSupplierById(int? supplierId)
        {

            try
            {
                var supplierDALObj = new SupplierDAL();
                var supplierDetails = supplierDALObj.GetSupplierById(supplierId);
                if (supplierDetails != null)
                {
                    SupplierModel supplierModel = new SupplierModel()
                    {
                        Id = supplierDetails.Id,
                       SupplierName=supplierDetails.SupplierName,
                       SupplierCode = supplierDetails.SupplierCode,
                       Address=supplierDetails.Address,
                       Phone=supplierDetails.Phone,
                        Status = supplierDetails.Status
                    };

                    return supplierModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("SupplierBAL-GetSupplierById-Exception", ex);
                return null;
            }
        }
        public void UpdateSupplier(SupplierModel supplierModel)
        {
            try
            {
                var supplierDALObj = new SupplierDAL();
                supplierDALObj.UpdateSupplier(new Supplier()
                {
                    Id = supplierModel.Id,
                    SupplierName=supplierModel.SupplierName,
                    SupplierCode = supplierModel.SupplierCode,
                    Address=supplierModel.Address,
                    Phone=supplierModel.Phone,
                    Status = supplierModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("SupplierBAL-UpdateSupplier-Exception", ex);
                throw;
            }
        }
        public void DeleteSupplier(int id)
        {
            try
            {
                var supplierDALObj = new SupplierDAL();
                supplierDALObj.DeleteSupplier(id);
            }
            catch (Exception ex)
            {
                Log.Error("SupplierBAL-DeleteSupplier-Exception", ex);
                throw;
            }
        }
    }
}
