namespace Coffe_ADO_DISCONNECTED_mode;

using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        hidePanels();
        dataGridView1.MultiSelect = false;
        dataGridView1.ReadOnly = true;
        ������ToolStripMenuItem.Enabled = false;
        �������ToolStripMenuItem.Enabled = false;
        comboBox_type.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox_country_filter.DropDownStyle = ComboBoxStyle.DropDownList;
        this.Icon = Res.coffee_121492;

    }
    int variant = 0;
    string ConnectionString = "Data Source=coffe_base.db;Version=3;";
    private void hidePanels()
    {
        panel_cost.Visible = false;
        panel_country.Visible = false;
        panel_number.Visible = false;
        panel_edit.Visible = false;
        panel_search.Visible = false;
        splitContainer1.Panel2Collapsed = true;

    }
    private void dataGridViewRefresh()
    {
        dataGridView1.DataSource = null;
        dataGridView1.Refresh();
    }
    private void combobox_clear()
    {
        comboBox_country.Items.Clear();
        comboBox_country_filter.Items.Clear();
        comboBox_type.Items.Clear();
    }
    private void combobox_fill()
    {
        DataSet dataSet = new DataSet();
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT DISTINCT Country FROM Coffee ORDER BY Country ASC", connection))
                {
                    adapter.Fill(dataSet, "Country");
                    DataTable coffeeTable = dataSet.Tables["Country"];

                    if (coffeeTable != null && coffeeTable.Rows.Count > 0)
                    {
                        // ����������� �� ������ DataTable, ��������� �������� ������� "Country"
                        foreach (DataRow row in coffeeTable.Rows)
                        {
                            // �������� �������� ����� � ��������� ����� � ������ ���� �� ����������
                            comboBox_country.Items.Add(row["Country"].ToString());
                            comboBox_country_filter.Items.Add(row["Country"].ToString());
                        }
                    }
                }
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT DISTINCT  Type FROM Coffee", connection))
                {
                    adapter.Fill(dataSet, "Type");
                    DataTable coffeeTable = dataSet.Tables["Type"];


                    if (coffeeTable != null && coffeeTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in coffeeTable.Rows)
                        {
                            comboBox_type.Items.Add(row["Type"].ToString());

                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                connection.Close();
                combobox_clear();
                combobox_fill();
                ������ToolStripMenuItem.Enabled = true;
                �������ToolStripMenuItem.Enabled = true;

                MessageBox.Show("���� ����� �������", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {

            ������ToolStripMenuItem.Enabled = false;
            �������ToolStripMenuItem.Enabled = false;
            combobox_clear();
            dataGridViewRefresh();
            MessageBox.Show("���� ����� �������", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        panel_search.Visible = true;
        panel_edit.Visible = true;
        splitContainer1.Panel2Collapsed = false;

        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            splitContainer1.Panel2Collapsed = false;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

    }
    //����� � �������
    private void textBox_search_TextChanged(object sender, EventArgs e)
    {
        string searchText = textBox_search.Text.ToLower().Replace("'", "''").Replace("\\", "\\\\");
        if (searchText.Length > 0)
        {
            DataTable coffeeTable = ((DataTable)dataGridView1.DataSource);
            DataView dv = coffeeTable.DefaultView;
            dv.RowFilter = string.Format("Name LIKE '%{0}%' OR Country LIKE '%{0}%' OR Type LIKE '%{0}%' OR Description LIKE '%{0}%'", searchText);
            dataGridView1.DataSource = dv.ToTable();
            //dataGridView1.DataSource = dv;
        }
        else
        {
            ���������ToolStripMenuItem_Click(sender, e);
        }
    }



    private void button_clear_Click(object sender, EventArgs e)
    {
        textBox_search.Text = string.Empty;
        textBox_name.Text = string.Empty;
        textBox_descr.Text = string.Empty;
        textBox_number.Text = string.Empty;
        textBox_cost.Text = string.Empty;
        textBox_id.Text = string.Empty;
        textBox_name.Focus();
    }

    private void textBox_number_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void textBox_cost_KeyPress(object sender, KeyPressEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        // ��������, �� ��������� ���� �����, ���� ��� ������ Backspace
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '.'))
        {
            e.Handled = true;
            return;
        }

        // ��������, �� ���� �� � ������ ��������
        if ((e.KeyChar == ',' && textBox.Text.Length == 0) || (e.KeyChar == '.' && textBox.Text.Length == 0))
        {
            e.Handled = true;
            return;
        }
        // ��������, �� ���� ��� �������� � �����
        if ((e.KeyChar == ',' || e.KeyChar == '.') && (textBox.Text.Contains(",") || textBox.Text.Contains(".")))
        {
            e.Handled = true;
            return;
        }
        // ��������, �� ���� ���� ������ ���� ���� �� �����
        if (char.IsDigit(e.KeyChar))
        {
            // ����������� �������� ������ ����� ���� ����
            if ((textBox.Text.Contains(",") && textBox.Text.Substring(textBox.Text.IndexOf(",")).Length > 2) || (textBox.Text.Contains(".") && textBox.Text.Substring(textBox.Text.IndexOf(".")).Length > 2))
            {
                e.Handled = true;
                return;
            }
        }




    }

    private void textBox_num_min_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void textBox_num_max_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void textBox_cost_min_KeyPress(object sender, KeyPressEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        // ��������, �� ��������� ���� �����, ���� ��� ������ Backspace
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '.'))
        {
            e.Handled = true;
            return;
        }

        // ��������, �� ���� �� � ������ ��������
        if ((e.KeyChar == ',' && textBox.Text.Length == 0) || (e.KeyChar == '.' && textBox.Text.Length == 0))
        {
            e.Handled = true;
            return;
        }
        // ��������, �� ���� ��� �������� � �����
        if ((e.KeyChar == ',' || e.KeyChar == '.') && (textBox.Text.Contains(",") || textBox.Text.Contains(".")))
        {
            e.Handled = true;
            return;
        }
        // ��������, �� ���� ���� ������ ���� ���� �� �����
        if (char.IsDigit(e.KeyChar))
        {
            // ����������� �������� ������ ����� ���� ����
            if ((textBox.Text.Contains(",") && textBox.Text.Substring(textBox.Text.IndexOf(",")).Length > 2) || (textBox.Text.Contains(".") && textBox.Text.Substring(textBox.Text.IndexOf(".")).Length > 2))
            {
                e.Handled = true;
                return;
            }
        }
    }

    private void textBox_cost_max_KeyPress(object sender, KeyPressEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        // ��������, �� ��������� ���� �����, ���� ��� ������ Backspace
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '.'))
        {
            e.Handled = true;
            return;
        }

        // ��������, �� ���� �� � ������ ��������
        if ((e.KeyChar == ',' && textBox.Text.Length == 0) || (e.KeyChar == '.' && textBox.Text.Length == 0))
        {
            e.Handled = true;
            return;
        }
        // ��������, �� ���� ��� �������� � �����
        if ((e.KeyChar == ',' || e.KeyChar == '.') && (textBox.Text.Contains(",") || textBox.Text.Contains(".")))
        {
            e.Handled = true;
            return;
        }
        // ��������, �� ���� ���� ������ ���� ���� �� �����
        if (char.IsDigit(e.KeyChar))
        {
            // ����������� �������� ������ ����� ���� ����
            if ((textBox.Text.Contains(",") && textBox.Text.Substring(textBox.Text.IndexOf(",")).Length > 2) || (textBox.Text.Contains(".") && textBox.Text.Substring(textBox.Text.IndexOf(".")).Length > 2))
            {
                e.Handled = true;
                return;
            }
        }

    }



    private void textBox_num_min_TextChanged(object sender, EventArgs e)
    {
        if (float.TryParse(textBox_num_min.Text, out float costMin) && float.TryParse(textBox_num_max.Text, out float costMax))
        {
            if (costMin > costMax)
            {
                textBox_num_max.Text = textBox_num_min.Text;
            }
        }
    }

    private void textBox_num_max_TextChanged(object sender, EventArgs e)
    {
        if (float.TryParse(textBox_num_min.Text, out float costMin) && float.TryParse(textBox_num_max.Text, out float costMax))
        {
            if (costMin > costMax)
            {
                textBox_num_min.Text = textBox_num_max.Text;
            }
        }
    }

    private void dataGridView1_SelectionChanged(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count > 0)
        {
            if (splitContainer1.Panel2Collapsed == false)
            {
                int selectedIndex = dataGridView1.SelectedCells[0].RowIndex;
                textBox_id.Text = dataGridView1.Rows[selectedIndex].Cells["Id"].Value.ToString();
                textBox_name.Text = dataGridView1.Rows[selectedIndex].Cells["Name"].Value.ToString();
                comboBox_country.Text = dataGridView1.Rows[selectedIndex].Cells["Country"].Value.ToString();
                comboBox_type.Text = dataGridView1.Rows[selectedIndex].Cells["Type"].Value.ToString();
                textBox_descr.Text = dataGridView1.Rows[selectedIndex].Cells["Description"].Value.ToString();
                textBox_number.Text = dataGridView1.Rows[selectedIndex].Cells["Number"].Value.ToString();
                textBox_cost.Text = dataGridView1.Rows[selectedIndex].Cells["Cost"].Value.ToString();
            }

        }
    }

    private void button_edit_Click(object sender, EventArgs e)
    {
        string id = textBox_id.Text;
        string name = textBox_name.Text;
        string country = comboBox_country.Text;
        string type = comboBox_type.Text;
        string description = textBox_descr.Text;
        string number = textBox_number.Text;
        string cost = textBox_cost.Text;

        if (string.IsNullOrEmpty(id))
        {
            MessageBox.Show("�� ������� ����� ��� �����������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("���� \"��'�\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(country))
        {
            MessageBox.Show("���� \"�����\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(type))
        {
            MessageBox.Show("���� \"���\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(number))
        {
            MessageBox.Show("���� \"ʳ������\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(cost) || cost == "0")
        {
            MessageBox.Show("���� \"�������\" �� ���� ���� ������� ��� ����� 0!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string updateSql = "UPDATE Coffee SET Name = @name, Country = @country, Type = @type, Description = @description, Number = @number, Cost = @cost WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id); // ID ������, ���� ������� �������
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@country", country);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@number", number);
                    command.Parameters.AddWithValue("@cost", cost);


                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("����� �������� ������", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("�� ������� ������� �����", "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            ���������ToolStripMenuItem_Click(sender, e);
            combobox_clear();
            combobox_fill();


        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
        int editIndex = -1;
        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.Cells["Id"].Value != null && row.Cells["Id"].Value.ToString() == id)
            {
                editIndex = row.Index;
                break;
            }
        }

        if (editIndex >= 0)
        {

            dataGridView1.Rows[editIndex].Cells[0].Selected = true;

        }

    }



    private void button_add_Click(object sender, EventArgs e)
    {
        string id = textBox_id.Text;
        string name = textBox_name.Text;
        string country = comboBox_country.Text;
        string type = comboBox_type.Text;
        string description = textBox_descr.Text;
        string number = textBox_number.Text;
        string cost = textBox_cost.Text;


        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("���� \"��'�\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(country))
        {
            MessageBox.Show("���� \"�����\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(type))
        {
            MessageBox.Show("���� \"���\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(number))
        {
            MessageBox.Show("���� \"ʳ������\" �� ���� ���� �������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrEmpty(cost) || cost == "0")
        {
            MessageBox.Show("���� \"�������\" �� ���� ���� ������� ��� ����� 0!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string addSql = "INSERT INTO Coffee (\"Name\", \"Country\", \"Type\", \"Description\", \"Number\", \"Cost\") VALUES (@name, @country, @type, @description, @number, @cost);";

                using (SQLiteCommand command = new SQLiteCommand(addSql, connection))
                {
                    // command.Parameters.AddWithValue("@Id", id); // ID ������, ���� ������� �������
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@country", country);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@number", number);
                    command.Parameters.AddWithValue("@cost", cost);


                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("����� ������ ������", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("�� ������� ������ �����", "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            ���������ToolStripMenuItem_Click(sender, e);
            combobox_clear();
            combobox_fill();
            int lastIndex = dataGridView1.Rows.Count - 1;

            // �������� �� ������ ������
            if (lastIndex >= 0)
            {
                // ������������ ��������� �����

                dataGridView1.Rows[lastIndex].Cells[0].Selected = true;

            }

        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    private void button_del_Click(object sender, EventArgs e)
    {
        string id = textBox_id.Text;

        if (string.IsNullOrEmpty(id))
        {
            MessageBox.Show("�� ������� ����� ��� ���������!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string updateSql = "DELETE FROM Coffee  WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id); // ID ������, ���� ������� �������


                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("����� �������� ������", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("�� ������� �������� �����", "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            ���������ToolStripMenuItem_Click(sender, e);
            combobox_clear();
            combobox_fill();


        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }

    }



    //������
    private void ������������������1�����ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {

            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT MIN(Cost) FROM Coffee;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];
                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "̳������� ������� 1 � ����";
                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

    }

    private void ���������������������1�����ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {

            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT MAX(Cost) FROM Coffee;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];
                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "����������� ������� 1 � ����";
                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void �������������������������1�����ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {

            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT ROUND(TotalCost / TotalNumber, 2) AS AverageCostPerUnit FROM (SELECT SUM(Cost*Number) AS TotalCost FROM Coffee) AS CostTotal, (SELECT SUM(Number) AS TotalNumber FROM Coffee) AS NumberTotal;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];
                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "��������������� ������� 1 � ����";
                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ����������̳������������������1�����ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee WHERE Cost = (SELECT MIN(Cost) FROM Coffee);", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ��������������������������������1�����ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee WHERE Cost = (SELECT MAX(Cost) FROM Coffee);", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ����������̳����������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT *, Cost*Number AS TotalCost FROM Coffee WHERE Cost*Number = (SELECT MIN(Cost*Number) FROM Coffee);", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";
                    dataGridView1.Columns[7].HeaderText = "������� ������";

                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ������������������������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT *, Cost*Number AS TotalCost FROM Coffee WHERE Cost*Number = (SELECT MAX(Cost*Number) FROM Coffee);", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";
                    dataGridView1.Columns[7].HeaderText = "������� ������";

                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void �������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT Type, SUM(Number) AS TotalNumber FROM Coffee GROUP BY Type;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "��� ����";
                    dataGridView1.Columns[1].HeaderText = "������� �� �����";


                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT Country, COUNT(DISTINCT Id) AS NumberOfSorts FROM Coffee GROUP BY Country;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "�����";
                    dataGridView1.Columns[1].HeaderText = "ʳ������ ����� ����";


                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void �������ʳ��������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT Country, AVG(Number) AS AverageNumber FROM Coffee GROUP BY Country;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "�����";
                    dataGridView1.Columns[1].HeaderText = "������� ������� ����� ���� ����� �����";


                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���������������������������ToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee ORDER BY Cost ASC LIMIT 3;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee ORDER BY Cost DESC LIMIT 3;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���3������ʳ���������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT Country, COUNT(DISTINCT Id) AS NumberOfSorts FROM Coffee GROUP BY Country ORDER BY COUNT(DISTINCT Id) DESC LIMIT 3;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "�����";
                    dataGridView1.Columns[1].HeaderText = "ʳ������ ����� ����";


                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���3������ʳ���������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT Country, SUM (Number) AS NumberAll FROM Coffee GROUP BY Country ORDER BY  SUM (Number) DESC LIMIT 3;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "�����";
                    dataGridView1.Columns[1].HeaderText = "ʳ������ ����� ���� ������";


                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���3����������������ʳ�����������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee WHERE Type=\"arabica\" ORDER BY Number DESC LIMIT 3;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���3�����������������ʳ�����������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee WHERE Type=\"robusta\" ORDER BY Number DESC LIMIT 3;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���3��������������������ʳ������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Coffee WHERE Type=\"blend\" ORDER BY Number DESC LIMIT 3;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ���3�������������������ʳ�����������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM (SELECT * FROM Coffee WHERE Type = 'robusta' ORDER BY Number DESC LIMIT 3) AS temp1 UNION ALL SELECT * FROM ( SELECT * FROM Coffee WHERE Type = 'blend' ORDER BY Number DESC LIMIT 3) AS temp2 UNION ALL SELECT * FROM ( SELECT * FROM Coffee WHERE Type = 'arabica' ORDER BY Number DESC LIMIT 3 ) AS temp3 ORDER BY Type, Number DESC;", connection))
                {
                    adapter.Fill(dataSet, "Coffee");
                    DataTable coffeeTable = dataSet.Tables["Coffee"];

                    dataGridView1.DataSource = coffeeTable;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "�����";
                    dataGridView1.Columns[2].HeaderText = "����� ����������";
                    dataGridView1.Columns[3].HeaderText = "���";
                    dataGridView1.Columns[4].HeaderText = "����";
                    dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                    dataGridView1.Columns[6].HeaderText = "������� 1 �";


                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ����������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        hidePanels();
        DataSet dataSet = new DataSet();
        string searchString = "����";

        // ��������� SQL ������� � ����������
        string sql = @"
                SELECT *
                FROM Coffee
                WHERE
                    Name LIKE '%' || @searchString || '%' OR
                    Country LIKE '%' || @searchString || '%' OR
                    Type LIKE '%' || @searchString || '%' OR
                    Description LIKE '%' || @searchString || '%' OR
                    Number LIKE '%' || @searchString || '%' OR
                    Cost LIKE '%' || @searchString || '%'
            ";
        try
        {
            dataGridViewRefresh();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@searchString", searchString);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataSet, "Coffee");
                        DataTable coffeeTable = dataSet.Tables["Coffee"];

                        dataGridView1.DataSource = coffeeTable;

                        dataGridView1.Columns[0].HeaderText = "Id";
                        dataGridView1.Columns[1].HeaderText = "�����";
                        dataGridView1.Columns[2].HeaderText = "����� ����������";
                        dataGridView1.Columns[3].HeaderText = "���";
                        dataGridView1.Columns[4].HeaderText = "����";
                        dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                        dataGridView1.Columns[6].HeaderText = "������� 1 �";


                        dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void ������������������1�ToolStripMenuItem_Click(object sender, EventArgs e)
    {


        ���������ToolStripMenuItem_Click(sender, e);
        hidePanels();
        panel_cost.Visible = true;
        textBox_cost_max.Text = "1000";
        textBox_cost_min.Text = "0";


    }

    private void textBox_cost_min_Leave(object sender, EventArgs e)
    {
        if (float.TryParse(textBox_cost_min.Text, out float costMin) && float.TryParse(textBox_cost_max.Text, out float costMax))
        {
            if (costMin > costMax || textBox_cost_max.Text == "")
            {
                textBox_cost_max.Text = textBox_cost_min.Text;
            }


        }
        if (float.TryParse(textBox_cost_min.Text, out float costMin1) && float.TryParse(textBox_cost_max.Text, out float costMax1))
        {
            DataSet dataSet = new DataSet();


            // ��������� SQL ������� � ����������
            string sql = @"
                SELECT *
                FROM Coffee
                WHERE
                    Cost >= @costMin1 AND Cost <= @costMax1
            ";
            try
            {
                dataGridViewRefresh();
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@costMin1", costMin1);
                        command.Parameters.AddWithValue("@costMax1", costMax1);
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataSet, "Coffee");
                            DataTable coffeeTable = dataSet.Tables["Coffee"];

                            dataGridView1.DataSource = coffeeTable;

                            dataGridView1.Columns[0].HeaderText = "Id";
                            dataGridView1.Columns[1].HeaderText = "�����";
                            dataGridView1.Columns[2].HeaderText = "����� ����������";
                            dataGridView1.Columns[3].HeaderText = "���";
                            dataGridView1.Columns[4].HeaderText = "����";
                            dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                            dataGridView1.Columns[6].HeaderText = "������� 1 �";


                            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }

    private void textBox_cost_max_Leave(object sender, EventArgs e)
    {
        if (float.TryParse(textBox_cost_min.Text, out float costMin) && float.TryParse(textBox_cost_max.Text, out float costMax))
        {
            if (costMin > costMax || textBox_cost_min.Text == "")
            {
                textBox_cost_min.Text = textBox_cost_max.Text;
            }
        }
        if (float.TryParse(textBox_cost_min.Text, out float costMin1) && float.TryParse(textBox_cost_max.Text, out float costMax1))
        {
            DataSet dataSet = new DataSet();


            // ��������� SQL ������� � ����������
            string sql = @"
                SELECT *
                FROM Coffee
                WHERE
                    Cost >= @costMin1 AND Cost <= @costMax1
            ";
            try
            {
                dataGridViewRefresh();
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@costMin1", costMin1);
                        command.Parameters.AddWithValue("@costMax1", costMax1);
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataSet, "Coffee");
                            DataTable coffeeTable = dataSet.Tables["Coffee"];

                            dataGridView1.DataSource = coffeeTable;

                            dataGridView1.Columns[0].HeaderText = "Id";
                            dataGridView1.Columns[1].HeaderText = "�����";
                            dataGridView1.Columns[2].HeaderText = "����� ����������";
                            dataGridView1.Columns[3].HeaderText = "���";
                            dataGridView1.Columns[4].HeaderText = "����";
                            dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                            dataGridView1.Columns[6].HeaderText = "������� 1 �";


                            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }

    private void textBox_num_min_Leave(object sender, EventArgs e)
    {
        if (float.TryParse(textBox_num_min.Text, out float numMin) && float.TryParse(textBox_num_max.Text, out float numMax))
        {
            if (numMin > numMax || textBox_num_min.Text == "")
            {
                textBox_num_max.Text = textBox_num_min.Text;
            }
        }
        if (float.TryParse(textBox_num_min.Text, out float numMin1) && float.TryParse(textBox_num_max.Text, out float numMax1))
        {
            DataSet dataSet = new DataSet();


            // ��������� SQL ������� � ����������
            string sql = @"
                SELECT *
                FROM Coffee
                WHERE
                    Number >= @numMin1 AND Number <= @numMax1
            ";
            try
            {
                dataGridViewRefresh();
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@numMin1", numMin1);
                        command.Parameters.AddWithValue("@numMax1", numMax1);
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataSet, "Coffee");
                            DataTable coffeeTable = dataSet.Tables["Coffee"];

                            dataGridView1.DataSource = coffeeTable;

                            dataGridView1.Columns[0].HeaderText = "Id";
                            dataGridView1.Columns[1].HeaderText = "�����";
                            dataGridView1.Columns[2].HeaderText = "����� ����������";
                            dataGridView1.Columns[3].HeaderText = "���";
                            dataGridView1.Columns[4].HeaderText = "����";
                            dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                            dataGridView1.Columns[6].HeaderText = "������� 1 �";


                            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }

    private void textBox_num_max_Leave(object sender, EventArgs e)
    {
        if (float.TryParse(textBox_num_min.Text, out float numMin) && float.TryParse(textBox_num_max.Text, out float numMax))
        {
            if (numMin > numMax || textBox_num_min.Text == "")
            {
                textBox_num_min.Text = textBox_num_max.Text;
            }
        }
        if (float.TryParse(textBox_num_min.Text, out float numMin1) && float.TryParse(textBox_num_max.Text, out float numMax1))
        {
            DataSet dataSet = new DataSet();


            // ��������� SQL ������� � ����������
            string sql = @"
                SELECT *
                FROM Coffee
                WHERE
                    Number >= @numMin1 AND Number <= @numMax1
            ";
            try
            {
                dataGridViewRefresh();
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@numMin1", numMin1);
                        command.Parameters.AddWithValue("@numMax1", numMax1);
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataSet, "Coffee");
                            DataTable coffeeTable = dataSet.Tables["Coffee"];

                            dataGridView1.DataSource = coffeeTable;

                            dataGridView1.Columns[0].HeaderText = "Id";
                            dataGridView1.Columns[1].HeaderText = "�����";
                            dataGridView1.Columns[2].HeaderText = "����� ����������";
                            dataGridView1.Columns[3].HeaderText = "���";
                            dataGridView1.Columns[4].HeaderText = "����";
                            dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                            dataGridView1.Columns[6].HeaderText = "������� 1 �";


                            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }

    private void �����������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ���������ToolStripMenuItem_Click(sender, e);
        hidePanels();
        panel_number.Visible = true;
        textBox_num_max.Text = "10000";
        textBox_num_min.Text = "0";
    }
    //������ �� ������
    private void ��������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        variant = 1;
        ���������ToolStripMenuItem_Click(sender, e);
        hidePanels();
        panel_country.Visible = true;


    }
    //3 ���������� ����� � �����
    private void �������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        variant = 2;
        ���������ToolStripMenuItem_Click(sender, e);
        hidePanels();
        panel_country.Visible = true;
        comboBox_country_filter_SelectedIndexChanged(sender, e);

    }

    //3 ���������� ����� � �����
    private void �������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        variant = 3;
        ���������ToolStripMenuItem_Click(sender, e);
        hidePanels();
        panel_country.Visible = true;
        comboBox_country_filter_SelectedIndexChanged(sender, e);
    }

    private void comboBox_country_filter_SelectedIndexChanged(object sender, EventArgs e)
    {

        string searchString = comboBox_country_filter.Text.Replace("'", "''").Replace("\\", "\\\\");
        string searchString1 = comboBox_country_filter.Text;

        if (variant == 1)
        {
            ���������ToolStripMenuItem_Click(sender, e);
            hidePanels();
            panel_country.Visible = true;
            if (searchString.Length > 0)
            {
                DataTable coffeeTable = ((DataTable)dataGridView1.DataSource);
                DataView dv = coffeeTable.DefaultView;
                dv.RowFilter = string.Format("Country LIKE '%{0}%'", searchString);

                dataGridView1.DataSource = dv.ToTable();

            }
            else
            {
                ���������ToolStripMenuItem_Click(sender, e);

            }
        }
        if (variant == 2)
        {


            DataSet dataSet = new DataSet();
            try
            {

                dataGridViewRefresh();
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    string sql = "SELECT * FROM Coffee WHERE Country LIKE @searchString OR Country LIKE @searchString1 ORDER BY Cost ASC LIMIT 3;";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchString", @searchString);
                        command.Parameters.AddWithValue("@searchString1", @searchString1);
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataSet, "Coffee");
                            DataTable coffeeTable = dataSet.Tables["Coffee"];

                            dataGridView1.DataSource = coffeeTable;

                            dataGridView1.Columns[0].HeaderText = "Id";
                            dataGridView1.Columns[1].HeaderText = "�����";
                            dataGridView1.Columns[2].HeaderText = "����� ����������";
                            dataGridView1.Columns[3].HeaderText = "���";
                            dataGridView1.Columns[4].HeaderText = "����";
                            dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                            dataGridView1.Columns[6].HeaderText = "������� 1 �";


                            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        if (variant == 3)
        {


            DataSet dataSet = new DataSet();
            try
            {

                dataGridViewRefresh();
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    string sql = "SELECT * FROM Coffee WHERE Country LIKE @searchString OR Country LIKE @searchString1 ORDER BY Cost DESC LIMIT 3;";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@searchString", @searchString);
                        command.Parameters.AddWithValue("@searchString1", @searchString1);
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataSet, "Coffee");
                            DataTable coffeeTable = dataSet.Tables["Coffee"];

                            dataGridView1.DataSource = coffeeTable;

                            dataGridView1.Columns[0].HeaderText = "Id";
                            dataGridView1.Columns[1].HeaderText = "�����";
                            dataGridView1.Columns[2].HeaderText = "����� ����������";
                            dataGridView1.Columns[3].HeaderText = "���";
                            dataGridView1.Columns[4].HeaderText = "����";
                            dataGridView1.Columns[5].HeaderText = "ʳ������ �� �����";
                            dataGridView1.Columns[6].HeaderText = "������� 1 �";


                            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}