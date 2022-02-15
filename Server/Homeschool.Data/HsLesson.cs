﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Homeschool.Data
{
    [Table("hs_lesson")]
    [Index(nameof(LessChapUid), Name = "IX_hs_lesson_less_chap_uid")]
    public partial class HsLesson
    {
        [Key]
        [Column("less_uid")]
        public Guid LessUid { get; set; }
        [Column("less_chap_uid")]
        public Guid LessChapUid { get; set; }
        [Column("less_title")]
        public string LessTitle { get; set; } = null!;
        [Column("less_url")]
        public string LessUrl { get; set; } = null!;
        [Column("less_points_earned")]
        public int? LessPointsEarned { get; set; }
        [Column("less_total_points")]
        public int? LessTotalPoints { get; set; }
        [Column("less_date_completed", TypeName = "date")]
        public DateTime? LessDateCompleted { get; set; }
        [Column("less_date_due", TypeName = "date")]
        public DateTime? LessDateDue { get; set; }
        [Column("less_is_completed")]
        public bool LessIsCompleted { get; set; }
        [Column("less_grade", TypeName = "money")]
        public decimal? LessGrade { get; set; }
        [Column("less_slug")]
        public string LessSlug { get; set; } = null!;
        [Column("less_display_order")]
        public int LessDisplayOrder { get; set; }
        [Column("less_skip_lesson")]
        public bool LessSkipLesson { get; set; }

        [ForeignKey(nameof(LessChapUid))]
        [InverseProperty(nameof(HsChapter.HsLessons))]
        public virtual HsChapter LessChapU { get; set; } = null!;
    }
}