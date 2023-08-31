using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace JMS.DAL.DataAccess
{
    public class HomeDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeDAL));
        public IDictionary<string, string> GetAllSalesData()
        {
            try
            {
                JewelTypeDAL jewelTypeDALObj = new JewelTypeDAL();
                var lstJewelTypes = jewelTypeDALObj.GetAllJewelTypes();

                IDictionary<string, string> saleValues = new Dictionary<string, string>();
                string connStr = ConfigurationManager.ConnectionStrings["JMSEntities123"].ToString();
                
                SqlConnection cn = new SqlConnection(connStr);
                cn.Open();
                string qry = "select JewelTypeId,count(*) CountValue from Inventory where Id in (select InventoryId from InvoiceItems where InvoiceId in (select Id from Invoice where (LayAwayInvoiceId='' OR LayAwayInvoiceId is Null) and CreatedAt >= '" + FirstDayOfYear(DateTime.Today) + "')) group by JewelTypeId";
                SqlCommand cmd = new SqlCommand(qry, cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var jewelTypeId = Convert.ToInt32(dr["JewelTypeId"].ToString());
                    var jewelTypeName = lstJewelTypes.Where(x => x.Id == jewelTypeId).FirstOrDefault().JewelTypeName;
                    saleValues.Add(jewelTypeName, dr["CountValue"].ToString());
                }
                
                return saleValues;
            }           
            catch (Exception ex)
            {
                Log.Error("HomeDAL-GetAllSalesData-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        public IDictionary<string, string> GetAllSalesDataByYear()
        {
            try
            {
               
                IDictionary<string, string> saleValues = new Dictionary<string, string>();
                string connStr = ConfigurationManager.ConnectionStrings["JMSEntities123"].ToString();

                SqlConnection cn = new SqlConnection(connStr);
                cn.Open();
                string qry = "select count(*) as InvoiceCount, datepart(yyyy, [CreatedAt]) as [YearValue] from Invoice where (LayAwayInvoiceId='' OR LayAwayInvoiceId is NULL ) group by datepart(yyyy, [CreatedAt]) order by [YearValue]";
                SqlCommand cmd = new SqlCommand(qry, cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    saleValues.Add(dr["YearValue"].ToString(), dr["InvoiceCount"].ToString());
                }

                return saleValues;
            }
            catch (Exception ex)
            {
                Log.Error("HomeDAL-GetAllSalesDataByYear-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        public DateTime FirstDayOfYear(DateTime y)
        {
            return new DateTime(y.Year, 1, 1);
        }
    }
}
