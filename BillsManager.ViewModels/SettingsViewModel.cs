﻿using BillsManager.Localization;
using BillsManager.Models;
using BillsManager.Services;
using BillsManager.ViewModels.Commanding;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BillsManager.ViewModels
{
    public partial class SettingsViewModel : Screen
    {
        #region fields

        private readonly ISettingsService settingsProvider;

        private readonly Settings oldSettings;

        #endregion

        #region ctor

        public SettingsViewModel(
            ISettingsService settingsProvider)
        {
            this.settingsProvider = settingsProvider;

            this.oldSettings = (Settings)this.settingsProvider.Settings.Clone();

            this.DisplayName = TranslationManager.Instance.Translate("Settings").ToString();
        }

        #endregion

        #region properties

        public IEnumerable<CultureInfo> AvailableLanguages
        {
            get { return TranslationManager.Instance.Languages; }
        }

        public CultureInfo CurrentLanguage
        {
            get { return this.settingsProvider.Settings.Language; }
            set
            {
                if (this.CurrentLanguage == value) return;

                this.settingsProvider.Settings.Language = value;
                this.NotifyOfPropertyChange(() => this.CurrentLanguage);
                TranslationManager.Instance.CurrentLanguage = value;
            }
        }

        public bool StartupDBLoad
        {
            get { return this.settingsProvider.Settings.StartupDBLoad; }
            set
            {
                if (this.StartupDBLoad == value) return;

                this.settingsProvider.Settings.StartupDBLoad = value;
                this.NotifyOfPropertyChange(() => this.StartupDBLoad);
            }
        }

        public string FeedbackToEmailAddress
        {
            get { return this.settingsProvider.Settings.FeedbackToEmailAddress; }
            set
            {
                if (this.FeedbackToEmailAddress == value) return;

                this.settingsProvider.Settings.FeedbackToEmailAddress = value;
                this.NotifyOfPropertyChange(() => this.FeedbackToEmailAddress);
            }
        }

        #endregion

        #region methods

        protected void RevertChanges()
        {
            try
            {
                this.CurrentLanguage = this.oldSettings.Language;
                this.StartupDBLoad = this.oldSettings.StartupDBLoad;
                this.FeedbackToEmailAddress = this.oldSettings.FeedbackToEmailAddress;
            }
            catch (Exception) { }
        }

        protected void SaveAndClose()
        {
            this.settingsProvider.Save();
            this.TryClose();
        }

        protected void CancelAndClose()
        {
            this.RevertChanges();
            this.SaveAndClose();
        }

        #endregion

        #region commands

        private RelayCommand saveAndCloseCommand;
        public RelayCommand SaveAndCloseCommand
        {
            get
            {
                return this.saveAndCloseCommand ?? (this.saveAndCloseCommand = 
                    new RelayCommand(
                        () => this.SaveAndClose()));
            }
        }

        private RelayCommand cancelAndCloseCommand;
        public RelayCommand CancelAndCloseCommand
        {
            get
            {
                return this.cancelAndCloseCommand ?? (this.cancelAndCloseCommand = 
                    new RelayCommand(
                        () => this.CancelAndClose()));
            }
        }

        #endregion
    }
}