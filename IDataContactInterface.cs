using System.Collections.Generic;
using System.Threading.Tasks;

namespace test1
{
    public interface IDataContactInterface
    {
        string FormatFile { get; }

        bool TryInitializationDB(string nameFile);
        Task<bool> TryAddContact(string name, string? phone);
        bool TryTakeContacts(int offset, int take, out List<Contact> outContacts);
        int AmountOfContact();
        bool TryCreateFile(string nameFile);
    }
}