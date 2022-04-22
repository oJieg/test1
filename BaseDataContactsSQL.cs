using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using NLog;

namespace test1
{
    public class BaseDataContactsSQL : IDataContactInterface
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private string _dataSourceBD = String.Empty;
        private const string formatFile = ".db";

        public bool TryInitializationDB(string nameFile)
        {
            if (!File.Exists(nameFile))
            {
                logger.Error($"При иниацилизации указаный файл не найден(файл -{nameFile})");
                return false;
            }
            _dataSourceBD = $"Data Source={nameFile}";
            try
            {
                using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadWrite");
                using SqliteCommand comandBDsql =
                    new("select Type from sqlite_master WHERE type='table' and name='Contact';", sqlBD);
                sqlBD.Open();

                if (comandBDsql.ExecuteScalar() == null)
                {
                    sqlBD.Close();
                    File.Copy($"{nameFile}", $"{nameFile}.Copy", true);
                    logger.Warn($"Файл {nameFile} имеет не корректную структуру, " +
                        $"он был переименован и создан новый");
                    if (!TryCreateNewTable(_dataSourceBD))
                    {
                        logger.Error($"не удалось добавить таблицы в файл {nameFile}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Ошибка чтения файла {nameFile}. Ошибка:{ex}");
                return false;
            }
        }

        private bool TryCreateNewTable(string fullNameFile)
        {
            try
            {
                using SqliteConnection sqlBD = new($"Data Source={fullNameFile}; mode=ReadWriteCreate");
                using SqliteCommand comandBDsql =
                    new("CREATE TABLE Contact(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Phone TEXT)", sqlBD);

                sqlBD.Open();
                comandBDsql.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"не удалось в db создать файл или добавить таблицу.Файл {fullNameFile} Ошибка: {ex}");
                return false;
            }
        }

        public bool TryAddContact(string name, string? phone)
        {
            if (!ValidationInputClass.TryValidationForbiddenInputContact(name, phone))
            {
                logger.Warn($"не корктные данные для ввода {name}, {phone}");
                return false;
            }

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
            catch (SqliteException ex)
            {
                logger.Error($"не удалось добавить контакты в файл {_dataSourceBD}");
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();

            if (offset < 0 && offset > AmountOfContact())
            {
                logger.Error($"не верные offset({offset}) и take({take}) в методе TryTakeContacts(.db)");
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
                    outContacts.Add(new Contact(reader.GetString("Name"), reader.GetString("Phone")));
                }
                return true;
            }
            catch (SqliteException ex)
            {
                logger.Error($"ошибка чтения файла {_dataSourceBD} для элементов с " +
    $"{offset}, {take}-количество элементов. Ошибка: {ex}");
                return false;
            }
        }

        public int AmountOfContact()
        {
            try
            {
                using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadOnly");
                using SqliteCommand comandBDsql = new("select Count(*) from Contact;", sqlBD);

                sqlBD.Open();
                return Convert.ToInt32(comandBDsql.ExecuteScalar());
            }
            catch (SqliteException ex)
            {
                logger.Error($"ошибка чтения файла {_dataSourceBD} при подсчете количества контактов. Ошибка: {ex}");
                return 0;
            }
        }

        public bool CreateFile(string directory, string nameFile)
        {
            if (TryCreateNewTable(
                Path.Combine(Directory.GetCurrentDirectory(), directory, $"{nameFile}.db")))
            {
                return true;
            }
            else
            {
                logger.Warn($"не удалось создать файл {nameFile}db.");
                return false;
            }
        }

        public string FormatFile()
        {
            return formatFile;
        }
    }
}
