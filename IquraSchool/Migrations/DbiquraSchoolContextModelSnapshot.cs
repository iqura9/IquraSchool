﻿// <auto-generated />
using System;
using IquraSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IquraSchool.Migrations
{
    [DbContext(typeof(DbiquraSchoolContext))]
    partial class DbiquraSchoolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IquraSchool.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<int>("SubjectId")
                        .HasColumnType("integer");

                    b.Property<int>("TeacherId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Course", (string)null);
                });

            modelBuilder.Entity("IquraSchool.Models.Grade", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<byte>("Absent")
                        .HasColumnType("smallint");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp");

                    b.Property<int?>("Grade1")
                        .HasColumnType("integer")
                        .HasColumnName("Grade");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Grade", (string)null);
                });

            modelBuilder.Entity("IquraSchool.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<int>("HeadTeacherId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "HeadTeacherId" }, "IX_Group")
                        .IsUnique();

                    b.ToTable("Group", (string)null);
                });

            modelBuilder.Entity("IquraSchool.Models.ScheduleInfo", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("DayOfTheWeek")
                        .HasColumnType("integer");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int>("LessonNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("GroupId");

                    b.ToTable("Schedule_Info", (string)null);
                });

            modelBuilder.Entity("IquraSchool.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Student", (string)null);
                });

            modelBuilder.Entity("IquraSchool.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Subject", (string)null);
                });

            modelBuilder.Entity("IquraSchool.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Teacher", (string)null);
                });

            modelBuilder.Entity("IquraSchool.Models.Course", b =>
                {
                    b.HasOne("IquraSchool.Models.Subject", "Subject")
                        .WithMany("Courses")
                        .HasForeignKey("SubjectId")
                        .IsRequired()
                        .HasConstraintName("FK_Course_Subject");

                    b.HasOne("IquraSchool.Models.Teacher", "Teacher")
                        .WithMany("Courses")
                        .HasForeignKey("TeacherId")
                        .IsRequired()
                        .HasConstraintName("FK_Course_Teacher");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("IquraSchool.Models.Grade", b =>
                {
                    b.HasOne("IquraSchool.Models.Course", "Course")
                        .WithMany("Grades")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK_Grade_Course");

                    b.HasOne("IquraSchool.Models.Student", "Student")
                        .WithMany("Grades")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK_Grade_Student");

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("IquraSchool.Models.Group", b =>
                {
                    b.HasOne("IquraSchool.Models.Teacher", "HeadTeacher")
                        .WithOne("Group")
                        .HasForeignKey("IquraSchool.Models.Group", "HeadTeacherId")
                        .IsRequired()
                        .HasConstraintName("FK_Group_Teacher");

                    b.Navigation("HeadTeacher");
                });

            modelBuilder.Entity("IquraSchool.Models.ScheduleInfo", b =>
                {
                    b.HasOne("IquraSchool.Models.Course", "Course")
                        .WithMany("ScheduleInfos")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK_Schedule_Info_Course");

                    b.HasOne("IquraSchool.Models.Group", "Group")
                        .WithMany("ScheduleInfos")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK_Schedule_Info_Group");

                    b.Navigation("Course");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("IquraSchool.Models.Student", b =>
                {
                    b.HasOne("IquraSchool.Models.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK_Student_Group");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("IquraSchool.Models.Course", b =>
                {
                    b.Navigation("Grades");

                    b.Navigation("ScheduleInfos");
                });

            modelBuilder.Entity("IquraSchool.Models.Group", b =>
                {
                    b.Navigation("ScheduleInfos");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("IquraSchool.Models.Student", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("IquraSchool.Models.Subject", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("IquraSchool.Models.Teacher", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Group");
                });
#pragma warning restore 612, 618
        }
    }
}
