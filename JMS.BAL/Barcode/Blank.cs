using System;
using System.Collections.Generic;
using System.Text;

namespace Jewel.Libraries.Barcode
{
    /// <summary>
    ///  Blank encoding template
    /// </summary>
    class Blank: BarcodeCommon, IBarcode
    {
        
        #region IBarcode Members

        public string Encoded_Value
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
