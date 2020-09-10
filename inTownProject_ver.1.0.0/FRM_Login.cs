using inTownProject_ver._1._0._0.DB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace inTownProject_ver._1._0._0
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 로그인 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Login_Click(object sender, EventArgs e)
        {
            LoginProcess();
        }

        /// <summary>
        /// 취소 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Cancle_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "프로그램을 종료하시겠습니까?", "경고",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 로그인 프로세스
        /// </summary>
        private void LoginProcess()
        {
            if (string.IsNullOrEmpty(Txt_UserID.Text) || string.IsNullOrEmpty(Txt_UserPassword.Text))
            {
                MessageBox.Show(this, "아이디 또는 패스워드를 입력하세요!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string strUserID = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Connect.CONNSTRING))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT userID, Name FROM dbo.TB_user " +
                                      " WHERE userID = @userID and userPW = @userPW ";

                    //ID 설정
                    SqlParameter parmUserID = new SqlParameter("@userID", SqlDbType.VarChar, 6);
                    parmUserID.Value = Txt_UserID.Text;
                    cmd.Parameters.Add(parmUserID);

                    //Password 설정
                    SqlParameter parmPassword = new SqlParameter("@userPW", SqlDbType.VarChar, 8);
                    parmPassword.Value = Txt_UserPassword.Text;
                    cmd.Parameters.Add(parmPassword);


                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    strUserID = reader["userID"] != null ? reader["userID"].ToString() : "";

                    if(strUserID != "") //로그인 성공
                    {
                        DB_Connect.LoginUserID = strUserID;
                        DB_Connect.LoginUserName = reader["Name"].ToString() + " 님";
                        MessageBox.Show(this, "접속성공", "로그인 성공");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(this, "접속실패", "로그인 실패",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(this, $"ERROR : {e.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        /// <summary>
        /// 초기화 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            Txt_UserID.Text = Txt_UserPassword.Text = "";
        }

        /// <summary>
        /// 버튼 활성화
        /// </summary>
        Control ActiveControl;

        /// <summary>
        /// 키패드 클릭(터치)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_0_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ActiveControl.Focus();
            SendKeys.Send(btn.Text);

            //Button btn = (Button)sender;
            //ActiveControl = Txt_UserID;
            //SendKeys.Send(btn.Text);

            //Button btn = (Button)sender;
            //ActiveControl = Txt_UserID;
            //if (Txt_UserID == ActiveControl)
            //{
            //    SendKeys.Send(btn.Text);
            //}
            //else
            //{
            //    SendKeys.Send(btn.Text);

            //}

        }

        private void Txt_UserID_Click(object sender, EventArgs e)
        {
            ActiveControl = (Control)sender;
        }

        private void Txt_UserPassword_Click(object sender, EventArgs e)
        {
            ActiveControl = (Control)sender;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            ActiveControl = Txt_UserID;
        }
    }
}
