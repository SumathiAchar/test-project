using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    public class PaymentTypeMedicareSequesterRepository : BaseRepository, IPaymentTypeMedicareSequesterRepository
    {
        // Variables
        private Database _databaseObj;
        private DbCommand _databaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareSequesterRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareSequesterRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        //FIXED-2016-R3-S2 : Naming convention of stored procedure parameter is wrong. It should be Pascal naming convention. Eg. @Paymenttypedetailid should be @PaymentTypeDetailID.
        /// <summary>
        /// Add and edit payment type Medicare Sequester.
        /// </summary>
        /// <param name="paymentTypePerCase">The payment type Medicare Sequester.</param>
        /// <returns></returns>
        public long AddEditPaymentTypeMedicareSequester(PaymentTypeMedicareSequester paymentTypePerCase)
        {
            //Checks if input request is not null
            if (paymentTypePerCase != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("AddEditMedicareSequesterPayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommand, "@PaymenTypeDetailId", DbType.Int64, paymentTypePerCase.PaymentTypeDetailId);
                _databaseObj.AddInParameter(_databaseCommand, "@Percentage", DbType.Decimal, paymentTypePerCase.Percentage);
                _databaseObj.AddInParameter(_databaseCommand, "@PaymentTypeId", DbType.Int64, paymentTypePerCase.PaymentTypeId);
                _databaseObj.AddInParameter(_databaseCommand, "@ContractId", DbType.Int64, paymentTypePerCase.ContractId);
                _databaseObj.AddInParameter(_databaseCommand, "@ContractServiceTypeId", DbType.Int64, paymentTypePerCase.ServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, paymentTypePerCase.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_databaseObj.ExecuteScalar(_databaseCommand).ToString());
            }
            return 0;
        }

        /// <summary>
        /// Get Payment Type Medicare Sequester.
        /// </summary>
        /// <param name="paymentTypeMedicareSequester"></param>
        /// <returns></returns>
        public PaymentTypeMedicareSequester GetPaymentTypeMedicareSequester(PaymentTypeMedicareSequester paymentTypeMedicareSequester)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommand, "@PaymentTypeID ", DbType.Int64, paymentTypeMedicareSequester.PaymentTypeId);
            _databaseObj.AddInParameter(_databaseCommand, "@ContractID", DbType.Int64, paymentTypeMedicareSequester.ContractId);
            _databaseObj.AddInParameter(_databaseCommand, "@ContractServiceTypeID", DbType.Int64, paymentTypeMedicareSequester.ServiceTypeId);
            _databaseObj.AddInParameter(_databaseCommand, "@ServiceLineTypeId", DbType.Int64, 0);
            _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, paymentTypeMedicareSequester.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet paymentTypePerCaseDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);
            if (paymentTypePerCaseDataSet != null && paymentTypePerCaseDataSet.Tables.Count > 0 &&
                (paymentTypePerCaseDataSet.Tables[0] != null && paymentTypePerCaseDataSet.Tables[0].Rows.Count > 0))
            {
                PaymentTypeMedicareSequester paymentMedicareSequester = new PaymentTypeMedicareSequester
                {
                    Percentage =
                        GetValue<double?>(paymentTypePerCaseDataSet.Tables[0].Rows[0]["Percentage"], typeof (double)),
                    PaymentTypeDetailId =
                        GetValue<long>(paymentTypePerCaseDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"], typeof (long))
                };
                return paymentMedicareSequester;
            }

            //returns response to Business layer
            return null;
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="medicareSequesterData">The medicare sequester data.</param>
        /// <returns></returns>
        // ReSharper disable UnusedParameter.Global - As if Future Medicare Sequester we need to apply at service-type level we don't need to change much logic.
        public static PaymentTypeMedicareSequester GetPaymentType(long? contractId, long? contractServiceTypeId, DataTable medicareSequesterData)
// ReSharper restore UnusedParameter.Global
        {
            PaymentTypeMedicareSequester paymentTypeMedicareSequester = null;
            if (medicareSequesterData != null && medicareSequesterData.Rows.Count > 0)
            {
                paymentTypeMedicareSequester = (from DataRow row in medicareSequesterData.Rows
                                       where
                                           ((row["ContractId"] != DBNull.Value &&
                                             GetValue<long>(row["ContractId"], typeof(long)) == contractId))
                                                select new PaymentTypeMedicareSequester
                                       {
                                           ContractId = DBNull.Value == row["ContractId"]
                                               ? (long?)null
                                               : GetValue<long>(
                                                   row["ContractId"], typeof(long)),
                                           Percentage = GetValue<long>(
                                               row["Percentage"], typeof(long)),
                                           PaymentTypeDetailId = GetValue<long>(row["PaymentTypeDetailID"], typeof(long)),
                                           PaymentTypeId = (byte)Enums.PaymentTypeCodes.MedicareSequester
                                       }).FirstOrDefault();
            }
            return paymentTypeMedicareSequester;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommand.Dispose();
            _databaseObj = null;
        }
    }
}
