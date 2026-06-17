using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSanse.Healpers
{
    public static class LoggedInUser
    {
        public static Guid UserId { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static byte[] Image { get; set; }

    }
}
