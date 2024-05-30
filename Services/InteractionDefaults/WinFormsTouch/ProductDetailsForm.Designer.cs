/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    partial class ProductDetailsForm
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.tlpProductInformation = new System.Windows.Forms.TableLayoutPanel();
            this.lblSearchName = new System.Windows.Forms.Label();
            this.lblSearchNameValue = new System.Windows.Forms.Label();
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblCategoryValue = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblDescriptionValue = new System.Windows.Forms.Label();
            this.tblProductImageAndBasicInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblPriceValue = new System.Windows.Forms.Label();
            this.lblBarCode = new System.Windows.Forms.Label();
            this.lblBarCodeValue = new System.Windows.Forms.Label();
            this.pbProductImage = new System.Windows.Forms.PictureBox();
            this.lblModelNumber = new System.Windows.Forms.Label();
            this.lblModelNumberValue = new System.Windows.Forms.Label();
            this.tblAttributeAndComponents = new System.Windows.Forms.TableLayoutPanel();
            this.tblProductAttributes = new System.Windows.Forms.TableLayoutPanel();
            this.lblProductAttributes = new System.Windows.Forms.Label();
            this.gridProductAttributes = new DevExpress.XtraGrid.GridControl();
            this.productAttributesBindingSource = new System.Windows.Forms.BindingSource();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tblKitComponents = new System.Windows.Forms.TableLayoutPanel();
            this.lblKitComponents = new System.Windows.Forms.Label();
            this.gridKitComponents = new DevExpress.XtraGrid.GridControl();
            this.gridKitComponentsView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colKitComponentNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKitComponentName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKitComponentQTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKitComponentUOM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnPgUpKitComponents = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUpKitComponents = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSubstitute = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDownKitComponents = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDownKitComponents = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnInventoryLookup = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnAddToSale = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tlpContent.SuspendLayout();
            this.tlpProductInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.tblProductImageAndBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).BeginInit();
            this.tblAttributeAndComponents.SuspendLayout();
            this.tblProductAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productAttributesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.tblKitComponents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponentsView)).BeginInit();
            this.tlpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeader.Location = new System.Drawing.Point(26, 11);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.lblHeader.Size = new System.Drawing.Size(972, 85);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Item name";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpContent
            // 
            this.tlpContent.ColumnCount = 1;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.Controls.Add(this.lblHeader, 0, 0);
            this.tlpContent.Controls.Add(this.tlpProductInformation, 0, 2);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(0, 0);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.Padding = new System.Windows.Forms.Padding(26, 11, 26, 11);
            this.tlpContent.RowCount = 4;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.Size = new System.Drawing.Size(1024, 768);
            this.tlpContent.TabIndex = 0;
            // 
            // tlpProductInformation
            // 
            this.tlpProductInformation.ColumnCount = 3;
            this.tlpProductInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProductInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpProductInformation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProductInformation.Controls.Add(this.lblSearchName, 0, 1);
            this.tlpProductInformation.Controls.Add(this.lblSearchNameValue, 0, 2);
            this.tlpProductInformation.Controls.Add(this.lblCategory, 0, 3);
            this.tlpProductInformation.Controls.Add(this.lblCategoryValue, 0, 4);
            this.tlpProductInformation.Controls.Add(this.lblDescription, 0, 5);
            this.tlpProductInformation.Controls.Add(this.lblDescriptionValue, 0, 6);
            this.tlpProductInformation.Controls.Add(this.tblProductImageAndBasicInfo, 0, 0);
            this.tlpProductInformation.Controls.Add(this.tblAttributeAndComponents, 2, 0);
            this.tlpProductInformation.Controls.Add(this.tlpButtons, 0, 7);
            this.tlpProductInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProductInformation.Location = new System.Drawing.Point(26, 96);
            this.tlpProductInformation.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProductInformation.Name = "tlpProductInformation";
            this.tlpProductInformation.RowCount = 8;
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProductInformation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInformation.Size = new System.Drawing.Size(972, 661);
            this.tlpProductInformation.TabIndex = 1;
            // 
            // lblSearchName
            // 
            this.lblSearchName.AutoSize = true;
            this.lblSearchName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblSearchName.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblSearchName.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblSearchName.Location = new System.Drawing.Point(0, 255);
            this.lblSearchName.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new System.Drawing.Size(101, 20);
            this.lblSearchName.TabIndex = 1;
            this.lblSearchName.Text = "Search name:";
            // 
            // lblSearchNameValue
            // 
            this.lblSearchNameValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearchNameValue.AutoSize = true;
            this.lblSearchNameValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SearchName", true));
            this.lblSearchNameValue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblSearchNameValue.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchNameValue.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblSearchNameValue.Location = new System.Drawing.Point(0, 275);
            this.lblSearchNameValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblSearchNameValue.Name = "lblSearchNameValue";
            this.lblSearchNameValue.Size = new System.Drawing.Size(473, 25);
            this.lblSearchNameValue.TabIndex = 2;
            this.lblSearchNameValue.Text = "Item name";
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.ProductDetailsViewModel);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblCategory.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblCategory.Location = new System.Drawing.Point(0, 310);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(76, 20);
            this.lblCategory.TabIndex = 3;
            this.lblCategory.Text = "Category:";
            // 
            // lblCategoryValue
            // 
            this.lblCategoryValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategoryValue.AutoSize = true;
            this.lblCategoryValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FormattedProductCategoryHierarchy", true));
            this.lblCategoryValue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCategoryValue.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryValue.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblCategoryValue.Location = new System.Drawing.Point(0, 330);
            this.lblCategoryValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblCategoryValue.Name = "lblCategoryValue";
            this.lblCategoryValue.Size = new System.Drawing.Size(473, 25);
            this.lblCategoryValue.TabIndex = 4;
            this.lblCategoryValue.Text = "Category";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDescription.Location = new System.Drawing.Point(0, 365);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(91, 20);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            // 
            // lblDescriptionValue
            // 
            this.lblDescriptionValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescriptionValue.AutoSize = true;
            this.lblDescriptionValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Description", true));
            this.lblDescriptionValue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblDescriptionValue.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescriptionValue.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDescriptionValue.Location = new System.Drawing.Point(0, 385);
            this.lblDescriptionValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblDescriptionValue.Name = "lblDescriptionValue";
            this.lblDescriptionValue.Size = new System.Drawing.Size(473, 212);
            this.lblDescriptionValue.TabIndex = 6;
            this.lblDescriptionValue.Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor i" +
    "ncididunt ut labore et dolore magna aliqua.";
            // 
            // tblProductImageAndBasicInfo
            // 
            this.tblProductImageAndBasicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblProductImageAndBasicInfo.AutoSize = true;
            this.tblProductImageAndBasicInfo.ColumnCount = 3;
            this.tblProductImageAndBasicInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblProductImageAndBasicInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblProductImageAndBasicInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProductImageAndBasicInfo.Controls.Add(this.lblPrice, 2, 0);
            this.tblProductImageAndBasicInfo.Controls.Add(this.lblPriceValue, 2, 1);
            this.tblProductImageAndBasicInfo.Controls.Add(this.lblBarCode, 2, 2);
            this.tblProductImageAndBasicInfo.Controls.Add(this.lblBarCodeValue, 2, 3);
            this.tblProductImageAndBasicInfo.Controls.Add(this.pbProductImage, 0, 0);
            this.tblProductImageAndBasicInfo.Controls.Add(this.lblModelNumber, 2, 4);
            this.tblProductImageAndBasicInfo.Controls.Add(this.lblModelNumberValue, 2, 5);
            this.tblProductImageAndBasicInfo.Location = new System.Drawing.Point(0, 0);
            this.tblProductImageAndBasicInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tblProductImageAndBasicInfo.Name = "tblProductImageAndBasicInfo";
            this.tblProductImageAndBasicInfo.RowCount = 7;
            this.tblProductImageAndBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductImageAndBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductImageAndBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductImageAndBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductImageAndBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductImageAndBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductImageAndBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProductImageAndBasicInfo.Size = new System.Drawing.Size(473, 240);
            this.tblProductImageAndBasicInfo.TabIndex = 0;
            // 
            // lblPrice
            // 
            this.lblPrice.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrice.AutoSize = true;
            this.lblPrice.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblPrice.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblPrice.Location = new System.Drawing.Point(266, 0);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(47, 20);
            this.lblPrice.TabIndex = 0;
            this.lblPrice.Text = "Price:";
            // 
            // lblPriceValue
            // 
            this.lblPriceValue.AutoSize = true;
            this.lblPriceValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Price", true));
            this.lblPriceValue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPriceValue.Font = new System.Drawing.Font("Segoe UI Semilight", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceValue.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblPriceValue.Location = new System.Drawing.Point(266, 20);
            this.lblPriceValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblPriceValue.Name = "lblPriceValue";
            this.lblPriceValue.Size = new System.Drawing.Size(118, 47);
            this.lblPriceValue.TabIndex = 1;
            this.lblPriceValue.Text = "$20.00";
            // 
            // lblBarCode
            // 
            this.lblBarCode.AutoSize = true;
            this.lblBarCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBarCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBarCode.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblBarCode.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblBarCode.Location = new System.Drawing.Point(266, 77);
            this.lblBarCode.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblBarCode.Name = "lblBarCode";
            this.lblBarCode.Size = new System.Drawing.Size(207, 20);
            this.lblBarCode.TabIndex = 2;
            this.lblBarCode.Text = "Bar code:";
            // 
            // lblBarCodeValue
            // 
            this.lblBarCodeValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBarCodeValue.AutoSize = true;
            this.lblBarCodeValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Barcode", true));
            this.lblBarCodeValue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBarCodeValue.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBarCodeValue.Location = new System.Drawing.Point(266, 97);
            this.lblBarCodeValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblBarCodeValue.Name = "lblBarCodeValue";
            this.lblBarCodeValue.Size = new System.Drawing.Size(152, 25);
            this.lblBarCodeValue.TabIndex = 3;
            this.lblBarCodeValue.Text = "00000000000000";
            // 
            // pbProductImage
            // 
            this.pbProductImage.BackColor = System.Drawing.Color.White;
            this.pbProductImage.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.bindingSource, "Image", true));
            this.pbProductImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbProductImage.ErrorImage = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.ProductUnavailable;
            this.pbProductImage.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.ProductUnavailable;
            this.pbProductImage.InitialImage = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.ProductUnavailable;
            this.pbProductImage.Location = new System.Drawing.Point(0, 0);
            this.pbProductImage.Margin = new System.Windows.Forms.Padding(0);
            this.pbProductImage.Name = "pbProductImage";
            this.tblProductImageAndBasicInfo.SetRowSpan(this.pbProductImage, 6);
            this.pbProductImage.Size = new System.Drawing.Size(240, 240);
            this.pbProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbProductImage.TabIndex = 20;
            this.pbProductImage.TabStop = false;
            // 
            // lblModelNumber
            // 
            this.lblModelNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblModelNumber.AutoSize = true;
            this.lblModelNumber.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblModelNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblModelNumber.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblModelNumber.Location = new System.Drawing.Point(266, 132);
            this.lblModelNumber.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblModelNumber.Name = "lblModelNumber";
            this.lblModelNumber.Size = new System.Drawing.Size(102, 20);
            this.lblModelNumber.TabIndex = 4;
            this.lblModelNumber.Text = "Item number:";
            // 
            // lblModelNumberValue
            // 
            this.lblModelNumberValue.AutoSize = true;
            this.lblModelNumberValue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblModelNumberValue.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelNumberValue.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblModelNumberValue.Location = new System.Drawing.Point(266, 152);
            this.lblModelNumberValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblModelNumberValue.Name = "lblModelNumberValue";
            this.lblModelNumberValue.Size = new System.Drawing.Size(115, 25);
            this.lblModelNumberValue.TabIndex = 5;
            this.lblModelNumberValue.Text = "Item number";
            // 
            // tblAttributeAndComponents
            // 
            this.tblAttributeAndComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblAttributeAndComponents.ColumnCount = 1;
            this.tblAttributeAndComponents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAttributeAndComponents.Controls.Add(this.tblProductAttributes, 0, 0);
            this.tblAttributeAndComponents.Controls.Add(this.tblKitComponents, 0, 1);
            this.tblAttributeAndComponents.Location = new System.Drawing.Point(499, 0);
            this.tblAttributeAndComponents.Margin = new System.Windows.Forms.Padding(0);
            this.tblAttributeAndComponents.Name = "tblAttributeAndComponents";
            this.tblAttributeAndComponents.RowCount = 2;
            this.tlpProductInformation.SetRowSpan(this.tblAttributeAndComponents, 8);
            this.tblAttributeAndComponents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblAttributeAndComponents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblAttributeAndComponents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblAttributeAndComponents.Size = new System.Drawing.Size(473, 661);
            this.tblAttributeAndComponents.TabIndex = 24;
            // 
            // tblProductAttributes
            // 
            this.tblProductAttributes.AutoSize = true;
            this.tblProductAttributes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblProductAttributes.ColumnCount = 5;
            this.tblProductAttributes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblProductAttributes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblProductAttributes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProductAttributes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblProductAttributes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblProductAttributes.Controls.Add(this.lblProductAttributes, 0, 0);
            this.tblProductAttributes.Controls.Add(this.gridProductAttributes, 0, 1);
            this.tblProductAttributes.Controls.Add(this.btnPgUp, 0, 2);
            this.tblProductAttributes.Controls.Add(this.btnUp, 1, 2);
            this.tblProductAttributes.Controls.Add(this.btnDown, 3, 2);
            this.tblProductAttributes.Controls.Add(this.btnPgDown, 4, 2);
            this.tblProductAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblProductAttributes.Location = new System.Drawing.Point(0, 0);
            this.tblProductAttributes.Margin = new System.Windows.Forms.Padding(0);
            this.tblProductAttributes.Name = "tblProductAttributes";
            this.tblProductAttributes.RowCount = 3;
            this.tblAttributeAndComponents.SetRowSpan(this.tblProductAttributes, 2);
            this.tblProductAttributes.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductAttributes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProductAttributes.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProductAttributes.Size = new System.Drawing.Size(473, 305);
            this.tblProductAttributes.TabIndex = 0;
            // 
            // lblProductAttributes
            // 
            this.lblProductAttributes.AutoSize = true;
            this.tblProductAttributes.SetColumnSpan(this.lblProductAttributes, 5);
            this.lblProductAttributes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblProductAttributes.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblProductAttributes.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblProductAttributes.Location = new System.Drawing.Point(0, 0);
            this.lblProductAttributes.Margin = new System.Windows.Forms.Padding(0);
            this.lblProductAttributes.Name = "lblProductAttributes";
            this.lblProductAttributes.Size = new System.Drawing.Size(136, 20);
            this.lblProductAttributes.TabIndex = 0;
            this.lblProductAttributes.Text = "Product attributes:";
            // 
            // gridProductAttributes
            // 
            this.gridProductAttributes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tblProductAttributes.SetColumnSpan(this.gridProductAttributes, 5);
            this.gridProductAttributes.DataBindings.Add(new System.Windows.Forms.Binding("DataSource", this.bindingSource, "ProductAttributes", true));
            this.gridProductAttributes.DataSource = this.productAttributesBindingSource;
            this.gridProductAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProductAttributes.Location = new System.Drawing.Point(0, 23);
            this.gridProductAttributes.MainView = this.gridView;
            this.gridProductAttributes.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.gridProductAttributes.Name = "gridProductAttributes";
            this.gridProductAttributes.Size = new System.Drawing.Size(473, 218);
            this.gridProductAttributes.TabIndex = 1;
            this.gridProductAttributes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // productAttributesBindingSource
            // 
            this.productAttributesBindingSource.DataMember = "ProductAttributes";
            this.productAttributesBindingSource.DataSource = this.bindingSource;
            // 
            // gridView
            // 
            this.gridView.Appearance.FooterPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.FooterPanel.Options.UseFont = true;
            this.gridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.gridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView.Appearance.Row.Options.UseFont = true;
            this.gridView.ColumnPanelRowHeight = 40;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName,
            this.colValue});
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView.GridControl = this.gridProductAttributes;
            this.gridView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsCustomization.AllowColumnMoving = false;
            this.gridView.OptionsCustomization.AllowColumnResizing = false;
            this.gridView.OptionsCustomization.AllowFilter = false;
            this.gridView.OptionsCustomization.AllowGroup = false;
            this.gridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView.OptionsCustomization.AllowRowSizing = true;
            this.gridView.OptionsCustomization.AllowSort = false;
            this.gridView.OptionsMenu.EnableColumnMenu = false;
            this.gridView.OptionsMenu.EnableFooterMenu = false;
            this.gridView.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.OptionsView.ShowIndicator = false;
            this.gridView.RowHeight = 30;
            this.gridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.OnGridView_FocusedRowChanged);
            // 
            // colName
            // 
            this.colName.AppearanceCell.Options.UseTextOptions = true;
            this.colName.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colName.Caption = "Name";
            this.colName.FieldName = "Name";
            this.colName.MinWidth = 125;
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 0;
            this.colName.Width = 125;
            // 
            // colValue
            // 
            this.colValue.AppearanceCell.Options.UseTextOptions = true;
            this.colValue.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colValue.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colValue.Caption = "Value";
            this.colValue.FieldName = "RawValue";
            this.colValue.MinWidth = 125;
            this.colValue.Name = "colValue";
            this.colValue.Visible = true;
            this.colValue.VisibleIndex = 1;
            this.colValue.Width = 125;
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(0, 245);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(65, 60);
            this.btnPgUp.TabIndex = 2;
            this.btnPgUp.Tag = "";
            this.btnPgUp.Click += new System.EventHandler(this.OnBtnPgUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(73, 245);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 60);
            this.btnUp.TabIndex = 3;
            this.btnUp.Tag = "";
            this.btnUp.Click += new System.EventHandler(this.OnBtnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(335, 245);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 60);
            this.btnDown.TabIndex = 4;
            this.btnDown.Tag = "";
            this.btnDown.Click += new System.EventHandler(this.OnBtnDown_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(408, 245);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(65, 60);
            this.btnPgDown.TabIndex = 5;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Click += new System.EventHandler(this.OnBtnPgDown_Click);
            // 
            // tblKitComponents
            // 
            this.tblKitComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblKitComponents.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblKitComponents.ColumnCount = 5;
            this.tblKitComponents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblKitComponents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblKitComponents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKitComponents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblKitComponents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblKitComponents.Controls.Add(this.lblKitComponents, 0, 0);
            this.tblKitComponents.Controls.Add(this.gridKitComponents, 0, 1);
            this.tblKitComponents.Controls.Add(this.btnPgUpKitComponents, 0, 2);
            this.tblKitComponents.Controls.Add(this.btnUpKitComponents, 1, 2);
            this.tblKitComponents.Controls.Add(this.btnSubstitute, 2, 2);
            this.tblKitComponents.Controls.Add(this.btnDownKitComponents, 3, 2);
            this.tblKitComponents.Controls.Add(this.btnPgDownKitComponents, 4, 2);
            this.tblKitComponents.Location = new System.Drawing.Point(0, 305);
            this.tblKitComponents.Margin = new System.Windows.Forms.Padding(0);
            this.tblKitComponents.Name = "tblKitComponents";
            this.tblKitComponents.RowCount = 3;
            this.tblKitComponents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblKitComponents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKitComponents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblKitComponents.Size = new System.Drawing.Size(473, 356);
            this.tblKitComponents.TabIndex = 0;
            this.tblKitComponents.Visible = false;
            // 
            // lblKitComponents
            // 
            this.lblKitComponents.AutoSize = true;
            this.tblKitComponents.SetColumnSpan(this.lblKitComponents, 5);
            this.lblKitComponents.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblKitComponents.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblKitComponents.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblKitComponents.Location = new System.Drawing.Point(0, 10);
            this.lblKitComponents.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblKitComponents.Name = "lblKitComponents";
            this.lblKitComponents.Size = new System.Drawing.Size(136, 20);
            this.lblKitComponents.TabIndex = 0;
            this.lblKitComponents.Text = "Included products:";
            this.lblKitComponents.Visible = false;
            // 
            // gridKitComponents
            // 
            this.gridKitComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridKitComponents.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tblKitComponents.SetColumnSpan(this.gridKitComponents, 5);
            this.gridKitComponents.Location = new System.Drawing.Point(0, 30);
            this.gridKitComponents.MainView = this.gridKitComponentsView;
            this.gridKitComponents.Margin = new System.Windows.Forms.Padding(0);
            this.gridKitComponents.Name = "gridKitComponents";
            this.gridKitComponents.Size = new System.Drawing.Size(473, 262);
            this.gridKitComponents.TabIndex = 1;
            this.gridKitComponents.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridKitComponentsView});
            this.gridKitComponents.Visible = false;
            // 
            // gridKitComponentsView
            // 
            this.gridKitComponentsView.Appearance.FooterPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridKitComponentsView.Appearance.FooterPanel.Options.UseFont = true;
            this.gridKitComponentsView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.gridKitComponentsView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridKitComponentsView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridKitComponentsView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridKitComponentsView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridKitComponentsView.Appearance.Row.Options.UseFont = true;
            this.gridKitComponentsView.ColumnPanelRowHeight = 40;
            this.gridKitComponentsView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colKitComponentNumber,
            this.colKitComponentName,
            this.colKitComponentQTY,
            this.colKitComponentUOM});
            this.gridKitComponentsView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridKitComponentsView.GridControl = this.gridKitComponents;
            this.gridKitComponentsView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridKitComponentsView.Name = "gridKitComponentsView";
            this.gridKitComponentsView.OptionsBehavior.Editable = false;
            this.gridKitComponentsView.OptionsCustomization.AllowColumnMoving = false;
            this.gridKitComponentsView.OptionsCustomization.AllowColumnResizing = false;
            this.gridKitComponentsView.OptionsCustomization.AllowFilter = false;
            this.gridKitComponentsView.OptionsCustomization.AllowGroup = false;
            this.gridKitComponentsView.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridKitComponentsView.OptionsCustomization.AllowSort = false;
            this.gridKitComponentsView.OptionsMenu.EnableColumnMenu = false;
            this.gridKitComponentsView.OptionsMenu.EnableFooterMenu = false;
            this.gridKitComponentsView.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridKitComponentsView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridKitComponentsView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridKitComponentsView.OptionsView.ShowGroupPanel = false;
            this.gridKitComponentsView.OptionsView.ShowIndicator = false;
            this.gridKitComponentsView.RowHeight = 40;
            this.gridKitComponentsView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridKitComponentsView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridKitComponentsView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridKitComponentsView_FocusedRowChanged);
            // 
            // colKitComponentNumber
            // 
            this.colKitComponentNumber.AppearanceCell.Options.UseTextOptions = true;
            this.colKitComponentNumber.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colKitComponentNumber.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colKitComponentNumber.Caption = "Number";
            this.colKitComponentNumber.FieldName = "Number";
            this.colKitComponentNumber.MinWidth = 75;
            this.colKitComponentNumber.Name = "colKitComponentNumber";
            this.colKitComponentNumber.Visible = true;
            this.colKitComponentNumber.VisibleIndex = 0;
            this.colKitComponentNumber.Width = 118;
            // 
            // colKitComponentName
            // 
            this.colKitComponentName.AppearanceCell.Options.UseTextOptions = true;
            this.colKitComponentName.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colKitComponentName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colKitComponentName.Caption = "Name";
            this.colKitComponentName.FieldName = "Name";
            this.colKitComponentName.MinWidth = 125;
            this.colKitComponentName.Name = "colKitComponentName";
            this.colKitComponentName.Visible = true;
            this.colKitComponentName.VisibleIndex = 1;
            this.colKitComponentName.Width = 198;
            // 
            // colKitComponentQTY
            // 
            this.colKitComponentQTY.AppearanceCell.Options.UseTextOptions = true;
            this.colKitComponentQTY.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colKitComponentQTY.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colKitComponentQTY.Caption = "QTY";
            this.colKitComponentQTY.DisplayFormat.FormatString = "n2";
            this.colKitComponentQTY.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colKitComponentQTY.FieldName = "QTY";
            this.colKitComponentQTY.Name = "colKitComponentQTY";
            this.colKitComponentQTY.Visible = true;
            this.colKitComponentQTY.VisibleIndex = 2;
            this.colKitComponentQTY.Width = 50;
            // 
            // colKitComponentUOM
            // 
            this.colKitComponentUOM.Caption = "Unit";
            this.colKitComponentUOM.FieldName = "UnitOfMeasure";
            this.colKitComponentUOM.Name = "colKitComponentUOM";
            this.colKitComponentUOM.Visible = true;
            this.colKitComponentUOM.VisibleIndex = 3;
            this.colKitComponentUOM.Width = 50;
            // 
            // btnPgUpKitComponents
            // 
            this.btnPgUpKitComponents.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPgUpKitComponents.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUpKitComponents.Appearance.Options.UseFont = true;
            this.btnPgUpKitComponents.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUpKitComponents.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUpKitComponents.Location = new System.Drawing.Point(0, 296);
            this.btnPgUpKitComponents.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            this.btnPgUpKitComponents.Name = "btnPgUpKitComponents";
            this.btnPgUpKitComponents.Size = new System.Drawing.Size(65, 60);
            this.btnPgUpKitComponents.TabIndex = 2;
            this.btnPgUpKitComponents.Tag = "";
            this.btnPgUpKitComponents.Visible = false;
            this.btnPgUpKitComponents.Click += new System.EventHandler(this.btnPgUpKitComponents_Click);
            // 
            // btnUpKitComponents
            // 
            this.btnUpKitComponents.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnUpKitComponents.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUpKitComponents.Appearance.Options.UseFont = true;
            this.btnUpKitComponents.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUpKitComponents.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUpKitComponents.Location = new System.Drawing.Point(73, 296);
            this.btnUpKitComponents.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.btnUpKitComponents.Name = "btnUpKitComponents";
            this.btnUpKitComponents.Size = new System.Drawing.Size(65, 60);
            this.btnUpKitComponents.TabIndex = 3;
            this.btnUpKitComponents.Tag = "";
            this.btnUpKitComponents.Visible = false;
            this.btnUpKitComponents.Click += new System.EventHandler(this.btnUpKitComponents_Click);
            // 
            // btnSubstitute
            // 
            this.btnSubstitute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubstitute.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSubstitute.Appearance.Options.UseFont = true;
            this.btnSubstitute.Enabled = false;
            this.btnSubstitute.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSubstitute.Location = new System.Drawing.Point(146, 296);
            this.btnSubstitute.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.btnSubstitute.Name = "btnSubstitute";
            this.btnSubstitute.Size = new System.Drawing.Size(181, 59);
            this.btnSubstitute.TabIndex = 4;
            this.btnSubstitute.Text = "Change product";
            this.btnSubstitute.Visible = false;
            this.btnSubstitute.Click += new System.EventHandler(this.btnSubstitute_Click);
            // 
            // btnDownKitComponents
            // 
            this.btnDownKitComponents.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDownKitComponents.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDownKitComponents.Appearance.Options.UseFont = true;
            this.btnDownKitComponents.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDownKitComponents.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDownKitComponents.Location = new System.Drawing.Point(335, 296);
            this.btnDownKitComponents.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.btnDownKitComponents.Name = "btnDownKitComponents";
            this.btnDownKitComponents.Size = new System.Drawing.Size(65, 60);
            this.btnDownKitComponents.TabIndex = 5;
            this.btnDownKitComponents.Tag = "";
            this.btnDownKitComponents.Visible = false;
            this.btnDownKitComponents.Click += new System.EventHandler(this.btnDownKitComponents_Click);
            // 
            // btnPgDownKitComponents
            // 
            this.btnPgDownKitComponents.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPgDownKitComponents.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDownKitComponents.Appearance.Options.UseFont = true;
            this.btnPgDownKitComponents.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDownKitComponents.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDownKitComponents.Location = new System.Drawing.Point(408, 296);
            this.btnPgDownKitComponents.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.btnPgDownKitComponents.Name = "btnPgDownKitComponents";
            this.btnPgDownKitComponents.Size = new System.Drawing.Size(65, 60);
            this.btnPgDownKitComponents.TabIndex = 6;
            this.btnPgDownKitComponents.Tag = "";
            this.btnPgDownKitComponents.Visible = false;
            this.btnPgDownKitComponents.Click += new System.EventHandler(this.btnPgDownKitComponents_Click);
            // 
            // tlpButtons
            // 
            this.tlpButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tlpButtons.AutoSize = true;
            this.tlpButtons.ColumnCount = 3;
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButtons.Controls.Add(this.btnInventoryLookup, 1, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tlpButtons.Controls.Add(this.btnAddToSale, 0, 0);
            this.tlpButtons.Location = new System.Drawing.Point(0, 597);
            this.tlpButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.RowCount = 1;
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButtons.Size = new System.Drawing.Size(473, 64);
            this.tlpButtons.TabIndex = 7;
            // 
            // btnInventoryLookup
            // 
            this.btnInventoryLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInventoryLookup.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnInventoryLookup.Appearance.Options.UseFont = true;
            this.btnInventoryLookup.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnInventoryLookup.Location = new System.Drawing.Point(144, 4);
            this.btnInventoryLookup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.btnInventoryLookup.Name = "btnInventoryLookup";
            this.btnInventoryLookup.Size = new System.Drawing.Size(185, 60);
            this.btnInventoryLookup.TabIndex = 1;
            this.btnInventoryLookup.Tag = "";
            this.btnInventoryLookup.Text = "Check inventory";
            this.btnInventoryLookup.Click += new System.EventHandler(this.OnBtnInventoryLookup_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCancel.Location = new System.Drawing.Point(337, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(136, 60);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Tag = "";
            this.btnCancel.Text = "Cancel";
            // 
            // btnAddToSale
            // 
            this.btnAddToSale.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddToSale.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddToSale.Appearance.Options.UseFont = true;
            this.btnAddToSale.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAddToSale.Location = new System.Drawing.Point(0, 4);
            this.btnAddToSale.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            this.btnAddToSale.Name = "btnAddToSale";
            this.btnAddToSale.Padding = new System.Windows.Forms.Padding(0, 4, 4, 4);
            this.btnAddToSale.Size = new System.Drawing.Size(136, 60);
            this.btnAddToSale.TabIndex = 0;
            this.btnAddToSale.Text = "Add to sale";
            this.btnAddToSale.Click += new System.EventHandler(this.OnBtnAddToSale_Click);
            // 
            // ProductDetailsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tlpContent);
            this.HelpButton = false;
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "ProductDetailsForm";
            this.Text = "ProductDetailsForm";
            this.Controls.SetChildIndex(this.tlpContent, 0);
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.tlpProductInformation.ResumeLayout(false);
            this.tlpProductInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.tblProductImageAndBasicInfo.ResumeLayout(false);
            this.tblProductImageAndBasicInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).EndInit();
            this.tblAttributeAndComponents.ResumeLayout(false);
            this.tblAttributeAndComponents.PerformLayout();
            this.tblProductAttributes.ResumeLayout(false);
            this.tblProductAttributes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductAttributes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productAttributesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.tblKitComponents.ResumeLayout(false);
            this.tblKitComponents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridKitComponentsView)).EndInit();
            this.tlpButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private System.Windows.Forms.TableLayoutPanel tlpProductInformation;
        private System.Windows.Forms.Label lblBarCode;
        private System.Windows.Forms.Label lblBarCodeValue;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblProductAttributes;
        private System.Windows.Forms.Label lblPriceValue;
        private System.Windows.Forms.TableLayoutPanel tblProductAttributes;
        private System.Windows.Forms.PictureBox pbProductImage;
        private System.Windows.Forms.BindingSource bindingSource;
        private DevExpress.XtraGrid.GridControl gridProductAttributes;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colValue;
        private DevExpress.XtraGrid.GridControl gridKitComponents;
        private DevExpress.XtraGrid.Views.Grid.GridView gridKitComponentsView;
        private DevExpress.XtraGrid.Columns.GridColumn colKitComponentNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colKitComponentName;
        private DevExpress.XtraGrid.Columns.GridColumn colKitComponentQTY;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSubstitute;
        private DevExpress.XtraGrid.Columns.GridColumn colKitComponentUOM;
        private System.Windows.Forms.TableLayoutPanel tblProductImageAndBasicInfo;
        private System.Windows.Forms.Label lblKitComponents;
        private System.Windows.Forms.TableLayoutPanel tblKitComponents;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUpKitComponents;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUpKitComponents;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDownKitComponents;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDownKitComponents;
        private System.Windows.Forms.Label lblModelNumber;
        private System.Windows.Forms.Label lblModelNumberValue;
        private System.Windows.Forms.TableLayoutPanel tblAttributeAndComponents;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnInventoryLookup;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnAddToSale;
        private System.Windows.Forms.BindingSource productAttributesBindingSource;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.Label lblSearchNameValue;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblCategoryValue;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblDescriptionValue;
    }
}