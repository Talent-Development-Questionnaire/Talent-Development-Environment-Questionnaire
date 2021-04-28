using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Classes
{
    public class DetailedGoalsPageFunctions
    {
        //function to put the existing goals into a list to be displayed
        public static string[] PopulateListOnAppearing(string[] list)
        {
            List<string> goals = new List<string>();
            //starts index at 3, this is where the goals start in the array
            for (int i = 3; i < list.Length; i++)
            {
                //Adds goal to new list as long as the element is not empty.
                if (list[i] != "")
                    goals.Add(list[i]);
            }

            //returns the new array of just goals
            return goals.ToArray();
        }
    }
}
