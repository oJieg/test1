using System.Collections.Generic;

namespace test1
{
    public interface IDataContactInterface
    {
        string FormatFile { get; }

        bool TryInitializationDB(string nameFile);
        bool TryAddContact(string name, string? phone);
        bool TryTakeContacts(int offset, int take, out List<Contact> outContacts);
        int AmountOfContact();
        bool CreateFile(string directory, string nameFile);
    }
}