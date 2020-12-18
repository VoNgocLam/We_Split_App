using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace We_Slipt_App
{
    public class Trips
    {
        public string Name { get; set; }
        public string Introduce { get; set; }
        public string Picture { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public enum TripType : int { Processing, Accomplished };
        public TripType Type { get; set; }
        public BindingList<string> Members { get; set; }
        public BindingList<string> Routes { get; set; }
        public ObservableCollection<Cash> Expenses { get; set; }
        public ObservableCollection<Cash> ReceivedMoney { get; set; }
        public string Description { get; set; }
        public BindingList<string> Images { get; set; }
        public string sMembers { get; set; }
    }

    public class Cash
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
