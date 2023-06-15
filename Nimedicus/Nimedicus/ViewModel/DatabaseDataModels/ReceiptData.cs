using Chilkat;
using GalaSoft.MvvmLight.CommandWpf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Nimedicus.Model;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nimedicus.ViewModel.DatabaseDataModels
{
    public class ReceiptData
    {
        public int ReceiptDataId { get; set; }
        public DateTime DataCreation { get; set; }
        public DateTime DataExpiration { get; set; }
        public List<string> ListDrugs { get; set; }
        public string PatientLogin { get; set; }
        public string DoctorLogin { get; set; }

        public ReceiptData(int receiptDataId, int receiptID, DateTime dataCreation, DateTime dataExpiration, List<string> listDrugs, string patientLogin, string doctorLogin)
        {
            ReceiptDataId = receiptDataId;
            DataCreation = dataCreation;
            DataExpiration = dataExpiration;
            ListDrugs = listDrugs;
            PatientLogin = patientLogin;
            DoctorLogin = doctorLogin;
        }
        public ReceiptData(Model.DatabaseDataModels.ReceiptData receiptData)
        {
            ReceiptDataId = receiptData.ReceiptDataId;
            DataCreation = receiptData.DataCreation;
            DataExpiration = receiptData.DataExpiration;
            ListDrugs = receiptData.GetDrugsList();
            PatientLogin = receiptData.PatientLogin;
            DoctorLogin = receiptData.DoctorLogin;
            new DataContext();
        }

        public Doctor Doctor => DoctorLogin == null ? null : new DataContext().Doctors.FirstOrDefault(x => x.Login == DoctorLogin);

        public string DoctorFullName => Doctor == null? null : Doctor.LastName + " " + Doctor.Name + " " + Doctor.MiddleName;

        public Patient Patient => PatientLogin == null ? null : new DataContext().Patients.FirstOrDefault(x => x.Login == PatientLogin);

        public string PatientFullName => Patient == null ? null : Patient.LastName + " " + Patient.Name + " " + Patient.MiddleName;
        public ICommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFile)); }
        }

        private async void DownloadFile()
        {
            // Создание диалогового окна выбора пути для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Receipt№"+ ReceiptDataId.ToString("00000") + ".pdf"; // Имя файла по умолчанию
            saveFileDialog.Filter = "All Files (*.*)|*.*"; // Фильтр для выбора всех файлов
            saveFileDialog.Title = "Выберите путь для сохранения файла";

            // Отображение диалогового окна и ожидание выбора пути файла
            if (saveFileDialog.ShowDialog() == true)
            {
                string localFilePath = saveFileDialog.FileName;
                GenerateReceiptPDF(localFilePath, ReceiptDataId, DataCreation, DataExpiration, ListDrugs, DoctorFullName, PatientFullName, true);
            }
        }

        private ICommand _downloadFileCommand;

        public void GenerateReceiptPDF(string filePath, int receiptDataId, DateTime dataCreation, DateTime dataExpiration, List<string> listDrugs, string doctorFullName, string patientFullName, bool isOpenFile = false)
        {
            // Создание документа PDF
            Document document = new Document();

            // Создание объекта PdfWriter для записи в файл
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Открытие документа для записи
            document.Open();

            // Загрузка кириллического шрифта
            BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, 12);

            // Добавление заголовка
            Paragraph title = new Paragraph("Рецепт", new Font(baseFont, 20, Font.BOLD));
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Добавление информации о рецепте с использованием кириллического шрифта
            document.Add(new Paragraph($"ID рецепта: {receiptDataId}", font));
            document.Add(new Paragraph($"Дата создания: {dataCreation.ToString("dd.mm.yyyy")}", font));
            document.Add(new Paragraph($"Дата истечения: {dataExpiration.ToString("dd.mm.yyyy")}", font));
            document.Add(new Paragraph($"Лекарства:", font));
            for (int i = 0; i < listDrugs.Count; i++)
            {
                string drug = listDrugs[i];
                document.Add(new Paragraph($" №{i+1} : {drug}", font));
            }
            document.Add(new Paragraph($"Врач: {doctorFullName}", font));
            document.Add(new Paragraph($"Пациент: {patientFullName}", font));

            // Закрытие документа
            document.Close();

            if (isOpenFile)
                Process.Start(filePath);
        }
    }

}

