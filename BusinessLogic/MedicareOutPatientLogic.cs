using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.MWServiceUtil;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using NetAPCCalc;
using NetAPCEdit;

namespace SSI.ContractManagement.BusinessLogic
{
    public class MedicareOutPatientLogic
    {

        //FIXED - Change these to private variables
        private NetAPCPay _apcPayDll;
        [ThreadStatic]
        static NetAPCDLL instance;

        public static NetAPCDLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetAPCDLL();
                }
                return instance;
            }
        }

        /// <summary>
        /// Initializes repository
        /// </summary>
        private readonly IMedicareOutPatientRepository _medicareOutPatientRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContractLogic"/> class.
        /// </summary>
        public MedicareOutPatientLogic()
        {
            _medicareOutPatientRepository = Factory.CreateInstance<IMedicareOutPatientRepository>();
        }

        /// <summary>
        ///  Actual Method call for Microdyn output retrieval
        /// </summary>
        public MedicareOutPatientResult GetMedicareOutPatientResult(long claimId)
        {
            StringBuilder inputData = new StringBuilder();
            MedicareOutPatientResult medicareOutPatientResult = new MedicareOutPatientResult();

            MicrodynApcEditInput microdynApcEditInput = _medicareOutPatientRepository.GetMicrodynApcEditInput(claimId);

            string cLine = GetCRecordLine(microdynApcEditInput.CRecord);
            string dLine = GetDRecordLine(microdynApcEditInput.DRecord);
            string lLine = GetLRecordLine(microdynApcEditInput.LRecords);

            if (GlobalConfigVariable.IsMicrodynEnabled)
            {
                if (!string.IsNullOrEmpty(cLine) && !string.IsNullOrEmpty(dLine) && !string.IsNullOrEmpty(lLine))
                {
                    int calcTotal = cLine.Length + dLine.Length + (microdynApcEditInput.LRecords.Count * 200);

                    inputData.Append(cLine);
                    inputData.Append(dLine);
                    inputData.Append(lLine);

                    if (inputData.Length != calcTotal)
                    {
                        medicareOutPatientResult.ReturnCode = 1;
                        medicareOutPatientResult.Message = "Claim Data Errors Found";
                        return medicareOutPatientResult;
                    }
                }
                else
                {
                    medicareOutPatientResult.ReturnCode = 1;
                    medicareOutPatientResult.Message = "Claim Data Errors Found";
                    return medicareOutPatientResult;
                }

                Instance.InputLineCount = microdynApcEditInput.LRecords.Count;
                Instance.InputData = inputData.ToString();
                Instance.AutoDenyReject = false;

                Instance.APCEdit();

                if (GlobalConfigVariable.IsMicrodynLogEnabled)
                {
                    Log.LogInfo(Instance.ResultsData + "ClaimId-" + claimId, "MicrodynApcEditDllResultsData");
                    Log.LogInfo(Instance.PaymentInputData + "ClaimId-" + claimId, "MicrodynApcEditDllPaymentInputData");
                }
                if (Instance.APCErrorsFound)
                {
                    //Processing Unsuccessful - Edit errors found
                    //ResultsData contains claim and line item errors

                    switch (Instance.ReturnCode)
                    {
                        case 1:
                            medicareOutPatientResult.ReturnCode = 1;
                            medicareOutPatientResult.Message = "Claim Errors Found";
                            break;
                        case 5:
                            medicareOutPatientResult.ReturnCode = 5;
                            medicareOutPatientResult.Message = "Major claim errors found - processing suspended";
                            break;
                        case 11:
                            medicareOutPatientResult.ReturnCode = 11;
                            medicareOutPatientResult.Message = "Misc. APCActive Error";
                            break;
                        case 98:
                            medicareOutPatientResult.ReturnCode = 98;
                            medicareOutPatientResult.Message = "SQL tables corrupted or loaded incorrectly";
                            break;
                        case 99:
                            medicareOutPatientResult.ReturnCode = 99;
                            medicareOutPatientResult.Message = "APCActive license id expired";
                            break;
                        case 100:
                            medicareOutPatientResult.ReturnCode = 100;
                            medicareOutPatientResult.Message = "SQL data access error";
                            break;
                        // *** Ignore
                    }
                }
                else
                {
                    string inputDataString = Instance.PaymentInputData;

                    MedicareOutPatient objPatientData = microdynApcEditInput.MedicareOutPatientRecord;

                    _apcPayDll = new NetAPCPay
                    {
                        InputLineCount = microdynApcEditInput.LRecords.Count,
                        InputData = inputDataString,

                        // Test data - this gives some value for the total payment amount
                        //InputLineCount = 25,
                        //InputData = @"T$11411-018       001Q00810012000120 T 1200 0000000010000017000020030303                                                                                                                                T$11411-018       002Q00810012000120 T 1200 0000000010000017000020030304                                                                                                                                T$11411-018       003Q00810012000120 T 1200 0000000010000017000020030318                                                                                                                                T$11411-018       004856100000000000 A 2100 0200000010000002098020030303                                                                                                                                T$11411-018       005856100000000000 A 2100 0200000010000002098020030318                                                                                                                                T$11411-018       006850250000000000 A 2100 0200000010000002625020030319                                                                                                                                T$11411-018       007856510000000000 A 2100 0200000010000001890020030319                                                                                                                                T$11411-018       008365400000000000 N 9101 0000000010000005000020030303                                                                                                                                T$11411-018       009907840035900359 X 1100 0000000020000010000020030303                                                                                                                                T$11411-018       010907840035900359 X 1100 0000000030000015000020030304                                                                                                                                T$11411-018       011365400000000000 N 9101 0000000010000005000020030318                                                                                                                                T$11411-018       012907840035900359 X 1100 0000000030000015000020030318                                                                                                                                T$11411-018       013J11000000000000 N 9101 0000000030000001395020030303                                                                                                                                T$11411-018       014J12000000000000 N 9101 0000000010000001885020030303                                                                                                                                T$11411-018       015J15630090500905 K 1100 0000000720001352160020030303                                                                                                                                T$11411-018       016J19400000000000 N 9101 0000000020000003660020030303                                                                                                                                T$11411-018       017J11000000000000 N 9101 0000000030000001395020030304                                                                                                                                T$11411-018       018J12000000000000 N 9101 0000000010000001885020030304                                                                                                                                T$11411-018       019J15630090500905 K 1100 0000000720001352160020030304                                                                                                                                T$11411-018       020J19400000000000 N 9101 0000000020000003660020030304                                                                                                                                T$11411-018       021J34870911509115 G 5100 1000000040000199400020030304                                                                                                                                T$11411-018       022J11000000000000 N 9101 0000000030000001395020030318                                                                                                                                T$11411-018       023J12000000000000 N 9101 0000000010000001885020030318                                                                                                                                T$11411-018       024J15630090500905 K 1100 0000000720001352160020030318                                                                                                                                T$11411-018       025J19400000000000 N 9101 0000000020000003660020030318                                                                                                                                ",
                        //ServiceFromDate = "03/03/2003",
                         
                        ProviderNumber = objPatientData.NPI,
                        ServiceFromDate = Convert.ToString(objPatientData.ServiceDate),
                        BeneDeductible = Convert.ToDouble(objPatientData.BeneDeductible),
                        BloodDeductiblePints = Convert.ToInt32(objPatientData.BloodDeductiblePints),
                        AllowTerminatedProvider = Convert.ToBoolean(objPatientData.AllowTerminatorProvider),
                        AdjustmentFactor = Convert.ToDouble(objPatientData.AdjustFactor),
                        AdjustmentOption = Convert.ToInt32(objPatientData.AdjustmentOptions)
                    };

                    _apcPayDll.APCCalc();

                    if (!_apcPayDll.CalcErrorsFound)
                    {
                        //No Errors found
                        //Parse the V and W records to get the out patient payment details

                        medicareOutPatientResult = GetResultData(_apcPayDll.ResultsData);
                        medicareOutPatientResult.ReturnCode = 0;
                        medicareOutPatientResult.Message = "No errors Found";
                    }
                    else
                    {
                        //Errors found in APC Payment calculation
                        medicareOutPatientResult.ReturnCode = _apcPayDll.ReturnCode;
                        switch (_apcPayDll.ReturnCode)
                        {
                            //Claim Level 'Error' Return Codes
                            case 50:
                                medicareOutPatientResult.Message = "Wage Index not located ";
                                break;

                            case 51:
                                medicareOutPatientResult.Message = "Wage Index = 0 ";
                                break;

                            case 52:
                                medicareOutPatientResult.Message = "Provider file Wage Index reclassification code invalid or missing ";
                                break;

                            case 53:
                                medicareOutPatientResult.Message = "Service date not numeric or < 8/1/2000  Provider file waiver indicator = “Y”. Case cannot be processed. ";
                                break;

                            case 54:
                                medicareOutPatientResult.Message = "Service date < provider effective date ";
                                break;

                            case 55:
                                medicareOutPatientResult.Message = "Invalid provider number ";
                                break;

                            case 56:
                                medicareOutPatientResult.Message = "Invalid beneficiary deductible amount ";
                                break;

                            case 90:
                                medicareOutPatientResult.Message = "Input number of line items in error ";
                                break;

                            case 95:
                                medicareOutPatientResult.Message = "Misc. APCActive Pricer DLL error ";
                                break;

                            case 99:
                                medicareOutPatientResult.Message = "APCActive Pricer License is expired ";
                                break;

                            case 100:
                                medicareOutPatientResult.Message = "SQL data access error ";
                                break;

                            //Reimbursement calculation is terminated for return codes > 50

                            //Line Item Level 'Error' Return Codes

                            case 30:
                                medicareOutPatientResult.Message = "Line item APC is missing or invalid";
                                break;
                            case 38:
                                medicareOutPatientResult.Message = "Line item Discount factor is invalid";
                                break;
                            case 40:
                                medicareOutPatientResult.Message = "Line item Status Indicator is invalid";
                                break;
                            case 48:
                                medicareOutPatientResult.Message = "Line item Payment Adjustment Flag is invalid";
                                break;

                            //Line Item Level 'Processing' Return Codes

                            case 1:
                                medicareOutPatientResult.Message = " Line processed successfully ";
                                break;
                            case 20:
                                medicareOutPatientResult.Message = " Line processed but payment =0, all applied to deductible.  ";
                                break;
                            case 22:
                                medicareOutPatientResult.Message = " Daily coinsurance limitation – claim processed ";
                                break;
                            case 25:
                                medicareOutPatientResult.Message = " Line item has reduced coinsurance amount ";
                                break;
                            case 41:
                                medicareOutPatientResult.Message = " Status Indicator not processed by OPPS Pricer ";
                                break;
                            case 42:
                                medicareOutPatientResult.Message = " APC = 00000 ";
                                break;
                            case 43:
                                medicareOutPatientResult.Message = " Payment Indicator not = 1 or 5 thru 9 ";
                                break;
                            case 44:
                                medicareOutPatientResult.Message = " Status indicator = 'H', but payment indicator not = '6' ";
                                break;
                            case 45:
                                medicareOutPatientResult.Message = " Packaging flag <> '0' ";
                                break;
                            case 46:
                                medicareOutPatientResult.Message = " Line item denial/reject flag not = to 0 and line item action flag not = to 1 ";
                                break;
                            case 47:
                                medicareOutPatientResult.Message = " Line item action flag = 2 or 3 ";
                                break;
                            case 49:
                                medicareOutPatientResult.Message = " Payment Method Flag not = to 0 ";
                                break;
                        }
                        if (GlobalConfigVariable.IsMicrodynLogEnabled)
                        {
                            Log.LogInfo(medicareOutPatientResult.Message + "ClaimId-" + claimId, "MicrodynApcEditDllResultsData");
                        }
                    }

                    _apcPayDll.Dispose();
                }
            }
            else
            {
                medicareOutPatientResult.ReturnCode = 0;
                medicareOutPatientResult.TotalPaymentAmount = 0;
                medicareOutPatientResult.Message = "Microdyne Switched Off";
            }
            return medicareOutPatientResult;
        }

        /// <summary>
        ///  This Method is called for getting C Line values from DB
        /// </summary>
        private string GetCRecordLine(CRecord cRecordDataRaw)
        {
            if (cRecordDataRaw != null)
            {
                //We assume that cRecordDataRaw does not have any null properties. If the db returns null, the data access 
                //layer will be replacing the nulls with string.Empty or 0.

                //REVIEW-MARCH This method has dependencies. Need to use DI.
                CRecordLogic cRecordLogic = new CRecordLogic();
                if (cRecordLogic.ValidateCRecord(cRecordDataRaw))
                {
                    return cRecordLogic.BuildCRecordString(cRecordDataRaw);
                }
            }
            return string.Empty;
        }

        /// <summary>
        ///  This Method is called for getting D Line values from DB
        /// </summary>
        private string GetDRecordLine(DRecord dRecordDataRaw)
        {
            if (dRecordDataRaw != null)
            {
                //We assume that dRecordDataRaw does not have any null properties. If the db returns null, the data access 
                //layer will be replacing the nulls with string.Empty or 0.

                //REVIEW-MARCH This method has dependencies. Need to use DI.
                DRecordLogic dRecordLogic = new DRecordLogic();
                if (dRecordLogic.ValidateDRecord(dRecordDataRaw))
                {
                    return dRecordLogic.BuildDRecordString(dRecordDataRaw);
                }
            }
            return string.Empty;
        }

        /// <summary>
        ///  This Method is called for getting L Line values from DB
        /// </summary>
        private string GetLRecordLine(IEnumerable<LRecord> lRecordList)
        {
            int count = 0;
            StringBuilder lRecordAllLines = new StringBuilder();
            //REVIEW-MARCH This method has dependencies. Need to use DI.
            LRecordLogic lRecordLogic = new LRecordLogic();
            foreach (LRecord lRecordData in lRecordList)
            {
                if (lRecordLogic.ValidateLRecord(lRecordData))
                {
                    lRecordAllLines.Append(lRecordLogic.BuildLRecordString(lRecordData));

                    //count represents only the validated count of L records
                    count = count + 1;
                }
            }
            return lRecordAllLines.ToString();
        }

        private MedicareOutPatientResult GetResultData(string apcResultsData)
        {
            MedicareOutPatientResult result = new MedicareOutPatientResult();

            string vRecord = apcResultsData.Substring(0, 200);
            result.ClaimId = vRecord.Substring(1, 17).Trim();
            result.TotalPaymentAmount = Convert.ToDouble(vRecord.Substring(53, 9));

            string wRecordFull = apcResultsData.Substring(200, apcResultsData.Length - 200);
            int lineItemsCount = (apcResultsData.Length - 200) / 200;  //This will give the number of W Records
            List<string> wRecords = SplitW(wRecordFull, 200).ToList();

            result.ReturnCode = 0;
            result.Message = string.Empty;
            result.LineItemMedicareDetails = new List<WRecordData>();

            if (wRecords.Count == lineItemsCount) //This ensures that the parsing if fine
            {
                foreach (string wRecord in wRecords)
                {
                    WRecordData wRecordData = new WRecordData { LineItemId = Convert.ToInt32(wRecord.Substring(18, 3)) };
                    string wholeNumber = wRecord.Substring(21, 7);
                    string decimalNumber = wRecord.Substring(28, 2);
                    wRecordData.LinePaymentAmount = Convert.ToDouble(wholeNumber + "." + decimalNumber);
                    wRecordData.ReturnCode = 0;
                    wRecordData.ErrorMessage = string.Empty;
                    result.LineItemMedicareDetails.Add(wRecordData);
                }
            }
            else
            {
                return null;
            }

            return result;
        }

        private static IEnumerable<string> SplitW(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
