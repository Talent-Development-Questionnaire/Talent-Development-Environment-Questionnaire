﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Classes
{
    public class Verification
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
