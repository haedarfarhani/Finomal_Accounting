using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Domain.Abstractions
{
    public abstract class BaseEntity
    {
        // کلید اصلی (Primary Key) برای تمام جداول
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // تاریخ و زمان ایجاد رکورد
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // تاریخ و زمان آخرین ویرایش (می‌تواند نال باشد)
        public DateTime? UpdatedAt { get; set; }

        // برای پیاده‌سازی حذف نرم (Soft Delete)
        public bool IsDeleted { get; set; } = false;

        // شناسه کاربری که این رکورد را ایجاد کرده است (اختیاری)
        public string? CreatedByUserId { get; set; }

        // شناسه کاربری که این رکورد را ویرایش کرده است (اختیاری)
        public string? UpdatedByUserId { get; set; }
    }
}
