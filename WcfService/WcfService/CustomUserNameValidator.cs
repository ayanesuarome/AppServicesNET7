﻿using System;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace WcfService
{
    public class CustomUserNameValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (null == userName || null == password)
            {
                throw new ArgumentNullException();
            }

            if (userName != "admin" && password != "admin")
            {
                // This throws an informative fault to the client.
                throw new FaultException("Unknown Username or Incorrect Password");
                // When you do not want to throw an informative fault to the client,
                // throw the following exception.
                // throw new SecurityTokenException("Unknown Username or Incorrect Password");
            }
        }
    }
}