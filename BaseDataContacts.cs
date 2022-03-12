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

        //проверка бд на наличие
        public void InitializationBD()
        {
            //и тут не совсем понял с сахором, он какой то страный вариант предлагает, не уверен что равнозначный
            using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadWriteCreate");

            SqliteCommand comandBDsql = new("select Type from sqlite_master WHERE type='table' and name='Contact';", sqlBD);
            sqlBD.Open();

            if (comandBDsql.ExecuteScalar() == null)
            {
                sqlBD.Close();
                //сюда вход только если бд некоректная, но файл есть, по этому переименуем вот так
                try
                {
                    File.Copy($"{_nameFile}.db", $"{_nameFile}Copy.db");
                }
                catch(IOException)
                {
                    File.Delete($"{_nameFile}Copy.db"); //если уже есть такой файл архивный удалить его.
                }


                CreateNewTable();
            }
        }

        private void CreateNewTable()
        {
            using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadWrite");
            SqliteCommand comandBDsql = new(
                "CREATE TABLE Contact(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Phone TEXT)", sqlBD);

            sqlBD.Open();
            comandBDsql.ExecuteNonQuery();
        }

        //методы добавления контактов
        public void AddContact(string name, string? phone)
        {
            using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadWrite");

            //добавить SqliteParameter
            SqliteCommand commandBDsql = new($"INSERT INTO Contact(Name, Phone) VALUES(@name, @phone)", sqlBD);
            SqliteParameter nameParametr = new("@name", name);
            commandBDsql.Parameters.Add(nameParametr);
            SqliteParameter phoneParametr = new("@phone", phone);
            phoneParametr.IsNullable = true;
            commandBDsql.Parameters.Add(phoneParametr);

            
            try
            {
                sqlBD.Open();
                commandBDsql.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                //вот чего тут писать? Тупо же наверное создавать эту таблицу если состоялась эта ошибка
                //получится же что добавление контакта чинит все))
                InitializationBD();
                AddContact(name, phone);
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

            using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadOnly");
            int exitOffset = offset + take;
            SqliteCommand comandBDsql = new($"select * from Contact WHERE _id > {offset} AND _id <= {exitOffset};", sqlBD);

           
            try
            {
                sqlBD.Open();
                using (SqliteDataReader reader = comandBDsql.ExecuteReader())
                {
                    for (int i = 0; reader.Read(); i++)
                    {
                        outContacts[i] = new Contact(reader.GetString(1), reader.GetString(2));
                    }
                }
                return outContacts;
            }
            catch (SqliteException)
            {
                Contact[] outContactsError = new Contact[1];
                outContactsError[0] = new Contact("error file .db", "sqlBD not correct, call InitializationBD()");
                return outContactsError;
            }

        }

        //количество контактов в базе
        public int AmountOfContact()
        {
            using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadOnly") ;
            SqliteCommand comandBDsql = new("select Count(*) from Contact;", sqlBD);
            try
            {
                sqlBD.Open();
                int amount = Convert.ToInt32(comandBDsql.ExecuteScalar());
                return amount;
            }
            catch (SqliteException)
            {
                return 0; //в общем если ошибка по чтению бд то результат запроса количества контактов будет 0
            }
        }
    }
}
