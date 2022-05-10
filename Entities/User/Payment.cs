using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.User
{
    public class Payment : BaseEntity<Guid>
    {
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public DateTime DT { get; set; }
        public string Authority { get; set; }
        public String MerchantID { get; set; }
        public bool IsSuccess { get; set; }

        /// <summary>
        ///  شماره تراکنش
        /// </summary>
        public int RefID { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// (پن شماره کارت واریز کننده(چندرقم وسط مخفی 
        /// </summary>
        public string CardPan { get; set; }

        /// <summary>
        /// هش شماره کارت واریز کننده
        /// </summary>
        public string CardHash { get; set; }

        /// <summary>
        /// مقدار کارمزد
        /// </summary>
        public int Fee { get; set; }

        /// <summary>
        /// نوع کارمزد
        /// </summary>
        public string FeeType { get; set; }
    }
    public class PaymentConfiguartion : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
        }
    }
}
