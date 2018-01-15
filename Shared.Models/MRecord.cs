using System.Collections.Generic;
using System.Text;

namespace SSI.ContractManagement.Shared.Models
{
    public class MRecord
    {
        private string ClaimId { get; set; }
        private string ClaimProcessedFlag { get; set; }
        private string OverallClaimDisposition { get; set; }
        private string ClaimRejectionDisposition { get; set; }
        private string ClaimDenialDisposition { get; set; }
        private string ClaimBackToProviderDisposition { get; set; }
        private string ClaimSuspensionDisposition { get; set; }
        private string LineItemRejectionDisposition { get; set; }
        private string LineItemDenialDisposition { get; set; }
        public List<string> ClaimRejectionReasons { get; private set; }
        public List<string> ClaimDenialReasons { get; private set; }
        public List<string> ClaimBackToProviderReasons { get; private set; }
        private bool IsValidRecord { get; set; }
        private StringBuilder InterpretedMessage { get; set; }

        private const int RecordLength = 3;

        public MRecord()
        {
            InterpretedMessage = new StringBuilder();
        }

        public MRecord Convert(string inputMRecord)
        {
            MRecord mRecord = new MRecord();
            if (inputMRecord.Length == 200)
            {
                mRecord.ClaimId = inputMRecord.Substring(1, 17).Trim();
                mRecord.ClaimProcessedFlag = inputMRecord.Substring(18, 1);
                mRecord.OverallClaimDisposition = inputMRecord.Substring(19, 1);
                mRecord.ClaimRejectionDisposition = inputMRecord.Substring(20, 1);
                mRecord.ClaimDenialDisposition = inputMRecord.Substring(21, 1);
                mRecord.ClaimBackToProviderDisposition = inputMRecord.Substring(22, 1);
                mRecord.ClaimSuspensionDisposition = inputMRecord.Substring(23, 1);
                mRecord.LineItemRejectionDisposition = inputMRecord.Substring(24, 1);
                mRecord.LineItemDenialDisposition = inputMRecord.Substring(25, 1);
                mRecord.IsValidRecord = true;
                mRecord.ClaimRejectionReasons = new List<string>();
                mRecord.ClaimDenialReasons = new List<string>();
                mRecord.ClaimBackToProviderReasons = new List<string>();

                string claimRejectionReason = inputMRecord.Substring(26, 12);
                for (int count = 0; count < claimRejectionReason.Length; count = count + RecordLength)
                {
                    mRecord.ClaimRejectionReasons.Add(claimRejectionReason.Substring(count, RecordLength));
                }

                string claimDenialReasons = inputMRecord.Substring(38, 24);
                for (int count = 0; count < claimDenialReasons.Length; count = count + RecordLength)
                {
                    mRecord.ClaimDenialReasons.Add(claimDenialReasons.Substring(count, RecordLength));
                }

                string claimBackToProviderReasons = inputMRecord.Substring(62, 90);
                for (int count = 0; count < claimBackToProviderReasons.Length; count = count + RecordLength)
                {
                    mRecord.ClaimBackToProviderReasons.Add(claimBackToProviderReasons.Substring(count, RecordLength));
                }    
            }
            else
            {
                mRecord.IsValidRecord = false;
            }
            
            return mRecord;
        }
    }
}