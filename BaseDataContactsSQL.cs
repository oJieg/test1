using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace test1
{
    public class BaseDataContactsSQL : IDataContactInterface
    {
        private readonly ILogger _logger;

        private string _dataSourceBD = String.Empty;
        private bool _needCreatFile = false;
        private string _nameFile;
        public string FormatFile { get { return ".db"; } }

        public BaseDataContactsSQL(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BaseDataContactsSQL>();
        }

        public bool TryInitializationDB(string nameFile)
        {
            _nameFile = nameFile;
            if (!File.Exists(nameFile))
            {
                _needCreatFile = true;
                return true;
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
                    _logger.LogInformation("Файл {nameFile} имеет не корректную структуру, " +
                        "он был переименован и создан новый", nameFile);
                    if (!TryCreateNewTable(_dataSourceBD))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка чтения файла {nameFile}.", nameFile);
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
                _logger.LogError(ex, "Не удалось в db создать файл или добавить таблицу.Файл {fullNameFile}", fullNameFile);
                return false;
            }
        }

        public bool TryAddContact(string name, string? phone)
        {
            if (!ValidationInputClass.TryValidationForbiddenInputContact(name, phone))
            {
                _logger.LogWarning("Не верные данные пользователя name - {name} phone - {phone}", name, phone);
                return false;
            }
            if (_needCreatFile && !TryCreateFile(_nameFile))
            {
                return false;
            }
            _needCreatFile = false;

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
                _logger.LogError(ex, "не удалось добавить контакты в файл {_dataSourceBD}", _dataSourceBD);
                return false;
            }
        }

        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();
            if (_needCreatFile)
            {
                return true;
            }

            if (!HelperBaseData.ValidationOffset(offset, AmountOfContact()))
            {
                _logger.LogError("не верные offset({offset})", offset);
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
                _logger.LogError(ex, "ошибка чтения файла {_dataSourceBD} для элементов с " +
    "{offset}, {take}-количество элементов.", _dataSourceBD, offset, take);
                return false;
            }
        }

        public int AmountOfContact()
        {
            if (_needCreatFile)
            {
                return 0;
            }

            try
            {
                using SqliteConnection sqlBD = new($"{_dataSourceBD}; mode=ReadOnly");
                using SqliteCommand comandBDsql = new("select Count(*) from Contact;", sqlBD);

                sqlBD.Open();
                return Convert.ToInt32(comandBDsql.ExecuteScalar());
            }
            catch (SqliteException ex)
            {
                _logger.LogError(ex, "ошибка чтения файла {_dataSourceBD} при подсчете количества контактов.", _dataSourceBD);
                return 0;
            }
        }

        public bool TryCreateFile(string nameFile)
        {
            if (ValidationInputClass.TryValidatinNameFile(nameFile) &&
                !File.Exists(nameFile) &&
                TryCreateNewTable(nameFile))
            {
                TryInitializationDB(nameFile);
                return true;
            }
            else
            {
                _logger.LogWarning("не удалось создать файл {nameFile}db.", nameFile);
                return false;
            }
        }
    }
}