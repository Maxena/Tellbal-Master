using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Payment
{
    public class VerificationResponse
    {

        public bool IsSuccess { get { return Status == 100; } set { this.IsSuccess = value; } }
        public int RefID { get; set; }
        public int Status { get; set; }
        public ExtraDetail ExtraDetail { get; set; }
    }

    public class ExtraDetail
    {
        public Transaction Transaction;
    }


    public class Transaction
    {
        public String CardPanHash;
        public String CardPanMask;
    }
}
