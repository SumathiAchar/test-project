using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ip"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ip")]
    public class MedicareIpAcuteOptionLogic
    {
        private readonly IMedicareIpAcuteOptionRepository _medicareIpAcuteOptionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareIpAcuteOptionLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MedicareIpAcuteOptionLogic(string connectionString)
        {
            _medicareIpAcuteOptionRepository = Factory.CreateInstance<IMedicareIpAcuteOptionRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareIpAcuteOptionLogic"/> class.
        /// </summary>
        /// <param name="medicareIpAcuteOptionRepository">The medicare ip acute option repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ip"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ip")]
        public MedicareIpAcuteOptionLogic(IMedicareIpAcuteOptionRepository medicareIpAcuteOptionRepository)
        {
            if (medicareIpAcuteOptionRepository != null)
            {
                _medicareIpAcuteOptionRepository = medicareIpAcuteOptionRepository;
            }
        }

        /// <summary>
        /// Gets the medicare ip acute options.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ip"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ip")]
        public List<MedicareIpAcuteOption> GetMedicareIpAcuteOptions()
        {
            return _medicareIpAcuteOptionRepository.GetMedicareIpAcuteOptions();
        }
    }
}
