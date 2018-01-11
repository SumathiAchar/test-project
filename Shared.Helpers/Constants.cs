using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Helpers
{
    /// <summary>
    /// Constants added to different payment types and Service codes
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Constants
    {
        public const string LastExpandedNodeIdSessionString = "LastExpandedNodeId";
        public const string LastRequestedNodeSessionString = "LastRequestedNode";
        public const string LastHighlightedNodeIdSessionString = "LastHighlightedNodeId";

        public const string InValidClaimStatusForAdjudication = "unbilled";
        public const string ClaimTypeInstitutionalContract = "hosp";
        public const string ClaimTypeProfessionalContract = "phys";
        public const string CarveOutNodeText = "Carve Outs";
        public const string CssPrimaryModel = "tv-primary-model";
        public const string CssSecondaryModel = "tv-secondary-model";
        public const string CssHighlightedPrimaryModel = "tv-primary-model highlight";
        public const string CssHighlightedSecondaryModel = "tv-secondary-model highlight";
        public const string CssFacilityNodeClass = "facility-node";
        public const string CssHighlight = "highlight";
        public const string CssContracts = "tv-contracts";
        public const string CssServicetype = "tv-servicetype";



        public const string Processing = "Processing...";
        public const string DateTimeExtendedFormat = "yyyyMMddHHmmssfff";
        public const string VarianceReportFileBaseName = "VarinaceReport";
        public const string AuditLogReportFileBaseName = "AuditLogReportReport";
        public const string ModelingReportFileBaseName = "ModelingReport";
        public const string ClaimVarianceReportFileBaseName = "ClaimVarianceReport";
        public const string ContractVarianceReportFileBaseName = "ContractVarianceReport";

        //If search criteria contains "-99|" means search criteria contains "adjudication request name"
        public const string AdjudicationRequestCriteria = "-99|";
        public const string ClaimAdjudicationReportFileBaseName = "AdjudicationReport";

        //if threshold is Constants.ReportThreshold, report can't be displayed as number of claims exceeds the requirement.
        public const int ReportThreshold = -1;
        public const int ReportLevelClaim = 1;
        public const int ReportLevelContract = 2;

        public const int DownloadFileTypeExcel = 2;
        public const string DateTime24HourFormat = "MM/dd/yyyy hh:mm tt";
        public const string CentralTimeZone = "Central Standard Time";
        public const string ModelingActiveReport = "Active Only";
        public const string ModelingInActiveReport = "InActive Only";
        public const string FacilityName = "Facility: {0}";
        // ReSharper disable once UnusedMember.Global
        public const string ReportDate = "Report Date: {0}";
        public const string LoggedInUser = "User: {0}";
        public const string DateTimeSimpleFormat = "yyyyMMddHHmmss";
        public const string ActiveReport = "Active";
        public const string InActiveReport = "InActive";
        public const string EmptyReportResult = "-2";
        public const int DefaultDateType = -1;
        public const int MinColumnForUploadedFile = 2;
        public const string DateTimeFormatWithSecond = "MM/dd/yyyy hh:mm:ss";
        public const string LargeCriteriaMessage =
            "Too many selection criteria for claimfields. Please correct it and try again.";

        public const string McareFileNameFormat = "MCare Lab Fee Schedule";
        public const string DuplicateTableNameMessage = "Table name already exists, Please give different name.";

        public const string ColumnRowCount = "The table cannot be uploaded as column count on row {0} does not match the header row.";
        public const string BlankValueInCell = "The table cannot be uploaded as a blank value is present in row {0}.";
        public const string InValidDrgCode = "The table cannot be uploaded as Invalid DRG code is present in row {0}.";
        public const string DuplicateValue = "The table cannot be uploaded as the code {0} is repeating.";
        public const string ColumnNumeric = "The table cannot be uploaded as non-numeric value is present in row {0}.";
        public const string InCorrectQuotes = "The table cannot be uploaded as there are incorrect number of quotes in row {0}.";
        public const string InValidCode = "The table cannot be uploaded as Invalid code is present in row {0}.";
        public const string RemoveCommaRegex = @",(?=[^""]*""(?:[^""]*""[^""]*"")*[^""]*$)";

        public const int InternalServerError = 500;
        public const int UnauthorizedUser = 401;
        public const int ResourceNotFound = 404;
        public const int SessionTimeout = 590;

        public const int AjaxStatusCode = 590; //used for response code

        public const string LogOffUrl = "/Common/Account/LogOff";
        public const string LogOffUrlWithSessionTimeOut = "/Common/LogOff?isSessionTimeOut=true";
        public const string LogOffUrlWithOutSessionTimeOut = "/Common/LogOff?isSessionTimeOut=false";
        public const string LogInUrl = "SSI.ContractManagement.Web";
        //max image size is in KB
        // ReSharper disable once UnusedMember.Global
        public const int LetterTemplateMaxImageSize = 256;

        public const string BillDate = "[[ Bill Date ]]";
        public const string BillType = "[[ Bill Type ]]";
        public const string ContractName = "[[ Contract Name ]]";
        public const string Drg = "[[ DRG ]]";
        public const string ExpectedAllowed = "[[ Expected Allowed ]]";
        public const string Ftn = "[[ FTN ]]";
        public const string PrimaryGroupNumber = "[[ Primary Group Number ]]";
        public const string SecondaryGroupNumber = "[[ Secondary Group Number ]]";
        public const string TertiaryGroupNumber = "[[ Tertiary Group Number ]]";
        public const string PrimaryMemberId = "[[ Primary Member ID ]]";
        public const string MedRecNumber = "[[ Med Rec Number ]]";
        public const string Npi = "[[ NPI ]]";
        public const string PatientAccountNumber = "[[ Patient Account Number ]]";
        public const string PatientDob = "[[ Patient DOB ]]";
        public const string PatientFirstName = "[[ Patient First Name ]]";
        public const string PatientLastName = "[[ Patient Last Name ]]";
        public const string PatientMiddleName = "[[ Patient Middle Name ]]";
        public const string PatientResponsibility = "[[ Patient Responsibility ]]";
        public const string PayerName = "[[ Payer Name ]]";
        public const string ProviderName = "[[ Provider Name ]]";
        public const string RemitCheckDate = "[[ Remit Check Date ]]";
        public const string RemitPayment = "[[ Remit Payment ]]";
        public const string StatementFrom = "[[ Admit Date ]]";
        public const string StatementThru = "[[ Discharge Date ]]";
        public const string ClaimTotal = "[[ Total Charges ]]";
        // ReSharper disable once UnusedMember.Global
        public const string ClaimState = "[[ Claim State ]]";
        // ReSharper disable once UnusedMember.Global
        public const string DischargeStatus = "[[ Discharge Status ]]";
        public const string CurrentDate = "[[ Current Date ]]";
        public const string Icn = "[[ ICN ]]";
        public const string Los = "[[ LOS ]]";
        public const string Age = "[[ Age ]]";
        //Given name as "x-msg/h1" so user/hacker will not able to figure out what it will do
        public const string BubbleDataSource = "x-msg-h1";
        public const string UserFacilitiesSessionString = "UserFacilitiesSession";
        public const string IsUserLoggedIn = "IsUserLoggedIn";
        public const string CurrentFacilityIdSessionString = "CurrentFacilityId";
        public const string UserInfo = "UserInfo";
        public const string ClaimSelectErrorMsg = "Please select at least one claim.";
        public const string ReassignClaimEmptyErrorMsg =
            "No data available for the selected criteria. Please modify your selection criteria and try again.";
        public const string ContractSelectErrorMsg = "Some of the selected claims require contract selection. Please choose a contract and retry.";
        public const string PrefixValue = "0";

        public static readonly Dictionary<string, string> LetterVariables = new Dictionary<string, string>
        {
            {"Select Field", ""},
            {"Admit Date", StatementFrom},
            {"Age", Age},
            {"Bill Date", BillDate},
            {"Bill Type", BillType},
            {"Contract Name", ContractName},
            {"Current Date", CurrentDate},
            {"Discharge Date", StatementThru},
            {"DRG", Drg},
            {"Expected Allowed", ExpectedAllowed},
            {"FTN", Ftn},
            {"ICN",Icn},
            {"LOS",Los},
            {"Med Rec Number", MedRecNumber},
            {"NPI", Npi},
            {"Patient Account Number", PatientAccountNumber},
            {"Patient DOB", PatientDob},
            {"Patient First Name", PatientFirstName},
            {"Patient Last Name", PatientLastName},
            {"Patient Middle Name", PatientMiddleName},
            {"Patient Responsibility", PatientResponsibility},
            {"Payer Name", PayerName},
            {"Primary Group Number", PrimaryGroupNumber}, 
            {"Primary Member ID", PrimaryMemberId}, 
            {"Provider Name", ProviderName},
            {"Remit Check Date", RemitCheckDate},
            {"Remit Payment", RemitPayment},
            {"Total Charges", ClaimTotal}
           
            
        };

        public static readonly Dictionary<string, string> LetterStylePairs = new Dictionary<string, string>
        {
            {"style=\"text-align:left;\"", "align=\"left\""},
            {"style=\"text-align:right;\"", "align=\"right\""},
            {"style=\"text-align:center;\"", "align=\"center\""},
            {"style=\"text-align:justify;\"", "align=\"justify\""},
            {"<span style=\"text-decoration:underline;\">", "<u>"},
            {"&nbsp","&"},
            {"&lt;","<"},
            {"&gt;",">"},
            {"</span>", "</u>"},
            {"<del>", "<strike>"},
            {"</del>", "</strike>"},
        };

        public const string AppealLetterFileBaseName = "Letter";
        public const string AppealLetterPreviewFileBaseName = "LetterPreview";
        public const string AppealLetterFileExtension = "rtf";
        public const string SourceRegex = @"src=(?:(['""])(?<src>(?:(?!\1).)*)\1|(?<src>[^\s>]+))";
        public const string UrlSourceRegex = @"^(http[s]{0,1}://)(?:[\w][\w.-]?)+";
        public const string Imagepath = "ImageBrowser/Image?path=";
        public const string ImageSourceBasePath = "src=\"";
        public const string Path = "path=";
        public const string AlignLeft = "left";
        public const string AlignRight = "right";
        public const string AlignCenter = "center";
        public const int ImageHeight = 550;
        public const int ImageWidth = 650;
        public const string Oledb4Connection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO\"";

        public const string Oledb12Connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO\"";

        public static readonly List<string> MedicareLabFeeScheduleFields = new List<string>
        {
            "Hcpcs", "Carrier", "Amount"
        };
        public static readonly List<string> PaymentTableFields = new List<string>
        {
           "Identifier", "Value"
        };
        public static readonly List<string> OpenClaimFields = new List<string>
        {
           "Reviewed", "ClaimIdValue","SsiNumber","PatientAccountNumber","Age","Npi","ClaimType","PayerSequence","BillType","Drg","PriIcddCode","PriIcdpCode","PriPayerName",
           "SecPayerName","TerPayerName","IsRemitLinked","ClaimStat","ClaimLink","LinkedRemitId","DischargeStatus","CustomField1","CustomField2","CustomField3","CustomField4",
           "CustomField5","CustomField6","MemberId","Icn","Mrn","ClaimTotal","AdjudicatedValue","ActualPayment","PatientResponsibility","RemitAllowedAmt","RemitNonCovered",
           "CalculatedAdjustment","ActualAdjustment","ContractualVariance","PaymentVariance","ClaimState","AdjudicatedDateValue","StatementFromValue","StatementThruValue",
           "Los","BillDateValue","ClaimDateValue","LastFiledDateValue","CheckDate","CheckNumber","AdjudicatedContractName","InsuredsGroupNumber"
        };

        public const string CodeField = "Code";
        public const string AmountField = "Amount";
        public const string Currency = "$";
        public const string TableSuccessfullyUploaded = "Table successfully uploaded.";
        public const string TableFailedToUpload = "Error occurred while uploading the table.";
        public const string DocumentError = "Document format error.";
        public const string FileMaxSize = "The file cannot be uploaded as it exceeds the maximum limit of {0} MB.";
        public const string ConvertToMb = "1048576";
        public const string FormCollectionTableName = "TableName";
        public const string FormCollectionClaimFieldId = "ClaimFieldId";
        public const string FormCollectionImportTable = "ImportTable";
        public const string EmptyFile = "Empty file cannot be uploaded.";
        public const string DropDownListTextField = "text";
        public const string DropDownListValueField = "value";
        public const string PayerMappingReportFileBaseName = "PayerMappingReport";
        private const string MappedPayer = "Mapped Payers Only";
        private const string UnmappedPayers = "Unmapped Payers Only";
        private const string AllPayers = "All Payers";
        public const string InsufficientColumnsErrorMessage = "Table should have a minimum of two columns.";
        public const string SameHeaderName = "Two columns cannot have the same header name";
        public const string ColumnHeaderNameSeparator = "_";
        public const string InvalidHeaderErrorMessage = "The table cannot be uploaded as column header names used in the system are invalid.";
        public const int KiloBytes = 1024;
        public const string TableNameExists = "IsTableNameExists";
        public const string IsModelExist = "IsModelExist";
        public const string ReAdjudicate = "ReAdjudicate";

        public static readonly Dictionary<int, string> PayerMapping = new Dictionary<int, string>
        {
            {7, MappedPayer},
            {8, UnmappedPayers},
            {9, AllPayers}
        };

        public const string HtmlParagraphStartTag = "<p>";
        public const int IndexOfHtmlImageSourceValue = 5;
        public const string RtfParagraphTag = @"\par";
        public const string RtfParagraphResetTag = @"\pard";
        public const string RtfPageBreak = @"\par\page";
        public const string RtfDocumentEndTag = "}";
        public const string HtmlDivClosingTag = "</div>";
        public const string HtmlParagraphEndTag = "</p>";
        public const string HtmlLineBreakTag = "<br />";
        public const string HtmlDivOpeningTag = "<div>";
        public const string HtmlParagraphEnclosure = "</p><p><p>";
        public const string HtmlParagraphStartEnclosure = "</p></p>";
        public const string HtmlParagraphEndEnclosure = "<p><p>";
        public const string HtmlParagraphTagName = "p";
        public const string HtmlAlignAttribute = "align";
        public const string HtmlDivXPath = "//div";
        public const string HtmlImageXPath = "//img";
        public const string HtmlSpanTagName = "span";
        public const string HtmlStyleAttribute = "style";
        public const string HtmlStartTag = "<";
        public const string HtmlImageTagName = "img";

        public static readonly Dictionary<string, string> ImageStylePairs = new Dictionary<string, string>
        {
            {"float:right", "right"},
            {"float:left", "left"},
            {"display:block", "center"},
            {string.Empty, "left"}
        };

        public const string PropertyBillType = "BillType";
        public const string PropertyDrg = "Drg";
        public const string PropertyClaimState = "ClaimState";
        public const string PropertyPriPayerName = "PriPayerName";
        public const string PropertyClaimType = "ClaimType";
        public const string PropertyPatAcctNum = "PatAcctNum";
        public const string PropertyTotalCharges = "ClaimTotal";
        public const string PropertyStatementFrom = "StatementFrom";
        public const string PropertyStatementThru = "StatementThru";
        public const string PropertyStatementCoversPeriod = "StatementCoversPeriod";
        public const string PropertyNpi = "Npi";
        public const string PropertyCustomField1 = "CustomField1";
        public const string PropertyCustomField2 = "CustomField2";
        public const string PropertyCustomField3 = "CustomField3";
        public const string PropertyCustomField4 = "CustomField4";
        public const string PropertyCustomField5 = "CustomField5";
        public const string PropertyCustomField6 = "CustomField6";
        public const string PropertyDischargeStatus = "DischargeStatus";
        public const string PropertyIcn = "Icn";
        public const string PropertyMrn = "Mrn";
        public const string PropertyLos = "Los";
        public const string PropertyAge = "Age";
        public const string PropertyCheckDate = "CheckDate";
        public const string PropertyCheckNumber = "CheckNumber";

        public const string PropertyRevCode = "ClaimCharges.RevCode";
        public const string PropertyHcpcsCode = "ClaimCharges.HcpcsCode";
        public const string PropertyPlaceOfService = "ClaimCharges.PlaceOfService";
        public const string PropertyHcpcsCodeWithModifier = "ClaimCharges.HcpcsCodeWithModifier";
        public const string PropertyIcdpCode = "ProcedureCodes.IcdpCode";
        public const string PropertyIcddCode = "DiagnosisCodes.IcddCode";
        public const string PropertyReferringPhysician = "Referring";
        public const string PropertyRenderingPhysician = "Rendering";
        public const string PropertyAttendingPhysician = "Attending";
        public const string PropertyInsuredGroup = "InsuredCodes.GroupNumber";
        public const string PropertyInsuredId = "InsuredCodes.CertificationNumber";
        public const string PropertyValueCode = "ValueCodes.Code";
        public const string PropertyOccurenceCode = "OccurrenceCodes.Code";
        public const string PropertyConditionCode = "ConditionCodes.Code";


        public const string MessageNoError = "No errors found.";
        public const string MessageClaimError = "Claim errors found.";
        public const string MessageClaimDataError = "Claim data errors found.";
        public const string MessageMajorClaimError = "Major claim errors found - processing suspended.";
        public const string MessageApcActiveError = "Misc. APCActive Error.";
        public const string MessageSqlTableError = "SQL tables corrupted or loaded incorrectly.";
        public const string MessageApcActiveLicenseExpired = "APCActive license id expired.";
        public const string MessageSqlError = "SQL data access error.";
        public const string MessageMicrodyneOff = "Microdyne Switched Off.";
        public const string MessageWageIndexNotLocated = "Wage Index not located.";
        public const string MessageWageIndexZero = "Wage Index = 0 ";
        public const string MessageWageIndexError = "Provider file wage index reclassification code invalid or missing.";
        public const string MessageServiceDateError = "Service date not numeric or < 8/1/2000  provider file waiver indicator = “Y”. Case cannot be processed.";
        public const string MessageLesserServiceDate = "Service date < provider effective date.";
        public const string MessageInvalidProviderNumber = "Invalid provider number.";
        public const string MessageInvalidBeneficiaryAmount = "Invalid beneficiary deductible amount.";
        public const string MessageInputNumberError = "Input number of line items in error.";
        public const string MessageApcActivePricerError = "Misc. APCActive Pricer DLL error.";
        public const string MessageApcActivePricerLicenseExpired = "APCActive Pricer License is expired.";
        public const string MessageLineItemApcError = "Line item APC is missing or invalid.";
        public const string MessageLineItemDiscountFactorError = "Line item Discount factor is invalid.";
        public const string MessageLineItemStatusIndicatorError = "Line item Status Indicator is invalid.";
        public const string MessageLineItemPaymentAdjustmentFlagError = "Line item Payment Adjustment Flag is invalid.";
        public const string MessageLineSuccess = "Line processed successfully.";
        public const string MessageLinepaymentZero = "Line processed but payment =0, all applied to deductible.";
        public const string MessageLineCoInsuranceLimitation = "Daily coinsurance limitation – claim processed.";
        public const string MessageLineReducedCoInsurance = "Line item has reduced coinsurance amount.";
        public const string MessageStatusIndicatorNotProcessed = "Status indicator not processed by OPPS pricer.";
        public const string MessageApcZero = "APC = 00000";
        public const string MessagePaymentIndicatorError = "Payment Indicator not = 1 or 5 thru 9";
        public const string MessageStatusIndicatorError = "Status indicator = 'H', but payment indicator not = '6'";
        public const string MessagePackagingFlagError = "Packaging flag <> '0'";
        public const string MessageLineItemReject = "Line item denial/reject flag not = to 0 and line item action flag not = to 1";
        public const string MessageLineFlag = "Line item action flag = 2 or 3";
        public const string MessagePaymentMethodFlag = "Payment Method Flag not = to 0";
        public const string MessagePaidDrgPayment = "Paid Normal DRG Payment";
        public const string MessagePaidAsCostOutlier = "Paid as a Cost Outlier";
        public const string MessageTrasferPaid = "Transfer Paid on a Perdiem Basis";
        public const string MessageTrasferPaidWithCostOutlier = "Transfer Paid on a Perdiem Basis with Cost Outlier";
        public const string MessageTrasferPaidRefused = "Transfer Paid on a Perdiem Basis- Cost Outlier Refused";
        public const string MessagePostAcuteTransfer = "Post Acute Transfer with 50/50 payment";
        public const string MessagePostAcuteTransferFully = "Post Acute Transfer with full perdiem payment";
        public const string MessagePaidDrgDays = "Paid as Normal DRG - Days > =  Avg Length of Stay";
        public const string MessagePaidCostOutlierDays = "Paid as a Cost Outlier - Days > =  Avg Length of Stay";
        public const string MessageCostOutlier = "Cost Outlier Threshold Calculated";
        public const string MessageProviderNumberError = "Provider Number Not Found";
        public const string MessageInvalidMsa = "Invalid MSA or Wage Index";
        public const string MessageWaiverFlag = "Waiver flag = 'Y' - No Reimb. Calculated";
        public const string MessageInvalidDrg = "Invalid DRG or DRG Not Found";
        public const string MessageDischargeDateError = "Discharge Date < Provider Eff. Date";
        public const string MessageInvalidLos = "Invalid Length of Stay";
        public const string MessageInvalidReviewCode = "Invalid Review Code";
        public const string MessageNoTotalChargeSubmitted = "No Total Charges Submitted";
        public const string MessageMoreLtrDays = "More Than 60 LTR Days";
        public const string MessageInvalidCoveredDays = "Invalid Number of Covered Days";
        public const string MessageInvalidHrrData = "Invalid Capital Pay Code or HRR data";
        public const string MessageCostOutlierWithLos = "Cost Outlier with LOS > Covered Days";
        public const string MessageInvalidVbp = "Invalid VBP data";
        public const string MessagePricerActiveError = "Miscellaneous PRICERActive error.";
        public const string MessageReviewCodeError = "Reviewcode not set and DStat not specified.";
        public const string MessagePricerActiveExpired = "PRICERActive license period has expired.";
        public const string MessagePricerActiveDataAccessError = "PRICERActive data access error.";

        public const string DoubleNewLine = " \r\n  \r\n ";
        public const char RangeOperator = ':';
        public const string Comma = ",";
        public const string SemiColon = ";";
        public const string Space = " ";
        public const string NewLine = "\n";
        public const string CarriageReturn = "\r";
        public const string CommaReplaceString = "#&#";
        public const string AmpercentString = "&";
        public const string AmpercentReplaceString = "&amp;";
        public const string LessThanString = "<";
        public const string LessThanReplaceString = "&lt;";
        public const string ReplaceDotString = "#$#";
        public const string MaskCommaInValue = "__";
        public const string Apostrophe = "'";
        public const char CommaCharacter = ',';
        public const string UnderScoreCharacter = "#_#";
        public const string Dollar = "$";
        public const string Dot = ".";
        public const int One = 1;
        public const int Zero = 0;
        public const int CustomPaymentColumnCount = 10;
        public const int DrgCodeLength = 3;
        public const int Two = 2;
        public const int Four = 4;
        public const char DoubleQuoteChar = '\"';
        public const string DoubleQuote = "\"";
        public const string ThreeDoubleQuote = "\"\"\"";
        public const string TwoDoubleQuote = "\"\"\"";
        public const int Stringlength = 200;
        public const string ReviewedOptionYes = "Yes";
        public const string ReviewedOptionNo = "No";
        public const string ReplaceCommaInString = "+&+";

        public const string NoValue = "(No Value)";
        public const string Total = "Total";

        public const string CommentTextWhenManualJobIsRunning =
            "Background Job is waiting for completion of manual job for ";

        public const string CommentTextWhenBackgroundJobCompleted =
            "Background Job execution completed and went to sleep for ";

        //Do not change these values. These values must be same as the property of SmartBox class 
        public static readonly List<string> ThresholdSmartBoxVariables = new List<string>
        {
            "LOS", "CAA", "TCC", "LC"
        };

        public static readonly Dictionary<string, string> StopLossClaimValuePair = new Dictionary<string, string>
        {
            {"CAA", "20"},
            {"LOS", "30"},
            {"TCC", "50"},
            {"GREATEROF", "Math.Max"},
            {"LESSEROF", "Math.Min"},
            {"IF", "Math.Min"}
        };


        public const string NoRemit = "no remit";
        public const string UnAdjudicated = "Un-adjudicated";

        public const string ClientHeader = "ClaimID" + "," + "AdjudicatedContractName" + "," + "Reviewed" + ", " + "SSINumber" + ", " + "PatientAccountNumber" + ", " + "Age" + ", " + "NPI" + ", " +
                                                "ClaimType" + ", " + "PayerSequence" + ", " + "BillType" +
                                                ", " + "DRG" + ", " + "PriICDDCode" + ", " + "PriICDPCode" + ", " +
                                                "PriPayerName" + ", " + "SecPayerName" + ", "
                                                + "TerPayerName" + ", " + "IsRemitLinked" + ", " + "ClaimStat" + ", " +
                                                "ClaimLink" + ", " + "LinkedRemitID" + ", " + "DischargeStatus" + ", " + "CustomField1" + ", " +
                                                "CustomField2" + ", " + "CustomField3" + ", " + "CustomField4" + ", " +
                                                "CustomField5" + ", " + "CustomField6" + ", " + "MemberID" + ", " + "ICN" + ", " + "MRN" + ", " + "InsuredsGroupNumber" + ", " + "ClaimTotal" + ", " +
                                                "AdjudicatedValue" + ", " + "ActualPayment" + ", "
                                                + "PatientResponsibility" + ", " + "RemitAllowedAmt" + ", " +
                                                "RemitNonCovered" + ", " + "CalculatedAdj" + ", " +
                                                "ActualAdj" + ", " + "ContractualVariance" + ", " + "PaymentVariance" + ", " +
                                                "ClaimState" + ", " +
                                                "AdjudicatedDate" + ", " + "CheckDate" + ", " + "CheckNumber" + ", " + "StatementFrom" + ", " + "StatementThru" + ", " + "LOS" + ", "
                                                + "BillDate" + ", " + "ClaimDate" + ", " + "LastFiledDate";
        public const string DateTime1900 = "1/1/1900 12:00:00 AM";
        public const int CRecordLength = 200;
        public const int DRecordLength = 200;
        public const int LRecordLength = 200;
        public static readonly char[] AllowedOperators = { '+', '-', '/', '*', '(', ')', ',', '>', '<', '=' };
        public const string ReferenceTableTag = "T_";
        public const string StartSquareBracket = "[";
        public const string EndSquareBracket = "]";
        public const string RegExPatternDoubleUnderscore = "_{2,}";
        public const string RegExPatternSpecialCharacter = "[$^]";
        public const string VariableCalculatedAllowedAmount = "CAA";

        public const string ModelComparisonReportFileBaseName = "ModelComparisonReport";
        public const string ModelComparisonHeader = " " + ", " + "ClaimCount" + ", " + "ClaimTotalCharges" + ", " + "AdjudicatedValue" + ", " +
                                               "Actualpayment" + ", " + "PatientResponsibility" + ", " + "RemitAllowed" + ", " + "RemitNonCovered" +
                                               ", " + "CalculatedAdjustment" + ", " + "ActualAdjustment" + ", " + "ContractualVariance" + ", " +
                                               "PaymentVariance";

        public const string AuditLogReportHeader =
            "AuditLogId" + ", " + "LoggedDate" + ", " + "UserName" + ", " + "Action" + ", " +
            "ObjectType" + ", " + "FacilityName" + ", " + "ModelName" + ", " + "ContractName" + ", " + "ServiceTypeName" +
            ", " + "Description";

        public const string FileNameSeperator = "\\";

        public const string RegexDigitPattern = @"^\d";

        #region API Controller

        public const string ContractPayerMapReport = "ContractPayerMapReport";
        public const string PaymentTypeCustomTable = "PaymentTypeCustomTable";
        public const string PaymentTypeStopLoss = "PaymentTypeStopLoss";
        public const string ModelComparisonReport = "ModelComparisonReport";
        public const string PaymentTypeAscFeeSchedule = "PaymentTypeAscFeeSchedule";
        public const string PaymentTypeMedicareIp = "PaymentTypeMedicareIp";
        public const string MedicareIpAcuteOption = "MedicareIpAcuteOption";
        public const string GetMedicareIpAcuteOptions = "GetMedicareIpAcuteOptions";
        public const string GetPaymentTypeMedicareIpDetails = "GetPaymentTypeMedicareIpDetails";
        public const string AddEditPaymentTypeMedicareIpDetails = "AddEditPaymentTypeMedicareIpDetails";
        public const string GetAscFeeScheduleOptions = "GetAscFeeScheduleOptions";
        public const string AddEditPaymentTypeAscFeeSchedule = "AddEditPaymentTypeAscFeeSchedule";
        public const string GetTableNameSelection = "GetTableNameSelection";
        public const string PaymentTable = "PaymentTable";
        public const string ClaimFieldDoc = "ClaimFieldDoc";
        public const string MedicareLabFeeSchedule = "MedicareLabFeeSchedule";
        public const string JobsData = "JobsData";
        public const string IsContractNameExist = "IsContractNameExist";
        public const string AddEditContractBasicInfo = "AddEditContractBasicInfo";
        public const string IsModelNameExit = "IsModelNameExit";
        public const string IsContractServiceTypeNameExit = "IsContractServiceTypeNameExit";
        public const string ClaimSelection = "ClaimSelection";
        public const string ReviewClaim = "ReviewClaim";
        public const string ReviewedAllClaims = "ReviewedAllClaims";
        public const string AuditLogReport = "AuditLogReport";
        public const string GetAuditLogReport = "GetAuditLogReport";
        public const string GetClaimLinkedCount = "GetClaimLinkedCount";
        public const string GetAllJobs = "GetAllJobs";
        public const string UpdateJobStatus = "UpdateJobStatus";
        public const string JobCountAlert = "JobCountAlert";
        public const string UpdateJobVerifiedStatus = "UpdateJobVerifiedStatus";
        public const string GetSelectedClaim = "GetSelectedClaim";
        public const string GetOpenClaimColumnNamesBasedOnUserId = "GetOpenClaimColumnNamesBasedOnUserId";
        public const string ContractAlertCount = "ContractAlertCount";
        public const string UpdateAlertVerifiedStatus = "UpdateAlertVerifiedStatus";
        public const string CheckAdjudicationRequestNameExist = "CheckAdjudicationRequestNameExist";
        public const string AddEditSelectClaims = "AddEditSelectClaims";
        public const string GetSelectedClaimList = "GetSelectedClaimList";
        public const string Payer = "Payer";
        public const string GetPayers = "GetPayers";
        public const string AddClaimNote = "AddClaimNote";
        public const string DeleteClaimNote = "DeleteClaimNote";
        public const string GetClaimNotes = "GetClaimNotes";
        #endregion

        public const string Fsp = "FSP";
        public const string Hsp = "HSP";
        public const string Co = "CO";
        public const string Ime = "IME";
        public const string Dsh = "DSH";
        public const string PassThru = "PassThru";
        public const string TechAddOnPayment = "TechAddOnPayment";
        public const string LowVolumeAddOn = "LowVolumeAddOn";
        public const string Hrr = "HRR";
        public const string Vbp = "VBP";
        public const string Ucc = "UCC";
        public const string Hac = "HAC";
        public const string Cap = "CAP";
        public const string CapDsh = "CAP_DSH";
        public const string CapExceptionsAmount = "CAP_Exceptions Amt";
        public const string CapFsp = "CAP_FSP";
        public const string CapHsp = "CAP_HSP";
        public const string CapIme = "CAP_IME";
        public const string CapOldHarm = "CAP_OldHarm";
        public const string CapOutlier = "CAP_Outlier";
        public const string MedicareIpAcuteOptionId = "MedicareIpAcuteOptionID";
        public const string MedicareIpAcuteOptionCode = "MedicareIpAcuteOptionCode";
        public const string MedicareIpAcuteOptionName = "MedicareIpAcuteOptionName";
        public const string MedicareIpAcuteOptionChildParentId = "MedicareIPAcuteOptionChildParentID";
        public const string MedicareIpAcuteOptionChildId = "MedicareIpAcuteOptionChildID";
        public const string MedicareIpAcuteOptionChildCode = "MedicareIpAcuteOptionChildCode";
        public const string MedicareIpAcuteOptionChildName = "MedicareIpAcuteOptionChildName";
        public const string Contract = "Contract";
        public const string ContractTreeView = "ContractTreeView";
        public const string ContractServiceType = "ContractServiceType";
        public const int PaymentTypeLesserOfPercentage = 100;
        public const string IsDocumentInUse = "IsDocumentInUse";
        public const string AmountFormat = "C2";
        public const string Minus = "-";
        public const string ReportSelection = "ReportSelection";
        public const string GetClaimReviewedOption = "GetClaimReviewedOption";
        public const string InsertAuditLog = "InsertAuditLog";
        public const string LetterTemplate = "LetterTemplate";
        public const string IcdVersionDate = "10/01/2015 12:00:00 AM";
        public const string IcdVersion9 = "9";
        public const string IcdVersion10 = "0";
        public const int DeviceCreditAmount = 0;
        public const bool DificidOnClaim = false;
        public const bool HmoClaim = false;
        public const bool IncludePassthru = false;
        public const bool AllowTerminatedProvider = false;
        public const int AdjustmentFactor = 1;
        public const int AdjustmentOption = 0;
        public const char CreteriaSeparatorChar = '~';
        public const string CreteriaSeparatorString = "~";
        public const string ReplaceValue = "$_$";
        public const string ActualValue = "&";
        public const char ConditionSeparatorChar = '|';
        public const string ConditionSeparatorString = "|";
        public const string ContractModel = "ContractModel";
        public const string GetAllContractFacilities = "GetAllContractFacilities";
        public const string DRecordString = "D";
        public const string ERecordString = "E";
        public const string CRecordString = "C";

        public const string ReassignClaim = "ReassignClaim";
        public const string GetReassignGridData = "GetReassignGridData";
        public const string GetContractsByNodeId = "GetContractsByNodeId";
        public const string AddReassignedClaimJob = "AddReassignedClaimJob";
        public const string HasPermission = "HasPermission";
        public const string ContentTypeApplication = "application/json";
        public const string MathMax = "Math.max";
        public const string MathMin = "Math.min";
        public const string GreaterOf = "GREATEROF";
        public const string LesserOf = "LESSEROF";
        public const string If = "IF";
        public const string GetPaymentTypeCustomTable = "GetPaymentTypeCustomTable";
        public const string GetMedicareLabFeeSchedule = "GetMedicareLabFeeSchedule";
        public const string Facility = "Facility";
        public const string GetAllFacilities = "GetAllFacilities";
        public const string DeleteFacility = "DeleteFacility";
        public const string GetActiveStates = "GetActiveStates";
        public const string GetBubbles = "GetBubbles";
        public const string User = "User";
        public const string GetAllUserModels = "GetAllUserModels";
        public const string Delete = "DeleteUser";
        public const string GetUserById = "GetUserById";
        public const string GetAllFacilityList = "GetAllFacilityList";
        public const string GetFacilityMedicareDetails = "GetFacilityMedicareDetails";
        public const string AddEditUser = "AddEditUser";
        public const string GetUserDetails = "GetUserDetails";
        public const string SaveAuditLog = "SaveAuditLog";
        public const string GetUserFacilityDetails = "GetUserFacilityDetails";
        public const string UpdateUserLogin = "UpdateUserLogin";
        public const string MailFrom = "MailFrom";
        public const string SmtpServer = "SmtpServer";
        public const string SmtpPort = "SmtpPort";
        public const string SmtpPassword = "SmtpPassword";
        public const string MailSubject = "CM Membership Account";
        public const string MailSubjectAccountReset = "CM Membership Account – Account Reset";
        public const string MailSubjectPasswordReset = "CM Membership Account – Password Reset";
        public const string MailSubjectChangePassword = "CM Membership Account – Change Password";
        public const string MailSubjectRecoverPassword = "CM Membership Account – Forgot Password";
        public const string ActivationMailBody =
            "Your new SSI Contract Management account has been created by your administrator. Please follow this link within the next 24 hours to create a password:";
        public const string PasswordMailBody =
            "Please follow this link within the next 24 hours to create a new password:";

        public const string ActivationMailLink = "Click here to set your password";
        public const string ResetAccountMailLink = "Click here to reset your account";
        public const string ChangePasswordMailLink = "Click here to change your password";
        public const string ResetPasswordMailLink = "Click here to reset your password";
        public const string RecoverPasswordMailLink = "Click here to reset your password";
        public const string GetFacilityById = "GetFacilityById";
        public const string SaveFacility = "SaveFacility";
        public const string MailMessageTemplate = "~/Areas/UserManagement/EmailTemplate/MailMessageTemplate.htm";
        public const string PasswordResetTemplate = "~/Areas/UserManagement/EmailTemplate/PasswordResetTemplate.htm";
        public const string PasswordBytes = "MAKV2SPBNI99212";
        public const int SaltMaxLength = 32;
        public const int AesKeySize = 256;
        public const int AesBlockSize = 128;
        public const string AccountActivation = "AccountActivation";
        public const string GetSecurityQuestion = "GetSecurityQuestion";
        public const string IsUserEmailExist = "IsUserEmailExist";
        public const string IsUserEmailLockedOrNot = "IsUserEmailLockedOrNot";
        public const string AddQuestionAnswerAndPassword = "AddQuestionAnswerAndPassword";
        public const string ConnectionString = "Data Source=69.85.245.160;Initial Catalog=bubble2;Persist Security Info=True;User ID=sa;Password=Cmsqldevu$3r";

        public const int DefaultUserId = 0;
        public const int UserNameExist = -1;
        public const int InValidDomainName = -2;
        public const string SaveUserAccountSetting = "SaveUserAccountSetting";
        public const string SaveEmailLog = "SaveEmailLog";
        public const string ValidateToken = "ValidateToken";
        public const string EmailUrl = "UserManagement/AccountActivation/AccountActivation?token=";
        public const string AppealLetter = "AppealLetter";
        public const string GetAppealTemplates = "GetAppealTemplates";
        public const string GetAppealLetter = "GetAppealLetter";
        public const string Get = "Get";
        public const string IsLetterNameExists = "IsLetterNameExists";
        public const string Save = "Save";
        public const string GetAll = "GetAll";
        public const string GetModels = "GetModels";
        public const string DeleteLetter = "Delete";
        public const string GetContractHierarchy = "GetContractHierarchy";
        public const string RenameContract = "RenameContract";
        public const string CopyContractByNodeAndParentId = "CopyContractByNodeAndParentId";
        public const string CopyContractByContractId = "CopyContractByContractId";
        public const string DeleteNodeAndContractByNodeId = "DeleteNodeAndContractByNodeId";
        public const string DeleteContractServiceTypeById = "DeleteContractServiceTypeById";
        public const string CopyContractServiceTypeById = "CopyContractServiceTypeById";
        public const string RenameContractServiceType = "RenameContractServiceType";
        public const string ClaimAdjudicationReport = "ClaimAdjudicationReport";
        public const string GetClaimAdjudicationReport = "GetClaimAdjudicationReport";
        public const string ContractAlert = "ContractAlert";
        public const string GetContractAlerts = "GetContractAlerts";
        public const int OnDemandThreadWaitingTime = 500;
        public const string UpdateContractAlerts = "UpdateContractAlerts";
        public const string GetAllContractModels = "GetAllContractModels";
        public const string DefaultSelectedFacility = "0";
        public const string SpecialCharacterPatterne = "[^0-9a-z]+";
        public const string LogOff = "LogOff";
        public const string LogOn = "LogOn";
        public const string UserLoggedOut = "User logged out";
        public const string UserSessionTimedOut = "User session timed out";
        public const string ModelingReport = "ModelingReport";
        public const string GetAllModelingDetails = "GetAllModelingDetails";
        public const string VarianceReport = "VarianceReport";
        public const string GetVarianceReport = "GetVarianceReport";
        public const string Jobstatus = "jobstatus";
        public const string GetAllClaimFieldsOperators = "GetAllClaimFieldsOperators";
        public const string GetAllClaimFields = "GetAllClaimFields";
        public const string GetAdjudicationRequestNames = "GetAdjudicationRequestNames";
        public const string AdjudicationExceptionLog = "Adjudication Exception TaskId-{0},ClaimId-{1} : ";
        public const string BackgroundServiceUser = "BackgroundServiceUser";
        public const string MicrodynExceptionLog = "Exception occurred while building MicrodynApcEditInput for claim.{0}";
        public const string ClaimSelectionExceptionLog = "Adjudication Exception. TaskId : ";
        public const string FirstParameter = "{0}, ";
        public const string SecondParameter = "{1}.";
        public const string ConversionFailedError = "Double Conversion Failed. Doc Id:{0}, Value:{1}.";
        public const string MicrodynInPatientMessage = "MicrodynInPatientMessage";
        public const string MedicareIpClaimIdLog = "{0} ClaimId - {1}";
        public const string MicrodynApcEditDllResultsData = "MicrodynApcEditDllResultsData";
        public const string ErrorCodes = "Error Codes: ";
        public const string MicrodynApcCalcDllResultsData = "MicrodynApcCalcDllResultsData";
        public const string HomeIndex = "HomeIndex";
        public const string ContractContainer = "ContractContainer";
        public const string ReassignedClaims = "ReassignedClaims";
        public const string LastDeletedNodeId = "LastDeletedNodeId";
        public const string Account = "Account";
        public const string Login = "Login";
        public const string SetSession = "SetSession";
        public const string SessionStore = "SessionStore";
        public const string Common = "Common";
        public const string GetMedicareLabFeeScheduleTableNames = "GetMedicareLabFeeScheduleTableNames";
        public const string GetPaymentTable = "GetPaymentTable";
        public const string GetContractFullInfo = "GetContractFullInfo";
        public const string CurrentDatetime = "localdatetime";
        public const string PaymentTypeMedicareSequester = "PaymentTypeMedicareSequester";
        public const string GetPaymentMedicareSequester = "GetPaymentMedicareSequester";
        public const string AddEditPaymentMedicareSequester = "AddEditPaymentMedicareSequester";
        public const string GetOpenClaimColumnOptionByUserId = "GetOpenClaimColumnOptionByUserId";
        public const string SaveClaimColumnOptions = "SaveClaimColumnOptions";
        public const string IsReviewed = "IsReviewed";
        public const string ActualContractualAdjustment = "ActualContractualAdjustment";
        public const string ExpectedContractualAdjustment = "ExpectedContractualAdjustment";
        public const string ActualAdjustment = "ActualAdjustment";
        public const string CalculatedAdjustment = "CalculatedAdjustment";
        public const string AdjudicatedValue = "AdjudicatedValue";
        public const string ClaimPatientResponsibility = "PatientResponsibility";
        public const string ActualPayment = "ActualPayment";
        public const string RemitAllowedAmt = "RemitAllowedAmt";
        public const string RemitNonCovered = "RemitNonCovered";
        public const string RemitNonCoveredWithSpace = "Remit Non Covered";
        public const string ContractualVariance = "ContractualVariance";
        public const string PaymentVariance = "PaymentVariance";
        public const string ClaimId = "ClaimId";
        public const string ClaimIdValue = "ClaimIdValue";
        public const string SecPayerName = "SecPayerName";
        public const string TerPayerName = "TerPayerName";
        public const string ClaimStatus = "ClaimStatus";
        public const string ClaimStat = "ClaimStat";
        public const string SsiNumberWithCaps = "SSINumber";
        public const string SsiNumber = "SsiNumber";
        public const string NpiCaps = "NPI";
        public const string PriIcddCode = "PriIcddCode";
        public const string PriIcdpCode = "PriIcdpCode";
        public const string MemberId = "MemberId";
        public const string AdjudicatedDateValue = "AdjudicatedDateValue";
        public const string StatementFromValue = "StatementFromValue";
        public const string StatementThruValue = "StatementThruValue";
        public const string BillDateValue = "BillDateValue";
        public const string ClaimDateValue = "ClaimDateValue";
        public const string LastFiledDateValue = "LastFiledDateValue";
        public const string LinkedRemitId = "LinkedRemitId";
        public const string PropertyLinkedRemitId = "LinkedRemitID";
        public const string LastFiledDate = "LastFiledDate";
        public const string ClaimDate = "ClaimDate";
        public const string PropertyBillDate = "BillDate";
        public const string LosWithCaps = "LOS";
        public const string MrnWithCaps = "MRN";
        public const string IcnWithCaps = "ICN";
        public const string MemberIdCaps = "MemberID";
        public const string PriIcdPCodeCaps = "PriICDPCode";
        public const string PriIcddCodeCaps = "PriICDDCode";
        public const string DrgWithCaps = "DRG";
        public const string AdjudicatedDate = "AdjudicatedDate";
        public const string CommaWithSpace = ", ";
        public const string RenamePaymentTable = "RenamePaymentTable";
        public const string CheckDate = "CheckDate";
        public const string CheckNumber = "CheckNumber";
        public const string CheckDateWithSpace = "Check Date";
        public const string CheckNumberWithSpace = "Check Number";
        public const string AdjudicatedContractName = "AdjudicatedContractName";
        public const string AdjudicatedContractNameWithSpace = "Adjudicated Contract Name";
        public const string InsertDate = "InsertDate";
        public const string PropertySsinumber = "ssinumber";
        public const string PayerSequence = "PayerSequence";
        public const string IcddCode = "ICDDCode";
        public const string IcdpCode = "ICDPCode";
        public const string PropertyIsRemitLinked = "IsRemitLinked";
        public const string CalAllow = "CalAllow";
        public const string PropertyClaimLink = "ClaimLink";
        public const string PropertyLastRemitId = "lastRemitID";
        public const string PropertyClaimId = "ClaimID";
        public const string CertificationNumber = "CertificationNumber";
        public const string PropertyMedRecNo = "MedRecNo";
        public const string PropertyCalNonCov = "CalNonCov";
        public const string PropertyPatientAccountNumber = "PatientAccountNumber";
        public const string ClaimLink = "claimLink";
        public const string MemberIdWithSpace = "Member ID";
        public const string ClaimIdWithSpace = "Claim ID";
        public const string DischargeStatusWithSpace = "Discharge Status";
        public const string LinkedRemitIdWithSpace = "Linked Remit ID";
        public const string ClaimLinkWithSpace = "Claim Link";
        public const string RemitAllowedAmtWithSpace = "Remit Allowed Amt";
        public const string PaymentVarianceWithSpace = "Payment Variance";
        public const string ContractualVarianceWithSpace = "Contractual Variance";
        public const string PatientResponsibilityWithSpace = "Patient Responsibility";
        public const string CalculatedAdjWithSpace = "Calculated Adj";
        public const string ActualAdjWithSpace = "Actual Adj";
        public const string ActualPaymentWithSpace = "Actual Payment";
        public const string AdjudicatedValueWithSpace = "Adjudicated Value";
        public const string ClaimStatusWithSpace = "Claim Status";
        public const string IsRemitLinkedWithSpace = "Is Remit Linked";
        public const string TerPayerNameWithSpace = "Ter Payer Name";
        public const string SecPayerNameWithSpace = "Sec Payer Name";
        public const string PriPayerNameWithSpace = "Pri Payer Name";
        public const string PriIcdpCodeWithSpace = "Pri ICDP Code";
        public const string PriIcddCodeWithSpace = "Pri ICDD Code";
        public const string BillTypeWithSpace = "Bill Type";
        public const string LastFiledDateWithSpace = "Last Filed Date";
        public const string BillDateWithSpace = "Bill Date";
        public const string ClaimDateWithSpace = "Claim Date";
        public const string StatementThruWithSpace = "Statement Thru";
        public const string StatementFromWithSpace = "Statement From";
        public const string ClaimTotalWithSpace = "Claim Total";
        public const string PayerSequenceWithSpace = "Payer Sequence";
        public const string ClaimStateWithSpace = "Claim State";
        public const string ClaimTypeWithSpace = "Claim Type";
        public const string SsiNumberWithSpace = "SSI Number";
        public const string AdjudicatedDateWithSpace = "Adjudicated Date";
        public const string PatientAccountNumberWithSpace = "Patient Account Number";
        public const string Reviewed = "Reviewed";
        public const int MedicareSequesterPercentage = 100;
        public const string WhiteSpace = "$0 ";
        public const string HcpcsModifierPattern = ".{2}";
        public const string GetAdjudicatedContracts = "GetAdjudicatedContracts";
        public const string SaveErrorLog = "SaveErrorLog";
        public const string SaveUserLandingPage = "SaveUserLandingPage";
        public const string GetUserLandingPage = "GetUserLandingPage";
        public const string AddEditQueryName = "AddEditQueryName";
        public const string DeleteQueryName = "DeleteQueryName";
        public const string GetQueriesById = "GetQueriesById";
        public const string Http = "http";
        public const string Https = "https";
        public const string HttpUrls = "69.85.245.163,69.85.245.164,192.168.30.27,192.168.30.28,localhost";
        public const string StringOne = "1";
        public const int LimitOccurence = -1;
        public const string InsuredsGroupNumber = "InsuredsGroupNumber";
        public const string InsuredsGroupNumberWithSpace = "Insured’s Group Number";
        public const int PaymentResultInterval = 1000;
        public const string SingleQuote = "\'";
        public const string SingleQuotes = "'";
        public const string IsAutoRefresh = "IsAutoRefresh";
        public const string WildCard = "*";
        public const string RegexHash = "#";
        public const string FirstMultiplier = "First = ";
        public const string SecondMultiplier = "Second = ";
        public const string ThirdMultiplier = "Third = ";
        public const string FourthMultiplier = "Fourth = ";
        public const string OthersMultiplier = "Others = ";
        public const string FormulaError = "(Formula error)";
        public new const string Equals = "=";
    }
}
