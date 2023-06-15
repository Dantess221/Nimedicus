using Nimedicus.Model.DatabaseDataModels;
using SharpVectors.Dom.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Event = Nimedicus.Model.DatabaseDataModels.Event;

namespace Nimedicus.Model
{
    public class DataContext : DbContext
    {
        public DataContext() : base("Nimedicus")
        {
        }

        public DbSet<Auth> Auths { get; set; }
        public DbSet<BaseUser> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<AnalysData> Analysis { get; set; }
        public DbSet<ReceiptData> Receipts { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOptional(p => p.Doctor)
                .WithMany(d => d.AttachedPatients)
                .HasForeignKey(p => p.DoctorLogin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.DoctorsNotes)
                .WithRequired(n => n.Patient)
                .HasForeignKey(n => n.PatientLogin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Analysis)
                .WithRequired(a => a.Patient)
                .HasForeignKey(a => a.PatientLogin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Receipts)
                .WithRequired(r => r.Patient)
                .HasForeignKey(r => r.PatientLogin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.History)
                .WithRequired(e => e.Patient)
                .HasForeignKey(e => e.PatientLogin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Note>()
                .HasRequired(n => n.AttachedNurse)
                .WithMany()
                .HasForeignKey(n => n.AttachedNurseId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
