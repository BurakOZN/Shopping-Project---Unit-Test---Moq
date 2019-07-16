using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL.Manager
{
    public interface IPrint
    {
        void ExportDataTableToPdf(DataTable dtblTable, String strPdfPath, string strHeader);
    }
}
