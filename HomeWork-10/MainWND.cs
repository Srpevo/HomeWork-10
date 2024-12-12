using HomeWork_10.Core.Classes.JsonFormater;
using HomeWork_10.Core.Enums.MakeEnabledOptions;
using System.Text.Json.Serialization;

namespace HomeWork_10
{
    public partial class MainWND : Form
    {
        public MainWND()
        {
            InitializeComponent();
        }



        private void ObjectCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox box)
            {
                switch (box.Text.ToLower())
                {
                    case "student":
                            MakeEnabled(ObjectType.Student);
                            break;
                    case "employee":
                            MakeEnabled(ObjectType.Employee);
                            break;                        
                    case "university":
                            MakeEnabled(ObjectType.University);
                            break;
                    case "address":
                            MakeEnabled(ObjectType.Address);
                            break;
                }
            }
        }


        private void createObjectBtn_Click(object sender, EventArgs e)
        {
            if (sender is  Button)
            {
                CreateObject(_currentObjType);
            }
        }

        private void ConvertBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                if (fileNameBox.Text == null)
                {
                    Error.SetError(fileNameBox, "File name is empty!");
                    return;
                }
                Convert();
                _objects.Clear();
                Objects.Items.Clear();
                fileNameBox.Text = string.Empty;
            }
        }

        private void ReadBtn_Click(object sender, EventArgs e)
        {
            Read();
        }
    }
}
