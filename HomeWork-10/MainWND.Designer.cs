using HomeWork_10.Core.Classes.Address;
using HomeWork_10.Core.Classes.Employee;
using HomeWork_10.Core.Classes.JsonFormater;
using HomeWork_10.Core.Classes.Student;
using HomeWork_10.Core.Classes.University;
using HomeWork_10.Core.Enums.MakeEnabledOptions;
using HomeWork_10.Core.Interfaces.Istudent;
using HomeWork_10.Utils.Validators.EmailValidator;
using HomeWork_8.Utils.Validators.DataValidators.UserNameValidator;
using System.IO;
using System.Reflection;

namespace HomeWork_10
{
    partial class MainWND
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private List<Student> _students = new List<Student>();
        private List<Employee> _employees = new List<Employee>();
        private List<University> _universities = new List<University>();
        private List<Address> _addresses = new List<Address>();
        private List<object> _objects = new List<object>();
        private ObjectType _currentObjType;
        

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Clear()
        {
            foreach (var textBox in new[] { zerroTextBox, firstTextBox, secondTextBox, thirdTextBox })
                textBox.Text = string.Empty;
        }

        private void SetLabels(string zero, string first, string second, string fourth, bool showThird)
        {
            zeroLabel.Text = zero;
            firstLabel.Text = first;
            secondLabel.Text = second;
            fourthLabel.Text = fourth;

            thirdTextBox.Visible = fourthLabel.Visible = showThird;
        }

        private void MakeEnabled(ObjectType type)
        {
            _currentObjType = type;
            zerroTextBox.Enabled = firstTextBox.Enabled = secondTextBox.Enabled = thirdTextBox.Enabled = true;

            switch (type)
            {
                case ObjectType.Student:
                    SetLabels("id", "name", "surname", "email", true);
                    break;
                case ObjectType.Employee:
                    SetLabels("id", "name", "surname", "salary", true);
                    break;
                case ObjectType.University:
                    SetLabels("id", "name", "description", "", false);
                    break;
                case ObjectType.Address:
                    SetLabels("country", "region", "city", "postalcode", true);
                    break;
                default:
                    SetLabels("id", "name", "surname", "", true);
                    break;
            }
        }

        private bool CheckData(out int value_1, out int value_2, ObjectType type)
        {
            bool hasError = false;
            value_1 = value_2 = 0;

            Error.Clear();

            if (type != ObjectType.Address)
            {
                if (!int.TryParse(zerroTextBox.Text, out value_1))
                {
                    Error.SetError(zerroTextBox, "Enter a valid ID!");
                    hasError = true;
                }
            }
           
            if (type == ObjectType.Student && !EmailValidator.CheckEmail(thirdTextBox.Text))
            {
                Error.SetError(thirdTextBox, "Enter a valid email!");
                hasError = true;
            }
            else if (type == ObjectType.Employee && !int.TryParse(thirdTextBox.Text, out value_2))
            {
                Error.SetError(thirdTextBox, "Enter a valid salary!");
                hasError = true;
            }
            else if (type == ObjectType.Address && !int.TryParse(thirdTextBox.Text, out value_1))
            {
                Error.SetError(thirdTextBox, "Enter a valid postal code!");
                hasError = true;
            }

            return hasError;
        }

        private void CreateObject(ObjectType type)
        {
            if (CheckData(out int id, out int value2, type)) return;

            switch (type)
            {
                case ObjectType.Student:
                    AddObject(new Student(id, firstTextBox.Text, secondTextBox.Text, thirdTextBox.Text), "Student");
                    break;
                case ObjectType.Employee:
                    AddObject(new Employee(id, firstTextBox.Text, secondTextBox.Text, value2), "Employee");
                    break;
                case ObjectType.University:
                    AddObject(new University(id, firstTextBox.Text, secondTextBox.Text), "University");
                    break;
                case ObjectType.Address:
                    AddObject(new Address(zerroTextBox.Text, firstTextBox.Text, secondTextBox.Text, value2), "Address");
                    break;
                default:
                    MessageBox.Show("Select Object Type", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void AddObject<T>(T obj, string typeName)
        {
            _objects.Add(obj);
            MessageBox.Show($"{typeName} Created", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            switch (obj)
            {
                case Student student:
                    Objects.Items.Add($"Student Id: {student.Id}, Name: {student.Name}, Email: {student.email}");
                    _students.Add(student);
                    break;
                case Employee employee:
                    Objects.Items.Add($"Employee Id: {employee.Id}, Salary: {employee.salary}");
                    _employees.Add(employee);
                    break;
                case University university:
                    Objects.Items.Add($"University Id: {university.Id}, Desc: {university.Description}");
                    _universities.Add(university);
                    break;
                case Address address:
                    Objects.Items.Add($"City: {address.City}, Postal Code: {address.PostalCode}");
                    _addresses.Add(address);
                    break;
            }

            Clear();
        }

        private string ShowDialog<T>(bool isFolderDialog, string description = "")
        {
            string result = string.Empty;
            if (isFolderDialog)
            {
                using (var dialog = new FolderBrowserDialog { Description = description, ShowNewFolderButton = true })
                    if (dialog.ShowDialog() == DialogResult.OK) result = dialog.SelectedPath;
            }
            else
            {
                using (var dialog = new OpenFileDialog())
                    if (dialog.ShowDialog() == DialogResult.OK) result = dialog.FileName;
            }
            return result;
        }

        private void ConvertToJson<T>(string fileName, List<T> list)
        {
            if (list.Count == 0) return;

            string dirPath = ShowDialog<FolderBrowserDialog>(true, "Select the path where the files will be stored");
            string path = Path.Combine(dirPath, $"{fileName}_{typeof(T).Name}.json");
            JsonTool.ToJson(list, path);
        }

        private void Convert()
        {
            string fileName = fileNameBox.Text;

            ConvertToJson(fileName, _students);
            ConvertToJson(fileName, _employees);
            ConvertToJson(fileName, _universities);
            ConvertToJson(fileName, _addresses);

            if (_objects.Count == 0)
                MessageBox.Show("No Objects :(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                MessageBox.Show("Successful :)");
                _students.Clear();
                _employees.Clear();
                _universities.Clear();
                _addresses.Clear();
                _objects.Clear();
            }
        }

        private void Read()
        {
            string path = ShowDialog<OpenFileDialog>(false);

            if (path.Contains(nameof(Student)))
                LoadFromFile<Student>(path);
            else if (path.Contains(nameof(Employee)))
                LoadFromFile<Employee>(path);
            else if (path.Contains(nameof(University)))
                LoadFromFile<University>(path);
            else if (path.Contains(nameof(Address)))
                LoadFromFile<Address>(path);
        }

        private void LoadFromFile<T>(string path)
        {
            var items = JsonTool.ToList<T>(path);
            foreach (var item in items)
            {
                Type itemType = item.GetType();
                PropertyInfo[] itemProps = itemType.GetProperties();

                ResultList.Items.Add($"Object: {itemType.Name}");
                foreach (var prop in itemProps)
                {
                    ResultList.Items.Add($"{prop.Name}: {prop.GetValue(item).ToString()}");
                }
            }
        }





        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWND));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            createObjectBtn = new Button();
            label2 = new Label();
            panel1 = new Panel();
            thirdTextBox = new TextBox();
            fourthLabel = new Label();
            secondTextBox = new TextBox();
            secondLabel = new Label();
            firstTextBox = new TextBox();
            firstLabel = new Label();
            zerroTextBox = new TextBox();
            zeroLabel = new Label();
            label1 = new Label();
            ObjectCombo = new ComboBox();
            tabPage2 = new TabPage();
            label8 = new Label();
            fileNameBox = new TextBox();
            Objects = new ListBox();
            ConvertBtn = new Button();
            label7 = new Label();
            tabPage3 = new TabPage();
            ReadBtn = new Button();
            label9 = new Label();
            ResultList = new ListBox();
            Error = new ErrorProvider(components);
            folderBrowserDialog1 = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Error).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1466, 607);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(createObjectBtn);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(panel1);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(ObjectCombo);
            tabPage1.Location = new Point(8, 46);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1450, 553);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Create";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // createObjectBtn
            // 
            createObjectBtn.Location = new Point(18, 484);
            createObjectBtn.Name = "createObjectBtn";
            createObjectBtn.Size = new Size(242, 46);
            createObjectBtn.TabIndex = 4;
            createObjectBtn.Text = "Create New Object";
            createObjectBtn.UseVisualStyleBackColor = true;
            createObjectBtn.Click += createObjectBtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(288, 7);
            label2.Name = "label2";
            label2.Size = new Size(184, 32);
            label2.TabIndex = 3;
            label2.Text = "Fill object props";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(thirdTextBox);
            panel1.Controls.Add(fourthLabel);
            panel1.Controls.Add(secondTextBox);
            panel1.Controls.Add(secondLabel);
            panel1.Controls.Add(firstTextBox);
            panel1.Controls.Add(firstLabel);
            panel1.Controls.Add(zerroTextBox);
            panel1.Controls.Add(zeroLabel);
            panel1.Location = new Point(288, 42);
            panel1.Name = "panel1";
            panel1.Size = new Size(1138, 488);
            panel1.TabIndex = 2;
            // 
            // thirdTextBox
            // 
            thirdTextBox.BorderStyle = BorderStyle.FixedSingle;
            thirdTextBox.Enabled = false;
            thirdTextBox.Location = new Point(31, 286);
            thirdTextBox.Name = "thirdTextBox";
            thirdTextBox.Size = new Size(443, 39);
            thirdTextBox.TabIndex = 7;
            // 
            // emailSalaryLabel
            // 
            fourthLabel.AutoSize = true;
            fourthLabel.Location = new Point(31, 251);
            fourthLabel.Name = "emailSalaryLabel";
            fourthLabel.Size = new Size(72, 32);
            fourthLabel.TabIndex = 6;
            fourthLabel.Text = "email";
            // 
            // secondTextBox
            // 
            secondTextBox.BorderStyle = BorderStyle.FixedSingle;
            secondTextBox.Enabled = false;
            secondTextBox.Location = new Point(31, 212);
            secondTextBox.Name = "secondTextBox";
            secondTextBox.Size = new Size(443, 39);
            secondTextBox.TabIndex = 5;
            // 
            // snameOrDescriptionLabel
            // 
            secondLabel.AutoSize = true;
            secondLabel.Location = new Point(31, 177);
            secondLabel.Name = "snameOrDescriptionLabel";
            secondLabel.Size = new Size(106, 32);
            secondLabel.TabIndex = 4;
            secondLabel.Text = "surname";
            // 
            // firstTextBox
            // 
            firstTextBox.BorderStyle = BorderStyle.FixedSingle;
            firstTextBox.Enabled = false;
            firstTextBox.Location = new Point(31, 135);
            firstTextBox.Name = "firstTextBox";
            firstTextBox.Size = new Size(443, 39);
            firstTextBox.TabIndex = 3;
            // 
            // firstLabel
            // 
            firstLabel.AutoSize = true;
            firstLabel.Location = new Point(31, 100);
            firstLabel.Name = "firstLabel";
            firstLabel.Size = new Size(74, 32);
            firstLabel.TabIndex = 2;
            firstLabel.Text = "name";
            // 
            // zerroTextBox
            // 
            zerroTextBox.BorderStyle = BorderStyle.FixedSingle;
            zerroTextBox.Enabled = false;
            zerroTextBox.Location = new Point(31, 59);
            zerroTextBox.Name = "zerroTextBox";
            zerroTextBox.Size = new Size(443, 39);
            zerroTextBox.TabIndex = 1;
            // 
            // zeroLabel
            // 
            zeroLabel.AutoSize = true;
            zeroLabel.Location = new Point(31, 24);
            zeroLabel.Name = "zeroLabel";
            zeroLabel.Size = new Size(34, 32);
            zeroLabel.TabIndex = 0;
            zeroLabel.Text = "id";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 7);
            label1.Name = "label1";
            label1.Size = new Size(142, 32);
            label1.TabIndex = 1;
            label1.Text = "Object Type";
            // 
            // ObjectCombo
            // 
            ObjectCombo.FormattingEnabled = true;
            ObjectCombo.Items.AddRange(new object[] { "Student", "Employee", "University", "Address" });
            ObjectCombo.Location = new Point(18, 42);
            ObjectCombo.Name = "ObjectCombo";
            ObjectCombo.Size = new Size(242, 40);
            ObjectCombo.TabIndex = 0;
            ObjectCombo.SelectedIndexChanged += ObjectCombo_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label8);
            tabPage2.Controls.Add(fileNameBox);
            tabPage2.Controls.Add(Objects);
            tabPage2.Controls.Add(ConvertBtn);
            tabPage2.Controls.Add(label7);
            tabPage2.Location = new Point(8, 46);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1450, 553);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Convert";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(18, 7);
            label8.Name = "label8";
            label8.Size = new Size(122, 32);
            label8.TabIndex = 9;
            label8.Text = "File Name";
            // 
            // fileNameBox
            // 
            fileNameBox.Location = new Point(18, 42);
            fileNameBox.Name = "fileNameBox";
            fileNameBox.Size = new Size(242, 39);
            fileNameBox.TabIndex = 8;
            // 
            // Objects
            // 
            Objects.FormattingEnabled = true;
            Objects.Location = new Point(288, 42);
            Objects.Name = "Objects";
            Objects.Size = new Size(1136, 484);
            Objects.TabIndex = 7;
            // 
            // ConvertBtn
            // 
            ConvertBtn.Location = new Point(18, 486);
            ConvertBtn.Name = "ConvertBtn";
            ConvertBtn.Size = new Size(242, 46);
            ConvertBtn.TabIndex = 6;
            ConvertBtn.Text = "Convert";
            ConvertBtn.UseVisualStyleBackColor = true;
            ConvertBtn.Click += ConvertBtn_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(288, 7);
            label7.Name = "label7";
            label7.Size = new Size(128, 32);
            label7.TabIndex = 5;
            label7.Text = "All Objects";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(ReadBtn);
            tabPage3.Controls.Add(label9);
            tabPage3.Controls.Add(ResultList);
            tabPage3.Location = new Point(8, 46);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1450, 553);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Read";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // ReadBtn
            // 
            ReadBtn.Location = new Point(18, 489);
            ReadBtn.Name = "ReadBtn";
            ReadBtn.Size = new Size(242, 46);
            ReadBtn.TabIndex = 12;
            ReadBtn.Text = "Read";
            ReadBtn.UseVisualStyleBackColor = true;
            ReadBtn.Click += ReadBtn_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(290, 7);
            label9.Name = "label9";
            label9.Size = new Size(78, 32);
            label9.TabIndex = 11;
            label9.Text = "Result";
            // 
            // ResultList
            // 
            ResultList.FormattingEnabled = true;
            ResultList.Location = new Point(290, 42);
            ResultList.Name = "ResultList";
            ResultList.Size = new Size(1136, 484);
            ResultList.TabIndex = 10;
            // 
            // Error
            // 
            Error.ContainerControl = this;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainWND
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1462, 603);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainWND";
            Text = "Object Converter";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Error).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Label label1;
        private ComboBox ObjectCombo;
        private Panel panel1;
        private Label label2;
        private Button createObjectBtn;
        private TextBox thirdTextBox;
        private Label fourthLabel;
        private TextBox secondTextBox;
        private Label secondLabel;
        private TextBox firstTextBox;
        private Label firstLabel;
        private TextBox zerroTextBox;
        private Label zeroLabel;
        private ErrorProvider Error;
        private Button ConvertBtn;
        private Label label7;
        private ListBox Objects;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label8;
        private TextBox fileNameBox;
        private Button ReadBtn;
        private Label label9;
        private ListBox ResultList;
        private OpenFileDialog openFileDialog1;
    }
}
