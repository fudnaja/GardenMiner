using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusCom;

namespace GardenMinerApp
{
    public partial class frmMain : Form
    {
        FormWindowState currentWindowState;
        LogFile vLog = new LogFile();
        Converter vCon = new Converter();
        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-US");

        public frmMain()
        {
            InitializeComponent();
            tm_time.Enabled = true;
            tm_time.Start();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtTokenNotify.Select();
            txtTokenNotify.Select(txtTokenNotify.Text.Length, 0);

            txtURL.Select();
            txtURL.Select(txtURL.Text.Length, 0);
        }

        public void Notify(string Message)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + txtTokenNotify.Text);
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("message",Message.ToString())
                });

                    HttpResponseMessage response = client.PostAsync("https://notify-api.line.me/api/notify", formContent).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    vLog.WriteLogEvent(result.ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CoinsGPU()
        {
            DataTable dt_coin = new DataTable();
            dt_coin.Columns.Add("coins", typeof(string));
            dt_coin.Columns.Add("id", typeof(int));
            dt_coin.Columns.Add("tag", typeof(string));
            dt_coin.Columns.Add("algorithm", typeof(string));
            dt_coin.Columns.Add("block_time", typeof(Double));
            dt_coin.Columns.Add("block_reward", typeof(Double));
            dt_coin.Columns.Add("block_reward24", typeof(Double));
            dt_coin.Columns.Add("last_block", typeof(Int64));
            dt_coin.Columns.Add("difficulty", typeof(Double));
            dt_coin.Columns.Add("difficulty24", typeof(Double));
            dt_coin.Columns.Add("nethash", typeof(Int64));
            dt_coin.Columns.Add("exchange_rate", typeof(Double));
            dt_coin.Columns.Add("exchange_rate24", typeof(Double));
            dt_coin.Columns.Add("exchange_rate_vol", typeof(Double));
            dt_coin.Columns.Add("exchange_rate_curr", typeof(string));
            dt_coin.Columns.Add("market_cap", typeof(string));
            dt_coin.Columns.Add("estimated_rewards", typeof(Double));
            dt_coin.Columns.Add("estimated_rewards24", typeof(Double));
            dt_coin.Columns.Add("btc_revenue", typeof(Double));
            dt_coin.Columns.Add("btc_revenue24", typeof(Double));
            dt_coin.Columns.Add("profitability", typeof(Int64));
            dt_coin.Columns.Add("profitability24", typeof(Int64));
            dt_coin.Columns.Add("lagging", typeof(bool));
            dt_coin.Columns.Add("timestamp", typeof(int));

            string coins_res = clsWebApi.GetCoinsWhattomine();
            try
            {
                DataSet ds = vCon.JsonToDataset(coins_res);
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        string coins = ds.Tables[i].TableName;
                        int id = int.Parse(ds.Tables[i].Rows[0]["id"].ToString());
                        string tag = ds.Tables[i].Rows[0]["tag"].ToString();
                        string algorithm = ds.Tables[i].Rows[0]["algorithm"].ToString();
                        Double block_time = Double.Parse(ds.Tables[i].Rows[0]["block_time"].ToString());
                        Double block_reward = Double.Parse(ds.Tables[i].Rows[0]["block_reward"].ToString());
                        Double block_reward24 = Double.Parse(ds.Tables[i].Rows[0]["block_reward24"].ToString());
                        Int64 last_block = Int64.Parse(ds.Tables[i].Rows[0]["last_block"].ToString());
                        Double difficulty = Double.Parse(ds.Tables[i].Rows[0]["difficulty"].ToString());
                        Double difficulty24 = Double.Parse(ds.Tables[i].Rows[0]["difficulty24"].ToString());
                        Int64 nethash = Int64.Parse(ds.Tables[i].Rows[0]["nethash"].ToString());
                        Double exchange_rate = Double.Parse(ds.Tables[i].Rows[0]["exchange_rate"].ToString());
                        Double exchange_rate24 = Double.Parse(ds.Tables[i].Rows[0]["exchange_rate24"].ToString());
                        Double exchange_rate_vol = Double.Parse(ds.Tables[i].Rows[0]["exchange_rate_vol"].ToString());
                        string exchange_rate_curr = ds.Tables[i].Rows[0]["exchange_rate_curr"].ToString();
                        string market_cap = ds.Tables[i].Rows[0]["market_cap"].ToString();
                        Double estimated_rewards = Double.Parse(ds.Tables[i].Rows[0]["estimated_rewards"].ToString());
                        Double estimated_rewards24 = Double.Parse(ds.Tables[i].Rows[0]["estimated_rewards24"].ToString());
                        Double btc_revenue = Double.Parse(ds.Tables[i].Rows[0]["btc_revenue"].ToString());
                        Double btc_revenue24 = Double.Parse(ds.Tables[i].Rows[0]["btc_revenue24"].ToString());
                        Int64 profitability = Int64.Parse(ds.Tables[i].Rows[0]["profitability"].ToString());
                        Int64 profitability24 = Int64.Parse(ds.Tables[i].Rows[0]["profitability24"].ToString());
                        bool lagging = bool.Parse(ds.Tables[i].Rows[0]["lagging"].ToString());
                        int timestamp = int.Parse(ds.Tables[i].Rows[0]["timestamp"].ToString());

                        dt_coin.Rows.Add(coins, id, tag, algorithm, block_time, block_reward, block_reward24, last_block, difficulty, difficulty24, nethash,
                            exchange_rate, exchange_rate24, exchange_rate_vol, exchange_rate_curr, market_cap, estimated_rewards, estimated_rewards24, btc_revenue,
                            btc_revenue24, profitability, profitability24, lagging, timestamp);
                    }

                    cbbCoin.DisplayMember = "coins";
                    cbbCoin.ValueMember = "id";
                    cbbCoin.DataSource = dt_coin;

                    dgvCoin.DataSource = dt_coin;
                    dgvCoin.ClearSelection();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CoinsNews()
        {
            DataTable dt_coin = new DataTable();
            dt_coin.Columns.Add("coins", typeof(string));
            dt_coin.Columns.Add("id", typeof(int));
            dt_coin.Columns.Add("tag", typeof(string));
            dt_coin.Columns.Add("algorithm", typeof(string));
            dt_coin.Columns.Add("lagging", typeof(bool));
            dt_coin.Columns.Add("listed", typeof(bool));
            dt_coin.Columns.Add("status", typeof(string));
            dt_coin.Columns.Add("testing", typeof(bool));

            string coins_res = clsWebApi.GetCoinsNews();
            try
            {
                DataSet ds = vCon.JsonToDataset(coins_res);
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        string coins = ds.Tables[i].TableName;
                        int id = int.Parse(ds.Tables[i].Rows[0]["id"].ToString());
                        string tag = ds.Tables[i].Rows[0]["tag"].ToString();
                        string algorithm = ds.Tables[i].Rows[0]["algorithm"].ToString();
                        bool lagging = bool.Parse(ds.Tables[i].Rows[0]["lagging"].ToString());
                        bool listed = bool.Parse(ds.Tables[i].Rows[0]["listed"].ToString());
                        string status = ds.Tables[i].Rows[0]["status"].ToString();
                        bool testing = bool.Parse(ds.Tables[i].Rows[0]["testing"].ToString());

                        dt_coin.Rows.Add(coins, id, tag, algorithm, lagging, listed, status, testing);
                    }

                    cbbCoinStatus.DisplayMember = "coins";
                    cbbCoinStatus.ValueMember = "id";
                    cbbCoinStatus.DataSource = dt_coin;

                    dgvCoinStatus.DataSource = dt_coin;
                    dgvCoinStatus.ClearSelection();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetExplorer()
        {
            string url = txtExplorerLUX.Text + txtWalletLUX.Text;
            double received_amount = clsWebApi.GetCoinsExplorer(url);
            double unpaid_amount = double.Parse(txtUnpaid.Text);
            double total_amount = received_amount + unpaid_amount;
            lblTotalAmount.Text = total_amount.ToString("#,##0.#########0");
        }

        private void cbbCoin_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tm_sync_Tick(object sender, EventArgs e)
        {
            tm_sync.Stop();
            CoinsGPU();
            CoinsNews();
            Application.DoEvents();
            tm_sync.Start();
        }

        private void PicLUX_Click(object sender, EventArgs e)
        {
            string url = txtPoolLUX.Text + "/api/wallet?address=" + txtWalletLUX.Text;
            string json = clsWebApi.GetCoinsStatus(url);
            txtJsonLUX.Text = json;

            lblUnitName1.Text = lblUnitName2.Text = lblUnitName3.Text = lblUnitName4.Text = lblUnitName5.Text = lblUnitName6.Text = lblLUX.Text;
            ViewCoinDetail(json);
            GetExplorer();
        }

        private void txtJsonLUX_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtUnsold_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtUnpaid_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtPaid24h_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void PicNotify_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Garden Miner : LUX\r\n");
            str.Append("ยังไม่ได้ขาย : " + txtUnsold.Text + "\r\n");
            str.Append("คงเหลือสุทธิ : " + txtBalance.Text + "\r\n");
            str.Append("ยังไม่ได้จ่าย : " + txtUnpaid.Text + "\r\n");
            str.Append("จ่ายแล้วในช่วง 24 ชม. : " + txtPaid24h.Text + "\r\n");
            str.Append("รวมทั้งสิ้น : " + txtTotal.Text + "\r\n");
            str.Append("---------------------------");
            Notify(str.ToString());
        }

        private void PicSTAK_Click(object sender, EventArgs e)
        {
            string url = txtPoolSTAK.Text + "/api/wallet?address=" + txtWalletSTAK.Text;
            string json = clsWebApi.GetCoinsStatus(url);
            txtJsonSTAK.Text = json;
            lblUnitName1.Text = lblUnitName2.Text = lblUnitName3.Text = lblUnitName4.Text = lblUnitName5.Text = lblUnitName6.Text = lblSTAK.Text;
            ViewCoinDetail(json);
        }

        private void ViewCoinDetail(string json)
        {
            string res_json = "{\"res\":{\"info\":" + json + "}}";
            DataSet ds = vCon.JsonToDataset(res_json);
            if (ds.Tables.Contains("info"))
            {
                DataTable dt = ds.Tables["info"];

                if (dt.Columns.Contains("currency"))
                    lblCoin.Text = dt.Rows[0]["currency"].ToString();

                if (dt.Columns.Contains("unsold"))
                    txtUnsold.Text = double.Parse(dt.Rows[0]["unsold"].ToString()).ToString("#,##0.###################0");

                if (dt.Columns.Contains("balance"))
                    txtBalance.Text = double.Parse(dt.Rows[0]["balance"].ToString()).ToString("#,##0.#########0");

                if (dt.Columns.Contains("unpaid"))
                    txtUnpaid.Text = double.Parse(dt.Rows[0]["unpaid"].ToString()).ToString("#,##0.#########0");

                if (dt.Columns.Contains("paid24h"))
                    txtPaid24h.Text = double.Parse(dt.Rows[0]["paid24h"].ToString()).ToString("#,##0.#########0");

                if (dt.Columns.Contains("total"))
                    txtTotal.Text = double.Parse(dt.Rows[0]["total"].ToString()).ToString("#,##0.#########0");
            }
        }

        private void tm_BTC_Tick(object sender, EventArgs e)
        {
            tm_BTC.Stop();
            GetBTCPrice();
            tm_BTC.Start();
        }

        private void GetBTCPrice()
        {
            double btc = clsWebApi.GetCoinsBTC();
            double last_btc = 0;
            lblBTCPrice.Text = btc.ToString("#,##0.#0");
            if (lblBTCPrice.Tag.ToString() == "0")
            {
                lblBTCPrice.Tag = btc;
            }
            else
            {
                last_btc = double.Parse(lblBTCPrice.Tag.ToString());
            }

            if (btc > last_btc)
            {
                PicStatusBTC.Image = GardenMinerApp.Properties.Resources.happy;
                lblBTCPrice.ForeColor = Color.Green;
                PicUp.Visible = true;
                PicDown.Visible = false;
            }
            else if (btc == last_btc)
            {
                PicStatusBTC.Image = GardenMinerApp.Properties.Resources.neutral;
                lblBTCPrice.ForeColor = Color.Orange;
                PicUp.Visible = false;
                PicDown.Visible = false;
            }
            else
            {
                PicStatusBTC.Image = GardenMinerApp.Properties.Resources.sad;
                lblBTCPrice.ForeColor = Color.Red;
                PicUp.Visible = false;
                PicDown.Visible = true;
            }
            Application.DoEvents();
        }

        private void tm_LUX_Tick(object sender, EventArgs e)
        {
            tm_LUX.Stop();
            LUX_Now();
            tm_LUX.Start();
        }

        private void LUX_Now()
        {
            string url = txtPoolLUX.Text + "/api/wallet?address=" + txtWalletLUX.Text;
            string json = clsWebApi.GetCoinsStatus(url);
            string res_json = "{\"res\":{\"info\":" + json + "}}";
            DataSet ds = vCon.JsonToDataset(res_json);
            if (ds.Tables.Contains("info"))
            {
                DataTable dt = ds.Tables["info"];
                double unpaid = 0;
                if (dt.Columns.Contains("unpaid"))
                    unpaid = double.Parse(dt.Rows[0]["unpaid"].ToString());

                url = txtExplorerLUX.Text + txtWalletLUX.Text;
                double received_amount = clsWebApi.GetCoinsExplorer(url);
                double total_amount = received_amount + unpaid;
                double BTC_THB = clsWebApi.GetCoinsBTC();
                double LUX_BTC = clsWebApi.GetCoinsRate("LUX");
                double LUX_THB = LUX_BTC * total_amount * BTC_THB;
                lblLUXPrice.Text = LUX_THB.ToString("#,##0.#0");


                if (chkNotify.Checked)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("อัพเดตแท่นขุด LUX ⚒️\r\n");
                    str.Append("กำลังขุดได้ (รอจ่าย) : " + unpaid.ToString("#,##0.#######0") + " 💱\r\n");
                    str.Append("ขุดสำเร็จแล้ว : " + received_amount.ToString("#,##0.#######0") + " 💱\r\n");
                    str.Append("ยอดเหรียญรวม : " + total_amount.ToString("#,##0.#######0") + " 💱\r\n");
                    str.Append("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$\r\n");
                    str.Append("อัตราแลกเปลี่ยนเหรียญล่าสุด\r\n");
                    str.Append("LUX-BTC : " + LUX_BTC.ToString("#,##0.####0") + "\r\n");
                    str.Append("BTC-THB : " + BTC_THB.ToString("#,##0.#0") + "\r\n");
                    str.Append("รายได้ล่าสุด : " + LUX_THB.ToString("#,##0.#0") + " บาท\r\n");
                    str.Append("#################################\r\n");
                    Notify(str.ToString());
                }

                StringBuilder noti = new StringBuilder();
                noti.Append("อัพเดตแท่นขุด LUX ⚒️\r\n");
                noti.Append("ยอดเหรียญรวม : " + total_amount.ToString("#,##0.#######0") + " 💱\r\n");
                noti.Append("รายได้ล่าสุด : " + LUX_THB.ToString("#,##0.#0") + " บาท\r\n");

                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipText = noti.ToString();
                notifyIcon1.BalloonTipTitle = "Gerden Miner";
                notifyIcon1.ShowBalloonTip(2000);
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            CoinsGPU();
            CoinsNews();
            GetBTCPrice();
            LUX_Now();
            tm_sync.Enabled = true;
            tm_sync.Start();
            tm_BTC.Enabled = true;
            tm_BTC.Start();
            tm_LUX.Enabled = true;
            tm_LUX.Start();
            WindowState = FormWindowState.Minimized;
        }

        private void tm_time_Tick(object sender, EventArgs e)
        {
            txtDateTime.Text = DateTime.Now.ToString("dd MMMM yyyy hh:mm:tt:ss", enGB);

            if(DateTime.Now.Second==0)
            {
                if(this.WindowState != FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
        }

        private void lblHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void PicHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
            else
            {
                currentWindowState = this.WindowState;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = this.currentWindowState;
            }
        }
    }
}
