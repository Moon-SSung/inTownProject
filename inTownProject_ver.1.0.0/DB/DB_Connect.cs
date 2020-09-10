using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inTownProject_ver._1._0._0.DB
{
    class DB_Connect
    {
        //공용 연결문자열, DB연결
        public static string CONNSTRING =
            "Data Source=127.0.0.1; " +
            "Initial Catalog=inTownProjectDB; " +
            "Persist Security Info=True; " +
            "User ID=sa; " +
            "Password=p@ssw0rd! ";

        public static string LoginUserID = "";
        public static string LoginUserName = "";
    }
}
