﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Homeschool.Data
{
    [Table("hs_gradebook")]
    public partial class HsGradebook
    {
        [Key]
        [Column("grad_uid")]
        public Guid GradUid { get; set; }
        [Column("grad_less_title")]
        public string GradLessTitle { get; set; } = null!;
        [Column("grad_chap_title")]
        public string GradChapTitle { get; set; } = null!;
        [Column("grad_cour_title")]
        public string GradCourTitle { get; set; } = null!;
        [Column("grad_date_completed", TypeName = "date")]
        public DateTime? GradDateCompleted { get; set; }
        [Column("grad_points_earned")]
        public int? GradPointsEarned { get; set; }
        [Column("grad_total_points")]
        public int? GradTotalPoints { get; set; }
    }
}