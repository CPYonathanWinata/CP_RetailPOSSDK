/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis;
namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class frmKitDisassembly
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.lblKitIdQuery = new System.Windows.Forms.Label();
            this.btnSearchKit = new DevExpress.XtraEditors.SimpleButton();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblKitName = new System.Windows.Forms.Label();
            this.lblKitNameVal = new System.Windows.Forms.Label();
            this.lblKitDescriptionVal = new System.Windows.Forms.Label();
            this.lblKitQty = new System.Windows.Forms.Label();
            this.txtKitIdInput = new DevExpress.XtraEditors.TextEdit();
            this.txtKitQtyInput = new DevExpress.XtraEditors.TextEdit();
            this.tablePanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.gridKitComponents = new DevExpress.XtraGrid.GridControl();
            this.gridKitComponentsView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.componentIdCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.nameCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.descriptionCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.quantityCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.unitCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.addToCartQtyCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qtyToInventoryCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKitIdInput.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKitQtyInput.Properties)).BeginInit();
            this.tablePanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponentsView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tablePanelButtons, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.gridKitComponents, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblHeader, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(26, 20, 26, 11);
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnClear, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblKitIdQuery, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSearchKit, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDescription, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblKitName, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblKitNameVal, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblKitDescriptionVal, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblKitQty, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtKitIdInput, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtKitQtyInput, 1, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(29, 115);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(961, 204);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Appearance.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.remove;
            this.btnClear.Appearance.Options.UseImage = true;
            this.btnClear.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.remove;
            this.btnClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClear.Location = new System.Drawing.Point(900, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(57, 32);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblKitIdQuery
            // 
            this.lblKitIdQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKitIdQuery.AutoSize = true;
            this.lblKitIdQuery.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKitIdQuery.Location = new System.Drawing.Point(3, 17);
            this.lblKitIdQuery.Name = "lblKitIdQuery";
            this.lblKitIdQuery.Size = new System.Drawing.Size(47, 21);
            this.lblKitIdQuery.TabIndex = 8;
            this.lblKitIdQuery.Text = "Kit ID";
            this.lblKitIdQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearchKit
            // 
            this.btnSearchKit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearchKit.Appearance.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.search;
            this.btnSearchKit.Appearance.Options.UseImage = true;
            this.btnSearchKit.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.search;
            this.btnSearchKit.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSearchKit.Location = new System.Drawing.Point(837, 3);
            this.btnSearchKit.Name = "btnSearchKit";
            this.btnSearchKit.Size = new System.Drawing.Size(57, 32);
            this.btnSearchKit.TabIndex = 1;
            this.btnSearchKit.Text = "Search";
            this.btnSearchKit.Click += new System.EventHandler(this.btnGetKit_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(3, 145);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(89, 21);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKitName
            // 
            this.lblKitName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKitName.AutoSize = true;
            this.lblKitName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKitName.Location = new System.Drawing.Point(3, 100);
            this.lblKitName.Name = "lblKitName";
            this.lblKitName.Size = new System.Drawing.Size(71, 21);
            this.lblKitName.TabIndex = 4;
            this.lblKitName.Text = "Kit name";
            this.lblKitName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKitNameVal
            // 
            this.lblKitNameVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKitNameVal.AutoSize = true;
            this.lblKitNameVal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKitNameVal.Location = new System.Drawing.Point(98, 100);
            this.lblKitNameVal.Name = "lblKitNameVal";
            this.lblKitNameVal.Size = new System.Drawing.Size(0, 21);
            this.lblKitNameVal.TabIndex = 4;
            this.lblKitNameVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKitDescriptionVal
            // 
            this.lblKitDescriptionVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKitDescriptionVal.AutoSize = true;
            this.lblKitDescriptionVal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKitDescriptionVal.Location = new System.Drawing.Point(98, 145);
            this.lblKitDescriptionVal.Name = "lblKitDescriptionVal";
            this.lblKitDescriptionVal.Size = new System.Drawing.Size(0, 21);
            this.lblKitDescriptionVal.TabIndex = 5;
            this.lblKitDescriptionVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKitQty
            // 
            this.lblKitQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKitQty.AutoSize = true;
            this.lblKitQty.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKitQty.Location = new System.Drawing.Point(3, 55);
            this.lblKitQty.Name = "lblKitQty";
            this.lblKitQty.Size = new System.Drawing.Size(70, 21);
            this.lblKitQty.TabIndex = 10;
            this.lblKitQty.Text = "Quantity";
            this.lblKitQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtKitIdInput
            // 
            this.txtKitIdInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtKitIdInput.Location = new System.Drawing.Point(98, 3);
            this.txtKitIdInput.Name = "txtKitIdInput";
            this.txtKitIdInput.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtKitIdInput.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtKitIdInput.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtKitIdInput.Properties.Appearance.Options.UseBackColor = true;
            this.txtKitIdInput.Properties.Appearance.Options.UseFont = true;
            this.txtKitIdInput.Properties.Appearance.Options.UseForeColor = true;
            this.txtKitIdInput.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtKitIdInput.Size = new System.Drawing.Size(733, 32);
            this.txtKitIdInput.TabIndex = 0;
            this.txtKitIdInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKitIdInput_KeyDown);
            // 
            // txtKitQtyInput
            // 
            this.txtKitQtyInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtKitQtyInput.Location = new System.Drawing.Point(98, 41);
            this.txtKitQtyInput.Name = "txtKitQtyInput";
            this.txtKitQtyInput.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtKitQtyInput.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtKitQtyInput.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtKitQtyInput.Properties.Appearance.Options.UseBackColor = true;
            this.txtKitQtyInput.Properties.Appearance.Options.UseFont = true;
            this.txtKitQtyInput.Properties.Appearance.Options.UseForeColor = true;
            this.txtKitQtyInput.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtKitQtyInput.Size = new System.Drawing.Size(733, 32);
            this.txtKitQtyInput.TabIndex = 2;
            this.txtKitQtyInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKitQtyInput_KeyDown);
            // 
            // tablePanelButtons
            // 
            this.tablePanelButtons.AutoSize = true;
            this.tablePanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tablePanelButtons.ColumnCount = 8;
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.Controls.Add(this.btnPgUp, 0, 0);
            this.tablePanelButtons.Controls.Add(this.btnOk, 3, 0);
            this.tablePanelButtons.Controls.Add(this.btnDown, 6, 0);
            this.tablePanelButtons.Controls.Add(this.btnPgDown, 7, 0);
            this.tablePanelButtons.Controls.Add(this.btnUp, 1, 0);
            this.tablePanelButtons.Controls.Add(this.btnClose, 4, 0);
            this.tablePanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanelButtons.Location = new System.Drawing.Point(26, 688);
            this.tablePanelButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tablePanelButtons.Name = "tablePanelButtons";
            this.tablePanelButtons.RowCount = 1;
            this.tablePanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelButtons.Size = new System.Drawing.Size(972, 69);
            this.tablePanelButtons.TabIndex = 13;
            // 
            // btnPgUp
            // 
            this.btnPgUp.AllowFocus = false;
            this.btnPgUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Appearance.Options.UseImage = true;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(4, 4);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(65, 61);
            this.btnPgUp.TabIndex = 13;
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Location = new System.Drawing.Point(355, 6);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.ShowToolTips = false;
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(830, 4);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 61);
            this.btnDown.TabIndex = 9;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Appearance.Options.UseImage = true;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(903, 4);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(65, 61);
            this.btnPgDown.TabIndex = 11;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(77, 4);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 61);
            this.btnUp.TabIndex = 14;
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(490, 6);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Cancel";
            // 
            // gridKitComponents
            // 
            this.gridKitComponents.Location = new System.Drawing.Point(29, 325);
            this.gridKitComponents.MainView = this.gridKitComponentsView;
            this.gridKitComponents.Name = "gridKitComponents";
            this.gridKitComponents.Size = new System.Drawing.Size(960, 335);
            this.gridKitComponents.TabIndex = 6;
            this.gridKitComponents.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridKitComponentsView});
            // 
            // gridKitComponentsView
            // 
            this.gridKitComponentsView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gridKitComponentsView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridKitComponentsView.Appearance.Preview.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridKitComponentsView.Appearance.Preview.Options.UseFont = true;
            this.gridKitComponentsView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridKitComponentsView.Appearance.Row.Options.UseFont = true;
            this.gridKitComponentsView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.componentIdCol,
            this.nameCol,
            this.descriptionCol,
            this.quantityCol,
            this.unitCol,
            this.addToCartQtyCol,
            this.qtyToInventoryCol});
            this.gridKitComponentsView.DetailHeight = 50;
            this.gridKitComponentsView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridKitComponentsView.GridControl = this.gridKitComponents;
            this.gridKitComponentsView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridKitComponentsView.Name = "gridKitComponentsView";
            this.gridKitComponentsView.OptionsCustomization.AllowColumnMoving = false;
            this.gridKitComponentsView.OptionsCustomization.AllowFilter = false;
            this.gridKitComponentsView.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridKitComponentsView.OptionsView.AutoCalcPreviewLineCount = true;
            this.gridKitComponentsView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.gridKitComponentsView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridKitComponentsView.OptionsView.ShowGroupPanel = false;
            this.gridKitComponentsView.OptionsView.ShowIndicator = false;
            this.gridKitComponentsView.OptionsView.ShowPreview = true;
            this.gridKitComponentsView.PaintStyleName = "Skin";
            this.gridKitComponentsView.PreviewFieldName = "COMMENT";
            this.gridKitComponentsView.RowHeight = 40;
            this.gridKitComponentsView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;            
            this.gridKitComponentsView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridKitComponentsView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridKitComponentsView_CustomDrawCell);
            this.gridKitComponentsView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridKitComponentsView_MouseUp);
            this.gridKitComponentsView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridKitComponentsView_MouseDown);
            this.gridKitComponentsView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridKitComponentsView_MouseMove);
            // 
            // componentIdCol
            // 
            this.componentIdCol.Caption = "Component Id";
            this.componentIdCol.FieldName = "ITEMID";
            this.componentIdCol.Name = "componentIdCol";
            this.componentIdCol.OptionsColumn.AllowEdit = false;
            this.componentIdCol.Visible = true;
            this.componentIdCol.VisibleIndex = 0;
            this.componentIdCol.Width = 121;
            // 
            // nameCol
            // 
            this.nameCol.Caption = "Name";
            this.nameCol.FieldName = "NAME";
            this.nameCol.Name = "nameCol";
            this.nameCol.OptionsColumn.AllowEdit = false;
            this.nameCol.Visible = true;
            this.nameCol.VisibleIndex = 1;
            this.nameCol.Width = 191;
            // 
            // descriptionCol
            // 
            this.descriptionCol.Caption = "Description";
            this.descriptionCol.FieldName = "DESCRIPTION";
            this.descriptionCol.Name = "descriptionCol";
            this.descriptionCol.OptionsColumn.AllowEdit = false;
            this.descriptionCol.Visible = true;
            this.descriptionCol.VisibleIndex = 2;
            this.descriptionCol.Width = 211;
            // 
            // quantityCol
            // 
            this.quantityCol.AppearanceHeader.Options.UseTextOptions = true;
            this.quantityCol.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.quantityCol.Caption = "Total quantity";
            this.quantityCol.DisplayFormat.FormatString = "n2";
            this.quantityCol.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.quantityCol.FieldName = "COMPONENTQTY";
            this.quantityCol.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.quantityCol.Name = "quantityCol";
            this.quantityCol.OptionsColumn.AllowEdit = false;
            this.quantityCol.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.quantityCol.Visible = true;
            this.quantityCol.VisibleIndex = 4;
            this.quantityCol.Width = 117;
            // 
            // unitCol
            // 
            this.unitCol.Caption = "Unit";
            this.unitCol.FieldName = "UNIT";
            this.unitCol.Name = "unitCol";
            this.unitCol.OptionsColumn.AllowEdit = false;
            this.unitCol.Visible = true;
            this.unitCol.VisibleIndex = 3;
            this.unitCol.Width = 63;
            // 
            // addToCartQtyCol
            // 
            this.addToCartQtyCol.AppearanceHeader.Options.UseTextOptions = true;
            this.addToCartQtyCol.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.addToCartQtyCol.AppearanceCell.Options.UseTextOptions = true;
            this.addToCartQtyCol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.addToCartQtyCol.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.addToCartQtyCol.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.addToCartQtyCol.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.addToCartQtyCol.Caption = "Add to cart";
            this.addToCartQtyCol.DisplayFormat.FormatString = "n2";
            this.addToCartQtyCol.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.addToCartQtyCol.FieldName = "QTYTOCART";
            this.addToCartQtyCol.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.addToCartQtyCol.Name = "addToCartQtyCol";
            this.addToCartQtyCol.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.addToCartQtyCol.Visible = true;
            this.addToCartQtyCol.VisibleIndex = 5;
            this.addToCartQtyCol.Width = 93;
            this.addToCartQtyCol.OptionsColumn.AllowEdit = false;
            // 
            // qtyToInventoryCol
            // 
            this.qtyToInventoryCol.AppearanceHeader.Options.UseTextOptions = true;
            this.qtyToInventoryCol.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.qtyToInventoryCol.Caption = "Return to inventory";
            this.qtyToInventoryCol.DisplayFormat.FormatString = "n2";
            this.qtyToInventoryCol.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.qtyToInventoryCol.FieldName = "QTYTOINVENTORY";
            this.qtyToInventoryCol.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.qtyToInventoryCol.Name = "qtyToInventoryCol";
            this.qtyToInventoryCol.OptionsColumn.AllowEdit = false;
            this.qtyToInventoryCol.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.qtyToInventoryCol.Visible = true;
            this.qtyToInventoryCol.VisibleIndex = 6;
            this.qtyToInventoryCol.Width = 162;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeader.Location = new System.Drawing.Point(345, 20);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.lblHeader.Size = new System.Drawing.Size(333, 85);
            this.lblHeader.TabIndex = 7;
            this.lblHeader.Text = "Disassemble kit";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmKitDisassembly
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmKitDisassembly";
            this.Text = "frmKitDisassembly";
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKitIdInput.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKitQtyInput.Properties)).EndInit();
            this.tablePanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponentsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblKitNameVal;
        private System.Windows.Forms.Label lblKitName;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblKitDescriptionVal;
        private System.Windows.Forms.TableLayoutPanel tablePanelButtons;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private DevExpress.XtraGrid.GridControl gridKitComponents;
        private DevExpress.XtraGrid.Views.Grid.GridView gridKitComponentsView;
        private System.Windows.Forms.Label lblKitIdQuery;
        private DevExpress.XtraEditors.SimpleButton btnSearchKit;
        private System.Windows.Forms.Label lblKitQty;
        private DevExpress.XtraGrid.Columns.GridColumn componentIdCol;
        private DevExpress.XtraGrid.Columns.GridColumn nameCol;
        private DevExpress.XtraGrid.Columns.GridColumn descriptionCol;
        private DevExpress.XtraGrid.Columns.GridColumn quantityCol;
        private DevExpress.XtraGrid.Columns.GridColumn addToCartQtyCol;
        private DevExpress.XtraGrid.Columns.GridColumn qtyToInventoryCol;
        private DevExpress.XtraEditors.TextEdit txtKitIdInput;
        private DevExpress.XtraEditors.TextEdit txtKitQtyInput;
        private DevExpress.XtraGrid.Columns.GridColumn unitCol;

    }
}