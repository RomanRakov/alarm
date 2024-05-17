using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace alarm
{
    public partial class AlarmListForm : Form
    {
        public AlarmListForm()
        {
            InitializeComponent();
            LoadAlarmsFromDatabase();
            AutoScroll = true;
        }

        private static string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string databasePath = currentDirectory + "DataBaseAlarms.mdf";
        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True";
        private void LoadAlarmsFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM [Table]";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int panelIndex = 0;
                        int panelHeight = 230;

                        while (reader.Read())
                        {
                            CreateAlarmPanel(reader, panelIndex, panelHeight);
                            panelIndex++;
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void CreateAlarmPanel(SqlDataReader reader, int panelIndex, int panelHeight)
        {
            int id = (int)reader["Id"];

            TableLayoutPanel panelAlarm = new TableLayoutPanel();
            panelAlarm.BackColor = Color.DarkBlue;
            panelAlarm.Location = new Point(10, panelIndex * (panelHeight + 10));
            panelAlarm.Margin = new Padding(5);

            panelAlarm.ColumnCount = 2;
            panelAlarm.RowCount = 6;
            panelAlarm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panelAlarm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            RichTextBox timeRichTextBox = CreateRichTextBox(reader["time"]);
            RichTextBox nameRichTextBox = CreateRichTextBox(reader["name"]);
            CheckBox workCheckBox = CreateCheckBox(reader["work"], "Работает");
            CheckBox frequencyCheckBox = CreateCheckBox(reader["frequency"], "Срабатывает постоянно");
            CheckBox mondayCheckBox = CreateCheckBox(reader["Monday"], "Понедельник");
            CheckBox tuesdayCheckBox = CreateCheckBox(reader["Tuesday"], "Вторник");
            CheckBox wednesdayCheckBox = CreateCheckBox(reader["Wednesday"], "Среда");
            CheckBox thursdayCheckBox = CreateCheckBox(reader["Thursday"], "Четверг");
            CheckBox fridayCheckBox = CreateCheckBox(reader["Friday"], "Пятница");
            CheckBox saturdayCheckBox = CreateCheckBox(reader["Saturday"], "Суббота");
            CheckBox sundayCheckBox = CreateCheckBox(reader["Sunday"], "Воскресенье");

            timeRichTextBox.Size = new Size(200, 30);
            nameRichTextBox.Size = new Size(200, 30);
            workCheckBox.Size = new Size(200, 30);
            frequencyCheckBox.Size = new Size(200, 30);
            mondayCheckBox.Size = new Size(200, 30);
            tuesdayCheckBox.Size = new Size(200, 30);
            wednesdayCheckBox.Size = new Size(200, 30);
            thursdayCheckBox.Size = new Size(200, 30);
            fridayCheckBox.Size = new Size(200, 30);
            saturdayCheckBox.Size = new Size(200, 30);
            sundayCheckBox.Size = new Size(200, 30);

            panelAlarm.Controls.Add(timeRichTextBox, 0, 0);
            panelAlarm.Controls.Add(nameRichTextBox, 1, 0);
            panelAlarm.Controls.Add(workCheckBox, 0, 1);
            panelAlarm.Controls.Add(frequencyCheckBox, 1, 1);
            panelAlarm.Controls.Add(mondayCheckBox, 0, 2);
            panelAlarm.Controls.Add(tuesdayCheckBox, 1, 2);
            panelAlarm.Controls.Add(wednesdayCheckBox, 0, 3);
            panelAlarm.Controls.Add(thursdayCheckBox, 1, 3);
            panelAlarm.Controls.Add(fridayCheckBox, 0, 4);
            panelAlarm.Controls.Add(saturdayCheckBox, 1, 4);
            panelAlarm.Controls.Add(sundayCheckBox, 0, 5);

            foreach (Control control in panelAlarm.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).ForeColor = Color.Black;
                }
                if (control is RichTextBox)
                {
                    ((RichTextBox)control).ForeColor = Color.Black;
                }
                control.BackColor = Color.White;
            }

            panelAlarm.Size = new Size(400, panelHeight);

            Controls.Add(panelAlarm);

            panelAlarm.Click += (sender, e) =>
            {
                ShowAddChangeForm(id, workCheckBox, nameRichTextBox, frequencyCheckBox, timeRichTextBox, mondayCheckBox, tuesdayCheckBox, wednesdayCheckBox, thursdayCheckBox, fridayCheckBox, saturdayCheckBox, sundayCheckBox, panelAlarm);
            };
        }

        private CheckBox CreateCheckBox(object value, string text)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Checked = Convert.ToBoolean(value);
            checkBox.Enabled = false;
            checkBox.Size = new Size(200, 30);
            checkBox.Text = text;
            checkBox.FlatStyle = FlatStyle.Flat;
            checkBox.BackColor = Color.White;
            checkBox.ForeColor = Color.Black;
            checkBox.Font = new Font("Arial", 10);
            return checkBox;
        }

        private RichTextBox CreateRichTextBox(object value)
        {
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Text = value.ToString();
            richTextBox.Size = new Size(200, 40);
            richTextBox.Font = new Font("Arial", 12);
            richTextBox.ReadOnly = true;
            richTextBox.BorderStyle = BorderStyle.None;
            richTextBox.BackColor = Color.White;
            richTextBox.ForeColor = Color.Black;
            return richTextBox;
        }

        private void ShowAddChangeForm(int id, CheckBox workCheckBox, RichTextBox nameRichTextBox, CheckBox frequencyCheckBox, RichTextBox timeRichTextBox, CheckBox mondayCheckBox, CheckBox tuesdayCheckBox, CheckBox wednesdayCheckBox, CheckBox thursdayCheckBox, CheckBox fridayCheckBox, CheckBox saturdayCheckBox, CheckBox sundayCheckBox, TableLayoutPanel panelAlarm)
        {
            AddChangeForm addChangeForm = new AddChangeForm();
            addChangeForm.NameRichTextBox.Text = nameRichTextBox.Text;
            addChangeForm.TimeRichTextBox.Text = timeRichTextBox.Text;
            addChangeForm.WorkCheckBox.Checked = workCheckBox.Checked;
            addChangeForm.FrequencyCheckBox.Checked = frequencyCheckBox.Checked;
            addChangeForm.mondayCheckBox.Checked = mondayCheckBox.Checked;
            addChangeForm.tuesdayCheckBox.Checked = tuesdayCheckBox.Checked;
            addChangeForm.wednesdayCheckBox.Checked = wednesdayCheckBox.Checked;
            addChangeForm.thursdayCheckBox.Checked = thursdayCheckBox.Checked;
            addChangeForm.fridayCheckBox.Checked = fridayCheckBox.Checked;
            addChangeForm.saturdayCheckBox.Checked = saturdayCheckBox.Checked;
            addChangeForm.sundayCheckBox.Checked = sundayCheckBox.Checked;

            addChangeForm.SaveButton.Click += (s, ev) =>
            {
                nameRichTextBox.Text = addChangeForm.NameRichTextBox.Text;
                timeRichTextBox.Text = addChangeForm.TimeRichTextBox.Text;
                workCheckBox.Checked = addChangeForm.WorkCheckBox.Checked;
                frequencyCheckBox.Checked = addChangeForm.FrequencyCheckBox.Checked;
                mondayCheckBox.Checked = addChangeForm.mondayCheckBox.Checked;
                tuesdayCheckBox.Checked = addChangeForm.tuesdayCheckBox.Checked;
                wednesdayCheckBox.Checked = addChangeForm.wednesdayCheckBox.Checked;
                thursdayCheckBox.Checked = addChangeForm.thursdayCheckBox.Checked;
                fridayCheckBox.Checked = addChangeForm.fridayCheckBox.Checked;
                saturdayCheckBox.Checked = addChangeForm.saturdayCheckBox.Checked;
                sundayCheckBox.Checked = addChangeForm.sundayCheckBox.Checked;

                if (IsTimeFormat(timeRichTextBox.Text))
                {
                    UpdateAlarmInDatabase(id, nameRichTextBox, timeRichTextBox, frequencyCheckBox, workCheckBox, mondayCheckBox, tuesdayCheckBox, wednesdayCheckBox, thursdayCheckBox, fridayCheckBox, saturdayCheckBox, sundayCheckBox, panelAlarm);
                    addChangeForm.Close();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите время в формате HH:mm:ss", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            };

            addChangeForm.CancelButton.Click += (s, ev) =>
            {
                addChangeForm.Close();
            };

            addChangeForm.ShowDialog();
        }

        bool IsTimeFormat(string input)
        {
            string pattern = @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
            return Regex.IsMatch(input, pattern);
        }

        private void UpdateAlarmInDatabase(int id, RichTextBox nameRichTextBox, RichTextBox timeRichTextBox, CheckBox frequencyCheckBox, CheckBox workCheckBox, CheckBox mondayCheckBox, CheckBox tuesdayCheckBox, CheckBox wednesdayCheckBox, CheckBox thursdayCheckBox, CheckBox fridayCheckBox, CheckBox saturdayCheckBox, CheckBox sundayCheckBox, TableLayoutPanel panelAlarm)
        {
            using (SqlConnection updateConnection = new SqlConnection(connectionString))
            {
                updateConnection.Open();
                string updateQuery = $"UPDATE [Table] SET name = @Name, time = @Time, frequency = @Frequency, Work = @Work, Monday = @Monday, Tuesday = @Tuesday, Wednesday = @Wednesday, Thursday = @Thursday, Friday = @Friday, Saturday = @Saturday, Sunday = @Sunday WHERE Id = @Id";
                using (SqlCommand updateCommand = new SqlCommand(updateQuery, updateConnection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", nameRichTextBox.Text);
                    updateCommand.Parameters.AddWithValue("@Time", timeRichTextBox.Text);
                    updateCommand.Parameters.AddWithValue("@Frequency", frequencyCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Work", workCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Monday", mondayCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Tuesday", tuesdayCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Wednesday", wednesdayCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Thursday", thursdayCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Friday", fridayCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Saturday", saturdayCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Sunday", sundayCheckBox.Checked ? 1 : 0);
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        private void AddNewAlarm(AddChangeForm addChangeForm, int panelIndex, int panelHeight)
        {
            string time = addChangeForm.TimeRichTextBox.Text;
            string name = addChangeForm.NameRichTextBox.Text;            
            bool work = addChangeForm.WorkCheckBox.Checked;
            bool frequency = addChangeForm.FrequencyCheckBox.Checked;
            bool monday = addChangeForm.mondayCheckBox.Checked;
            bool tuesday = addChangeForm.tuesdayCheckBox.Checked;
            bool wednesday = addChangeForm.wednesdayCheckBox.Checked;
            bool thursday = addChangeForm.thursdayCheckBox.Checked;
            bool friday = addChangeForm.fridayCheckBox.Checked;
            bool saturday = addChangeForm.saturdayCheckBox.Checked;
            bool sunday = addChangeForm.sundayCheckBox.Checked;

            TableLayoutPanel newPanel = new TableLayoutPanel();
            newPanel.BackColor = Color.DarkBlue;
            newPanel.Location = new Point(10, panelIndex * (panelHeight + 10));
            newPanel.Margin = new Padding(5);

            newPanel.ColumnCount = 2;
            newPanel.RowCount = 6;
            newPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            newPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            CheckBox WorkCheckBox = CreateCheckBox(work, "Работает");
            RichTextBox NameRichTextBox = CreateRichTextBox(name);
            CheckBox FrequencyCheckBox = CreateCheckBox(frequency, "Срабатывает постоянно");
            RichTextBox TimeRichTextBox = CreateRichTextBox(time);
            CheckBox MondayCheckBox = CreateCheckBox(monday, "Понедельник");
            CheckBox TuesdayCheckBox = CreateCheckBox(tuesday, "Вторник");
            CheckBox WednesdayCheckBox = CreateCheckBox(wednesday, "Среда");
            CheckBox ThursdayCheckBox = CreateCheckBox(thursday, "Четверг");
            CheckBox FridayCheckBox = CreateCheckBox(friday, "Пятница");
            CheckBox SaturdayCheckBox = CreateCheckBox(saturday, "Суббота");
            CheckBox SundayCheckBox = CreateCheckBox(sunday, "Воскресенье");

            WorkCheckBox.Height = 30;
            NameRichTextBox.Height = 30;
            FrequencyCheckBox.Height = 30;
            TimeRichTextBox.Height = 30;
            MondayCheckBox.Height = 30;
            TuesdayCheckBox.Height = 30;
            WednesdayCheckBox.Height = 30;
            ThursdayCheckBox.Height = 30;
            FridayCheckBox.Height = 30;
            SaturdayCheckBox.Height = 30;
            SundayCheckBox.Height = 30;

            newPanel.Controls.Add(TimeRichTextBox, 0, 0);
            newPanel.Controls.Add(NameRichTextBox, 1, 0);
            newPanel.Controls.Add(WorkCheckBox, 0, 1);
            newPanel.Controls.Add(FrequencyCheckBox, 1, 1);
            newPanel.Controls.Add(MondayCheckBox, 0, 2);
            newPanel.Controls.Add(TuesdayCheckBox, 1, 2);
            newPanel.Controls.Add(WednesdayCheckBox, 0, 3);
            newPanel.Controls.Add(ThursdayCheckBox, 1, 3);
            newPanel.Controls.Add(FridayCheckBox, 0, 4);
            newPanel.Controls.Add(SaturdayCheckBox, 1, 4);
            newPanel.Controls.Add(SundayCheckBox, 0, 5);

            foreach (Control control in newPanel.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).ForeColor = Color.Black;
                }
                if (control is RichTextBox)
                {
                    ((RichTextBox)control).ForeColor = Color.Black;
                }
                control.BackColor = Color.White;
            }

            newPanel.Size = new Size(400, panelHeight);

            Controls.Add(newPanel);

            SaveAlarmToDatabase(name, time, frequency, work, monday, tuesday, wednesday, thursday, friday, saturday, sunday);
        }

        private void SaveAlarmToDatabase(string name, string time, bool frequency, bool work, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        {
            using (SqlConnection newconnection = new SqlConnection(connectionString))
            {
                newconnection.Open();
                string insertQuery = "INSERT INTO [Table] (Id, name, time, frequency, Work, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday) VALUES (@Id, @Name, @Time, @Frequency, @Work, @Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday)";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, newconnection))
                {
                    insertCommand.Parameters.AddWithValue("@Id", GenerateUniqueId());
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@Time", time);
                    insertCommand.Parameters.AddWithValue("@Frequency", frequency ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Work", work ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Monday", monday ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Tuesday", tuesday ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Wednesday", wednesday ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Thursday", thursday ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Friday", friday ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Saturday", saturday ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@Sunday", sunday ? 1 : 0);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
        public static int GenerateUniqueId()
        {
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            int id = BitConverter.ToInt32(bytes, 0);
            return id;
        }

        private void AddAlarmButton_Click(object sender, EventArgs e)
        {
            int panelIndex = Controls.OfType<TableLayoutPanel>().Count();
            int panelHeight = 230;

            AddChangeForm addChangeForm = new AddChangeForm();

            addChangeForm.SaveButton.Click += (s, ev) =>
            {
                if (IsTimeFormat(addChangeForm.TimeRichTextBox.Text))
                {
                    AddNewAlarm(addChangeForm, panelIndex, panelHeight);
                    addChangeForm.Close();
                }

                else
                {
                    MessageBox.Show("Пожалуйста, введите время в формате HH:mm:ss", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            addChangeForm.CancelButton.Click += (s, ev) =>
            {
                addChangeForm.Close();
            };

            addChangeForm.ShowDialog();
        }
    }
}
