using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;

namespace JMS.BAL.BussinesLogic
{
    public class HomeBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeBAL));
        public IDictionary<string, string> GetAllSalesData()
        {
            try
            {
                HomeDAL obj = new HomeDAL();
                return obj.GetAllSalesData();
            }
            catch (Exception ex)
            {
                Log.Error("HomeBAL-GetAllSalesData-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        public IDictionary<string, string> GetAllSalesDataByYear()
        {
            try
            {
                HomeDAL obj = new HomeDAL();
                return obj.GetAllSalesDataByYear();
            }
            catch (Exception ex)
            {
                Log.Error("HomeBAL-GetAllSalesDataByYear-Exception", ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
