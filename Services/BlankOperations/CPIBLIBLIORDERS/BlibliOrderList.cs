using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CPIBLIBLIORDERS
{
    public partial class BlibliOrderList : Form
    {
        Timer timer = new Timer();
        string placeholder = "Search order/status...";
        APIAccess.APIParameter.ApiResponseBliBliListOrder responseAPI;
        IPosTransaction posTransaction;
        IApplication application;
        public BlibliOrderList(IPosTransaction _posTransaction, IApplication _application)
        {

            InitializeComponent();
            overlayPanel = new Panel();
            overlayPanel.Dock = DockStyle.Fill;
            overlayPanel.BackColor = Color.FromArgb(120, 0, 0, 0); // 120 = level gelap
            overlayPanel.Visible = false;

            this.Controls.Add(overlayPanel);

            posTransaction = _posTransaction;
            application = _application;
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            timer.Start();

            lblStore.Text = ApplicationSettings.Terminal.StoreId + " - " + ApplicationSettings.Terminal.StoreName;
            this.Load += BlibliOrderList_Load;

            // Hook event TextBox
            textBoxSearch.GotFocus += RemovePlaceholder;
            textBoxSearch.LostFocus += SetPlaceholder;

            generateList(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"), "ALL");
        }

        private string MapStatus(string statusCode)
        {
            if (string.IsNullOrWhiteSpace(statusCode))
                return "Semua";

            switch (statusCode.ToUpper())
            {
                case "FP":
                    return "Pesanan diterima";

                case "PF":
                    return "Pesanan dibuat";

                case "PU":
                    return "Pesanan siap dikirim";

                case "CX":
                    return "Pesanan dalam pengiriman";

                case "D":
                    return "Pesanan terkirim";

                case "X":
                    return "Pesanan dibatalkan";

                default:
                    return statusCode; // kalau ada kode baru, tetap tampil
            }
        }

        private void generateList(string _dateTime, string _status)
        {
            //int rowIndex;
            //string amountCashout = "";
           
            string storeId = ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            //var url = "https://devpfm.cp.co.id/api/grab/listOrder"
            string url = "";
            APIAccess.APIParameter.Receiver receiverParm;
            string functionName = "GetBlibliOrderAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

         

            if (url == "") 
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            else
            {               
                responseAPI = APIAccess.APIFunction.BlibliOrderAPI.getBlibliOrderList(url, ApplicationSettings.Terminal.InventLocationId, _dateTime);
                gridBlibliOrderList.Rows.Clear();
                if (responseAPI.data != null && responseAPI.data.Any()) 
                {
                    //gridBlibliOrderList.Rows.Clear();



                    List<APIAccess.APIParameter.OrderData> order = responseAPI.data;

                    string selectedStatus = _status;

                    IEnumerable<APIAccess.APIParameter.OrderData> filteredOrder;

                    // Kalau pilih ALL → tampilkan semua
                    if (string.Equals(selectedStatus, "ALL", StringComparison.OrdinalIgnoreCase))
                    {
                        filteredOrder = order;
                    }
                    else
                    {
                        filteredOrder = order.Where(x =>
                            !string.IsNullOrEmpty(x.status) &&
                            x.status.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase));
                    }

                    var groupedData = filteredOrder
                        .GroupBy(item => item.order_id)
                        .Select(group => new
                        {
                            OrderID = group.Key,
                            Items = group.ToList()
                        });


                    //List<APIAccess.APIParameter.OrderData> order = responseAPI.data;
                   
                    //var groupedData = order.GroupBy(item => item.order_id).Select(group => new
                    //{
                    //    OrderID = group.Key,
                    //    Items = group.ToList()

                    //});





                    // Add data to grabMartList
                    foreach (var group in groupedData)
                    {

                        foreach (var item in group.Items)
                        {
                            //merchantId = item.merchantID;

                            int rowIndex = gridBlibliOrderList.Rows.Add(
                                item.order_id,
                                MapStatus(item.status),
                                item.order_time


                            );

                            // Store the original item data in the row tag for later use
                            gridBlibliOrderList.Rows[rowIndex].Tag = item;


                        }
                    }

                }
                //else
                //{
                //    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(responseAPI.message, MessageBoxButtons.OK, MessageBoxIcon.Information))
                //    {
                //        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                //        return;
                //    }

                //}





            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBoxSearch.Text.Trim();

            // Kalau kosong → reset semua row jadi visible
            if (string.IsNullOrWhiteSpace(keyword))
            {
                foreach (DataGridViewRow row in gridBlibliOrderList.Rows)
                {
                    if (!row.IsNewRow)
                        row.Visible = true;
                }

                return;
            }

            keyword = keyword.ToLower();

            foreach (DataGridViewRow row in gridBlibliOrderList.Rows)
            {
                if (!row.IsNewRow)
                {
                    var item = row.Tag as APIAccess.APIParameter.OrderData;

                    if (item != null && item.order_id != null)
                    {
                        if (string.Equals(keyword, placeholder, StringComparison.OrdinalIgnoreCase))
                        //if (string.Compare keyword == placeholder)
                        { 
                            row.Visible = true; 
                        }
                        else
                        { 
                            //row.Visible = item.order_id.ToLower().Contains(keyword);
                            bool matchOrder = !string.IsNullOrEmpty(item.order_id) && item.order_id.ToLower().Contains(keyword);
                            bool matchStatus = !string.IsNullOrEmpty(MapStatus(item.status)) && MapStatus(item.status).ToLower().Contains(keyword);

                            row.Visible = matchOrder || matchStatus;  
                        }

                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        //private void textBoxSearch_TextChanged(object sender, EventArgs e)
        //{
        //    string keyword = textBoxSearch.Text.Trim().ToLower();

        //    if (string.IsNullOrWhiteSpace(keyword))
        //    {
        //        foreach (DataGridViewRow row in gridBlibliOrderList.Rows)
        //        {
        //            if (!row.IsNewRow)
        //                row.Visible = true;
        //        }
        //        return;
        //    }

        //    foreach (DataGridViewRow row in gridBlibliOrderList.Rows)
        //    {
        //        if (!row.IsNewRow)
        //        {
        //            var item = row.Tag as APIAccess.APIParameter.OrderData;

        //            if (item != null)
        //            {
        //                bool matchOrder = !string.IsNullOrEmpty(item.order_id) && item.order_id.ToLower().Contains(keyword);
        //                bool matchStatus = !string.IsNullOrEmpty(item.status) && item.status.ToLower().Contains(keyword);

        //                row.Visible = matchOrder || matchStatus;  
        //            }
        //            else
        //            {
        //                row.Visible = false;
        //            }
        //        }
        //    }
        //}

        private void gridBlibliOrderList_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex < 0)                return;

            if( e.ColumnIndex != -1)
            {
                if (gridBlibliOrderList.Columns[e.ColumnIndex].Name == "orderDetail") 
                {
              
                    var orderId = gridBlibliOrderList.Rows[e.RowIndex].Cells["order_id"].Value.ToString();
                    //var storeCode = gridBlibliOrde                                            rList.Rows[e.RowIndex].Cells["store_code"].Value.ToString();
                    var status = gridBlibliOrderList.Rows[e.RowIndex].Cells["status"].Value.ToString();


                    ShowOrderDetail(orderId, status); 
                }
            }

        }

        private void ShowOrderDetail(string orderId, string status)
        {
            var order = responseAPI.data.FirstOrDefault(o => o.order_id == orderId);
            if (order == null) return;

            var popup = new OrderDetailForm(
                            order.order_id,
                            status,
                            new List<APIAccess.APIParameter.OrderData> { order },
                            posTransaction,
                            application
                        );

            try
            {
                overlayPanel.BringToFront();
                overlayPanel.Visible = true;   

                var result = popup.ShowDialog(this);

                
                

                if (result == DialogResult.OK)
                {
                    this.Close();
                }
                else if (result == DialogResult.Cancel)
                {
                    generateList(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"), "ALL");
                }
            }
            finally
            {
                overlayPanel.Visible = false;  
            }
        }

 

        private void timer_Tick(object sender, EventArgs e)
        {
            lblCurDate.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");

        }
        private void BlibliOrderList_Load(object sender, EventArgs e)
        {
            // Set placeholder awal saat form load
            gridBlibliOrderList.CellClick += gridBlibliOrderList_CellClick;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            SetPlaceholder(null, null);
            InitStatusCombo(); 
        }

        private void InitStatusCombo()
        {
            var statusList = new List<object>
            {
                new { Text = "Semua", Value = "ALL" },
                new { Text = "Pesanan diterima", Value = "FP" },
                new { Text = "Pesanan dibuat", Value = "PF" },
                new { Text = "Pesanan siap dikirim", Value = "PU" },
                new { Text = "Pesanan dalam pengiriman", Value = "CX" },
                new { Text = "Pesanan terkirim", Value = "D" },
                new { Text = "Pesanan dibatalkan", Value = "X" }
            };

            comBoxStatus.DataSource = statusList;
            comBoxStatus.DisplayMember = "Text";
            comBoxStatus.ValueMember = "Value";
            comBoxStatus.SelectedIndex = 0;
        }

        void RemovePlaceholder(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == placeholder)
            {
                textBoxSearch.Text = "";
                textBoxSearch.ForeColor = Color.Black;
            }
        }

        void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                textBoxSearch.Text = placeholder;
                textBoxSearch.ForeColor = Color.Gray;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            DateTime tanggal = pickerTanggal.Value; 
            string formatted = tanggal.ToString("yyyy-MM-dd HH:mm:ss");
            string selectedStatus = comBoxStatus.SelectedValue.ToString();
            generateList(formatted, selectedStatus);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            generateList(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"), "ALL");
        }
    }
}

