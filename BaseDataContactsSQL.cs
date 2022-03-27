using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace test1
{
    /// <summary>
    /// первым делом вызови InitializationBD
    /// </summary>
    public class BaseDataContactsSQL : IDataContactInterface
    {
        private string _dataSourceBD = "";
        private string _nameFile = "";

        /// <summary>
        /// nameFile - имя файла без разширения. В случаи не допустимого имени, вернет false.
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public bool TryInitializationBD(string nameFile)
        {
            string forbiddenSymbols = new(Path.GetInvalidFileNameChars());
            Regex r = new(string.Format("[{0}]", Regex.Escape(forbiddenSymbols)));
            if (r.Match(nameFile).Success)
            {
                return false;
            }
            _dataSourceBD = $"Data Source={_nameFile}.db";
            _nameFile = nameFile;

            try
            {
                using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadWriteCreate");
                using SqliteCommand comandBDsql =
                    new("select Type from sqlite_master WHERE type='table' and name='Contact';", sqlBD);
                sqlBD.Open();

                if (comandBDsql.ExecuteScalar() == null)
                {
                    sqlBD.Close();

                    File.Copy($"{_nameFile}.db", $"{_nameFile}Copy.db", true);

                    CreateNewTable();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CreateNewTable()
        {
            using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadWrite");
            using SqliteCommand comandBDsql =
                new("CREATE TABLE Contact(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Phone TEXT)", sqlBD);

            sqlBD.Open();
            comandBDsql.ExecuteNonQuery();
        }

        public bool TryAddContact(string name, string? phone)
        {
            try
            {
                using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadWrite");

                using SqliteCommand commandBDsql = new($"INSERT INTO Contact(Name, Phone) VALUES(@name, @phone)", sqlBD);
                commandBDsql.Parameters.Add(new("@name", name));
                SqliteParameter phoneParametr = new("@phone", phone);
                phoneParametr.IsNullable = true;
                commandBDsql.Parameters.Add(phoneParametr);

                sqlBD.Open();
                commandBDsql.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException)
            {
                return false;
            }
        }

        //офсет - начальный элемент, тейк - количество элементов
        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();
            //валидация
            int amoutOfContact = AmountOfContact();
            if (offset < 0 && offset > amoutOfContact)
            {
                return false;
            }

            try
            {
                using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadOnly");
                using SqliteCommand comandBDsql = new($"select Name, Phone from Contact LIMIT {take} OFFSET {offset};", sqlBD);

                sqlBD.Open();
                using SqliteDataReader reader = comandBDsql.ExecuteReader();
                while (reader.Read())
                {
                    outContacts.Add(new Contact(reader.GetString("Name"), reader.GetString("phone")));
                }
                return true;
            }
            catch (SqliteException)
            {
                return false;
            }
        }

        //количество контактов в базе
        public int AmountOfContact()
        {
            try
            {
                using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadOnly");
                using SqliteCommand comandBDsql = new("select Count(*) from Contact;", sqlBD);

                sqlBD.Open();
                return Convert.ToInt32(comandBDsql.ExecuteScalar());
            }
            catch (SqliteException)
            {
                return 0;
            }
        }
    }
}
