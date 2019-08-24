using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1221_HW4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet2.縣市' 資料表。您可以視需要進行移動或移除。
            this.縣市TableAdapter.Fill(this.pizzaDBDataSet2.縣市);
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet2.訂購明細' 資料表。您可以視需要進行移動或移除。
            this.訂購明細TableAdapter.Fill(this.pizzaDBDataSet2.訂購明細);
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet1.餐點' 資料表。您可以視需要進行移動或移除。
            this.餐點TableAdapter.Fill(this.pizzaDBDataSet1.餐點);
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet1.餐點' 資料表。您可以視需要進行移動或移除。
            this.餐點TableAdapter.Fill(this.pizzaDBDataSet1.餐點);
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet1.餐點' 資料表。您可以視需要進行移動或移除。
            this.餐點TableAdapter.Fill(this.pizzaDBDataSet1.餐點);
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet.餐點' 資料表。您可以視需要進行移動或移除。
            this.餐點TableAdapter.Fill(this.pizzaDBDataSet.餐點);
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet2.鄉鎮市區' 資料表。您可以視需要進行移動或移除。
            this.鄉鎮市區TableAdapter.Fill(this.pizzaDBDataSet2.鄉鎮市區);
            // TODO: 這行程式碼會將資料載入 'pizzaDBDataSet2.訂購單' 資料表。您可以視需要進行移動或移除。
            this.訂購單TableAdapter.Fill(this.pizzaDBDataSet2.訂購單);
            groupBox1.Enabled = false; groupBox2.Enabled = false; groupBox3.Enabled = false;
            panel4.Enabled = false; btn_exit.Enabled = true;
            textBox7.Enabled = false; textBox8.Enabled = false; textBox9.Enabled = false; textBox10.Enabled = false;

            string s = "選擇餐點";
            comboBox6.Text = s; comboBox9.Text = s; comboBox8.Text = s; comboBox10.Text = s;
            comboBox7.Text = "帕瑪森";
            dateTimePicker1.MinDate = new DateTime(1980,1,1);
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker2.MinDate = DateTime.Today;
            
        }

        bool isNewMember = false;

        private void button1_Click(object sender, EventArgs e)
        {//新增
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = cnStr;
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("InsertMember", cn);
                    dateTimePicker1.MinDate = new DateTime(1980, 1, 1);
                    dateTimePicker1.MaxDate = DateTime.Today;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@birth", SqlDbType.Date));///
                    cmd.Parameters.Add(new SqlParameter("@sex", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@add_1", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@add_2", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@add_3", SqlDbType.NVarChar));

                    cmd.Parameters["@name"].Value = textBox1.Text;
                    cmd.Parameters["@birth"].Value = dateTimePicker1.Value;
                    cmd.Parameters["@sex"].Value = radioButton1.Checked ? true : false;
                    cmd.Parameters["@mail"].Value = textBox3.Text;
                    cmd.Parameters["@phone"].Value = textBox4.Text;
                    cmd.Parameters["@add_1"].Value = comboBox1.Text;
                    cmd.Parameters["@add_2"].Value = comboBox2.Text;
                    cmd.Parameters["@add_3"].Value = textBox5.Text;

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("新增成功");
                    isNewMember = true; member_id_number++;
                    groupBox3.Enabled = true; panel4.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "新增失敗");
            }
        }
        static bool isMemberExist;
        static bool isAlreadyCash=false;
        static int additional_price;
        static int[] Additional_price = new int[3] { 80, 100, 40 };
        static string[] PIE = new string[3] { "芝心", "酥香菠蘿芝心", "酥香菠蘿" };
        static string food_name = "";
        static double food_money = 0, total_money = 0, item_number = 0;

        string cnStr = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|PizzaDB.mdf;" +
                    "Integrated Security=SSPI";
        int discount = 0;
        int member_id_number = 100;

        private void button2_Click(object sender, EventArgs e)
        {//更新(修改)

            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = cnStr;
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("UpdateMember", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@birth", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@sex", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@add_1", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@add_2", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@add_3", SqlDbType.NVarChar));

                    cmd.Parameters["@name"].Value = textBox1.Text;
                    cmd.Parameters["@birth"].Value = dateTimePicker1.Value;
                    cmd.Parameters["@sex"].Value = radioButton1.Checked ? true : false;
                    cmd.Parameters["@mail"].Value = textBox3.Text;
                    cmd.Parameters["@phone"].Value = textBox4.Text;
                    cmd.Parameters["@add_1"].Value = comboBox1.Text;
                    cmd.Parameters["@add_2"].Value = comboBox2.Text;
                    cmd.Parameters["@add_3"].Value = textBox5.Text;

                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("更新成功");
                groupBox3.Enabled = true; panel4.Enabled = true;
                place = new ComboBox[3] { comboBox3, comboBox4, comboBox5 };

                for (int i = 0; i <= 2; i++)
                {
                    place[i].Enabled = false;
                }
                textBox6.Enabled = false;
                comboBox5.Text = "台科一門市";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "更新失敗");
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {//查詢
            using (SqlConnection cn = new SqlConnection())
            {
                //1.Connection
                cn.ConnectionString = cnStr;
                cn.Open();
                string member_name = textBox1.Text;
                //2.Command
                SqlCommand cmd = new SqlCommand("select * from 會員 where 姓名 = '" + member_name.Replace("'", "''") + "'", cn);
                //3.DataReader
                SqlDataReader dr = cmd.ExecuteReader();

                //開放部分區域可供user輸入
                groupBox1.Enabled = !(false); groupBox2.Enabled = !(false); textBox2.Enabled = false;

                isMemberExist = dr.Read();
                if (isMemberExist == true)//當輸入的會員name存在table時
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        if (member_name == dr[1].ToString())//會員姓名相符
                        {
                            textBox2.Text = dr[0].ToString();//自動填入會員卡號
                            switch (dr[3])
                            {//自動選擇性別
                                case true: //馬芙丸馬猶猶
                                    radioButton1.Checked = true;
                                    break;
                                case false:
                                    radioButton2.Checked = true;
                                    break;
                            }
                            dateTimePicker1.Value = Convert.ToDateTime(dr[2]);
                            textBox3.Text = dr[4].ToString();//自動填入電子信箱
                            textBox4.Text = dr[5].ToString();//自動填入手機號碼
                            comboBox1.Text = dr[6].ToString();//自動選擇地址_縣市
                            comboBox2.Text = dr[7].ToString();//自動選擇地址_鄉鎮市區
                            textBox5.Text = dr[8].ToString();//自動填入通訊地址_街道名
                            break;
                        }
                    }
                }
                else if (isMemberExist != true)
                {
                    member_id_number++;
                    MessageBox.Show("會員資料不存在");
                }
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {//加入購物車 
            double rate = 1;
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = cnStr;
                    cn.Open();
                    food_name = comboBox6.Text;
                    SqlCommand cmd = new SqlCommand("select * from 餐點 where 餐點名稱 = '" + food_name + "'", cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    string pie;

                    if (dr.Read())
                    {
                        food_money = Convert.ToInt16(dr[2]);
                        textBox7.Text = food_money.ToString("0");
                        total_money += food_money;
                        pie = comboBox7.Text;
                        if (pie != "")
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (pie == PIE[i])
                                {
                                    additional_price += Additional_price[i];
                                }
                            }
                        }
                        total_money = (int)(total_money + additional_price);
                        if (total_money >= 1000 && total_money < 2000)
                        {
                            rate = 0.9; total_money *= rate;
                        }
                        else if (total_money >= 2000 && total_money < 3500)
                        {
                            total_money /= 0.9;
                            rate = 0.8;
                            total_money *= rate;
                        }
                        else if (total_money > 3500)
                        {
                            total_money /= 0.8;
                            rate = 0.7;
                            total_money *= rate;
                        }
                        item_number++;
                        textBox11.Text = item_number.ToString();
                        textBox12.Text = total_money.ToString("0");
                        textBox13.Text = (rate * 100).ToString() + "%";
                        listBox1.Items.Add(food_name);
                    }
                    discount = Convert.ToInt16(rate*100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "加入購物車失敗");
            }
        }
        private void btn2_Click(object sender, EventArgs e)
        {
            //加入購物車 pizza
            double rate = 1;
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = cnStr;
                    cn.Open();
                    food_name = comboBox9.Text;
                    SqlCommand cmd = new SqlCommand("select * from 餐點 where 餐點名稱 = '" + food_name + "'", cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        food_money = Convert.ToInt16(dr[2]);
                        textBox8.Text = food_money.ToString("0");
                        total_money += food_money;

                        total_money = (int)(total_money + additional_price);
                        if (total_money >= 1000 && total_money < 2000)
                        {
                            rate = 0.9; total_money *= rate;
                        }
                        else if (total_money >= 2000 && total_money < 3500)
                        {
                            total_money /= 0.9;
                            rate = 0.8;
                            total_money *= rate;
                        }
                        else if (total_money > 3500)
                        {
                            total_money /= 0.8;
                            rate = 0.7;
                            total_money *= rate;
                        }
                        item_number++;
                        textBox11.Text = item_number.ToString();
                        textBox12.Text = total_money.ToString("0");
                        textBox13.Text = (rate * 100).ToString() + "%";

                        listBox1.Items.Add(food_name);
                    }
                    discount = Convert.ToInt16(rate * 100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "加入購物車失敗");
            }

        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            TextBox[] t = new TextBox[] { textBox11, textBox12, textBox13 };
            listBox1.Items.Clear();
            for (int i = 0; i <= 2; i++)
            {
                t[i].Text = "";
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            //加入購物車 pizza
            double rate = 1;
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = cnStr;
                    cn.Open();
                    food_name = comboBox8.Text;
                    SqlCommand cmd = new SqlCommand("select * from 餐點 where 餐點名稱 = '" + food_name + "'", cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        food_money = Convert.ToInt16(dr[2]);
                        textBox9.Text = food_money.ToString("0");
                        total_money += food_money;

                        total_money = (int)(total_money + additional_price);
                        if (total_money >= 1000 && total_money < 2000)
                        {
                            rate = 0.9; total_money *= rate;
                        }
                        else if (total_money >= 2000 && total_money < 3500)
                        {
                            total_money /= 0.9;
                            rate = 0.8;
                            total_money *= rate;
                        }
                        else if (total_money > 3500)
                        {
                            total_money /= 0.8;
                            rate = 0.7;
                            total_money *= rate;
                        }
                        item_number++;
                        textBox11.Text = item_number.ToString();
                        textBox12.Text = total_money.ToString("0");
                        textBox13.Text = (rate * 100).ToString() + "%";

                        listBox1.Items.Add(food_name);
                    }
                    discount = Convert.ToInt16(rate * 100);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "加入購物車失敗");
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            //加入購物車 pizza
            double rate = 1;
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = cnStr;
                    cn.Open();
                    food_name = comboBox10.Text;
                    SqlCommand cmd = new SqlCommand("select * from 餐點 where 餐點名稱 = '" + food_name + "'", cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        food_money = Convert.ToInt16(dr[2]);
                        textBox10.Text = food_money.ToString("0");
                        total_money += food_money;

                        total_money = (int)(total_money + additional_price);
                        if (total_money >= 1000 && total_money < 2000)
                        {
                            rate = 0.9; total_money *= rate;
                        }
                        else if (total_money >= 2000 && total_money < 3500)
                        {
                            total_money /= 0.9;
                            rate = 0.8;
                            total_money *= rate;
                        }
                        else if (total_money > 3500)
                        {
                            total_money /= 0.8;
                            rate = 0.7;
                            total_money *= rate;
                        }
                        item_number++;
                        textBox11.Text = item_number.ToString();
                        textBox12.Text = total_money.ToString("0");
                        textBox13.Text = (rate * 100).ToString() + "%";

                        listBox1.Items.Add(food_name);
                    }
                    discount = Convert.ToInt16(rate*100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "加入購物車失敗");
            }
        }

        private void btn_exit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExit3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        ComboBox[] place;
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            place = new ComboBox[3] { comboBox3, comboBox4, comboBox5 };
            if (radioButton4.Checked)
            {
                for (int i = 0; i <= 2; i++)
                {
                    place[i].Enabled = false;
                }
                textBox6.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            place = new ComboBox[3] { comboBox3, comboBox4, comboBox5 };
            if (radioButton3.Checked)
            {
                for (int i = 0; i <= 2; i++)
                {
                    place[i].Enabled = true;
                }
                textBox6.Enabled = false;
            }
        }

        private void btnBUY_Click(object sender, EventArgs e)
        {
            if((comboBox3.Text == ""||comboBox4.Text == ""||comboBox5.Text == "")&&(textBox6.Text == ""))
            {
                MessageBox.Show("門市未選擇或外送住址未輸入");
            }
            else
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection())
                    {
                        cn.ConnectionString = cnStr;
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("InsertBill", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@member_id", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@order_date", SqlDbType.DateTime));
                        cmd.Parameters.Add(new SqlParameter("@get_method", SqlDbType.Bit));
                        cmd.Parameters.Add(new SqlParameter("@get_date", SqlDbType.Date));
                        cmd.Parameters.Add(new SqlParameter("@address_1", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@address_2", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@address_3", SqlDbType.NVarChar));
                        cmd.Parameters.Add(new SqlParameter("@discount", SqlDbType.Int));

                        if (isNewMember == true)
                            cmd.Parameters["@member_id"].Value = member_id_number;
                        else
                            cmd.Parameters["@member_id"].Value = int.Parse(textBox2.Text);
                        cmd.Parameters["@order_date"].Value = DateTime.Today;
                        cmd.Parameters["@get_method"].Value = (radioButton3.Checked) ? true : false;
                        cmd.Parameters["@get_date"].Value = dateTimePicker2.Value;
                        cmd.Parameters["@address_1"].Value = comboBox3.Text;
                        cmd.Parameters["@address_2"].Value = comboBox4.Text;
                        cmd.Parameters["@address_3"].Value = comboBox5.Text;
                        cmd.Parameters["@discount"].Value = discount;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("結帳完成");
                        isAlreadyCash = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString() + "結帳失敗");
                }
                
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabControl1.SelectedIndex == 2) && (isAlreadyCash == true))
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = cnStr;
                    cn.Open();
                    string del_item = listBox1.SelectedItem.ToString();
                    SqlCommand cmd = new SqlCommand("select * from 餐點 where 餐點名稱 = '" + del_item + "'", cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        listBox1.Items.Remove(del_item);
                        item_number--;
                        textBox11.Text = item_number.ToString();
                        total_money -= Convert.ToInt32(dr[2]);
                        textBox12.Text = total_money.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "刪除失敗");
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}


