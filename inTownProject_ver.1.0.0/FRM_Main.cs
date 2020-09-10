using System;
using System.Windows.Forms;
using inTownProject_ver._1._0._0.DB;
using inTownProject_ver._1._0._0.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace inTownProject_ver._1._0._0
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            DT_RealTime dtr = new DT_RealTime();
            dtr.initializeDateTimePicker(Dtp_RealTime);
            if (login.ShowDialog() != DialogResult.OK)
            {
                Environment.Exit(0);
            }

            View_Cmb_Order();
            View_Cmb_Machine();
        }

        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Logout_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show(this, "로그아웃 하시겠습니까?", "정보",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                //여기에 내용들 다 비우는 코드
                this.Opacity = 0;
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    Environment.Exit(0);
                }

                //여기에 내용 다시 초기화 코드
                this.Opacity = 100;
            }
        }

        //작업 지시 SQL문
        /*
            select M.코드,M.기기명,M.참조공정코드2,P.코드 from TB_Code_Mach as M
            inner join Tb_Code_Proc as P
            on M.참조공정코드2 = P.코드;
         */

        /// <summary>
        /// 로그인 후 사용자 정보 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Activated(object sender, EventArgs e)
        {
            Lbl_ID.Text = DB_Connect.LoginUserID;
            Lbl_Name.Text = DB_Connect.LoginUserName;
        }

        /// <summary>
        /// 작업지시 콤보박스 항목
        /// </summary>
        private void View_Cmb_Order()
        {
            using (SqlConnection conn = new SqlConnection(DB_Connect.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT 작업지시번호, 품번 FROM TB_order";
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<int, short> dict = new Dictionary<int, short>();
                while (reader.Read())
                {
                    dict.Add(int.Parse(reader[0].ToString()), short.Parse(reader[1].ToString()));
                }
                Cmb_Order.DataSource = new BindingSource(dict, null);
                Cmb_Order.DisplayMember = "Key";
                Cmb_Order.ValueMember = "Value";
                Cmb_Order.SelectedIndex = -1;
            }
        }

        private void View_Cmb_Machine()
        {
            using (SqlConnection conn = new SqlConnection(DB_Connect.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "     SELECT O.작업지시번호, M.코드, M.기기명, M.참조공정코드2, P.공정명 " +
                                  "       FROM TB_Code_Mach as M " +
                                  " INNER JOIN Tb_Code_Proc as P " +
                                  "         ON M.참조공정코드2 = P.코드 " +
                                  " INNER JOIN TB_order as O " +
                                  "         ON O.공정코드 = M.참조공정코드2 " +
                                  "";

                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<short, string> dict = new Dictionary<short, string>();
                while(reader.Read())
                {
                    dict.Add(short.Parse(reader[1].ToString()), reader[2].ToString());
                }

                Cmb_Machine.DataSource = new BindingSource(dict, null);
                Cmb_Machine.DisplayMember = "Value";
                Cmb_Machine.ValueMember = "Key";
                Cmb_Machine.SelectedIndex = -1; 
            }
        }
    }
}
