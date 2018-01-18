namespace SSI.ContractManagement.Web.Report
{
    partial class ContractModeling
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule2 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule3 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule4 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.TableGroup tableGroup7 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup8 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup9 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup10 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup11 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup12 = new Telerik.Reporting.TableGroup();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractModeling));
            Telerik.Reporting.TableGroup tableGroup13 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup14 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup15 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup16 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.tblEmployeeData = new Telerik.Reporting.Table();
            this.serviceTypeData = new Telerik.Reporting.TextBox();
            this.carveOutData = new Telerik.Reporting.TextBox();
            this.claimCriteriaData = new Telerik.Reporting.TextBox();
            this.paymentCriteriaData = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.table2 = new Telerik.Reporting.Table();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.panel1 = new Telerik.Reporting.Panel();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.facilityName = new Telerik.Reporting.TextBox();
            this.reportHeaderName = new Telerik.Reporting.TextBox();
            this.reportType = new Telerik.Reporting.TextBox();
            this.reportDate = new Telerik.Reporting.TextBox();
            this.userName = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.detail = new Telerik.Reporting.DetailSection();
            this.tblEmployeesData = new Telerik.Reporting.Table();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Pixel(40D);
            this.groupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.tblEmployeeData});
            this.groupFooterSection.KeepTogether = false;
            this.groupFooterSection.Name = "groupFooterSection";
            this.groupFooterSection.PageBreak = Telerik.Reporting.PageBreak.After;
            // 
            // tblEmployeeData
            // 
            this.tblEmployeeData.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "= ReportItem.DataObject"));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Pixel(179.99998474121094D)));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Pixel(120.00001525878906D)));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Pixel(280D)));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Pixel(280D)));
            this.tblEmployeeData.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Pixel(20D)));
            this.tblEmployeeData.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Pixel(20D)));
            this.tblEmployeeData.Body.SetCellContent(1, 0, this.serviceTypeData);
            this.tblEmployeeData.Body.SetCellContent(1, 1, this.carveOutData);
            this.tblEmployeeData.Body.SetCellContent(1, 2, this.claimCriteriaData);
            this.tblEmployeeData.Body.SetCellContent(1, 3, this.paymentCriteriaData);
            this.tblEmployeeData.Body.SetCellContent(0, 0, this.textBox3);
            this.tblEmployeeData.Body.SetCellContent(0, 1, this.textBox5);
            this.tblEmployeeData.Body.SetCellContent(0, 2, this.textBox6);
            this.tblEmployeeData.Body.SetCellContent(0, 3, this.textBox7);
            tableGroup4.Name = "Group1";
            this.tblEmployeeData.ColumnGroups.Add(tableGroup1);
            this.tblEmployeeData.ColumnGroups.Add(tableGroup2);
            this.tblEmployeeData.ColumnGroups.Add(tableGroup3);
            this.tblEmployeeData.ColumnGroups.Add(tableGroup4);
            this.tblEmployeeData.Filters.Add(new Telerik.Reporting.Filter("=Fields.IsContractSpecific", Telerik.Reporting.FilterOperator.NotEqual, "=1"));
            this.tblEmployeeData.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.serviceTypeData,
            this.carveOutData,
            this.claimCriteriaData,
            this.paymentCriteriaData,
            this.textBox3,
            this.textBox5,
            this.textBox6,
            this.textBox7});
            this.tblEmployeeData.KeepTogether = false;
            this.tblEmployeeData.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(0D), Telerik.Reporting.Drawing.Unit.Pixel(0D));
            this.tblEmployeeData.Name = "tblEmployeeData";
            tableGroup5.Name = "group4";
            tableGroup6.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup6.Name = "DetailGroup";
            this.tblEmployeeData.RowGroups.Add(tableGroup5);
            this.tblEmployeeData.RowGroups.Add(tableGroup6);
            this.tblEmployeeData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(860D), Telerik.Reporting.Drawing.Unit.Pixel(40D));
            this.tblEmployeeData.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.tblEmployeeData.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.tblEmployeeData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.tblEmployeeData.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.tblEmployeeData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            // 
            // serviceTypeData
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=RowNumber(\"tblEmployeeData\") % 2", Telerik.Reporting.FilterOperator.Equal, "1"));
            formattingRule1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.serviceTypeData.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.serviceTypeData.Name = "serviceTypeData";
            this.serviceTypeData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8749998807907105D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333283662796D));
            this.serviceTypeData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.serviceTypeData.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.serviceTypeData.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.serviceTypeData.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.serviceTypeData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.serviceTypeData.Value = "=Fields.ServiceType";
            // 
            // carveOutData
            // 
            formattingRule2.Filters.Add(new Telerik.Reporting.Filter("=RowNumber(\"tblEmployeeData\") % 2", Telerik.Reporting.FilterOperator.Equal, "1"));
            formattingRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.carveOutData.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule2});
            this.carveOutData.Name = "carveOutData";
            this.carveOutData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2500002384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333283662796D));
            this.carveOutData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.carveOutData.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.carveOutData.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.carveOutData.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.carveOutData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.carveOutData.Value = "= IIf(IsCarveOut, \"Yes\", \"No\")";
            // 
            // claimCriteriaData
            // 
            formattingRule3.Filters.Add(new Telerik.Reporting.Filter("=RowNumber(\"tblEmployeeData\") % 2", Telerik.Reporting.FilterOperator.Equal, "1"));
            formattingRule3.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.claimCriteriaData.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule3});
            this.claimCriteriaData.Name = "claimCriteriaData";
            this.claimCriteriaData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.9166667461395264D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333283662796D));
            this.claimCriteriaData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.claimCriteriaData.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.claimCriteriaData.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.claimCriteriaData.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            this.claimCriteriaData.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.claimCriteriaData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.claimCriteriaData.Value = "=Fields.ClaimTools";
            // 
            // paymentCriteriaData
            // 
            formattingRule4.Filters.Add(new Telerik.Reporting.Filter("=RowNumber(\"tblEmployeeData\") % 2", Telerik.Reporting.FilterOperator.Equal, "1"));
            formattingRule4.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.paymentCriteriaData.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule4});
            this.paymentCriteriaData.Name = "paymentCriteriaData";
            this.paymentCriteriaData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.9166667461395264D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333283662796D));
            this.paymentCriteriaData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.paymentCriteriaData.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.paymentCriteriaData.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.paymentCriteriaData.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.paymentCriteriaData.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.paymentCriteriaData.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.paymentCriteriaData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.paymentCriteriaData.StyleName = "";
            this.paymentCriteriaData.Value = "=Fields.PaymentTool";
            // 
            // textBox3
            // 
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8749998807907105D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333432674408D));
            this.textBox3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox3.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.StyleName = "";
            this.textBox3.Value = "Service Type";
            // 
            // textBox5
            // 
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2500002384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333432674408D));
            this.textBox5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox5.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox5.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.StyleName = "";
            this.textBox5.Value = "Carve Out";
            // 
            // textBox6
            // 
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.9166667461395264D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333432674408D));
            this.textBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox6.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox6.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.textBox6.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            this.textBox6.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.StyleName = "";
            this.textBox6.Value = "Claim Criteria";
            // 
            // textBox7
            // 
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.9166667461395264D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333432674408D));
            this.textBox7.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox7.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox7.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.textBox7.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.textBox7.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.StyleName = "";
            this.textBox7.Value = "Payment Criteria";
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Pixel(40D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.table2});
            this.groupHeaderSection.Name = "groupHeaderSection";
            // 
            // table2
            // 
            this.table2.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "= ReportItem.DataObject"));
            this.table2.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Pixel(640D)));
            this.table2.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Pixel(120D)));
            this.table2.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Pixel(99.9999771118164D)));
            this.table2.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Pixel(20D)));
            this.table2.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Pixel(20D)));
            this.table2.Body.SetCellContent(0, 0, this.textBox1);
            this.table2.Body.SetCellContent(0, 1, this.textBox2);
            this.table2.Body.SetCellContent(0, 2, this.textBox4);
            this.table2.Body.SetCellContent(1, 0, this.textBox27, 1, 3);
            tableGroup7.Name = "tableGroup";
            tableGroup8.Name = "group2";
            tableGroup9.Name = "group3";
            this.table2.ColumnGroups.Add(tableGroup7);
            this.table2.ColumnGroups.Add(tableGroup8);
            this.table2.ColumnGroups.Add(tableGroup9);
            this.table2.Filters.Add(new Telerik.Reporting.Filter("=Fields.IsContractSpecific", Telerik.Reporting.FilterOperator.Equal, "=1"));
            this.table2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox2,
            this.textBox4,
            this.textBox27});
            this.table2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(0D), Telerik.Reporting.Drawing.Unit.Pixel(0D));
            this.table2.Name = "table2";
            tableGroup10.Name = "group1";
            tableGroup12.Name = "group5";
            tableGroup11.ChildGroups.Add(tableGroup12);
            tableGroup11.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup11.Name = "detailTableGroup";
            this.table2.RowGroups.Add(tableGroup10);
            this.table2.RowGroups.Add(tableGroup11);
            this.table2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(860D), Telerik.Reporting.Drawing.Unit.Pixel(40D));
            this.table2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            // 
            // textBox1
            // 
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.6666665077209473D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333432674408D));
            this.textBox1.Style.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.textBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.StyleName = "";
            this.textBox1.Value = "=Fields.ContractName + \" (\" + Fields.ModelName + \")\"";
            // 
            // textBox2
            // 
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2500001192092896D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.textBox2.Style.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.textBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.StyleName = "";
            this.textBox2.Value = "=\"Effective\" + \" \" + Fields.StartDate.ToShortDateString()";
            // 
            // textBox4
            // 
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0416663885116577D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.textBox4.Style.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.textBox4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox4.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.StyleName = "";
            this.textBox4.Value = "=\"thru\" + \" \" + Fields.EndDate.ToShortDateString()";
            // 
            // textBox27
            // 
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.9583330154418945D), Telerik.Reporting.Drawing.Unit.Inch(0.2083333432674408D));
            this.textBox27.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox27.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.textBox27.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox27.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox27.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox27.StyleName = "";
            this.textBox27.Value = resources.GetString("textBox27.Value");
            // 
            // panel1
            // 
            this.panel1.KeepTogether = true;
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.7916250228881836D), Telerik.Reporting.Drawing.Unit.Inch(0.19992107152938843D));
            this.panel1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.panel1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.panel1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.panel1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.panel1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.panel1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Pixel(100D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox1,
            this.facilityName,
            this.reportHeaderName,
            this.reportType,
            this.reportDate,
            this.userName,
            this.shape1});
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.PrintOnFirstPage = true;
            this.pageHeader.PrintOnLastPage = true;
            this.pageHeader.ItemDataBound += new System.EventHandler(this.PageHeader_ItemDataBound);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Pixel(0D));
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(80D), Telerik.Reporting.Drawing.Unit.Pixel(60D));
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // facilityName
            // 
            this.facilityName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Pixel(60D));
            this.facilityName.Name = "facilityName";
            this.facilityName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(280D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.facilityName.Style.Font.Bold = true;
            this.facilityName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.facilityName.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.facilityName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.facilityName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.facilityName.Value = "";
            // 
            // reportHeaderName
            // 
            this.reportHeaderName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(280D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.reportHeaderName.Name = "reportHeaderName";
            this.reportHeaderName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(380D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.reportHeaderName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15D);
            this.reportHeaderName.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.reportHeaderName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.reportHeaderName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.reportHeaderName.Value = "Contract Management Modeling Report";
            // 
            // reportType
            // 
            this.reportType.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(360D), Telerik.Reporting.Drawing.Unit.Pixel(60D));
            this.reportType.Name = "reportType";
            this.reportType.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(220D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.reportType.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.reportType.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.reportType.Value = "";
            // 
            // reportDate
            // 
            this.reportDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(660D), Telerik.Reporting.Drawing.Unit.Pixel(0D));
            this.reportDate.Name = "reportDate";
            this.reportDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(200D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.reportDate.Style.Font.Bold = true;
            this.reportDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.reportDate.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.reportDate.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.reportDate.Value = "";
            // 
            // userName
            // 
            this.userName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(604.800048828125D), Telerik.Reporting.Drawing.Unit.Pixel(60D));
            this.userName.Name = "userName";
            this.userName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(255.199951171875D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.userName.Style.Font.Bold = true;
            this.userName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.userName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.userName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.userName.Value = "";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(4D), Telerik.Reporting.Drawing.Unit.Pixel(80D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.8999605178833D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            this.shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(2D);
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.detail.KeepTogether = false;
            this.detail.Name = "detail";
            this.detail.PageBreak = Telerik.Reporting.PageBreak.None;
            // 
            // tblEmployeesData
            // 
            this.tblEmployeesData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(2D)));
            this.tblEmployeesData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(2D)));
            this.tblEmployeesData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(2D)));
            this.tblEmployeesData.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.29999998211860657D)));
            this.tblEmployeesData.ColumnGroups.Add(tableGroup13);
            this.tblEmployeesData.ColumnGroups.Add(tableGroup14);
            this.tblEmployeesData.ColumnGroups.Add(tableGroup15);
            this.tblEmployeesData.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D), Telerik.Reporting.Drawing.Unit.Inch(0.20000028610229492D));
            this.tblEmployeesData.Name = "tblEmployeesData";
            tableGroup16.Groupings.Add(new Telerik.Reporting.Grouping(""));
            tableGroup16.Name = "DetailGroup";
            this.tblEmployeesData.RowGroups.Add(tableGroup16);
            this.tblEmployeesData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6D), Telerik.Reporting.Drawing.Unit.Inch(0.29999998211860657D));
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Pixel(20D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(760D), Telerik.Reporting.Drawing.Unit.Pixel(0D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(100D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.pageInfoTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // ContractModeling
            // 
            this.DocumentName = "";
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ContractId"));
            group1.Name = "group";
            group1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ContractName", Telerik.Reporting.SortDirection.Asc));
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.pageHeader,
            this.detail,
            this.pageFooterSection1});
            this.Name = "Test";
            this.PageNumberingStyle = Telerik.Reporting.PageNumberingStyle.Continue;
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.UnitOfMeasure = Telerik.Reporting.Drawing.UnitType.Inch;
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.9999608993530273D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.Table tblEmployeesData;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox facilityName;
        private Telerik.Reporting.TextBox reportHeaderName;
        private Telerik.Reporting.TextBox reportType;
        private Telerik.Reporting.TextBox reportDate;
        private Telerik.Reporting.TextBox userName;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.Table table2;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.Table tblEmployeeData;
        private Telerik.Reporting.TextBox serviceTypeData;
        private Telerik.Reporting.TextBox carveOutData;
        private Telerik.Reporting.TextBox claimCriteriaData;
        private Telerik.Reporting.TextBox paymentCriteriaData;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
    }
}