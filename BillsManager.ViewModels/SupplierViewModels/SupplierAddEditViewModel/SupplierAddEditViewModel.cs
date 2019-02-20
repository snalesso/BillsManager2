﻿using BillsManager.Localization;
using BillsManager.Localization.Attributes;
using BillsManager.Models;
using BillsManager.ViewModels.Commanding;
using Caliburn.Micro;
using System;

namespace BillsManager.ViewModels
{
    public partial class SupplierAddEditViewModel : SupplierViewModel
    {
        #region fields

        private readonly IWindowManager windowManager;
        private readonly IEventAggregator dbEventAggregator;

        #endregion

        #region ctor

        public SupplierAddEditViewModel(
            IWindowManager windowManager,
            IEventAggregator dbEventAggregator,
            Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException("supplier cannot be null.");

            // SERVICES
            this.exposedSupplier = supplier;
            this.windowManager = windowManager;
            this.dbEventAggregator = dbEventAggregator;

            // SUBSCRIPTIONS
            this.dbEventAggregator.Subscribe(this);

            // HANDLERS
            this.Deactivated +=
                (s, e) =>
                {
                    if (e.WasClosed)
                    {
                        this.dbEventAggregator.Unsubscribe(this);
                    }
                };

            this.rulesTracker = new Validation.ValidationRulesTracker<SupplierAddEditViewModel>(this);
        }

        #endregion

        #region properties

        #region wrapped from supplier

        [LocalizedRequired(ErrorMessageKey = "NameRequired")]
        public override string Name
        {
            get { return base.Name; }
            set
            {
                if (this.Name != value)
                {
                    base.Name = value;
                    this.NotifyOfPropertyChange(() => this.IsValid);
                    this.HasChanges = true;
                }
            }
        }

        public override string eMail
        {
            get { return base.eMail; }
            set
            {
                if (this.eMail != value)
                {
                    base.eMail = value;
                    this.HasChanges = true;
                }
            }
        }

        public override string Website
        {
            get { return base.Website; }
            set
            {
                if (this.Website != value)
                {
                    base.Website = value;
                    this.HasChanges = true;
                }
            }
        }

        public override string Phone
        {
            get { return base.Phone; }
            set
            {
                base.Phone = value;
                this.HasChanges = true;
            }
        }

        public override string Fax
        {
            get { return base.Fax; }
            set
            {
                base.Fax = value;
                this.HasChanges = true;
            }
        }

        [LocalizedStringLength(200, ErrorMessageFormatKey = "CannotExceedChars_format")]
        public override string Notes
        {
            get { return base.Notes; }
            set
            {
                base.Notes = value;
                this.NotifyOfPropertyChange(() => this.IsValid);
                this.HasChanges = true;
            }
        }

        public override string AgentName
        {
            get { return base.AgentName; }
            set
            {
                base.AgentName = value;
                this.NotifyOfPropertyChange(() => this.AgentName);
                this.HasChanges = true;
            }
        }

        public override string AgentSurname
        {
            get { return base.AgentSurname; }
            set
            {
                base.AgentSurname = value;
                this.NotifyOfPropertyChange(() => this.AgentSurname);
                this.HasChanges = true;
            }
        }

        public override string AgentPhone
        {
            get { return base.AgentPhone; }
            set
            {
                base.AgentPhone = value;
                this.NotifyOfPropertyChange(() => this.AgentPhone);
                this.HasChanges = true;
            }
        }

        #region address

        public override string Street
        {
            get { return base.Street; }
            set
            {
                if (this.Street != value)
                {
                    base.Street = value;
                    this.NotifyOfPropertyChange(() => this.IsValid);
                    this.HasChanges = true;
                }
            }
        }

        public override string Number
        {
            get { return base.Number; }
            set
            {
                if (this.Number != value)
                {
                    base.Number = value;
                    this.NotifyOfPropertyChange(() => this.IsValid);
                    this.HasChanges = true;
                }
            }
        }

        public override string City
        {
            get { return base.City; }
            set
            {
                if (this.City != value)
                {
                    base.City = value;
                    this.NotifyOfPropertyChange(() => this.IsValid);
                    this.HasChanges = true;
                }
            }
        }

        //[Required(ErrorMessage = "You must specify a zip.")]
        [LocalizedStringLength(5, MinimumLength = 5, ErrorMessageFormatKey = "MustBeNChars_format")]
        //[CustomValidation(typeof(UInt32), "Parse", ErrorMessage = "The zip can only contain digits.")] // TODO: complete constraints
        public override string Zip
        {
            get { return base.Zip; }
            set
            {
                if (this.Zip == value) return;

                base.Zip = value;
                this.NotifyOfPropertyChange(() => this.IsValid);
                this.HasChanges = true;
            }
        }

        //[Required(ErrorMessage = "You must specify a province.")]
        [LocalizedStringLength(2, MinimumLength = 2, ErrorMessageFormatKey = "MustBeNChars_format")]
        public override string Province
        {
            get { return base.Province; }
            set
            {
                if (this.Province == value) return;

                base.Province = value;
                this.NotifyOfPropertyChange(() => this.IsValid);
                this.HasChanges = true;
            }
        }

        public override string Country
        {
            get { return base.Country; }
            set
            {
                if (this.Country != value)
                {
                    base.Country = value;
                    this.NotifyOfPropertyChange(() => this.IsValid);
                    this.HasChanges = true;
                }
            }
        }

        #endregion

        #endregion

        #region overrides

        public new string DisplayName
        {
            // TODO: move the change to IsInEditMode & HasChanges
            get
            {
                return this.IsInEditMode
                    ? (TranslationManager.Instance.Translate("EditSupplier").ToString() + (this.IsInEditMode & this.HasChanges ? " *" : string.Empty))
                    : TranslationManager.Instance.Translate("NewSupplier").ToString();
            }
        }

        #endregion

        #endregion

        #region methods

        private void CancelddEditAndClose()
        {
            if (this.HasChanges)
            {
                DialogViewModel discardChangesDialog =
                    DialogViewModel.Show(
                        DialogType.Question,
                        this.IsInEditMode ?
                        TranslationManager.Instance.Translate("CancelEdit") :
                        TranslationManager.Instance.Translate("CancelAdd"),
                        TranslationManager.Instance.Translate("DiscardChangesQuestion"))
                    .YesNo();

                this.windowManager.ShowDialog(discardChangesDialog);

                if (discardChangesDialog.FinalResponse == ResponseType.Yes)
                {
                    if (this.IsInEditMode)
                        this.CancelEdit();

                    this.TryClose(false);
                }
            }
            else
            {
                if (this.IsInEditMode)
                    this.CancelEdit();

                this.TryClose(false);
            }
        }

        private void ConfirmAddEditAndClose()
        {
            if (this.IsInEditMode)
                this.EndEdit();

            this.TryClose(true);
        }

        #endregion

        #region commands

        private RelayCommand confirmAddEditAndCloseCommand;
        public RelayCommand ConfirmAddEditAndCloseCommand
        {
            get
            {
                return this.confirmAddEditAndCloseCommand ?? (this.confirmAddEditAndCloseCommand =
                    new RelayCommand(
                        () => this.ConfirmAddEditAndClose(),
                        () => this.IsValid));
            }
        }

        private RelayCommand cancelAddEditAndCloseCommand;
        public RelayCommand CancelAddEditAndCloseCommand
        {
            get
            {
                return this.cancelAddEditAndCloseCommand ?? (this.cancelAddEditAndCloseCommand =
                    new RelayCommand(
                        () => this.CancelddEditAndClose()));
            }
        }

        #endregion
    }
}