using System;
using System.Collections.Generic;

namespace TDQ.Classes
{
    public class GroupPageFunctions
    {
        public static string[] PopulateListOnAppearing(string[] list)
        {
            List<string> emails = new List<string>();
            //starts index at 3, this is where the emails start in the array
            for (int i = 3; i < list.Length; i++)
            {
                //Adds email to new list as long as the element is not empty.
                if (list[i] != "")
                    emails.Add(list[i]);
            }

            //returns the new array of just emails
            return emails.ToArray();
        }
    }
}
