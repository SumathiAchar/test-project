using System.Collections.Generic;
using System.Text;

namespace SSI.ContractManagement.Shared.Models
{
    public class NRecord
    {
        private string ClaimId { get; set; }
        public List<string> ClaimSuspensionReasons { get; private set; }
        public List<string> LineRejectionReasons { get; private set; }
        public List<string> LineDenialReasons { get; private set; }
        private bool IsValidRecord { get; set; }
        private StringBuilder InterpretedMessage { get; set; }

        private const int RecordLength = 3;

        public NRecord()
        {
            InterpretedMessage = new StringBuilder();
        }

        public NRecord Convert(string inputMRecord)
        {
            NRecord nRecord = new NRecord();
            if (inputMRecord.Length == 200)
            {
                nRecord.ClaimId = inputMRecord.Substring(1, 17).Trim();
                nRecord.IsValidRecord = true;

                nRecord.ClaimSuspensionReasons = new List<string>();
                nRecord.LineRejectionReasons = new List<string>();
                nRecord.LineDenialReasons = new List<string>();

                string claimSuspensionReason = inputMRecord.Substring(18, 48);
                for (int count = 0; count < claimSuspensionReason.Length; count = count + RecordLength)
                {
                    nRecord.ClaimSuspensionReasons.Add(claimSuspensionReason.Substring(count, RecordLength));
                }

                string lineRejectionReasons = inputMRecord.Substring(66, 36);
                for (int count = 0; count < lineRejectionReasons.Length; count = count + RecordLength)
                {
                    nRecord.LineRejectionReasons.Add(lineRejectionReasons.Substring(count, RecordLength));
                }

                string lineDenialReasons = inputMRecord.Substring(102, 18);
                for (int count = 0; count < lineDenialReasons.Length; count = count + RecordLength)
                {
                    nRecord.LineDenialReasons.Add(lineDenialReasons.Substring(count, RecordLength));
                }
            }
            else
            {
                nRecord.IsValidRecord = false;
            }
            return nRecord;
        }
    }
}