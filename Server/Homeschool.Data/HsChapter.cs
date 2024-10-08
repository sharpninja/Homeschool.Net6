﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Homeschool.Data
{
    [Table("hs_chapter")]
    [Index(nameof(ChapCourUid), Name = "IX_hs_chapter_chap_cour_uid")]
    public partial class HsChapter
    {
        public HsChapter()
        {
            HsLessons = new HashSet<HsLesson>();
        }

        [Key]
        [Column("chap_uid")]
        public Guid ChapUid { get; set; }
        [Column("chap_cour_uid")]
        public Guid? ChapCourUid { get; set; }
        [Column("chap_title")]
        public string ChapTitle { get; set; } = null!;
        [Column("chap_slug")]
        public string ChapSlug { get; set; } = null!;
        [Column("chap_display_order")]
        public int ChapDisplayOrder { get; set; }

        [ForeignKey(nameof(ChapCourUid))]
        [InverseProperty(nameof(HsCourse.HsChapters))]
        public virtual HsCourse? ChapCourU { get; set; }
        [InverseProperty(nameof(HsLesson.LessChapU))]
        public virtual ICollection<HsLesson> HsLessons { get; set; }
    }
}