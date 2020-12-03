using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZJH.BaseTools.IO;

namespace ZJH.EsriGIS
{
    public class LicenseHelper
    {
        public static void CheckOutLicense()
        {
            if (!RuntimeManager.Bind(ProductCode.EngineOrDesktop))
            {
                MessageBox.Show("不能绑定ArcGIS runtime，应用程序即将关闭.");
                return;
            }
            IAoInitialize aoInit = new AoInitialize();
            try
            {
                aoInit.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            }
            catch (Exception ex) {
                Logger.log("CheckOutLicense", ex);
            }
            //esriLicenseStatus licenseStatus = esriLicenseStatus.esriLicenseUnavailable;
            //censeStatus = aoInit.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngine);
            //aoInit.CheckOutExtension(esriLicenseExtensionCode.);
            //esriLicenseProductCode productCode = esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB;
            //if (aoInit.IsProductCodeAvailable(productCode) == esriLicenseStatus.esriLicenseAvailable)
            //{
            //    aoInit.Initialize(productCode);
            //}
        }
    }
}
