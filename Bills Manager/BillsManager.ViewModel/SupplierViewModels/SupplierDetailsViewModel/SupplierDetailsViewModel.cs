﻿using BillsManager.Localization.Attributes;
using BillsManager.Models;
using BillsManager.ViewModels.Commanding;
using BillsManager.ViewModels.Messages;
using Caliburn.Micro;
using System;
using System.Linq;

namespace BillsManager.ViewModels
{
    public partial class SupplierDetailsViewModel :
        SupplierViewModel,
        IHandle<BillsListChangedMessage>,
        IHandle<BillAddedMessage>,
        IHandle<BillDeletedMessage>,
        IHandle<BillEditedMessage>
    {
        #region fields

        protected readonly IWindowManager windowManager;
        protected readonly IEventAggregator dbEventAggregator;

        #endregion

        #region ctor

        public SupplierDetailsViewModel(
            IWindowManager windowManager,
            IEventAggregator dbEventAggregator,
            Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException("supplier cannot be null");

            this.ExposedSupplier = supplier;

            this.windowManager = windowManager;
            this.dbEventAggregator = dbEventAggregator;

            this.dbEventAggregator.Subscribe(this);

            // HANDLERS
            this.Deactivated +=
                (s, e) =>
                {
                    if (e.WasClosed)
                        this.dbEventAggregator.Unsubscribe(this);
                };
        }

        #endregion

        #region properties

        #region wrapped from supplier

        public override string AgentName
        {
            get { return base.AgentName; }
        }

        public override string AgentPhone
        {
            get { return base.AgentPhone; }
        }

        public override string AgentSurname
        {
            get { return base.AgentSurname; }
        }

        public override string City
        {
            get { return base.City; }
        }

        public override string Country
        {
            get { return base.Country; }
        }

        public override string eMail
        {
            get { return base.eMail; }
        }

        public override string Fax
        {
            get { return base.Fax; }
        }

        public override string Name
        {
            get { return base.Name; }
        }

        public override string Notes
        {
            get { return base.Notes; }
        }

        public override string Number
        {
            get { return base.Number; }
        }

        #endregion

        #region added

        private double obligationAmount = 0;
        public double ObligationAmount
        {
            get { return obligationAmount; }
            set
            {
                if (this.obligationAmount != value)
                {
                    this.obligationAmount = value;
                    this.NotifyOfPropertyChange(() => this.ObligationAmount);
                    this.NotifyOfPropertyChange(() => this.ObligationState);
                    this.NotifyOfPropertyChange(() => this.ObligationStateString);
                }
            }
        }

        public Obligation ObligationState
        {
            get
            {
                if (this.ObligationAmount < 0) return Obligation.Creditor;
                if (this.ObligationAmount > 0) return Obligation.Debtor;
                return Obligation.Unbound;
            }
        }

        public string ObligationStateString
        {
            get
            {
                return
                    /*TranslationManager.Instance.Translate(*/
                    typeof(Obligation)
                    .GetMember(this.ObligationState.ToString())[0]
                    .GetAttributes<LocalizedDisplayNameAttribute>(true).FirstOrDefault().DisplayName
                    /*).ToString()*/;
            }
        }

        public string FullAddress
        {
            get
            {
                var fullAddress = string.Empty;

                fullAddress += this.Street;

                fullAddress += (fullAddress.Length > 0 & !string.IsNullOrWhiteSpace(this.Number)) ? " " : string.Empty;
                fullAddress += this.Number;

                fullAddress +=
                    (fullAddress.Length > 0 & (!string.IsNullOrWhiteSpace(this.Zip) | !string.IsNullOrWhiteSpace(this.City))) ?
                    ", " : string.Empty;
                fullAddress += this.Zip;

                fullAddress +=
                    (fullAddress.Length > 0 & !string.IsNullOrWhiteSpace(this.City) & !string.IsNullOrWhiteSpace(this.Zip)) ?
                    " " : string.Empty;
                fullAddress += this.City;

                var hasProv = !string.IsNullOrWhiteSpace(this.Province);
                fullAddress += (fullAddress.Length > 0 & hasProv) ? " " : string.Empty;
                if (hasProv) fullAddress += "(" + this.Province + ")";

                fullAddress += (fullAddress.Length > 0 & !string.IsNullOrWhiteSpace(this.Country)) ? " - " : string.Empty;
                fullAddress += this.Country;

                return fullAddress;
            }
        }

        #endregion

        #region overrides

        public new string DisplayName
        {
            get { return this.Name; }
        }

        #endregion

        #endregion

        #region methods

        // TODO: evaluate move to SuppliersViewModel.cs
        private void AddBill()
        {
            this.dbEventAggregator.Publish(new AddBillToSupplierOrder(this.ExposedSupplier));
        }

        #region message handlers

        public void Handle(BillsListChangedMessage message)
        {
            double newOblAmount = 0;

            foreach (var bill in message.Bills)
            {
                if (bill.SupplierID == this.ID & !bill.PaymentDate.HasValue)
                    newOblAmount += -bill.Amount;
            }

            this.ObligationAmount = newOblAmount;
        }

        public void Handle(BillAddedMessage message)
        {
            if (this.ID == message.AddedBill.SupplierID)
                if (!message.AddedBill.PaymentDate.HasValue)
                {
                    if (double.IsNaN(this.obligationAmount))
                        this.ObligationAmount = 0;
                    this.ObligationAmount += -message.AddedBill.Amount;
                }
        }

        public void Handle(BillDeletedMessage message)
        {
            if (this.ID == message.DeletedBill.SupplierID)
                if (!message.DeletedBill.PaymentDate.HasValue)
                    this.ObligationAmount += message.DeletedBill.Amount;
        }

        public void Handle(BillEditedMessage message)
        {
            bool supplierChanged = message.NewBillVersion.SupplierID != message.OldBillVersion.SupplierID;

            if (supplierChanged)
            {
                if (this.ID == message.OldBillVersion.SupplierID)
                    if (!message.OldBillVersion.PaymentDate.HasValue)
                        this.ObligationAmount += message.OldBillVersion.Amount;

                if (this.ID == message.NewBillVersion.SupplierID)
                    if (!message.NewBillVersion.PaymentDate.HasValue)
                        this.ObligationAmount += -message.NewBillVersion.Amount;
            }
            else
            {
                if (this.ID == message.NewBillVersion.SupplierID)
                {
                    if (!message.OldBillVersion.PaymentDate.HasValue)
                        this.ObligationAmount += message.OldBillVersion.Amount;

                    if (!message.NewBillVersion.PaymentDate.HasValue)
                        this.ObligationAmount += -message.NewBillVersion.Amount;
                }
            }
        }

        #endregion

        #endregion

        #region commands

        private RelayCommand switchToEditCommand;
        public RelayCommand SwitchToEditCommand
        {
            get
            {
                if (this.switchToEditCommand == null)
                    this.switchToEditCommand = new RelayCommand(
                        () =>
                        {
                            this.TryClose();
                            this.dbEventAggregator.Publish(new EditSupplierOrder(this.ExposedSupplier));
                        });

                return this.switchToEditCommand;
            }
        }

        private RelayCommand closeDetailsViewCommand;
        public RelayCommand CloseDetailsViewCommand
        {
            get
            {
                if (this.closeDetailsViewCommand == null)
                    this.closeDetailsViewCommand = new RelayCommand(
                    () =>
                    {
                        this.TryClose();
                    });

                return this.closeDetailsViewCommand;
            }
        }

        private RelayCommand addBillCommand;
        public RelayCommand AddBillCommand
        {
            get
            {
                if (this.addBillCommand == null)
                    this.addBillCommand = new RelayCommand(
                        () => this.AddBill());

                return this.addBillCommand;
            }
        }

        #endregion
    }
}