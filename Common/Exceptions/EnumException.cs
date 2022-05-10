using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public enum BadRequest
    {
        /// <summary>
        /// بازه ی درصدی اشتباه
        /// </summary>
        PricingRateError,

        /// <summary>
        /// فرمت اشتباه در مقدار بازه
        /// </summary>
        PricingRateInCorrectFormat,

        /// <summary>
        /// فرمت اشتباه در متن خطا و توضیح خطا
        /// </summary>
        PricingErrorInCorrectFormat,


        /// <summary>
        /// فرمت اشتباه در خطا و مقدار بازه
        /// </summary>
        PricingBothRateAndErrorInCorrectFormant,
    }
}
