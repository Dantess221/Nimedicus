using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Utils.Enums;
using System.Security.Cryptography;
using System.Windows;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Nimedicus.Utils;
using Nimedicus.ViewModel.DatabaseDataModels;
using ReceiptData = Nimedicus.Model.DatabaseDataModels.ReceiptData;
using AnalysData = Nimedicus.Model.DatabaseDataModels.AnalysData;
using Event = Nimedicus.Model.DatabaseDataModels.Event;
using System.IO;
using Nimedicus.View;

namespace Nimedicus.Model
{
    public class DatabaseManager
    {
        private DataContext context;

        public DatabaseManager()
        {
            context = new DataContext();
        }

        public bool CreatePatient(string login, Sex sex, DateTime birthdayDate, string name, string lastName, string middleName, string phone, string address, string mail, string doctorLogin = null, string password = null, bool isUIConfirmNeeded = false)
        {
            // Проверка наличия уникального логина и электронной почты
            var existingUser = context.Patients.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            if (existingUser == null)
            {
                existingUser = context.Doctors.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            }
            if (existingUser == null)
            {
                existingUser = context.Nurses.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            }
            if (existingUser != null)
            {
                // Пользователь с таким логином или электронной почтой уже существует
                if (isUIConfirmNeeded)
                {
                    if (MessageBox.Show("Пользователь с таким логином или электронной почтой уже существует.") == MessageBoxResult.OK)
                        return false;
                }
                return false;
            }

            var user = new Patient
            {
                Login = login,
                Sex = sex,
                BirthdayDate = birthdayDate,
                Name = name,
                LastName = lastName,
                MiddleName = middleName,
                Phone = phone,
                Address = address,
                DoctorLogin = doctorLogin,
                Mail = mail
            };

            // Генерация пароля
            string generatedPassword;
            if (string.IsNullOrEmpty(password))
            {
                generatedPassword = GenerateRandomPassword();
            }
            else
            {
                generatedPassword = password;
            }

            // Создание записи в таблице Auth
            var auth = new Auth
            {
                Login = login,
                Password = generatedPassword
            };


            context.Patients.Add(user);
            context.Auths.Add(auth);
            context.SaveChanges();

            return true;
        }

        public void DeletePatient(Patient patient)
        {
            // Удаление связанных записей пациента
            foreach (var note in context.Events.Where(x=>x.PatientLogin == patient.Login))
            {
                context.Events.Remove(note);
            }

            foreach (var analysis in context.Analysis.Where(x => x.PatientLogin == patient.Login))
            {
                context.Analysis.Remove(analysis);
            }

            foreach (var receipt in context.Receipts.Where(x => x.PatientLogin == patient.Login))
            {
                context.Receipts.Remove(receipt);
            }


            // Удаление пациента
            var patientDB = context.Patients.FirstOrDefault(x=>x.Login == patient.Login);
            context.Patients.Remove(patientDB);

            var authDB = context.Auths.FirstOrDefault(x => x.Login == patient.Login);
            context.Auths.Remove(authDB);

            // Сохранение изменений в базе данных
            context.SaveChanges();
        }

        public void CreateDoctor(string login, Sex sex, DateTime birthdayDate, string name, string lastName, string middleName, string phone, string address, string mail, string password = null, bool isUIConfirmNeeded = false)
        {
            // Проверка наличия уникального логина и электронной почты
            var existingUser = context.Patients.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            if (existingUser == null)
            {
                existingUser = context.Doctors.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            }
            if (existingUser == null)
            {
                existingUser = context.Nurses.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            }
            if (existingUser != null)
            {
                // Пользователь с таким логином или электронной почтой уже существует
                if (isUIConfirmNeeded)
                {
                    if (MessageBox.Show("Пользователь с таким логином или электронной почтой уже существует.") == MessageBoxResult.OK)
                        return;
                }
                return;
            }

            var user = new Doctor
            {
                Login = login,
                Sex = sex,
                BirthdayDate = birthdayDate,
                Name = name,
                LastName = lastName,
                MiddleName = middleName,
                Phone = phone,
                Address = address,
                Mail = mail
            };

            // Генерация пароля
            string generatedPassword;
            if (string.IsNullOrEmpty(password))
            {
                generatedPassword = GenerateRandomPassword();
            }
            else
            {
                generatedPassword = password;
            }

            // Создание записи в таблице Auth
            var auth = new Auth
            {
                Login = login,
                Password = generatedPassword
            };


            context.Doctors.Add(user);
            context.Auths.Add(auth);
            context.SaveChanges();
        }
        public void CreateNurse(string login, Sex sex, DateTime birthdayDate, string name, string lastName, string middleName, string phone, string address, string mail, string password = null, bool isUIConfirmNeeded = false)
        {
            // Проверка наличия уникального логина и электронной почты
            var existingUser = context.Patients.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            if (existingUser == null)
            {
                existingUser = context.Doctors.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            }
            if (existingUser == null)
            {
                existingUser = context.Nurses.FirstOrDefault(u => u.Login == login || u.Mail == mail) as BaseUser;
            }
            if (existingUser != null)
            {
                // Пользователь с таким логином или электронной почтой уже существует
                if (isUIConfirmNeeded)
                {
                    if (MessageBox.Show("Пользователь с таким логином или электронной почтой уже существует.") == MessageBoxResult.OK)
                        return;
                }
                return;
            }

            var user = new Nurse
            {
                Login = login,
                Sex = sex,
                BirthdayDate = birthdayDate,
                Name = name,
                LastName = lastName,
                MiddleName = middleName,
                Phone = phone,
                Address = address,
                Mail = mail
            };

            // Генерация пароля
            string generatedPassword;
            if (string.IsNullOrEmpty(password))
            {
                generatedPassword = GenerateRandomPassword();
            }
            else
            {
                generatedPassword = password;
            }

            // Создание записи в таблице Auth
            var auth = new Auth
            {
                Login = login,
                Password = generatedPassword
            };


            context.Nurses.Add(user);
            context.Auths.Add(auth);
            context.SaveChanges();
        }
        public async void CreateAnalys(string analysName, string filePath, Patient patient)
        {

            var analys = new AnalysData
            {
                AnalysName = analysName,
                DataCreation = DateTime.Now,
                PatientLogin = patient.Login,
            };

            context.Analysis.Add(analys);
            context.SaveChanges();

            await new AzureBlobStorageHelper(containerName: "nimedicus-analysis").UploadFileToAzure($"Analys_{analys.AnalysDataId.ToString("00000")}", filePath);
            analys.AnalysFileBlobName = $"Analys_{analys.AnalysDataId.ToString("00000")}" + Path.GetExtension(filePath);

            context.SaveChanges();

            new DatabaseManager().CreateEvent($"Создан Анализ №{analys.AnalysDataId.ToString("00000")}", patient);
        }
        public void CreateAnalys(string analysName, Patient patient, out AnalysData analysData)
        {

            analysData = new AnalysData
            {
                AnalysName = analysName,
                DataCreation = DateTime.Now,
                PatientLogin = patient.Login
            };


            context.Analysis.Add(analysData);
            context.SaveChanges();


            new DatabaseManager().CreateEvent($"Создан Анализ №{analysData.AnalysDataId.ToString("00000")}", patient);
        }

        public void CreateReceipt(List<string> drugs, DateTime dataExpiration, Patient patient, Doctor doctor)
        {

            var receiptData = new ReceiptData
            {
                DataCreation = DateTime.Now,
                DataExpiration = dataExpiration,
                PatientLogin = patient.Login,
                DoctorLogin = doctor.Login
            };
            receiptData.SetDrugsList(drugs);


            context.Receipts.Add(receiptData);
            context.SaveChanges();


            new DatabaseManager().CreateEvent($"Создан рецепт №{receiptData.ReceiptDataId.ToString("00000")}", patient);

        }
        public void CreateReceipt(List<string> drugs, DateTime dataExpiration, Patient patient, Doctor doctor, out ReceiptData receiptData)
        {

            receiptData = new ReceiptData
            {
                DataCreation = DateTime.Now,
                DataExpiration = dataExpiration,
                PatientLogin = patient.Login,
                DoctorLogin = doctor.Login
            };
            receiptData.SetDrugsList(drugs);


            context.Receipts.Add(receiptData);
            context.SaveChanges();

            new DatabaseManager().CreateEvent($"Создан рецепт №{receiptData.ReceiptDataId.ToString("00000")}", patient);
        }
        public void CreateEvent(string message, Patient patient)
        {

            var Event = new Event
            {
                Message = message,
                DataCreation = DateTime.Now,
                PatientLogin = patient.Login
            };


            context.Events.Add(Event);
            context.SaveChanges();
        }
        private string GenerateRandomPassword()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomBytes = new byte[10];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            var result = new StringBuilder();
            foreach (byte b in randomBytes)
            {
                result.Append(allowedChars[b % allowedChars.Length]);
            }

            return result.ToString();
        }
    }
}
