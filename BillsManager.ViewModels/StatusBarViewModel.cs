﻿using BillsManager.Models;
using BillsManager.ViewModels.Messages;
using Caliburn.Micro;
using System.ComponentModel;
using System.Linq;

namespace BillsManager.ViewModels
{
    public partial class StatusBarViewModel : Screen,
        IHandle<DBConnectionStateChangedMessage>,
        IHandle<BillsListChangedMessage>,
        IHandle<SuppliersListChangedMessage>,
        IHandle<AddedMessage<Bill>>,
        IHandle<DeletedMessage<Bill>>,
        IHandle<AddedMessage<Supplier>>,
        IHandle<DeletedMessage<Supplier>>
    {
        #region fields

        private readonly IEventAggregator gloablEventAggregator;

        #endregion

        #region ctor

        public StatusBarViewModel(IEventAggregator gloablEventAggregator)
        {
            this.gloablEventAggregator = gloablEventAggregator;
            this.gloablEventAggregator.Subscribe(this);
            this.Deactivated +=
                (s, e) =>
                {
                    if (e.WasClosed)
                        this.gloablEventAggregator.Unsubscribe(this);
                };
        }

        #endregion

        #region properties

        private DBConnectionState connectionState = DBConnectionState.Disconnected;
        public DBConnectionState ConnectionState
        {
            get { return this.connectionState; }
            set
            {
                if (this.connectionState == value) return;

                this.connectionState = value;
                this.NotifyOfPropertyChange(() => this.ConnectionState);
                this.NotifyOfPropertyChange(() => this.ConnectionStateString);

                this.NotifyOfPropertyChange(() => this.IsConnected);
                this.NotifyOfPropertyChange(() => this.IsDirty);
            }
        }

        public string ConnectionStateString
        {
            get
            {
                var tcs =
                    /*TranslationManager.Instance.Translate(*/
                    typeof(DBConnectionState)
                    .GetMember(this.ConnectionState.ToString())[0]
                    .GetAttributes<DisplayNameAttribute>(true).FirstOrDefault().DisplayName
                    /*).ToString()*/;

                return tcs;
            }
        }

        public bool IsConnected
        {
            get { return this.ConnectionState != DBConnectionState.Disconnected; }
        }

        public bool IsDirty
        {
            get { return this.ConnectionState != DBConnectionState.Unsaved; }
        }

        private int suppliersCount;
        public int SuppliersCount
        {
            get { return this.suppliersCount; }
            set
            {
                if (this.suppliersCount == value) return;

                this.suppliersCount = value;
                this.NotifyOfPropertyChange(() => this.SuppliersCount);
            }
        }

        private int billsCount;
        public int BillsCount
        {
            get { return this.billsCount; }
            set
            {
                if (this.billsCount == value) return;

                this.billsCount = value;
                this.NotifyOfPropertyChange(() => this.BillsCount);
            }
        }

        #endregion

        #region methods

        #region message handlers

        public void Handle(DBConnectionStateChangedMessage message)
        {
            this.ConnectionState = message.DBState;
        }

        public void Handle(SuppliersListChangedMessage message)
        {
            this.SuppliersCount = message.Suppliers.Count;
        }

        public void Handle(BillsListChangedMessage message)
        {
            this.BillsCount = message.Bills.Count;
        }

        public void Handle(AddedMessage<Bill> message)
        {
            this.BillsCount++;
        }

        public void Handle(DeletedMessage<Bill> message)
        {
            this.BillsCount--;
        }

        public void Handle(AddedMessage<Supplier> message)
        {
            this.SuppliersCount++;
        }

        public void Handle(DeletedMessage<Supplier> message)
        {
            this.SuppliersCount--;
        }

        #endregion

        #endregion
    }
}