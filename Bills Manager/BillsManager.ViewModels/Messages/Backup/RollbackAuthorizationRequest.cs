﻿using System;

namespace BillsManager.ViewModels.Messages
{
    public class RollbackAuthorizationRequest
    {
        public RollbackAuthorizationRequest(Action confirm, Action negate)
        {
            this.confirm = confirm;
            this.negate = negate;
        }
        
        private readonly Action confirm;
        public Action ConfirmAuthorization
        {
            get { return this.confirm; }
        }
        
        private readonly Action negate;
        public Action NegateAuthorization
        {
            get { return this.negate; }
        }
    }
}