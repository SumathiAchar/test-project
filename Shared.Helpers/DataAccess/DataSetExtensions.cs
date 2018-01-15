using System.Data;


namespace SSI.ContractManagement.Shared.Helpers.DataAccess
{
    public static class DataSetExtensions
    {
        /// <summary>
        /// Determines whether [is table data populated] [the specified data set].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns>
        ///   <c>true</c> if [is table data populated] [the specified data set]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTableDataPopulated(this DataSet dataSet)
        {
            return dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Determines whether [is table data populated] [the specified data set].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tableIndex">Index of the table in dataset. First table will always have index 0.</param>
        /// <returns>
        ///   <c>true</c> if [is table data populated] [the specified data set]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTableDataPopulated(this DataSet dataSet, int tableIndex)
        {
            return dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables.Count >= tableIndex && dataSet.Tables[tableIndex].Rows.Count > 0;
        }
    }
}
