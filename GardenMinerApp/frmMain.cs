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
        LogFile vLog = new LogFile();
        Converter vCon = new Converter();

        public frmMain()
        {
            InitializeComponent();

            CoinsGPU();
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

                    dt_coin.Rows.Add(coins,id,tag,algorithm,block_time,block_reward,block_reward24,last_block,difficulty,difficulty24,nethash,
                        exchange_rate,exchange_rate24,exchange_rate_vol,exchange_rate_curr,market_cap,estimated_rewards,estimated_rewards24,btc_revenue,
                        btc_revenue24,profitability,profitability24, lagging,timestamp);
                }

                cbbCoin.DisplayMember = "coins";
                cbbCoin.ValueMember = "id";
                cbbCoin.DataSource = dt_coin;

                dgvCoin.DataSource = dt_coin;
                dgvCoin.ClearSelection();
            }
        }

        private void cbbCoin_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tm_sync_Tick(object sender, EventArgs e)
        {
            CoinsGPU();
            Application.DoEvents();
        }

        private void PicLUX_Click(object sender, EventArgs e)
        {
            string url =txtPoolLUX.Text+ "/api/wallet?address=" + txtWalletLUX.Text;
            string json = clsWebApi.GetCoinsStatus(url);
            txtJsonLUX.Text = json;
            ViewCoinDetail(json);
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
    }
}
