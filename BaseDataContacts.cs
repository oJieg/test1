using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace test1
{
    public class BaseDataContacts
    {
        private readonly List<Contact> _contacts = new();
        private readonly string _nameTable;

        //создание дб и ее валидация в конструкторе
        public BaseDataContacts(string nameTable)
        {
            _nameTable = nameTable;
            if(!File.Exists($"{nameTable}.db"))
            {
                CreateNewBDsql();
                return;
            }

            //и тут не совсем понял с сахором, он какой то страный вариант предлагает, не уверен что равнозначный
            using SqliteConnection _BDsql = new($"Data Source={nameTable}.db");
            _BDsql.Open();
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = _BDsql;
            comandBDsql.CommandText = "select * from sqlite_master WHERE type='table';";
            comandBDsql.ExecuteNonQuery();

            using (SqliteDataReader reader = comandBDsql.ExecuteReader())
            {
                reader.Read();
                if (!(reader.HasRows && (string)reader.GetValue(1) == "Contact"))
                {
                   // reader.RecordsAffected
                    CreateNewBDsql();
                }

                reader.Close();
            }
            _BDsql.Close();
        }
        private void CreateNewBDsql()
        {
            using SqliteConnection _BDsql = new($"Data Source={_nameTable}.db");
            _BDsql.Open();
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = _BDsql;
            comandBDsql.CommandText = "CREATE TABLE Contact(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Phone TEXT)";
            comandBDsql.ExecuteNonQuery();
            _BDsql.Close();
        }

        //методы добавления контактов
        public void AddContact(string name, string? phone)
        {
            using (SqliteConnection _BDsql = new($"Data Source={_nameTable}.db"))
            {
                _BDsql.Open();
                SqliteCommand commandBDsql = new();
                commandBDsql.Connection = _BDsql;
                commandBDsql.CommandText = $"INSERT INTO Contact(Name, Phone) VALUES('{name}', '{phone}')";
                commandBDsql.ExecuteNonQuery();
                _BDsql.Close();
            }
        }

        //офсет - начальный элемент, тейк - количество элементов
        public Contact[] TakeContact(int offset, int take)
        {
            //валидация
            if (offset < 0 && offset > AmountOfContact())
            {
                throw new Exception("error offset");
            }
            if (offset + take > AmountOfContact())
            {
                throw new Exception("error take");
            }

            Contact[] outContacts = new Contact[take];

            //делаем закпро в бд
            using SqliteConnection _BDsql = new($"Data Source={_nameTable}.db");
            _BDsql.Open();
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = _BDsql;
            int longTake = offset + take;
            comandBDsql.CommandText = $"select * from Contact WHERE _id > {offset} AND _id <= {longTake};";
            comandBDsql.ExecuteNonQuery();

            using (SqliteDataReader reader = comandBDsql.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    for( int i = 0; reader.Read();i++)
                    {
                        outContacts[i] = new Contact(reader.GetString(1), reader.GetString(2));
                    }
                }
                reader.Read();
            }
            _BDsql.Close();
            return outContacts;
        }

        //количество контактов в базе
        public int AmountOfContact()
        {
            int amount=0;
            using SqliteConnection _BDsql = new($"Data Source={_nameTable}.db");
            _BDsql.Open();
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = _BDsql;
            comandBDsql.CommandText = "select Count(*) from Contact;";
            comandBDsql.ExecuteNonQuery();

            using (SqliteDataReader reader = comandBDsql.ExecuteReader())
            {
                reader.Read();
                amount = reader.GetInt32(0);

            }
            _BDsql.Close();

            return amount;
        }
    }
}
