﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Homeschool.Data;
using Homeschool.Data.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
#nullable enable

namespace Homeschool.Data.Context
{
    public partial class HomeschoolContext : DbContext
    {
        public HomeschoolContext(DbContextOptions<HomeschoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HsChapter> HsChapters { get; set; } = null!;
        public virtual DbSet<HsCourse> HsCourses { get; set; } = null!;
        public virtual DbSet<HsGradebook> HsGradebooks { get; set; } = null!;
        public virtual DbSet<HsLesson> HsLessons { get; set; } = null!;
        public virtual DbSet<HsSubject> HsSubjects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.HsChapterConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.HsCourseConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.HsGradebookConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.HsLessonConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.HsSubjectConfiguration());

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
