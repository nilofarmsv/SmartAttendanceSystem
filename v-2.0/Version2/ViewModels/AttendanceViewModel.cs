using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSanse.ViewModels
{
    public class AttendanceViewModel
    {
        public string Name {  get; set; }   
        public Guid Id {  get; set; }   
        public string CheckIn {  get; set; }   
        public string CheckOut {  get; set; }   
        public string Status {  get; set; }   
        public bool IsFinger {  get; set; }   
        
        public DateTime Date {  get; set; }   
    }
}
