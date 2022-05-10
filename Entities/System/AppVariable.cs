using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.System
{
    public class AppVariable : BaseEntity
    {
        public string AboutUs { get; set; }
        public string SecurityAndPrivacy { get; set; }
        public string TermsAndConditions { get; set; }
    }
    public class AppVariableConfiguration : IEntityTypeConfiguration<AppVariable>
    {
        public AppVariableConfiguration()
        {

        }
        public void Configure(EntityTypeBuilder<AppVariable> builder)
        {
            builder.HasData(new AppVariable
            {
                Id = 1,
                AboutUs = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ",
                SecurityAndPrivacy = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ",
                TermsAndConditions = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ"
            });
        }
    }
}
