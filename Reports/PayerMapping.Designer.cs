namespace SSI.ContractManagement.Web.Report
{
    partial class PayerMapping
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayerMapping));
            Telerik.Reporting.TableGroup tableGroup7 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup8 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup9 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup10 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.groupFooterSection2 = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection2 = new Telerik.Reporting.GroupHeaderSection();
            this.tblEmployeeData = new Telerik.Reporting.Table();
            this.serviceTypeData = new Telerik.Reporting.TextBox();
            this.carveOutData = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.facilityName = new Telerik.Reporting.TextBox();
            this.reportHeaderName = new Telerik.Reporting.TextBox();
            this.reportType = new Telerik.Reporting.TextBox();
            this.reportDate = new Telerik.Reporting.TextBox();
            this.userName = new Telerik.Reporting.TextBox();
            this.panel1 = new Telerik.Reporting.Panel();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.tblEmployeesData = new Telerik.Reporting.Table();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D);
            this.groupFooterSection.Name = "groupFooterSection";
            this.groupFooterSection.PageBreak = Telerik.Reporting.PageBreak.None;
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.15007883310317993D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox12});
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.1999211311340332D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox2.Value = "= Fields.ContractName";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7000001072883606D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox12.Value = "= IIf(Fields.Priority = 2, \" \", IIf(Fields.IsActive, \"Active\", \"Inactive\")) ";
            // 
            // groupFooterSection2
            // 
            this.groupFooterSection2.Height = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.groupFooterSection2.Name = "groupFooterSection2";
            // 
            // groupHeaderSection2
            // 
            this.groupHeaderSection2.Height = Telerik.Reporting.Drawing.Unit.Inch(0.16871073842048645D);
            this.groupHeaderSection2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.tblEmployeeData});
            this.groupHeaderSection2.KeepTogether = false;
            this.groupHeaderSection2.Name = "groupHeaderSection2";
            // 
            // tblEmployeeData
            // 
            this.tblEmployeeData.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "= ReportItem.DataObject"));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.8109500408172607D)));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.82291728258132935D)));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0416678190231323D)));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0625007152557373D)));
            this.tblEmployeeData.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.1469190120697022D)));
            this.tblEmployeeData.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.16871075332164764D)));
            this.tblEmployeeData.Body.SetCellContent(0, 0, this.serviceTypeData);
            this.tblEmployeeData.Body.SetCellContent(0, 4, this.carveOutData);
            this.tblEmployeeData.Body.SetCellContent(0, 3, this.textBox6);
            this.tblEmployeeData.Body.SetCellContent(0, 2, this.textBox7);
            this.tblEmployeeData.Body.SetCellContent(0, 1, this.textBox11);
            tableGroup1.Name = "group4";
            tableGroup2.Name = "group2";
            tableGroup3.Name = "group";
            this.tblEmployeeData.ColumnGroups.Add(tableGroup1);
            this.tblEmployeeData.ColumnGroups.Add(tableGroup2);
            this.tblEmployeeData.ColumnGroups.Add(tableGroup3);
            this.tblEmployeeData.ColumnGroups.Add(tableGroup4);
            this.tblEmployeeData.ColumnGroups.Add(tableGroup5);
            this.tblEmployeeData.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.serviceTypeData,
            this.carveOutData,
            this.textBox6,
            this.textBox7,
            this.textBox11});
            this.tblEmployeeData.KeepTogether = false;
            this.tblEmployeeData.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.tblEmployeeData.Name = "tblEmployeeData";
            tableGroup6.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup6.Name = "DetailGroup";
            this.tblEmployeeData.RowGroups.Add(tableGroup6);
            this.tblEmployeeData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.8849544525146484D), Telerik.Reporting.Drawing.Unit.Inch(0.16871075332164764D));
            this.tblEmployeeData.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.tblEmployeeData.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.tblEmployeeData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.tblEmployeeData.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.tblEmployeeData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            // 
            // serviceTypeData
            // 
            this.serviceTypeData.Name = "serviceTypeData";
            this.serviceTypeData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8109493255615234D), Telerik.Reporting.Drawing.Unit.Inch(0.16871075332164764D));
            this.serviceTypeData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.serviceTypeData.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.serviceTypeData.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.serviceTypeData.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.serviceTypeData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.serviceTypeData.Value = "=Fields.PayerName";
            // 
            // carveOutData
            // 
            this.carveOutData.Name = "carveOutData";
            this.carveOutData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1469191312789917D), Telerik.Reporting.Drawing.Unit.Inch(0.16871075332164764D));
            this.carveOutData.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.carveOutData.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.carveOutData.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.carveOutData.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.carveOutData.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.carveOutData.Value = "=IIf(Fields.BilledDate = \"01/01/1900\", \" \", IIf(Fields.BilledDate = \"1/1/1900\", \"" +
    " \", Fields.BilledDate))";
            // 
            // textBox6
            // 
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0625009536743164D), Telerik.Reporting.Drawing.Unit.Inch(0.16871075332164764D));
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox6.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox6.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.StyleName = "";
            this.textBox6.Value = "=IIf(Fields.StatementThrough = \"01/01/1900\", \" \", IIf(Fields.StatementThrough = \"" +
    "1/1/1900\", \" \", Fields.StatementThrough))";
            // 
            // textBox7
            // 
            this.textBox7.Format = "{0:C2}";
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0416680574417114D), Telerik.Reporting.Drawing.Unit.Inch(0.16871075332164764D));
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox7.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox7.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.StyleName = "";
            this.textBox7.Value = "=Fields.TotalClaimCharges";
            // 
            // textBox11
            // 
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.82291698455810547D), Telerik.Reporting.Drawing.Unit.Inch(0.16871075332164764D));
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox11.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D);
            this.textBox11.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.StyleName = "";
            this.textBox11.Value = "= Fields.ClaimCount";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9383769035339355E-05D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.9998817443847656D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            this.shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(2D);
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(1.5D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox1,
            this.facilityName,
            this.reportHeaderName,
            this.reportType,
            this.reportDate,
            this.userName,
            this.shape1,
            this.panel1});
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.PrintOnFirstPage = true;
            this.pageHeader.PrintOnLastPage = true;
            this.pageHeader.ItemDataBound += new System.EventHandler(this.PageHeader_ItemDataBound);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.099999986588954926D));
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.85208523273468018D), Telerik.Reporting.Drawing.Unit.Inch(0.64025849103927612D));
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // facilityName
            // 
            this.facilityName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.74992114305496216D));
            this.facilityName.Name = "facilityName";
            this.facilityName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8000397682189941D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.facilityName.Style.Font.Bold = true;
            this.facilityName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.facilityName.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D);
            this.facilityName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.facilityName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.facilityName.Value = "";
            // 
            // reportHeaderName
            // 
            this.reportHeaderName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.reportHeaderName.Name = "reportHeaderName";
            this.reportHeaderName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.0895061492919922D), Telerik.Reporting.Drawing.Unit.Inch(0.2597624659538269D));
            this.reportHeaderName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15D);
            this.reportHeaderName.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.029999999329447746D);
            this.reportHeaderName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.reportHeaderName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.reportHeaderName.Value = "Payer Mapping Report";
            // 
            // reportType
            // 
            this.reportType.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.7000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(0.60000008344650269D));
            this.reportType.Name = "reportType";
            this.reportType.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.39992102980613708D));
            this.reportType.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.reportType.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.reportType.Value = "";
            // 
            // reportDate
            // 
            this.reportDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7D), Telerik.Reporting.Drawing.Unit.Inch(0.099999986588954926D));
            this.reportDate.Name = "reportDate";
            this.reportDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.0000004768371582D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.reportDate.Style.Font.Bold = true;
            this.reportDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.reportDate.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.reportDate.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.reportDate.Value = "";
            // 
            // userName
            // 
            this.userName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.3000006675720215D), Telerik.Reporting.Drawing.Unit.Inch(0.70000004768371582D));
            this.userName.Name = "userName";
            this.userName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.29992121458053589D));
            this.userName.Style.Font.Bold = true;
            this.userName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.userName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.userName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.userName.Value = "";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox8,
            this.textBox9,
            this.textBox10,
            this.textBox3,
            this.textBox4,
            this.textBox5});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.9998817443847656D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.panel1.Style.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.panel1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.panel1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.99992120265960693D), Telerik.Reporting.Drawing.Unit.Inch(0.19996054470539093D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.Value = "Contracts";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.8999606370925903D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0999603271484375D), Telerik.Reporting.Drawing.Unit.Inch(0.19996054470539093D));
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "Payers";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.6999607086181641D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.89999967813491821D), Telerik.Reporting.Drawing.Unit.Inch(0.19996054470539093D));
            this.textBox9.Style.Font.Bold = true;
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox9.Value = "Total Charges";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.6999607086181641D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.85829418897628784D), Telerik.Reporting.Drawing.Unit.Inch(0.19996054470539093D));
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "Claim Count";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7999610900878906D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.85829418897628784D), Telerik.Reporting.Drawing.Unit.Inch(0.19996054470539093D));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "First Stmt Thru";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.899960994720459D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.89999967813491821D), Telerik.Reporting.Drawing.Unit.Inch(0.19996054470539093D));
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "First Billed Date";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.0999612808227539D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.70000016689300537D), Telerik.Reporting.Drawing.Unit.Inch(0.19996054470539093D));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "Status";
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
            this.tblEmployeesData.ColumnGroups.Add(tableGroup7);
            this.tblEmployeesData.ColumnGroups.Add(tableGroup8);
            this.tblEmployeesData.ColumnGroups.Add(tableGroup9);
            this.tblEmployeesData.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D), Telerik.Reporting.Drawing.Unit.Inch(0.20000028610229492D));
            this.tblEmployeesData.Name = "tblEmployeesData";
            tableGroup10.Groupings.Add(new Telerik.Reporting.Grouping(""));
            tableGroup10.Name = "DetailGroup";
            this.tblEmployeesData.RowGroups.Add(tableGroup10);
            this.tblEmployeesData.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6D), Telerik.Reporting.Drawing.Unit.Inch(0.29999998211860657D));
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.14162738621234894D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.8000006675720215D), Telerik.Reporting.Drawing.Unit.Inch(0.010416666977107525D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0999997854232788D), Telerik.Reporting.Drawing.Unit.Inch(0.13121071457862854D));
            this.pageInfoTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // PayerMapping
            // 
            this.DocumentName = "";
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ContractId"));
            group1.Name = "group1";
            group1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.Priority", Telerik.Reporting.SortDirection.Asc));
            group1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ContractName", Telerik.Reporting.SortDirection.Asc));
            group2.GroupFooter = this.groupFooterSection2;
            group2.GroupHeader = this.groupHeaderSection2;
            group2.Name = "group3";
            group2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.PayerName", Telerik.Reporting.SortDirection.Asc));
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.groupHeaderSection2,
            this.groupFooterSection2,
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
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox facilityName;
        private Telerik.Reporting.TextBox reportHeaderName;
        private Telerik.Reporting.TextBox reportType;
        private Telerik.Reporting.TextBox reportDate;
        private Telerik.Reporting.TextBox userName;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection2;
        private Telerik.Reporting.GroupFooterSection groupFooterSection2;
        private Telerik.Reporting.Table tblEmployeeData;
        private Telerik.Reporting.TextBox serviceTypeData;
        private Telerik.Reporting.TextBox carveOutData;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox12;
    }
}