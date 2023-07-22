using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Doctors_Office
{

    public partial class patientsForm : Form
    {
        string gender;
        string marrage;
        public string path = @"server=DESKTOP-3ML4V2C;DataBase=Doctor;User Id=sa;password=123";
        public SqlCommand cmd;
        public SqlConnection conn;
        public SqlDataAdapter adapter;
        public DataTable dt;
        string nextService;
        List<string> services;
        int nextYear;
        int nextMonth;
        int nextDay;
        int year;
        int month;
        int day;
        double dayEarning = 0;
        public patientsForm()
        {
            InitializeComponent();
            lastVisitDate.Format = DateTimePickerFormat.Custom;
            lastVisitDate.CustomFormat = " yyyy/MM/dd";
            conn = new SqlConnection(path);

            services = new List<string>();
            services.Add("visit");
            services.Add("hair_laser");
            services.Add("surgery");
            services.Add("butox");
            services.Add("gel");
            services.Add("RFF");
            services.Add("Co2");
            services.Add("hair_transplant");
            services.Add("eyebrow_transplant");
            services.Add("crayo");
            services.Add("microderm");
            services.Add("micronideling");
            services.Add("mezo");
            services.Add("RF");
            services.Add("javansazi");
            services.Add("others");

            nextTimeServiceComboBox.SelectedIndex = 0;

            display();
        }


        public void display()
        {
            try
            {
                dt = new DataTable();
                conn.Open();
                adapter = new SqlDataAdapter("select * from patients", conn);
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        //تابع زیر مشخص میکند کاربر کدام گزینه را برای خدمات بعدی انتخاب کرده است
        public string what_is_next_service(ComboBox cb)
        {
            for (int i = 0; i < services.Count; i++)    
            {
                if (i == cb.SelectedIndex)
                    return services[i];
            }
            return null;
        }
        public void clear()
        {
            try
            {

                nameTxt.Text = "";
                lastNameTxt.Text = "";
                numberTxt.Text = "";
                nationalCodeTxt.Text = "";
                addressTxt.Text = "";
                singleCheckBox.Checked = false;
                marriedCheckBox.Checked = false;
                maleCheckBox.Checked = false;
                femaleCheckBox.Checked = false;
                infoTxt.Text = "";
                nextTimeServiceComboBox.SelectedIndex = 0;
                debtMoney.Text = "";
                nationalCodeSearchBox.Text = "";
                paymentTxt.Text = "";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }


        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                nameTxt.Text = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                lastNameTxt.Text = dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                numberTxt.Text = dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                nationalCodeTxt.Text = dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                addressTxt.Text = dataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                infoTxt.Text = dataGridView.Rows[e.RowIndex].Cells[10].Value.ToString();
                paymentTxt.Text = dataGridView.Rows[e.RowIndex].Cells[11].Value.ToString();
                debtMoney.Text = dataGridView.Rows[e.RowIndex].Cells[12].Value.ToString();

                if (dataGridView.Rows[e.RowIndex].Cells[5].Value.ToString() == "single")
                {
                    singleCheckBox.Checked = true;
                    marriedCheckBox.Checked = false;
                }   
                else
                {
                    marriedCheckBox.Checked = true;
                    singleCheckBox.Checked = false;
                }

                if (dataGridView.Rows[e.RowIndex].Cells[6].Value.ToString() == "male")
                {
                    maleCheckBox.Checked = true;
                    femaleCheckBox.Checked = false;
                }      
                else
                {
                    femaleCheckBox.Checked = true;
                    maleCheckBox.Checked = false;
                }
                nextService = dataGridView.Rows[e.RowIndex].Cells[13].Value.ToString();
                nextTimeServiceComboBox.SelectedIndex = services.IndexOf(nextService);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }    
        }
        //تابعی برای بررسی استثنا
        private bool check()
        {
            if (nameTxt.Text == "" || lastNameTxt.Text == "" || nationalCodeTxt.Text == "" || numberTxt.Text == "" || addressTxt.Text == "" || paymentTxt.Text == "")
            {
                MessageBox.Show("اطلاعات به صورت کامل وارد نشده است!");
                return false;
            }
            else if (marriedCheckBox.Checked && singleCheckBox.Checked)
            {
                MessageBox.Show("لطفا فقط یک گزینه را برای وضعیت تاهل انتخاب کنید!");
                marriedCheckBox.Checked = false;
                singleCheckBox.Checked = false;
                return false;
            }
            else if (maleCheckBox.Checked && femaleCheckBox.Checked)
            {
                MessageBox.Show("لطفا فقط یک گزینه را برای جنسیت انتخاب کنید!");
                maleCheckBox.Checked = false;
                femaleCheckBox.Checked = false;
                return false;
            }
            return true;
        }

        private void nationalCodeSearchBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                adapter = new SqlDataAdapter("select * from patients where national_code like '%" + nationalCodeSearchBox.Text + "%'", conn);
                dt = new DataTable();
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void dayPaymentBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                adapter = new SqlDataAdapter("select * from patients where visit_day = '" + DateTime.Now.Day + "' and visit_month = '" + DateTime.Now.Month + "' and visit_year = '" + DateTime.Now.Year + "'", conn);
                dt = new DataTable();
                adapter.Fill(dt);
                dataGridView.DataSource = dt;

                for (int i = 0; i < dt.Rows.Count; i++)
                    dayEarning += Convert.ToDouble(dataGridView.Rows[i].Cells[11].Value.ToString());

                MessageBox.Show(dayEarning.ToString());
                dayEarning = 0;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void exitBtn_Click_1(object sender, EventArgs e)
        {
            patientsForm.ActiveForm.Close();
        }

        private void refreshhBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                display();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void debtorBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                adapter = new SqlDataAdapter("select * from patients where debt != '" + 0 + "'", conn);
                dt = new DataTable();
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void clearrBtn_Click_1(object sender, EventArgs e)
        {
            clear();
        }

        private void deleteeBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("delete from patients where national_code = '" + nationalCodeTxt.Text + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                clear();
                MessageBox.Show("با موفقیت حذف شد");
                display();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void updateeBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (check())
                {
                    if (marriedCheckBox.Checked)
                        marrage = "married";
                    else
                        marrage = "single";
                    if (maleCheckBox.Checked)
                        gender = "male";
                    else
                        gender = "female";
                    conn.Open();

                    year = this.lastVisitDate.Value.Year;
                    month = this.lastVisitDate.Value.Month;
                    day = this.lastVisitDate.Value.Day;

                    nextService = what_is_next_service(nextTimeServiceComboBox);

                    cmd = new SqlCommand("update patients set name='" + nameTxt.Text + "', last_name = '" + lastNameTxt.Text + "',number = '" + numberTxt.Text + "',address = '" + addressTxt.Text + "',marrage= '" + marrage + "',gender = '" + gender + "',visit_day= '" + day + "',visit_month= '" + month + "',visit_year= '" + year + "',info= '" + infoTxt.Text + "',earning= '" + paymentTxt.Text + "',debt= '" + debtMoney.Text + "',service= '" + nextService + "'where national_code = '" + nationalCodeTxt.Text + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    clear();
                    MessageBox.Show("اطلاعات با موفقیت ویرایش شد!");
                    display();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void saveeBtn_Click_1(object sender, EventArgs e)
        {
            if (check())
            {
                if (singleCheckBox.Checked)
                    marrage = "single";
                else
                    marrage = "married";
                if (maleCheckBox.Checked)
                    gender = "male";
                else
                    gender = "female";

                year = this.lastVisitDate.Value.Year;
                month = this.lastVisitDate.Value.Month;
                day = this.lastVisitDate.Value.Day;
                nextService = what_is_next_service(nextTimeServiceComboBox);
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("insert into patients(name,last_name,number,national_code,address,marrage,gender,visit_day,visit_month,visit_year,info,earning,debt,service) values ('" + nameTxt.Text + "','" + lastNameTxt.Text + "','" + Convert.ToInt32(numberTxt.Text) + "','" + Convert.ToInt32(nationalCodeTxt.Text) + "','" + addressTxt.Text + "','" + marrage + "','" + gender + "','" + day + "','" + month + "','" + year + "','" + infoTxt.Text + "','" + Convert.ToInt32(paymentTxt.Text) + "','" + Convert.ToInt32(debtMoney.Text) + "','" + nextService + "')", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    clear();
                    MessageBox.Show("اطلاعات بیمار با موفقیت ذخیره شد");
                    display();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }
    }
}
