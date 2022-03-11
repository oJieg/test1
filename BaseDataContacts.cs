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
            if (!File.Exists($"{nameTable}.db"))
            {
                CreateNewBDsql();
            }
            else
            {
                ValidationBD();
            }
        }

        //проверка бд на наличие
        private void ValidationBD()
        {
            //и тут не совсем понял с сахором, он какой то страный вариант предлагает, не уверен что равнозначный
            using SqliteConnection sqlBD = new(_dataSourceBD);
            sqlBD.Open();
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = sqlBD;
            comandBDsql.CommandText = "select * from sqlite_master WHERE type='table' and name='Contact';";
            comandBDsql.ExecuteNonQuery();

            using (SqliteDataReader reader = comandBDsql.ExecuteReader())
            {
                reader.Read();
                if (!(reader.HasRows && (string)reader.GetValue(0) == "table"))
                {
                    reader.Close();
                    sqlBD.Close();
                    //сюда вход только если бд некоректная, но файл есть, по этому переименуем вот так
                    File.Copy($"{_nameFile}.db", $"{_nameFile}Copy");
                    File.Delete($"{_nameFile}.db");
                    CreateNewBDsql();
                }
                reader.Close();
            }
            sqlBD.Close();
        }

        private void CreateNewBDsql()
        {
            using SqliteConnection sqlBD = new(_dataSourceBD);
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = sqlBD;
            sqlBD.Open();
            comandBDsql.CommandText = "CREATE TABLE Contact(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Phone TEXT)";
            comandBDsql.ExecuteNonQuery();
            sqlBD.Close();
        }

        //методы добавления контактов
        public void AddContact(string name, string? phone)
        {
            using SqliteConnection sqlBD = new(_dataSourceBD);
            SqliteCommand commandBDsql = new();
            commandBDsql.Connection = sqlBD;
            sqlBD.Open();
            commandBDsql.CommandText = $"INSERT INTO Contact(Name, Phone) VALUES('{name}', '{phone}')";
            commandBDsql.ExecuteNonQuery();
            sqlBD.Close();
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
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = sqlBD;
            int longTake = offset + take;
            sqlBD.Open();
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
            sqlBD.Close();
            return outContacts;
        }

        //количество контактов в базе
        public int AmountOfContact()
        {
            using SqliteConnection sqlBD = new(_dataSourceBD);
            SqliteCommand comandBDsql = new();
            comandBDsql.Connection = sqlBD;
            sqlBD.Open();
            comandBDsql.CommandText = "select Count(*) from Contact;";
            comandBDsql.ExecuteNonQuery();

            int amount = Convert.ToInt32(comandBDsql.ExecuteScalar());

            sqlBD.Close();
            return amount;
        }
    }
}
