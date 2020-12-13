using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace We_Slipt_App
{
    class Trips
    {
        public string Name { get; set; }
        public string Introduce { get; set; }
        public string Picture { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public enum TripType : int { Processing, Accomplished };
        public TripType Type { get; set; }
        public BindingList<string> Members { get; set; }
        public ObservableCollection<string> Routes { get; set; }
        public ObservableCollection<string> Expenses { get; set; }
        public ObservableCollection<string> Money { get; set; }
        public ObservableCollection<string> Remark { get; set; }
        public string Description { get; set; }
        public string Leader { get; set; }
        public BindingList<string> Images { get; set; }
        public string Icon { get; set; }
        public string ThanhVien { get; set; }
        public string DiaDiem { get; set; }
    }
}
