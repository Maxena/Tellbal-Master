using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.System
{
    public class Notification : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DT { get; set; }
        public bool Seen { get; set; }
        public Identity.User User { get; set; }
        public Guid UserId { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
    }
    public class NotificationConfiguartion : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {

        }
    }
}
