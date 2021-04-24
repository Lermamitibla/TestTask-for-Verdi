using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace WindowsFormsTest
{
    class WorkingShedule
    {
        [JsonIgnore]
        public string TimeFormat { get; set; }
        [JsonIgnore]
        public string Status { get; set; }
        private TimeSpan StartTime;
        private TimeSpan EndTime;
        private TimeSpan LunchTimeStart;
        private TimeSpan LunchTimeEnd;
        private TimeSpan CellDuration;
        private string startTime;
        private string endTime;
        private string lunchTimeStart;
        private string lunchTimeEnd;
        private string cellDuration;
        public double FreeTime { get; set; }
        public List<Cell> Cells { get; set; }

        public WorkingShedule(string startTime, string endTime, string lunchTimeStart, string lunchTimeEnd, string cellDuration)
        {
            Cells = new List<Cell>();
            TimeFormat = "h\\:mm";
            this.startTime = startTime;
            this.endTime = endTime;
            this.lunchTimeStart = lunchTimeStart;
            this.lunchTimeEnd = lunchTimeEnd;
            this.cellDuration = cellDuration;
            BuildShedule();
        }

        public void BuildShedule()
        {
            if (!ParseInputs()) return;
            if (!CheckTimeMistakes()) return;
            CreateCells();
        }

        private bool ParseInputs()
        {
            bool result = true;
            CultureInfo ruRU = new CultureInfo("ru-RU");

            if (!TimeSpan.TryParseExact(startTime, TimeFormat, ruRU, TimeSpanStyles.None, out StartTime))
                result = false;
            if (!TimeSpan.TryParseExact(endTime, TimeFormat, ruRU, TimeSpanStyles.None, out EndTime))
                result = false;
            if (!TimeSpan.TryParseExact(lunchTimeStart, TimeFormat, ruRU, TimeSpanStyles.None, out LunchTimeStart))
                result = false;
            if (!TimeSpan.TryParseExact(lunchTimeEnd, TimeFormat, ruRU, TimeSpanStyles.None, out LunchTimeEnd))
                result = false;

            if (!Int32.TryParse(cellDuration, out int cellTimeDurationInt))
                result = false;
            else CellDuration = new TimeSpan(0, cellTimeDurationInt, 0);

            if (!result) Status = "Ошибка. Проверьте формат вводимых данных";
            return result;
        }

        private bool CheckTimeMistakes()
        {
            if (StartTime > EndTime)
            {
                Status = "Ошибка. Работа заканчивается раньше, чем начинается";
                return false;
            }
            if (LunchTimeStart > LunchTimeEnd)
            {
                Status = "Ошибка. Обед начинается после завершения рабочего дня";
                return false;
            }
            if (LunchTimeStart < StartTime)
            {
                Status = "Ошибка. Обед начинается раньше начала рабочего дня";
                return false;
            }
            if (LunchTimeEnd > EndTime)
            {
                Status = "Ошибка. Обед заканчивается позже рабочего дня";
                return false;
            }
            return true;
        }

        private void CreateCells()
        {
            GetWorkingCells(StartTime, LunchTimeStart);
            GetWorkingCells(LunchTimeEnd, EndTime);
            Cell.staticID = 1;
        }

        private void GetWorkingCells(TimeSpan start, TimeSpan end)
        {
            TimeSpan currentTime = start;
            while (currentTime < end)
            {
                var next = currentTime + CellDuration;
                if (next < end)
                {
                    Cells.Add(new Cell
                    {
                        Start = currentTime.ToString(TimeFormat),
                        End = next.ToString(TimeFormat)
                    });
                    currentTime = next;
                }
                else
                {
                    FreeTime += (end - currentTime).TotalMinutes;
                    break;
                }
            }
        }

        public void SaveToJsonFile(string directory)
        {
            if (directory == null || directory == String.Empty)
            {
                Status = "Укажите путь сохранения файла";
                return;
            }
            if (Status != null) return;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            try
            {
                string result = JsonSerializer.Serialize(this, options);
                File.WriteAllText(directory + @"\result.txt", result);
                if(Status == null) Status = "файл result.txt сохранен";
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
    }
}
