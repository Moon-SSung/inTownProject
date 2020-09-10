using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inTownProject_ver._1._0._0.Common
{
    public class DT_RealTime
    {

        /// <summary>
        /// Date형식 초기화
        /// </summary>
        public void initializeDateTimePicker(DateTimePicker dt)
        {
            //DateTimePicker dt = new DateTimePicker();
            dt.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dt.Format = DateTimePickerFormat.Custom;
        }
    }
}
