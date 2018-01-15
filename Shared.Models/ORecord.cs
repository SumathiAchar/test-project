using System.Collections.Generic;
using System.Text;

namespace SSI.ContractManagement.Shared.Models
{
    public class ORecord
    {
        private string ClaimId { get; set; }
        public List<string> Diagnosis1 { get; private set; }
        public List<string> Diagnosis2 { get; private set; }
        public List<string> Diagnosis3 { get; private set; }
        public List<string> Diagnosis4 { get; private set; }
        public List<string> Diagnosis5 { get; private set; }
        private bool IsValidRecord { get; set; }
        private StringBuilder InterpretedMessage { get; set; }

        private const int RecordLength = 3;

        public ORecord()
        {
            InterpretedMessage = new StringBuilder();
        }

        public ORecord Convert(string inputMRecord)
        {
            ORecord oRecord = new ORecord();
            if (inputMRecord.Length == 200)
            {
                oRecord.ClaimId = inputMRecord.Substring(1, 17).Trim();
                oRecord.IsValidRecord = true;

                oRecord.Diagnosis1 = new List<string>();
                oRecord.Diagnosis2 = new List<string>();
                oRecord.Diagnosis3 = new List<string>();
                oRecord.Diagnosis4 = new List<string>();
                oRecord.Diagnosis5 = new List<string>();

                string diagnosis1 = inputMRecord.Substring(18, 24);
                for (int count = 0; count < diagnosis1.Length; count = count + RecordLength)
                {
                    oRecord.Diagnosis1.Add(diagnosis1.Substring(count, RecordLength));
                }

                string diagnosis2 = inputMRecord.Substring(42, 24);
                for (int count = 0; count < diagnosis2.Length; count = count + RecordLength)
                {
                    oRecord.Diagnosis2.Add(diagnosis2.Substring(count, RecordLength));
                }

                string diagnosis3 = inputMRecord.Substring(66, 24);
                for (int count = 0; count < diagnosis3.Length; count = count + RecordLength)
                {
                    oRecord.Diagnosis3.Add(diagnosis3.Substring(count, RecordLength));
                }

                string diagnosis4 = inputMRecord.Substring(90, 24);
                for (int count = 0; count < diagnosis4.Length; count = count + RecordLength)
                {
                    oRecord.Diagnosis4.Add(diagnosis4.Substring(count, RecordLength));
                }

                string diagnosis5 = inputMRecord.Substring(114, 24);
                for (int count = 0; count < diagnosis5.Length; count = count + RecordLength)
                {
                    oRecord.Diagnosis5.Add(diagnosis5.Substring(count, RecordLength));
                }
            }
            return oRecord;
        }
    }
}
