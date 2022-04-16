using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    internal class ScreenMainBD : Screen
    {
        private readonly IDataContactInterface _dataContacts;
        public ScreenMainBD(int numberOfLinesOnRender, IDataContactInterface dataContacts)
            :base(numberOfLinesOnRender)
        {
            _dataContacts = dataContacts;
            _pageCounterRender = true;
        }

        protected override void Title()
        {
            Console.WriteLine("контакты:");
        }

        protected override List<string> DataForPageRender()
        {
            List<string> data = new List<string>();
            _dataContacts.TryTakeContacts(_offsetForTotalNumber, _takeForTotalNumber, out List<Contact> outContact);
            foreach(Contact contact in outContact)
            {
                data.Add(contact.ToString());
            }
            return data;
        }
    }
}
