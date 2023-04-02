using System;
using System.Collections.Generic;
using IquraSchool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IquraSchool.Data;

public partial class DbiquraSchoolContext : DbContext
{
    public DbiquraSchoolContext()
    {

    }

    public DbiquraSchoolContext(DbContextOptions<DbiquraSchoolContext> options): base(options)
    {

    }

    public virtual DbSet<AcademicYear> AcademicYears { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<ScheduleInfo> ScheduleInfos { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseNpgsql("Host=postgres_iqura_container;Port=5432;Database=DBIquraSchool;Username=root;Password=root;Pooling=true;Include Error Detail=true;");
    => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=DBIquraSchool;Username=root;Password=root;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicYear>(entity =>
        {
            entity
                .ToTable("AcademicYear");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.HasIndex(e => e.SubjectId, "IX_Course_SubjectId");

            entity.HasIndex(e => e.TeacherId, "IX_Course_TeacherId");

            entity.HasOne(d => d.Subject).WithMany(p => p.Courses)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Subject");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Teacher");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.ToTable("Grade");

            entity.HasIndex(e => e.CourseId, "IX_Grade_CourseId");

            entity.HasIndex(e => e.StudentId, "IX_Grade_StudentId");

            entity.Property(e => e.Date).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Grade1).HasColumnName("Grade");

            entity.HasOne(d => d.Course).WithMany(p => p.Grades)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grade_Course");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grade_Student");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group");

            entity.HasIndex(e => e.HeadTeacherId, "IX_Group").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(4);

            entity.Property(e => e.HeadTeacherId)
                .IsRequired(false);

            entity.HasOne(d => d.HeadTeacher).WithOne(p => p.Group)
                .HasForeignKey<Group>(d => d.HeadTeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_Teacher");
        });

        modelBuilder.Entity<ScheduleInfo>(entity =>
        {
            entity.ToTable("Schedule_Info");

            entity.HasIndex(e => e.CourseId, "IX_Schedule_Info_CourseId");

            entity.HasIndex(e => e.GroupId, "IX_Schedule_Info_GroupId");

            entity.HasOne(d => d.Course).WithMany(p => p.ScheduleInfos)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Info_Course");

            entity.HasOne(d => d.Group).WithMany(p => p.ScheduleInfos)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Info_Group");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.HasIndex(e => e.GroupId, "IX_Student_GroupId");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Group).WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Group");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
