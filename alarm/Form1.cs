using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alarm
{
    public partial class Form1 : Form
    {
        private static string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string databasePath = currentDirectory + "DataBaseAlarms.mdf";
        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True";

        private string connectionString1 = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Программы\с#\alarm\alarm\DataBaseAlarms.mdf;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
            LoadAlarmsFromDatabase();
        }

        private void LoadAlarmsFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [Table]";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                int panelY = 10;

                while (reader.Read())
                {
                    Panel panelAlarm = new Panel();
                    CheckBox workCheckBox = new CheckBox();
                    RichTextBox nameRichTextBox = new RichTextBox();
                    RichTextBox frequencyRichTextBox = new RichTextBox();
                    RichTextBox timeRichTextBox = new RichTextBox();

                    workCheckBox.Checked = Convert.ToBoolean(reader["work"]);
                    nameRichTextBox.Text = reader["name"].ToString();
                    frequencyRichTextBox.Text = GetFrequencyText(reader);
                    timeRichTextBox.Text = reader["time"].ToString();

                    // Установка размеров и расположения для каждого элемента
                    workCheckBox.Location = new Point(10, 10);
                    workCheckBox.Size = new Size(100, 20);

                    nameRichTextBox.Location = new Point(10, 40);
                    nameRichTextBox.Size = new Size(100, 20);

                    frequencyRichTextBox.Location = new Point(10, 70);
                    frequencyRichTextBox.Size = new Size(100, 20);

                    timeRichTextBox.Location = new Point(10, 100);
                    timeRichTextBox.Size = new Size(100, 20);

                    // Добавление элементов на панель
                    panelAlarm.Controls.Add(workCheckBox);
                    panelAlarm.Controls.Add(nameRichTextBox);
                    panelAlarm.Controls.Add(frequencyRichTextBox);
                    panelAlarm.Controls.Add(timeRichTextBox);

                    // Установка размеров панели
                    panelAlarm.Size = new Size(420, 130);
                    panelAlarm.Location = new Point(10, panelY);
                    panelAlarm.BackColor = Color.Blue;

                    // Добавление панели на форму
                    Controls.Add(panelAlarm);

                    // Увеличение значения Y для следующей панели
                    panelY += 140; // Учитывая высоту панели и отступ между панелями
                }
                reader.Close();
            }
        }

        private string GetFrequencyText(SqlDataReader reader)
        {
            string frequencyText;
            List<string> daysOfWeek = new List<string>();

            if (Convert.ToInt32(reader["Monday"]) == 1) daysOfWeek.Add("Понедельник");
            if (Convert.ToInt32(reader["Tuesday"]) == 1) daysOfWeek.Add("Вторник");
            if (Convert.ToInt32(reader["Wednesday"]) == 1) daysOfWeek.Add("Среда");
            if (Convert.ToInt32(reader["Thursday"]) == 1) daysOfWeek.Add("Четверг");
            if (Convert.ToInt32(reader["Friday"]) == 1) daysOfWeek.Add("Пятница");
            if (Convert.ToInt32(reader["Saturday"]) == 1) daysOfWeek.Add("Суббота");
            if (Convert.ToInt32(reader["Sunday"]) == 1) daysOfWeek.Add("Воскресенье");

            if (daysOfWeek.Count == 0)
            {
                frequencyText = "Сегодня";
            }
            else
            {
                frequencyText = string.Join(", ", daysOfWeek);
            }

            return frequencyText;
        }
    }
}
