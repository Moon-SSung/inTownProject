using System;
using System.Windows.Forms;
using inTownProject_ver._1._0._0.DB;
using inTownProject_ver._1._0._0.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace inTownProject_ver._1._0._0
{
    public partial class MainForm : Form
    {
        bool initializeComplete = false;
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
            //View_Cmb_Machine();
            View_Cmb_Frame();
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
                cmd.CommandText = "SELECT 작업지시번호 FROM TB_order";

                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, int> dict = new Dictionary<string, int>();
                while (reader.Read())
                {
                    dict.Add(reader[0].ToString(), int.Parse(reader[0].ToString()));
                }
                Cmb_Order.DataSource = new BindingSource(dict, null);
                Cmb_Order.DisplayMember = "Key";
                Cmb_Order.ValueMember = "Value";
                Cmb_Order.SelectedIndex = -1;
                //MessageBox.Show("TEST");
                initializeComplete = true;
            }
        }

        /// <summary>
        /// 작업지시번호의 사출기 항목
        /// </summary>
        private void View_Cmb_Machine(int orderCode)
        {
            using (SqlConnection conn = new SqlConnection(DB_Connect.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "     SELECT M.코드, M.기기명 " +
                                  "       FROM TB_Code_Mach as M " +
                                  " INNER JOIN Tb_Code_Proc as P " +
                                  "         ON M.참조공정코드2 = P.코드 " +
                                  " INNER JOIN TB_order as O " +
                                  "         ON O.공정코드 = P.코드 " +
                                  "      WHERE O.작업지시번호 = @orderCode";
                
                SqlParameter param_orderCode = new SqlParameter("@orderCode", SqlDbType.SmallInt);
                             param_orderCode.Value = orderCode;
                cmd.Parameters.Add(param_orderCode);

                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<short, string> dict = new Dictionary<short, string>();
                while(reader.Read())
                {
                    dict.Add(short.Parse(reader[0].ToString()), reader[1].ToString());
                }

                Cmb_Machine.DataSource = new BindingSource(dict, null);
                Cmb_Machine.DisplayMember = "Value";
                Cmb_Machine.ValueMember = "Key";
                Cmb_Machine.SelectedIndex = -1; 
            }
        }

        /// <summary>
        /// 작업지시 리스트 선택했을 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cmb_Order_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initializeComplete)
                return;
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Connect.CONNSTRING))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "     SELECT O.품번, I.품명, O.계획수량, P.공정명 " +
                                      "       FROM TB_order as O " +
                                      " INNER JOIN TB_Code_Proc as P " +
                                      "         ON O.공정코드 = P.코드 " +
                                      " INNER JOIN TB_product as I " +
                                      "      ON O.품번 = I.품번 " +
                                      "    WHERE O.작업지시번호 = @Code ";

                    //cmd.CommandText = "     SELECT O.작업지시번호, O.품번, I.품명, O.계획수량, O.공정코드, P.공정명 " +
                    //                  "       FROM TB_order as O " +
                    //                  " INNER JOIN TB_Code_Proc as P " +
                    //                  "         ON O.공정코드 = P.코드 " +
                    //                  " INNER JOIN TB_product as I " +
                    //                  "      ON O.품번 = I.품번 " +
                    //                  "    WHERE O.작업지시번호 = @Code ";

                    //작업지시번호
                    SqlParameter orderCode = new SqlParameter("@Code", SqlDbType.Int);
                    KeyValuePair<string, int> keyvalPair = (KeyValuePair<string, int>)Cmb_Order.SelectedItem;
                    orderCode.Value = keyvalPair.Value;
                    cmd.Parameters.Add(orderCode);
                    
                    //데이터를 받아 TextBox들에 대입
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        Txt_ProdNum.Text = reader[0].ToString();
                        Txt_ProdName.Text = reader[1].ToString();
                        Txt_PlanCnt.Text = reader[2].ToString();
                        Txt_ProcName.Text = reader[3].ToString();
                    }

                    View_Cmb_Machine(keyvalPair.Value);
                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// 금형코드 항목
        /// </summary>
        private void View_Cmb_Frame()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Connect.CONNSTRING))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT 금형코드, 금형명 FROM TB_frame";

                    SqlDataReader reader = cmd.ExecuteReader();
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        dict.Add(reader[0].ToString(), reader[1].ToString());
                    }
                    Cmb_FrameName.DataSource = new BindingSource(dict, null);
                    Cmb_FrameName.DisplayMember = "Key";
                    Cmb_FrameName.ValueMember = "Value";
                    Cmb_FrameName.SelectedIndex = -1;
                    //MessageBox.Show("TEST");
                    //initializeComplete = true;
                }
            }
            catch
            {

            }
        }

        private void Cmb_FrameName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Pic_Frame.Image = Image.FromFile("P1.jpg");
        }
    }
}
