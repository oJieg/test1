using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace test1
{
    public class BaseDataContacts
    {
        private readonly string _dataSourceBD;
        private readonly string _nameFile;

        //создание дб и ее валидация в конструкторе
        public BaseDataContacts(string nameTable)
        {
            _dataSourceBD = $"Data Source={nameTable}.db";
            _nameFile = nameTable;
        }

        public void InitializationBD()
        {
            if (!File.Exists($"{_nameFile}.db"))
            {
                CreateNewBDsql();
            }
            else
            {
                ValidationDB();
            }
        }

        //проверка бд на наличие
        private void ValidationDB()
        {
            //и тут не совсем понял с сахором, он какой то страный вариант предлагает, не уверен что равнозначный
            using SqliteConnection sqlBD = new(_dataSourceBD);

            SqliteCommand comandBDsql = new("select Type from sqlite_master WHERE type='table' and name='Contact';", sqlBD);
            sqlBD.Open();
            using SqliteDataReader reader = comandBDsql.ExecuteReader();

            reader.Read();

            if (!(reader.HasRows && reader.GetString(0) == "table"))
            {
                reader.Close();
                sqlBD.Close();
                //сюда вход только если бд некоректная, но файл есть, по этому переименуем вот так
                File.Copy($"{_nameFile}.db", $"{_nameFile}Copy.db");
                CreateNewBDsql();
            }
        }

        private void CreateNewBDsql()
        {
            using SqliteConnection sqlBD = new(_dataSourceBD);
            SqliteCommand comandBDsql = new(
                "CREATE TABLE Contact(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Phone TEXT)", sqlBD);

            sqlBD.Open();
            comandBDsql.ExecuteNonQuery();
        }

        //методы добавления контактов
        public void AddContact(string name, string? phone)
        {
            using SqliteConnection sqlBD = new(_dataSourceBD);

            //добавить SqliteParameter
            SqliteCommand commandBDsql = new($"INSERT INTO Contact(Name, Phone) VALUES(@name, @phone)", sqlBD);
            SqliteParameter nameParametr = new("@name", name);
            commandBDsql.Parameters.Add(nameParametr);
            SqliteParameter phoneParametr = new("@phone", phone);
            phoneParametr.IsNullable = true;
            commandBDsql.Parameters.Add(phoneParametr);

            sqlBD.Open();
            commandBDsql.ExecuteNonQuery();
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
            using SqliteConnection sqlBD = new(_dataSourceBD);
            int exitOffset = offset + take;
            SqliteCommand comandBDsql = new($"select * from Contact WHERE _id > {offset} AND _id <= {exitOffset};", sqlBD);
           
            sqlBD.Open();
            using (SqliteDataReader reader = comandBDsql.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    for (int i = 0; reader.Read(); i++)
                    {
                        outContacts[i] = new Contact(reader.GetString(1), reader.GetString(2));
                    }
                }
            }
            return outContacts;
        }

        //количество контактов в базе
        public int AmountOfContact()
        {
            using SqliteConnection sqlBD = new(_dataSourceBD);
            SqliteCommand comandBDsql = new("select Count(*) from Contact;", sqlBD);

            sqlBD.Open();
            int amount = Convert.ToInt32(comandBDsql.ExecuteScalar());

            return amount;
        }
    }
}
