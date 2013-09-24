﻿using System;
using System.Collections.Generic;
using System.Linq;
using BillsManager.Model;
using Caliburn.Micro;

namespace BillsManager.ViewModel
{
    public partial class BackupViewModel : PropertyChangedBase
    {
        #region fields
        #endregion

        #region ctor

        public BackupViewModel(
            Backup backup)
        {
            if (backup == null)
                throw new ArgumentNullException("backup cannot be null.");

            this.exposedBackup = backup;
        }

        #endregion

        #region properties

        private Backup exposedBackup;
        public Backup ExposedBackup
        {
            get { return this.exposedBackup; }
            protected set
            {
                if (this.exposedBackup != value)
                {
                    this.exposedBackup = value;
                    this.NotifyOfPropertyChange(() => this.ExposedBackup);
                }
            }
        }
        
        #region wrapped from backup

        public string Path
        {
            get { return this.ExposedBackup.Path; }
        }

        public DateTime CreationTime
        {
            get { return this.ExposedBackup.CreationTime; }
        }

        public IEnumerable<DateTime> RollbackDates
        {
            get { return this.ExposedBackup.RollbackDates; }
        }

        public uint SuppliersCount
        {
            get { return this.ExposedBackup.SuppliersCount; }
        }

        public uint BillsCount
        {
            get { return this.ExposedBackup.BillsCount; }
        }

        #endregion

        #region added

        public bool HasRollbacks
        {
            get { return this.TimesUsedForRollback > 0; }
        }

        public bool HasNoRollbacks
        {
            get { return !this.HasRollbacks; }
        }

        public ushort TimesUsedForRollback
        {
            get { return (ushort)this.RollbackDates.Count(); }
        }

        #endregion

        #endregion

        #region methods
        #endregion
    }
}
