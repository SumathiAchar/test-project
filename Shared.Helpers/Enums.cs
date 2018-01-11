namespace SSI.ContractManagement.Shared.Helpers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // Enum class no need to Instantiated
    // We disable Resharper for lots of enums member because this member's are used in adjudication 
    //  and report tab for dropdown deleting this member break the adjudication and report tab.
    public class Enums
    {
        /// <summary>
        ///  Service type code enum. Complete replica of database table [dbo].[ref.ServiceLineTypes]
        /// </summary>
        public enum ServiceLineCodes
        {
            [EnumHelperLibrary.FieldName("Bill Type")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            BillType = 1,

            [EnumHelperLibrary.FieldName("Revenue Code")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            RevenueCode = 2,

            [EnumHelperLibrary.FieldName("CPT")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            Cpt = 3,

            [EnumHelperLibrary.FieldName("DRG")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            Drg = 4,

            [EnumHelperLibrary.FieldName("Diagnosis Code")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            DiagnosisCode = 5,

            [EnumHelperLibrary.FieldName("Procedure Code")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            ProcedureCode = 6,

            [EnumHelperLibrary.FieldName("Claim Fields")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            // ReSharper disable once UnusedMember.Global
            ClaimFields = 7,

            [EnumHelperLibrary.FieldName("Table Selection")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            // ReSharper disable once UnusedMember.Global
            TableSelection = 8,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0
        };

        /// <summary>
        /// Payment type code enum. Complete replica of database table [dbo].[ref.PaymentTypes]
        /// </summary>
        public enum PaymentTypeCodes
        {
            [EnumHelperLibrary.FieldName("ASC Fee Schedules")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            AscFeeSchedule = 1,

            [EnumHelperLibrary.FieldName("DRG Schedules")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            DrgPayment = 2,

            [EnumHelperLibrary.FieldName("Fee Schedules")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            FeeSchedule = 3,

            [EnumHelperLibrary.FieldName("Medicare IP Acute")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            MedicareIp = 4,

            [EnumHelperLibrary.FieldName("Medicare OP APC")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            MedicareOp = 5,

            [EnumHelperLibrary.FieldName("Per Case")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            PerCase = 6,

            [EnumHelperLibrary.FieldName("Per Diem")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            PerDiem = 7,

            [EnumHelperLibrary.FieldName("Percent of Charges")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            PercentageDiscountPayment = 8,

            [EnumHelperLibrary.FieldName("Per Visit")]
            [EnumHelperLibrary.FieldIdentityNumber(9)]
            [EnumHelperLibrary.FieldOrderNumber(9)]
            PerVisit = 9,

            [EnumHelperLibrary.FieldName("Stop Loss")]
            [EnumHelperLibrary.FieldIdentityNumber(10)]
            [EnumHelperLibrary.FieldOrderNumber(10)]
            StopLoss = 10,

            [EnumHelperLibrary.FieldName("Lesser/Greater Of")]
            [EnumHelperLibrary.FieldIdentityNumber(11)]
            [EnumHelperLibrary.FieldOrderNumber(11)]
            LesserOf = 11,

            [EnumHelperLibrary.FieldName("CAP")]
            [EnumHelperLibrary.FieldIdentityNumber(12)]
            [EnumHelperLibrary.FieldOrderNumber(12)]
            Cap = 12,

            [EnumHelperLibrary.FieldName("Medicare OP Lab")]
            [EnumHelperLibrary.FieldIdentityNumber(13)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            MedicareLabFeeSchedule = 13,

            [EnumHelperLibrary.FieldName("Custom Table Formulas")]
            [EnumHelperLibrary.FieldIdentityNumber(14)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            CustomTableFormulas = 14,

            [EnumHelperLibrary.FieldName("Medicare Sequester")]
            [EnumHelperLibrary.FieldIdentityNumber(15)]
            [EnumHelperLibrary.FieldOrderNumber(13)]
            MedicareSequester = 15,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            None = 0
        };

        /// <summary>
        /// Claim Field enum. Complete replica of database table [dbo].[ref.ClaimField]
        /// </summary>
        public enum ClaimFieldTypes
        {
            [EnumHelperLibrary.FieldName("Patient  Account Number")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            PatientAccountNumber = 1,

            [EnumHelperLibrary.FieldName("Type of Bill (I)")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            BillType = 2,

            [EnumHelperLibrary.FieldName("Revenue Code(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            RevenueCode = 3,

            [EnumHelperLibrary.FieldName("HCPCS/RATE/HIPPS")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            HcpcsOrRateOrHipps = 4,

            [EnumHelperLibrary.FieldName("Payer Name")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            PayerName = 6,

            [EnumHelperLibrary.FieldName("Insured’s ID")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            InsuredId = 7,

            [EnumHelperLibrary.FieldName("DRG(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            Drg = 8,

            [EnumHelperLibrary.FieldName("Place of Service(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(9)]
            [EnumHelperLibrary.FieldOrderNumber(9)]
            PlaceOfService = 9,

            [EnumHelperLibrary.FieldName("Referring Physician(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(10)]
            [EnumHelperLibrary.FieldOrderNumber(10)]
            ReferringPhysician = 10,

            [EnumHelperLibrary.FieldName("Rendering Physician(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(11)]
            [EnumHelperLibrary.FieldOrderNumber(11)]
            RenderingPhysician = 11,

            [EnumHelperLibrary.FieldName("ICD-9 Diagnosis")]
            [EnumHelperLibrary.FieldIdentityNumber(12)]
            [EnumHelperLibrary.FieldOrderNumber(12)]
            IcdDiagnosis = 12,

            [EnumHelperLibrary.FieldName("ICD-9 Procedure(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(13)]
            [EnumHelperLibrary.FieldOrderNumber(13)]
            IcdProcedure = 13,

            [EnumHelperLibrary.FieldName("Attending Physician(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(14)]
            [EnumHelperLibrary.FieldOrderNumber(14)]
            AttendingPhysician = 14,

            [EnumHelperLibrary.FieldName("Total Charges")]
            [EnumHelperLibrary.FieldIdentityNumber(15)]
            [EnumHelperLibrary.FieldOrderNumber(15)]
            // ReSharper disable once UnusedMember.Global
            TotalCharges = 15,

            [EnumHelperLibrary.FieldName("Statement covers period(I)- Dates of service(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(16)]
            [EnumHelperLibrary.FieldOrderNumber(16)]
            // ReSharper disable once UnusedMember.Global
            StatementCoversPeriodToDatesOfService = 16,


            [EnumHelperLibrary.FieldName("Value Codes(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(17)]
            [EnumHelperLibrary.FieldOrderNumber(17)]
            ValueCodes = 17,

            [EnumHelperLibrary.FieldName("Occurrence Code(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(18)]
            [EnumHelperLibrary.FieldOrderNumber(18)]
            OccurrenceCode = 18,

            [EnumHelperLibrary.FieldName("Condition Codes(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(19)]
            [EnumHelperLibrary.FieldOrderNumber(19)]
            ConditionCodes = 19,

            [EnumHelperLibrary.FieldName("Insured’s group")]
            [EnumHelperLibrary.FieldIdentityNumber(20)]
            [EnumHelperLibrary.FieldOrderNumber(20)]
            InsuredGroup = 20,

            [EnumHelperLibrary.FieldName("ASC Fee Schedule")]
            [EnumHelperLibrary.FieldIdentityNumber(21)]
            [EnumHelperLibrary.FieldOrderNumber(21)]
            AscFeeSchedule = 21,

            [EnumHelperLibrary.FieldName("Fee Schedule")]
            [EnumHelperLibrary.FieldIdentityNumber(22)]
            [EnumHelperLibrary.FieldOrderNumber(22)]
            FeeSchedule = 22,

            [EnumHelperLibrary.FieldName("DRG Weight Table")]
            [EnumHelperLibrary.FieldIdentityNumber(23)]
            [EnumHelperLibrary.FieldOrderNumber(23)]
            DrgWeightTable = 23,

            [EnumHelperLibrary.FieldName("Adjudication Request Name")]
            [EnumHelperLibrary.FieldIdentityNumber(-99)]
            [EnumHelperLibrary.FieldOrderNumber(-99)]
            // ReSharper disable once UnusedMember.Global
            AdjudicationRequestName = -99,

            [EnumHelperLibrary.FieldName("ClaimID")]
            [EnumHelperLibrary.FieldIdentityNumber(24)]
            [EnumHelperLibrary.FieldOrderNumber(24)]
            // ReSharper disable once UnusedMember.Global
            ClaimId = 24,

            [EnumHelperLibrary.FieldName("M Care Lab Fee Schedule")]
            [EnumHelperLibrary.FieldIdentityNumber(-1)]
            [EnumHelperLibrary.FieldOrderNumber(-1)]
            MedicareLabFeeSchedule = -1,

            [EnumHelperLibrary.FieldName("Custom Payment Table")]
            [EnumHelperLibrary.FieldIdentityNumber(35)]
            [EnumHelperLibrary.FieldOrderNumber(35)]
            CustomPaymentType = 35,

            [EnumHelperLibrary.FieldName("CustomField1")]
            [EnumHelperLibrary.FieldIdentityNumber(29)]
            [EnumHelperLibrary.FieldOrderNumber(29)]
            CustomField1 = 29,

            [EnumHelperLibrary.FieldName("CustomField2")]
            [EnumHelperLibrary.FieldIdentityNumber(30)]
            [EnumHelperLibrary.FieldOrderNumber(30)]
            CustomField2 = 30,

            [EnumHelperLibrary.FieldName("CustomField3")]
            [EnumHelperLibrary.FieldIdentityNumber(31)]
            [EnumHelperLibrary.FieldOrderNumber(31)]
            CustomField3 = 31,

            [EnumHelperLibrary.FieldName("CustomField4")]
            [EnumHelperLibrary.FieldIdentityNumber(32)]
            [EnumHelperLibrary.FieldOrderNumber(32)]
            CustomField4 = 32,

            [EnumHelperLibrary.FieldName("CustomField5")]
            [EnumHelperLibrary.FieldIdentityNumber(33)]
            [EnumHelperLibrary.FieldOrderNumber(33)]
            CustomField5 = 33,

            [EnumHelperLibrary.FieldName("CustomField6")]
            [EnumHelperLibrary.FieldIdentityNumber(34)]
            [EnumHelperLibrary.FieldOrderNumber(34)]
            CustomField6 = 34,

            [EnumHelperLibrary.FieldName("NPI")]
            [EnumHelperLibrary.FieldIdentityNumber(36)]
            [EnumHelperLibrary.FieldOrderNumber(36)]
            // ReSharper disable once UnusedMember.Global
            Npi = 36,

            [EnumHelperLibrary.FieldName("ICN")]
            [EnumHelperLibrary.FieldIdentityNumber(50)]
            [EnumHelperLibrary.FieldOrderNumber(50)]
            // ReSharper disable once UnusedMember.Global
            Icn = 50,

            [EnumHelperLibrary.FieldName("MRN")]
            [EnumHelperLibrary.FieldIdentityNumber(51)]
            [EnumHelperLibrary.FieldOrderNumber(51)]
            // ReSharper disable once UnusedMember.Global
            Mrn = 51,

            [EnumHelperLibrary.FieldName("Reviewed")]
            [EnumHelperLibrary.FieldIdentityNumber(52)]
            [EnumHelperLibrary.FieldOrderNumber(52)]
            // ReSharper disable once UnusedMember.Global
            Reviewed = 52,

            [EnumHelperLibrary.FieldName("Los")]
            [EnumHelperLibrary.FieldIdentityNumber(53)]
            [EnumHelperLibrary.FieldOrderNumber(53)]
            // ReSharper disable once UnusedMember.Global
            Los = 53,

            [EnumHelperLibrary.FieldName("Age")]
            [EnumHelperLibrary.FieldIdentityNumber(54)]
            [EnumHelperLibrary.FieldOrderNumber(54)]
            // ReSharper disable once UnusedMember.Global
            Age = 54,

            [EnumHelperLibrary.FieldName("Adjudicated Contract Name")]
            [EnumHelperLibrary.FieldIdentityNumber(57)]
            [EnumHelperLibrary.FieldOrderNumber(57)]
            // ReSharper disable once UnusedMember.Global
            AdjudicatedContractName = 57,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            None = 0
        }

        /// <summary>
        /// Claim Field Operator enum.  Complete replica of database table [dbo].[ref.ClaimFieldOperators]
        /// </summary>
        public enum ClaimFieldOperator
        {
            [EnumHelperLibrary.FieldName("<>")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            // ReSharper disable once UnusedMember.Global
            NotEquals = 1,

            [EnumHelperLibrary.FieldName(">")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            GreaterThan = 2,

            [EnumHelperLibrary.FieldName("=")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            // ReSharper disable once UnusedMember.Global
            Equals = 3,

            [EnumHelperLibrary.FieldName("<")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            // ReSharper disable once UnusedMember.Global
            LessThan = 4,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0
        }

        /// <summary>
        /// Adjudication Or Variance Statuses enum.  Complete replica of database table [dbo].[ref.AdjudicationOrVarianceStatuses] 
        /// </summary>
        public enum AdjudicationOrVarianceStatuses
        {
            [EnumHelperLibrary.FieldName("00")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            UnAdjudicated = 1,

            [EnumHelperLibrary.FieldName("01")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            ClaimDataError = 2,

            [EnumHelperLibrary.FieldName("02")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            Adjudicated = 3,

            [EnumHelperLibrary.FieldName("02a")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            AdjudicationErrorMissingContract = 4,

            [EnumHelperLibrary.FieldName("02b")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            AdjudicationErrorMissingServiceLine = 5,

            [EnumHelperLibrary.FieldName("02c")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            AdjudicationErrorMissingPaymentType = 6,


            [EnumHelperLibrary.FieldName("02d")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            AdjudicationErrorInvalidPaymentData = 7,

            [EnumHelperLibrary.FieldName("03")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            // ReSharper disable once UnusedMember.Global
            VarianceComputed = 8,

            [EnumHelperLibrary.FieldName("03b")]
            [EnumHelperLibrary.FieldIdentityNumber(9)]
            [EnumHelperLibrary.FieldOrderNumber(9)]
            // ReSharper disable once UnusedMember.Global
            RemitDataError = 9,

            [EnumHelperLibrary.FieldName("08")]
            [EnumHelperLibrary.FieldIdentityNumber(10)]
            [EnumHelperLibrary.FieldOrderNumber(10)]
            // ReSharper disable once UnusedMember.Global
            ContractualAdjustmentComputed = 10,

            [EnumHelperLibrary.FieldName("0C")]
            [EnumHelperLibrary.FieldIdentityNumber(11)]
            [EnumHelperLibrary.FieldOrderNumber(11)]
            // ReSharper disable once UnusedMember.Global
            CancelledClaim = 11,

            [EnumHelperLibrary.FieldName("00")]
            [EnumHelperLibrary.FieldIdentityNumber(12)]
            [EnumHelperLibrary.FieldOrderNumber(12)]
            // ReSharper disable once UnusedMember.Global
            VVarianceNotComputed = 12,
            [EnumHelperLibrary.FieldName("E1")]
            [EnumHelperLibrary.FieldIdentityNumber(13)]
            [EnumHelperLibrary.FieldOrderNumber(13)]
            // ReSharper disable once UnusedMember.Global
            VVarianceClaimNotAdjudicated = 13,
            [EnumHelperLibrary.FieldName("E2")]
            [EnumHelperLibrary.FieldIdentityNumber(14)]
            [EnumHelperLibrary.FieldOrderNumber(14)]
            // ReSharper disable once UnusedMember.Global
            VVarianceInvalidProjectionData = 14,

            [EnumHelperLibrary.FieldName("01")]
            [EnumHelperLibrary.FieldIdentityNumber(15)]
            [EnumHelperLibrary.FieldOrderNumber(15)]
            // ReSharper disable once UnusedMember.Global
            VVarianceComputed = 15,

            [EnumHelperLibrary.FieldName("02e")]
            [EnumHelperLibrary.FieldIdentityNumber(16)]
            [EnumHelperLibrary.FieldOrderNumber(16)]
            LineItemMatchesAServiceLine = 16,

            [EnumHelperLibrary.FieldName("02f")]
            [EnumHelperLibrary.FieldIdentityNumber(17)]
            [EnumHelperLibrary.FieldOrderNumber(17)]
            NoneOfLineItemsFindAMatchingServiceLine = 17,

            [EnumHelperLibrary.FieldName("AdjudicationError")]
            [EnumHelperLibrary.FieldIdentityNumber(18)]
            [EnumHelperLibrary.FieldOrderNumber(18)]
            AdjudicationError = 18,


            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0
        }

        /// <summary>
        /// Report Type Filter enum, which contains the name of all the reports which are going to be a part of application. Used for section "20.	Report and Data Selection UI"
        /// </summary>
        public enum ReportTypeFilter
        {
            [EnumHelperLibrary.FieldName("Variance Analysis")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            // ReSharper disable once UnusedMember.Global
            VarianceContractReport = 1,
            
            [EnumHelperLibrary.FieldName("Contract Model - Active")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            // ReSharper disable once UnusedMember.Global
            ActiveContractModelReport = 3,

            [EnumHelperLibrary.FieldName("Contract Model - InActive")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            // ReSharper disable once UnusedMember.Global
            InActiveContractModelReport = 5,

            [EnumHelperLibrary.FieldName("Letter Template")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            // ReSharper disable once UnusedMember.Global
            Letters,

            [EnumHelperLibrary.FieldName("Payer Mapping – Mapped Only")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            // ReSharper disable once UnusedMember.Global
            MappedPayerReport,

            [EnumHelperLibrary.FieldName("Payer Mapping – Unmapped Only")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            // ReSharper disable once UnusedMember.Global
            UnMappedPayerReport,

            [EnumHelperLibrary.FieldName("Payer Mapping – All")]
            [EnumHelperLibrary.FieldIdentityNumber(9)]
            [EnumHelperLibrary.FieldOrderNumber(9)]
            // ReSharper disable once UnusedMember.Global
            AllPayerReport,

            [EnumHelperLibrary.FieldName("Model Comparison Report")]
            [EnumHelperLibrary.FieldIdentityNumber(10)]
            [EnumHelperLibrary.FieldOrderNumber(10)]
            // ReSharper disable once UnusedMember.Global
            ModelComparisonReport,

            [EnumHelperLibrary.FieldName("Audits")]
            [EnumHelperLibrary.FieldIdentityNumber(11)]
            [EnumHelperLibrary.FieldOrderNumber(11)]
            // ReSharper disable once UnusedMember.Global
            Audits,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0
        }

        /// <summary>
        /// Date Type Filter enum, which contains date type filter values. Used on section "20.	Report and Data Selection UI"
        /// </summary>
        public enum DateTypeFilter
        {
            [EnumHelperLibrary.FieldName("Date of Service From")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            DateofserviceandAdmissiondate = 1,

            [EnumHelperLibrary.FieldName("Date of Service Thru")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            // ReSharper disable once UnusedMember.Global
            Dateofservicethru = 4,

            [EnumHelperLibrary.FieldName("Date of Billing")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            // ReSharper disable once UnusedMember.Global
            Dateofbilling = 2,

            [EnumHelperLibrary.FieldName("Date of Submission")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            // ReSharper disable once UnusedMember.Global
            Dateofsubmission = 3,

            [EnumHelperLibrary.FieldName("Audit Date Range")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            // ReSharper disable once UnusedMember.Global
            AuditDateRange = 5,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0

        }

        /// <summary>
        /// Report Level Filter enum, which contains report level filter values. Used on section "20.	Report and Data Selection UI"
        /// </summary>
        public enum ReportLevelFilter
        {
            [EnumHelperLibrary.FieldName("Claim Level")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            // ReSharper disable once UnusedMember.Global
            Claim = 1,

            [EnumHelperLibrary.FieldName("Contract Level")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            // ReSharper disable once UnusedMember.Global
            Contract = 2,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0
        }

        /// <summary>
        /// Holds different job status
        /// </summary>
        public enum JobStatus
        {
            [EnumHelperLibrary.FieldName("Requested")]
            [EnumHelperLibrary.FieldIdentityNumber(128)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            Requested = 128,
            [EnumHelperLibrary.FieldName("Running")]
            [EnumHelperLibrary.FieldIdentityNumber(129)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            Running = 129,
            [EnumHelperLibrary.FieldName("Paused")]
            [EnumHelperLibrary.FieldIdentityNumber(130)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            Paused = 130,
            [EnumHelperLibrary.FieldName("Cancelled")]
            [EnumHelperLibrary.FieldIdentityNumber(131)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            Cancelled = 131,
            [EnumHelperLibrary.FieldName("Resumed")]
            [EnumHelperLibrary.FieldIdentityNumber(132)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            Resumed = 132,
            [EnumHelperLibrary.FieldName("Completed")]
            [EnumHelperLibrary.FieldIdentityNumber(100)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            Completed = 100,
            [EnumHelperLibrary.FieldName("Failed")]
            [EnumHelperLibrary.FieldIdentityNumber(133)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            Failed = 133,
            [EnumHelperLibrary.FieldName("Debug")]
            [EnumHelperLibrary.FieldIdentityNumber(301)]
            [EnumHelperLibrary.FieldOrderNumber(9)]
            Debug = 301,
            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None,
            [EnumHelperLibrary.FieldName("All")]
            [EnumHelperLibrary.FieldIdentityNumber(999)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            // ReSharper disable once UnusedMember.Global
            All = 999

        }

        /// <summary>
        /// Different Stop Loss Conditions
        /// </summary>
        public enum StopLossCondition
        {
            [EnumHelperLibrary.FieldName("Total Charge Lines")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            TotalChargeLines = 1,

            [EnumHelperLibrary.FieldName("Per Charge Line")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            PerChargeLine = 2,

            [EnumHelperLibrary.FieldName("Per Day of Stay")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            PerDayofStay = 3,
        }

        /// <summary>
        /// Different Stop Loss Conditions
        /// </summary>
        public enum DownloadFileType
        {
            [EnumHelperLibrary.FieldName("PDF")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            Pdf = 1,

            [EnumHelperLibrary.FieldName("XLS")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            Xls = 2,

            [EnumHelperLibrary.FieldName("CSV")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            Csv = 3,

            [EnumHelperLibrary.FieldName("XLSX")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            Xlsx = 4,

            [EnumHelperLibrary.FieldName("RTF")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            Rtf = 5
        }

        /// <summary>
        /// Table selection Claim Types
        /// </summary>
        public enum TableSelectionClaimType
        {
            [EnumHelperLibrary.FieldName("HCPCS/RATE/HIPPS")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            // ReSharper disable once UnusedMember.Global
            Hcpcs = 4,

            [EnumHelperLibrary.FieldName("DRG(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            // ReSharper disable once UnusedMember.Global
            Drg = 8,
        }

        /// <summary>
        /// Fileter operator(Kendo Grid Filter)
        /// </summary>
        public enum FilterOperator
        {
            [EnumHelperLibrary.FieldName("Start With")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            startswith = 1,
            [EnumHelperLibrary.FieldName("Equal To")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            eq = 2,
            [EnumHelperLibrary.FieldName("Not Equal To")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            neq = 3,
            [EnumHelperLibrary.FieldName("Contains")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            contains = 4,
            [EnumHelperLibrary.FieldName("Greater Than")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            gt = 5,
            [EnumHelperLibrary.FieldName("Greater Than Or Equal")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            gte = 6,
            [EnumHelperLibrary.FieldName("Less Than")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            lt = 7,
            [EnumHelperLibrary.FieldName("Less Than Or Equal")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            lte = 8,
            [EnumHelperLibrary.FieldName("Ends With")]
            [EnumHelperLibrary.FieldIdentityNumber(9)]
            [EnumHelperLibrary.FieldOrderNumber(9)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            endswith = 9,
            [EnumHelperLibrary.FieldName("Is After")]
            [EnumHelperLibrary.FieldIdentityNumber(10)]
            [EnumHelperLibrary.FieldOrderNumber(10)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            af = 10,
            [EnumHelperLibrary.FieldName("Is After Or Equal")]
            [EnumHelperLibrary.FieldIdentityNumber(11)]
            [EnumHelperLibrary.FieldOrderNumber(11)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            aeq = 11,
            [EnumHelperLibrary.FieldName("Is Before")]
            [EnumHelperLibrary.FieldIdentityNumber(12)]
            [EnumHelperLibrary.FieldOrderNumber(12)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            bf = 12,
            [EnumHelperLibrary.FieldName("Is Before Or Equal")]
            [EnumHelperLibrary.FieldIdentityNumber(13)]
            [EnumHelperLibrary.FieldOrderNumber(13)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            beq = 13,
            [EnumHelperLibrary.FieldName("Not Contains")]
            [EnumHelperLibrary.FieldIdentityNumber(14)]
            [EnumHelperLibrary.FieldOrderNumber(14)]
            // ReSharper disable once InconsistentNaming (As its coming from Kendo grid filter we need exact same string)
            // ReSharper disable once UnusedMember.Global
            doesnotcontain = 14,
            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0

        }

        /// <summary>
        /// Refer to operation to be conducted on the given condition
        /// </summary>
        public enum ConditionOperation
        {
            [EnumHelperLibrary.FieldName("Not Equal To")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            NotEqualTo = 1,

            [EnumHelperLibrary.FieldName("GreaterThan")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            GreaterThan = 2,

            [EnumHelperLibrary.FieldName("Equal To")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            EqualTo = 3,

            [EnumHelperLibrary.FieldName("Less Than")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            LessThan = 4,

            [EnumHelperLibrary.FieldName("Greater Than Equal To")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            GreaterThanEqualTo = 5,

            [EnumHelperLibrary.FieldName("Less Than Equal To")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            LessThanEqualTo = 6,

            [EnumHelperLibrary.FieldName("Contains")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            Contains = 7
        }

        /// <summary>
        /// Type of LHS operand in the condition.
        /// Used to identify kind of operation
        /// </summary>
        public enum OperandType
        {
            [EnumHelperLibrary.FieldName("Date")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            Date = 1,

            [EnumHelperLibrary.FieldName("Alpha Numeric")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            AlphaNumeric = 2,

            [EnumHelperLibrary.FieldName("Numeric")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            Numeric = 3,

            [EnumHelperLibrary.FieldName("Custom")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            // ReSharper disable once UnusedMember.Global
            Custom = 4
        }

        /// <summary>
        /// LHS operands for any condition to be evaluated
        /// </summary>
        public enum OperandIdentifier
        {
            [EnumHelperLibrary.FieldName("Patient  Account Number")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            PatientAccountNumber = 1,

            [EnumHelperLibrary.FieldName("Type of Bill (I)")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            BillType = 2,

            [EnumHelperLibrary.FieldName("Revenue Code(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            RevCode = 3,

            [EnumHelperLibrary.FieldName("HCPCS/RATE/HIPPS")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            HcpcsCode = 4,

            [EnumHelperLibrary.FieldName("Payer Name")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            PayerName = 6,

            [EnumHelperLibrary.FieldName("Insured’s ID")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            InsuredId = 7,

            [EnumHelperLibrary.FieldName("DRG(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            Drg = 8,

            [EnumHelperLibrary.FieldName("Place of Service(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(9)]
            [EnumHelperLibrary.FieldOrderNumber(9)]
            PlaceOfService = 9,

            [EnumHelperLibrary.FieldName("Referring Physician(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(10)]
            [EnumHelperLibrary.FieldOrderNumber(10)]
            ReferringPhysician = 10,

            [EnumHelperLibrary.FieldName("Rendering Physician(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(11)]
            [EnumHelperLibrary.FieldOrderNumber(11)]
            RenderingPhysician = 11,

            [EnumHelperLibrary.FieldName("ICD-9 Diagnosis")]
            [EnumHelperLibrary.FieldIdentityNumber(12)]
            [EnumHelperLibrary.FieldOrderNumber(12)]
            IcdDiagnosis = 12,

            [EnumHelperLibrary.FieldName("ICD-9 Procedure(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(13)]
            [EnumHelperLibrary.FieldOrderNumber(13)]
            IcdProcedure = 13,

            [EnumHelperLibrary.FieldName("Attending Physician(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(14)]
            [EnumHelperLibrary.FieldOrderNumber(14)]
            AttendingPhysician = 14,

            [EnumHelperLibrary.FieldName("Total Charges")]
            [EnumHelperLibrary.FieldIdentityNumber(15)]
            [EnumHelperLibrary.FieldOrderNumber(15)]
            TotalCharges = 15,

            [EnumHelperLibrary.FieldName("Statement covers period(I)- Dates of service(P)")]
            [EnumHelperLibrary.FieldIdentityNumber(16)]
            [EnumHelperLibrary.FieldOrderNumber(16)]
            StatementCoversPeriodToDatesOfService = 16,

            [EnumHelperLibrary.FieldName("Value Codes(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(17)]
            [EnumHelperLibrary.FieldOrderNumber(17)]
            ValueCodes = 17,

            [EnumHelperLibrary.FieldName("Occurrence Code(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(18)]
            [EnumHelperLibrary.FieldOrderNumber(18)]
            OccurrenceCode = 18,

            [EnumHelperLibrary.FieldName("Condition Codes(I)")]
            [EnumHelperLibrary.FieldIdentityNumber(19)]
            [EnumHelperLibrary.FieldOrderNumber(19)]
            ConditionCodes = 19,

            [EnumHelperLibrary.FieldName("Insured’s group")]
            [EnumHelperLibrary.FieldIdentityNumber(20)]
            [EnumHelperLibrary.FieldOrderNumber(20)]
            InsuredGroup = 20,

            [EnumHelperLibrary.FieldName("ASC Fee Schedule")]
            [EnumHelperLibrary.FieldIdentityNumber(21)]
            [EnumHelperLibrary.FieldOrderNumber(21)]
            // ReSharper disable once UnusedMember.Global
            AscFeeSchedule = 21,

            [EnumHelperLibrary.FieldName("Fee Schedule")]
            [EnumHelperLibrary.FieldIdentityNumber(22)]
            [EnumHelperLibrary.FieldOrderNumber(22)]
            // ReSharper disable once UnusedMember.Global
            FeeSchedule = 22,

            [EnumHelperLibrary.FieldName("DRG Weight Table")]
            [EnumHelperLibrary.FieldIdentityNumber(23)]
            [EnumHelperLibrary.FieldOrderNumber(23)]
            // ReSharper disable once UnusedMember.Global
            DrgWeightTable = 23,

            [EnumHelperLibrary.FieldName("Adjudication Request Name")]
            [EnumHelperLibrary.FieldIdentityNumber(-99)]
            [EnumHelperLibrary.FieldOrderNumber(-99)]
            // ReSharper disable once UnusedMember.Global
            AdjudicationRequestName = -99,

            [EnumHelperLibrary.FieldName("Claim Id")]
            [EnumHelperLibrary.FieldIdentityNumber(24)]
            [EnumHelperLibrary.FieldOrderNumber(24)]
            // ReSharper disable once UnusedMember.Global
            ClaimId = 24,

            [EnumHelperLibrary.FieldName("Claim State")]
            [EnumHelperLibrary.FieldIdentityNumber(37)]    //25
            [EnumHelperLibrary.FieldOrderNumber(37)]
            ClaimState = 37,

            [EnumHelperLibrary.FieldName("Claim Type")]
            [EnumHelperLibrary.FieldIdentityNumber(26)]
            [EnumHelperLibrary.FieldOrderNumber(26)]
            ClaimType = 26,

            [EnumHelperLibrary.FieldName("Claim Start Date")]
            [EnumHelperLibrary.FieldIdentityNumber(27)]
            [EnumHelperLibrary.FieldOrderNumber(27)]
            ClaimStartDate = 27,

            [EnumHelperLibrary.FieldName("Claim End Date")]
            [EnumHelperLibrary.FieldIdentityNumber(28)]
            [EnumHelperLibrary.FieldOrderNumber(28)]
            ClaimEndDate = 28,

            [EnumHelperLibrary.FieldName("CustomField1")]
            [EnumHelperLibrary.FieldIdentityNumber(29)]
            [EnumHelperLibrary.FieldOrderNumber(29)]
            CustomField1 = 29,

            [EnumHelperLibrary.FieldName("CustomField2")]
            [EnumHelperLibrary.FieldIdentityNumber(30)]
            [EnumHelperLibrary.FieldOrderNumber(30)]
            CustomField2 = 30,

            [EnumHelperLibrary.FieldName("CustomField3")]
            [EnumHelperLibrary.FieldIdentityNumber(31)]
            [EnumHelperLibrary.FieldOrderNumber(31)]
            CustomField3 = 31,

            [EnumHelperLibrary.FieldName("CustomField4")]
            [EnumHelperLibrary.FieldIdentityNumber(32)]
            [EnumHelperLibrary.FieldOrderNumber(32)]
            CustomField4 = 32,

            [EnumHelperLibrary.FieldName("CustomField5")]
            [EnumHelperLibrary.FieldIdentityNumber(33)]
            [EnumHelperLibrary.FieldOrderNumber(33)]
            CustomField5 = 33,

            [EnumHelperLibrary.FieldName("CustomField6")]
            [EnumHelperLibrary.FieldIdentityNumber(34)]
            [EnumHelperLibrary.FieldOrderNumber(34)]
            CustomField6 = 34,

            [EnumHelperLibrary.FieldName("NPI")]
            [EnumHelperLibrary.FieldIdentityNumber(36)]
            [EnumHelperLibrary.FieldOrderNumber(36)]
            Npi = 36,

            [EnumHelperLibrary.FieldName("Discharge Status")]
            [EnumHelperLibrary.FieldIdentityNumber(38)]
            [EnumHelperLibrary.FieldOrderNumber(38)]
            DischargeStatus = 38,

            [EnumHelperLibrary.FieldName("ICN")]
            [EnumHelperLibrary.FieldIdentityNumber(38)]
            [EnumHelperLibrary.FieldOrderNumber(38)]
            Icn = 50,

            [EnumHelperLibrary.FieldName("MRN")]
            [EnumHelperLibrary.FieldIdentityNumber(38)]
            [EnumHelperLibrary.FieldOrderNumber(38)]
            Mrn = 51,

            [EnumHelperLibrary.FieldName("LOS")]
            [EnumHelperLibrary.FieldIdentityNumber(53)]
            [EnumHelperLibrary.FieldOrderNumber(53)]
            Los = 53,

            [EnumHelperLibrary.FieldName("Age")]
            [EnumHelperLibrary.FieldIdentityNumber(54)]
            [EnumHelperLibrary.FieldOrderNumber(54)]
            Age = 54,

            [EnumHelperLibrary.FieldName("CheckDate")]
            [EnumHelperLibrary.FieldIdentityNumber(55)]
            [EnumHelperLibrary.FieldOrderNumber(55)]
            CheckDate = 55,

            [EnumHelperLibrary.FieldName("CheckNumber")]
            [EnumHelperLibrary.FieldIdentityNumber(56)]
            [EnumHelperLibrary.FieldOrderNumber(56)]
            CheckNumber = 56,

            [EnumHelperLibrary.FieldName("None")]
            [EnumHelperLibrary.FieldIdentityNumber(0)]
            [EnumHelperLibrary.FieldOrderNumber(0)]
            // ReSharper disable once UnusedMember.Global
            None = 0

        }

        public enum Modules
        {
            [EnumHelperLibrary.FieldName("Custom Payment Modeling")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            CustomPaymentModeling = 1,

            [EnumHelperLibrary.FieldName("Payment Tables")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            // ReSharper disable once UnusedMember.Global
            PaymentTables = 2,

            [EnumHelperLibrary.FieldName("Claim tool claim field")]
            [EnumHelperLibrary.FieldIdentityNumber(3)]
            [EnumHelperLibrary.FieldOrderNumber(3)]
            ClaimToolClaimField = 3,

            [EnumHelperLibrary.FieldName("Claim tool table selection")]
            [EnumHelperLibrary.FieldIdentityNumber(4)]
            [EnumHelperLibrary.FieldOrderNumber(4)]
            ClaimToolTableSelection = 4,

            [EnumHelperLibrary.FieldName("Reporting")]
            [EnumHelperLibrary.FieldIdentityNumber(5)]
            [EnumHelperLibrary.FieldOrderNumber(5)]
            Reporting = 5,

            [EnumHelperLibrary.FieldName("Adjudication")]
            [EnumHelperLibrary.FieldIdentityNumber(6)]
            [EnumHelperLibrary.FieldOrderNumber(6)]
            Adjudication = 6,

            [EnumHelperLibrary.FieldName("Model comparison report")]
            [EnumHelperLibrary.FieldIdentityNumber(7)]
            [EnumHelperLibrary.FieldOrderNumber(7)]
            ModelComparisonReport = 7,

            [EnumHelperLibrary.FieldName("Maintenance/View")]
            [EnumHelperLibrary.FieldIdentityNumber(8)]
            [EnumHelperLibrary.FieldOrderNumber(8)]
            MaintenanceView = 8,
        }


        public enum Action
        {
            // ReSharper disable once UnusedMember.Global
            Add,
            Delete,
            // ReSharper disable once UnusedMember.Global
            Update,
            Get,
            // ReSharper disable once UnusedMember.Global
            View,
            AddEditPaymentTypeStopLoss,
            AddEdit,
            AddClaimFieldDocs
        }

        /// <summary>
        /// Reviewd Option
        /// </summary>
        public enum ReviewdOption
        {
            [EnumHelperLibrary.FieldName("Yes")]
            [EnumHelperLibrary.FieldIdentityNumber(1)]
            [EnumHelperLibrary.FieldOrderNumber(1)]
            Yes = 1,

            [EnumHelperLibrary.FieldName("No")]
            [EnumHelperLibrary.FieldIdentityNumber(2)]
            [EnumHelperLibrary.FieldOrderNumber(2)]
            No = 2,



        }

        /// <summary>
        /// UserRoles
        /// </summary>
        public enum UserRoles
        {
            SsiAdmin = 1,
            CmAdmin = 2,
            CmUser = 3
        }

        /// <summary>
        /// Email Type
        /// </summary>
        public enum EmailType
        {
            AccountActivation = 1,
            AccountReset = 2,
            PasswordReset = 3,
            RecoverPassword = 4,
            ChangePassword = 5
        }

    }
}

