using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PW1.Model;
using PW1.BusinessLogic;     
using PW1.DataAccessLayer;   

namespace PW1.WinForms
{
    public partial class Form1 : Form
    {
        private Logic _logic = new Logic(new EntityRepository<Gnome>());

        // Элементы управления
        private DataGridView dataGridView;
        private TextBox txtName, txtClan, txtProfession, txtAge, txtBeard, txtSearchProf;
        private Button btnAdd, btnUpdate, btnDelete, btnClear, btnGroup, btnSearch;
        private Label lblSearchResult;

        public Form1()
        {
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Text = "Реестр Гномов (Практическая работа №1)";
            this.Size = new Size(950, 620);

            // ================= ТАБЛИЦА =================
            dataGridView = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(650, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            this.Controls.Add(dataGridView);

            // ================= ПАНЕЛЬ ВВОДА =================
            Panel inputPanel = new Panel { Location = new Point(670, 10), Size = new Size(260, 400) };

            // Имя
            inputPanel.Controls.Add(new Label { Text = "Имя гнома:", Location = new Point(10, 10), AutoSize = true });
            txtName = new TextBox { Location = new Point(10, 30), Width = 210 };
            inputPanel.Controls.Add(txtName);

            // Клан
            inputPanel.Controls.Add(new Label { Text = "Клан:", Location = new Point(10, 60), AutoSize = true });
            txtClan = new TextBox { Location = new Point(10, 80), Width = 210 };
            inputPanel.Controls.Add(txtClan);

            // Профессия
            inputPanel.Controls.Add(new Label { Text = "Профессия:", Location = new Point(10, 110), AutoSize = true });
            txtProfession = new TextBox { Location = new Point(10, 130), Width = 210 };
            inputPanel.Controls.Add(txtProfession);

            // Возраст
            inputPanel.Controls.Add(new Label { Text = "Возраст:", Location = new Point(10, 160), AutoSize = true });
            txtAge = new TextBox { Location = new Point(10, 180), Width = 210 };
            inputPanel.Controls.Add(txtAge);

            // Длина бороды
            inputPanel.Controls.Add(new Label { Text = "Длина бороды (см):", Location = new Point(10, 210), AutoSize = true });
            txtBeard = new TextBox { Location = new Point(10, 230), Width = 210 };
            inputPanel.Controls.Add(txtBeard);

            // Кнопки
            btnAdd = new Button { Text = "Добавить", Location = new Point(10, 270), Width = 100, BackColor = Color.LightGreen };
            btnAdd.Click += BtnAdd_Click;
            inputPanel.Controls.Add(btnAdd);

            btnUpdate = new Button { Text = "Изменить", Location = new Point(120, 270), Width = 100, BackColor = Color.LightBlue };
            btnUpdate.Click += BtnUpdate_Click;
            inputPanel.Controls.Add(btnUpdate);

            btnDelete = new Button { Text = "Удалить", Location = new Point(10, 310), Width = 100, BackColor = Color.LightCoral };
            btnDelete.Click += BtnDelete_Click;
            inputPanel.Controls.Add(btnDelete);

            btnClear = new Button { Text = "Очистить поля", Location = new Point(120, 310), Width = 100 };
            btnClear.Click += (s, e) => ClearInputs();
            inputPanel.Controls.Add(btnClear);

            this.Controls.Add(inputPanel);

            // ================= ПАНЕЛЬ БИЗНЕС-ЛОГИКИ =================
            Panel logicPanel = new Panel { Location = new Point(10, 420), Size = new Size(910, 130), BorderStyle = BorderStyle.FixedSingle };

            logicPanel.Controls.Add(new Label { Text = "Бизнес-функции:", Location = new Point(10, 10), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });

            // Кнопка группировки
            btnGroup = new Button { Text = "Собрать по кланам", Location = new Point(10, 40), Width = 200, Height = 30 };
            btnGroup.Click += BtnGroup_Click;
            logicPanel.Controls.Add(btnGroup);

            // Надпись поиска
            Label lblSearch = new Label();
            lblSearch.Text = "Поиск по профессии:";
            lblSearch.Location = new Point(240, 45);
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Arial", 10, FontStyle.Regular);
            logicPanel.Controls.Add(lblSearch);

            // Поле ввода для поиска
            txtSearchProf = new TextBox { Location = new Point(400, 44), Width = 130 };
            logicPanel.Controls.Add(txtSearchProf);

            // Кнопка Найти
            btnSearch = new Button { Text = "Найти", Location = new Point(540, 42), Width = 80, Height = 26 };
            btnSearch.Click += BtnSearch_Click;
            logicPanel.Controls.Add(btnSearch);

            // Метка результата
            lblSearchResult = new Label { Location = new Point(10, 80), AutoSize = true, ForeColor = Color.DarkBlue, Font = new Font("Arial", 9) };
            logicPanel.Controls.Add(lblSearchResult);

            this.Controls.Add(logicPanel);
        }

        private void LoadData()
        {
            dataGridView.DataSource = null;
            dataGridView.DataSource = _logic.GetAllGnomes();
            if (dataGridView.Columns.Count > 0 && dataGridView.Columns["Id"] != null)
            {
                dataGridView.Columns["Id"].Visible = false;
            }
        }

        private void ClearInputs()
        {
            txtName.Clear(); txtClan.Clear(); txtProfession.Clear(); txtAge.Clear(); txtBeard.Clear();
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var gnome = (Gnome)dataGridView.SelectedRows[0].DataBoundItem;
                txtName.Text = gnome.Name;
                txtClan.Text = gnome.Clan;
                txtProfession.Text = gnome.Profession;
                txtAge.Text = gnome.Age.ToString();
                txtBeard.Text = gnome.BeardLength.ToString();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            _logic.AddGnome(txtName.Text, txtClan.Text, txtProfession.Text, int.Parse(txtAge.Text), int.Parse(txtBeard.Text));
            LoadData();
            ClearInputs();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) { MessageBox.Show("Выберите гнома для редактирования!"); return; }
            if (!ValidateInputs()) return;

            var selectedGnome = (Gnome)dataGridView.SelectedRows[0].DataBoundItem;
            _logic.UpdateGnome(selectedGnome.Id, txtName.Text, txtClan.Text, txtProfession.Text, int.Parse(txtAge.Text), int.Parse(txtBeard.Text));
            LoadData();
            ClearInputs();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) { MessageBox.Show("Выберите гнома для удаления!"); return; }

            var selectedGnome = (Gnome)dataGridView.SelectedRows[0].DataBoundItem;
            if (MessageBox.Show($"Удалить гнома \"{selectedGnome.Name}\"?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _logic.DeleteGnome(selectedGnome.Id);
                LoadData();
                ClearInputs();
            }
        }

        private void BtnGroup_Click(object sender, EventArgs e)
        {
            var grouped = _logic.GroupGnomesByClan();
            string result = "Группировка гномов по кланам:\n\n";
            foreach (var group in grouped)
            {
                result += $"Клан {group.Key} ({group.Value.Count} гномов):\n";
                foreach (var g in group.Value)
                {
                    result += $"   - {g.Name} ({g.Profession}, {g.Age} лет)\n";
                }
                result += "\n";
            }
            MessageBox.Show(result, "Результат группировки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string prof = txtSearchProf.Text.Trim();
            if (string.IsNullOrEmpty(prof)) { MessageBox.Show("Введите профессию для поиска."); return; }

            var results = _logic.GetGnomesByProfession(prof);
            if (results.Count == 0)
            {
                lblSearchResult.Text = "Гномов с такой профессией не найдено.";
            }
            else
            {
                lblSearchResult.Text = $"Найдено {results.Count} гномов:\n" + string.Join("\n", results.Select(g => $"{g.Age} лет - {g.Name} (Борода: {g.BeardLength} см)"));
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtClan.Text) ||
                string.IsNullOrWhiteSpace(txtProfession.Text) || string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtBeard.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }
            if (!int.TryParse(txtAge.Text, out _) || !int.TryParse(txtBeard.Text, out _))
            {
                MessageBox.Show("Возраст и длина бороды должны быть целыми числами!");
                return false;
            }
            return true;
        }
    }
}