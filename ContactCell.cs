using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp_Auto_Newslatter
{
    class ContactCell
    {
        public string FullName { get; private set; }
        public string PhoneNumber { get; private set; }

        public ContactCell(string name, string phoneNumber)
        {
            FullName = name;
            PhoneNumber = phoneNumber;
        }
    }
}
