using System.Collections.Generic;

namespace test1
{
    public interface IDataContactInterface
    {
        bool TryInitializationBD(string nameFile);
        bool TryAddContact(string name, string? phone);
        bool TryTakeContacts(int offset, int take, out List<Contact> outContacts);
        int AmountOfContact();
    }
}
