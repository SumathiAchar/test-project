using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class MedicareLabFeeScheduleLogic
    {
        /// <summary>
        /// The _payment table repository
        /// </summary>
        private readonly IMedicareLabFeeScheduleRepository _medicareLabFeeScheduleRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareLabFeeScheduleLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MedicareLabFeeScheduleLogic(string connectionString)
        {
            _medicareLabFeeScheduleRepository = Factory.CreateInstance<IMedicareLabFeeScheduleRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTableLogic" /> class.
        /// </summary>
        /// <param name="medicareLabFeeScheduleRepository">The medicare lab fee schedule repository.</param>
        public MedicareLabFeeScheduleLogic(IMedicareLabFeeScheduleRepository medicareLabFeeScheduleRepository)
        {
            if (medicareLabFeeScheduleRepository != null)
            {
                _medicareLabFeeScheduleRepository = medicareLabFeeScheduleRepository;
            }
        }

        /// <summary>
        /// Gets the medicare lab fee schedule table names.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public List<MedicareLabFeeSchedule> GetMedicareLabFeeScheduleTableNames(MedicareLabFeeSchedule mCareFeeSchedule)
        {
            return _medicareLabFeeScheduleRepository.GetMedicareLabFeeScheduleTableNames(mCareFeeSchedule);
        }

        /// <summary>
        /// Gets the m care lab fee schedule table data.
        /// </summary>
        /// <param name="medicareLabFeeSchedule">The m care lab fee schedule table.</param>
        /// <returns></returns>
        public MedicareLabFeeScheduleResult GetMedicareLabFeeSchedule(
            MedicareLabFeeSchedule medicareLabFeeSchedule)
        {
            return _medicareLabFeeScheduleRepository.GetMedicareLabFeeSchedule(medicareLabFeeSchedule);
        }
    }
}
