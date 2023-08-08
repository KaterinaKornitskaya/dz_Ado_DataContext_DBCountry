using dz_Ado_DataContext_DBCountry.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dz_Ado_DataContext_DBCountry
{
    public partial class Form1 : Form
    {
        // создаем статический объект LibraryDataContext
        static CountryDataContext db = new CountryDataContext();
        public Form1()
        {
            InitializeComponent();           
        }

        // обработчик кнопки Добавить (добавление новой страны)
        private void buttonAdd_Click(object sender, EventArgs e)
        {          
            db.AddNewCountry(textBoxCountryAdd.Text, textBoxCapitalAdd.Text, 
                Convert.ToSingle(textBoxPopulatAdd.Text),
                Convert.ToSingle(textBoxSquareAdd.Text), textBoxPartOfAdd.Text);
        }

        // обработчик кнопки Показать (показать список стран)
        private void buttonShow_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = null;
            //dataGridView1.DataSource = db.Countries;
            listBox2.DataSource = null;
            listBox2.Items.Clear();
            listBox2.DataSource = db.Countries;
        }

        // обработчик кнопки Удалить (удалить страну)
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            db.DeleteCountry(textBoxCapitalDelete.Text);
        }

        // обработчик нажатия comboBox1 (вывод результатов linq-запросов)
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    listBox1.DataSource = db.Top3BySquare();
                    break;
                case 1:
                    listBox1.DataSource = db.Top3ByPopulation();
                    break;
                case 2:
                    listBox1.DataSource = db.MaxSquare();
                    break;
                case 3:
                    listBox1.DataSource = db.MinSqEurope();
                    break;
                case 4:
                    listBox1.DataSource = null;
                    listBox1.Items.Clear();
                    listBox1.Items.Add(db.AvSqEurope());
                    break;
                case 5:
                    listBox1.DataSource = null;
                    listBox1.Items.Clear();
                    listBox1.Items.Add(db.CountryCount());
                    break;
                case 6:
                    listBox1.DataSource = db.MaxPart();
                    break;
                case 7:
                    listBox1.DataSource = db.PartOfWorldCountriesAmount();
                    break;
            }
        }

        // обработчик кнопки Редактировать (изменения новой страны)
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            db.EditCountry(textBoxCountryEdit.Text, textBoxCapitalEdit.Text,
                Convert.ToSingle(textBoxPopulatEdit.Text),
                Convert.ToSingle(textBoxSquareEdit.Text), textBoxPartOfEdit.Text);
        }
    }
}
